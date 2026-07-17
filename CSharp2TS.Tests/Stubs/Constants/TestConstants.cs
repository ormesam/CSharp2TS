using CSharp2TS.Core.Attributes;
using CSharp2TS.Tests.Stubs.Enums;

namespace CSharp2TS.Tests.Stubs.Constants {
    [TSConstants]
    public static class TestConstants {
        public const string AppName = "MyApp";
        public const string? NullValue = null;
        public const string Quoted = "It's \"quoted\"\n";
        public const int MaxPageSize = 100;
        public const long LongValue = 9007199254740993;
        public const double Pi = 3.14159;
        public const float FloatValue = 1.5f;
        public const decimal Price = 19.99m;
        public const bool IsEnabled = true;
        public const char Separator = ';';
        public const TestEnum DefaultEnum = TestEnum.Value2;
        public const PlainEnum UnregisteredEnum = PlainEnum.A;
        public const TestEnum UndefinedEnumValue = (TestEnum)99;

        [TSExclude]
        public const string Excluded = "excluded";

        public static readonly string StaticReadonly = "skipped";
    }

    [TSConstants("CustomConstants")]
    public static class TestConstantsCustomName {
        public const int Value = 1;
    }

    [TSConstants(Folder = "SubFolder")]
    public static class TestConstantsInFolder {
        public const int Value = 1;
    }

    [TSConstants]
    public class TestConstantsNonStatic {
        public const string Name = "NonStatic";

        public static readonly int StaticReadonly = 5;
        public int InstanceField = 1;
        public string InstanceProperty { get; set; } = string.Empty;
    }

    [TSConstants]
    public static class TestConstantsEmpty {
    }

    // Intentionally not marked with TSEnum
    public enum PlainEnum {
        A = 1,
    }
}
