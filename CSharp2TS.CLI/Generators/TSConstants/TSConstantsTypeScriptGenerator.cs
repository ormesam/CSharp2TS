using System.Text;

namespace CSharp2TS.CLI.Generators.TSConstants {
    public static class TSConstantsTypeScriptGenerator {
        public static string Generate(TSConstants tsConstants) {
            StringBuilder sb = new();

            sb.AppendLine($"// Auto-generated from {tsConstants.Name}.cs");

            if (tsConstants.Imports.Count > 0) {
                sb.AppendLine();

                foreach (var item in tsConstants.Imports) {
                    sb.AppendLine($"import {item.Name} from '{item.Path}';");
                }
            }

            sb.AppendLine();
            sb.AppendLine($"const {tsConstants.Name} = {{");

            foreach (var item in tsConstants.Values) {
                sb.AppendLine($"  {item.Name}: {item.Value},");
            }

            sb.AppendLine("} as const;");
            sb.AppendLine();
            sb.AppendLine($"export default {tsConstants.Name};");

            return sb.ToString();
        }
    }
}
