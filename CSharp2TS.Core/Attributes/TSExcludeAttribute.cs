namespace CSharp2TS.Core.Attributes {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class TSExcludeAttribute : Attribute {
    }
}
