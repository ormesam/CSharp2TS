using CSharp2TS.CLI;
using CSharp2TS.CLI.Generators.Common;
using CSharp2TS.CLI.Generators.TSConstants;
using CSharp2TS.CLI.Utility;
using CSharp2TS.Tests.Generators;
using CSharp2TS.Tests.Stubs.Constants;
using CSharp2TS.Tests.Stubs.Enums;
using Mono.Cecil;

namespace CSharp2TS.Tests.Constants {
    public class TSConstantsGeneratorTest : GeneratorTestBase {
        private ModuleDefinition module = null!;
        private TSConstantsGenerator generator = null!;
        private TSConstantsGenerator pascalCaseGenerator = null!;
        private Dictionary<string, TSFileInfo> files = null!;
        private Options options = null!;

        [SetUp]
        public void Setup() {
            // Load the test assembly to get TypeReferences
            string assemblyPath = typeof(TSConstantsGeneratorTest).Assembly.Location;
            module = ModuleDefinition.ReadModule(assemblyPath);

            options = new Options();

            // Setup files dictionary - needed for import resolution
            files = [];

            AddType(typeof(TestConstants));
            AddType(typeof(TestConstantsCustomName));
            AddType(typeof(TestConstantsNonStatic));
            AddType(typeof(TestConstantsEmpty));
            AddType(typeof(TestEnum));

            generator = new(files, options);
            pascalCaseGenerator = new(files, new Options { MemberNameCasingStyle = CasingStyle.PascalCase });
        }

        private void AddType(Type type) {
            var typeRef = module.ImportReference(type);

            files[type.FullName!] = NameUtility.GetFileDetails(typeRef.Resolve(), options, "Models");
        }

        [TearDown]
        public void TearDown() {
            module?.Dispose();
        }

        [Test]
        public void ConstantsGenerator_AllSupportedValueTypes() {
            var typeRef = module.ImportReference(typeof(TestConstants));

            string result = generator.Generate(typeRef.Resolve());

            TestMatchesFile("Expected/TestConstants.ts", result);
        }

        [Test]
        public void ConstantsGenerator_PascalCaseMembers() {
            var typeRef = module.ImportReference(typeof(TestConstants));

            string result = pascalCaseGenerator.Generate(typeRef.Resolve());

            TestMatchesFile("Expected/TestConstantsPascalCase.ts", result);
        }

        [Test]
        public void ConstantsGenerator_CustomTypeName() {
            var typeRef = module.ImportReference(typeof(TestConstantsCustomName));

            string result = generator.Generate(typeRef.Resolve());

            TestMatchesFile("Expected/TestConstantsCustomName.ts", result);
        }

        [Test]
        public void ConstantsGenerator_NonStaticClass_OnlyConstsEmitted() {
            var typeRef = module.ImportReference(typeof(TestConstantsNonStatic));

            string result = generator.Generate(typeRef.Resolve());

            TestMatchesFile("Expected/TestConstantsNonStatic.ts", result);
        }

        [Test]
        public void ConstantsGenerator_EmptyClass() {
            var typeRef = module.ImportReference(typeof(TestConstantsEmpty));

            string result = generator.Generate(typeRef.Resolve());

            TestMatchesFile("Expected/TestConstantsEmpty.ts", result);
        }

        [Test]
        public void ConstantsGenerator_FolderAttribute_SetsFileDetails() {
            var typeDef = module.ImportReference(typeof(TestConstantsInFolder)).Resolve();

            var result = NameUtility.GetFileDetails(typeDef, new Options(), "Models");

            Assert.That(result.Folder, Is.EqualTo("Models/SubFolder"));
            Assert.That(result.TypeName, Is.EqualTo(nameof(TestConstantsInFolder)));
        }
    }
}
