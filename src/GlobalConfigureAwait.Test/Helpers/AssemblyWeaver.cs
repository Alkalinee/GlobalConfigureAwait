using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using Xunit;

namespace GlobalConfigureAwait.Test.Helpers
{
    public abstract class AssemblyWeaver : IAssemblyToProcess
    {
        private readonly Assembly _assembly;

        protected AssemblyWeaver(string assemblyName)
        {
            var fileInfo = new FileInfo(assemblyName);
            Assert.True(fileInfo.Exists, $"File '{fileInfo.FullName}' does not exist.");

            using (var moduleDefinition = ModuleDefinition.ReadModule(fileInfo.FullName))
            using (var defaultAssemblyResolver = new DefaultAssemblyResolver())
            {
                var weavingTask = new ModuleWeaver
                {
                    ModuleDefinition = moduleDefinition,
                    AssemblyResolver = defaultAssemblyResolver,
                    LogInfo = LogInfo,
                    LogWarning = LogWarning,
                    LogError = LogError,
                    DefineConstants = new List<string> {"DEBUG"} // Always testing the debug weaver
                };

                weavingTask.Execute();
                using (var memoryStream = new MemoryStream())
                {
                    moduleDefinition.Write(memoryStream);
                    _assembly = Assembly.Load(memoryStream.ToArray());
                }
            }
        }

        public List<string> Infos = new List<string>();
        public List<string> Warnings = new List<string>();
        public List<string> Errors = new List<string>();

        public Type GetClassType(string fullName)
        {
            return _assembly.GetType(fullName);
        }

        public Type GetContextType()
        {
            return _assembly.GetType("AssemblyToProcess.FlagSyncronizationContext");
        }

        private void LogInfo(string message)
        {
            Infos.Add(message);
        }

        private void LogWarning(string message)
        {
            Warnings.Add(message);
        }

        private void LogError(string message)
        {
            Errors.Add(message);
        }
    }
}