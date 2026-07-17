namespace CSharp2TS.Core.Attributes {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TSConstantsAttribute : TSAttributeBase {
        public TSConstantsAttribute() : base(null) {
        }

        public TSConstantsAttribute(string typeName) : base(typeName) {
        }
    }
}
