using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using Xunit;

namespace GlobalConfigureAwait.Tests
{
    public class AssemblyWeaver
    {
        public AssemblyWeaver(string assemblyName)
        {
            var fileInfo = new FileInfo(assemblyName);
            Assert.True(fileInfo.Exists, $"File '{fileInfo.FullName}' does not exist.");

            using (var moduleDefinition = ModuleDefinition.ReadModule(BeforeAssemblyPath, readerParameters))
            using (var defaultAssemblyResolver = new DefaultAssemblyResolver())
            {
                var weavingTask = new ModuleWeaver
                {
                    ModuleDefinition = moduleDefinition,
                    AssemblyResolver = defaultAssemblyResolver,
                    LogInfo = LogInfo,
                    LogWarning = LogWarning,
                    LogError = LogError,
                    DefineConstants = new[] { "DEBUG" } // Always testing the debug weaver
                };

                weavingTask.Execute();
                moduleDefinition.Write(AfterAssemblyPath, writerParameters);
            }
        }
    }
}