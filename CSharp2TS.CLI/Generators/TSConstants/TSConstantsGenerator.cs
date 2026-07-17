using CSharp2TS.CLI.Generators.Common;
using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Utility;
using CSharp2TS.Core.Attributes;
using Mono.Cecil;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CSharp2TS.CLI.Generators.TSConstants {
    public class TSConstantsGenerator {
        private readonly Dictionary<string, TSFileInfo> files;
        private readonly Options options;

        public TSConstantsGenerator(Dictionary<string, TSFileInfo> files, Options options) {
            this.files = files;
            this.options = options;
        }

        public string Generate(TypeDefinition typeDef) {
            TSConstants tsConstants = new TSConstants(NameUtility.GetName(typeDef));

            ParseFields(tsConstants, typeDef);

            return TSConstantsTypeScriptGenerator.Generate(tsConstants);
        }

        private void ParseFields(TSConstants tsConstants, TypeDefinition typeDef) {
            foreach (var field in typeDef.Fields) {
                if (field.IsSpecialName || !field.IsPublic || field.HasAttribute<TSExcludeAttribute>()) {
                    continue;
                }

                if (!TryFormatValue(tsConstants, typeDef, field, out string value)) {
                    continue;
                }

                tsConstants.Values.Add(new TSConstantValue(field.Name.ApplyCasing(options.MemberNameCasingStyle), value));
            }
        }

        private bool TryFormatValue(TSConstants tsConstants, TypeDefinition typeDef, FieldDefinition field, out string value) {
            if (field.IsLiteral && field.HasConstant) {
                if (TryFormatEnumConstant(tsConstants, typeDef, field, out value)) {
                    return true;
                }

                return TryFormatConstant(field.Constant, out value);
            }

            // A const decimal compiles to a static readonly field carrying a DecimalConstantAttribute
            if (TryGetDecimalConstant(field, out decimal decimalValue)) {
                value = decimalValue.ToString(CultureInfo.InvariantCulture);
                return true;
            }

            value = string.Empty;
            return false;
        }

        private bool TryFormatEnumConstant(TSConstants tsConstants, TypeDefinition typeDef, FieldDefinition field, out string value) {
            value = string.Empty;

            // The only value types C# allows as const are primitives, decimal and enums,
            // so avoid resolving anything else
            if (!field.FieldType.IsValueType || field.FieldType.IsPrimitive) {
                return false;
            }

            var enumDef = field.FieldType.Resolve();

            // Only reference the enum if it is part of the generated output
            if (enumDef == null || !enumDef.IsEnum || !files.TryGetValue(enumDef.FullName, out var enumFile)) {
                return false;
            }

            var enumField = enumDef.Fields.FirstOrDefault(i => !i.IsSpecialName && Equals(i.Constant, field.Constant));

            if (enumField == null) {
                return false;
            }

            string enumName = NameUtility.GetName(enumDef);

            if (!tsConstants.Imports.Any(i => i.FullName == enumDef.FullName)) {
                tsConstants.Imports.Add(new TSImport(enumDef.FullName, enumName, files[typeDef.FullName].GetImportPathTo(enumFile)));
            }

            value = $"{enumName}.{enumField.Name}";
            return true;
        }

        private static bool TryFormatConstant(object? constant, out string value) {
            switch (constant) {
                case null:
                    value = "null";
                    return true;
                case string str:
                    value = $"'{EscapeString(str)}'";
                    return true;
                case char c:
                    value = $"'{EscapeString(c.ToString())}'";
                    return true;
                case bool b:
                    value = b ? "true" : "false";
                    return true;
                case float f:
                    value = f.ToString(CultureInfo.InvariantCulture);
                    return true;
                case double d:
                    value = d.ToString(CultureInfo.InvariantCulture);
                    return true;
                case sbyte or byte or short or ushort or int or uint or long or ulong:
                    value = Convert.ToString(constant, CultureInfo.InvariantCulture)!;
                    return true;
                default:
                    value = string.Empty;
                    return false;
            }
        }

        private static bool TryGetDecimalConstant(FieldDefinition field, out decimal value) {
            value = default;

            if (!field.IsStatic || !field.IsInitOnly || !field.TryGetAttribute<DecimalConstantAttribute>(out var attr)) {
                return false;
            }

            var args = attr.ConstructorArguments;

            byte scale = Convert.ToByte(args[0].Value);
            bool isNegative = Convert.ToByte(args[1].Value) != 0;
            // The attribute has int and uint constructor overloads; keep the raw bits either way
            int hi = unchecked((int)Convert.ToInt64(args[2].Value));
            int mid = unchecked((int)Convert.ToInt64(args[3].Value));
            int low = unchecked((int)Convert.ToInt64(args[4].Value));

            value = new decimal(low, mid, hi, isNegative, scale);
            return true;
        }

        private static string EscapeString(string value) {
            return value
                .Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");
        }
    }
}
