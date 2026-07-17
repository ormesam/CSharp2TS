using CSharp2TS.CLI.Generators.Entities;

namespace CSharp2TS.CLI.Generators.TSConstants {
    public class TSConstants {
        public string Name { get; private set; }
        public IList<TSConstantValue> Values { get; private set; } = [];
        public IList<TSImport> Imports { get; private set; } = [];

        public TSConstants(string name) {
            Name = name;
        }
    }
}
