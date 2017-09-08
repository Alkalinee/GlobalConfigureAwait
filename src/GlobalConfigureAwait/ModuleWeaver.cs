using System;
using System.Linq;
using GlobalConfigureAwait.Extensions;
using GlobalConfigureAwait.Settings;
using Mono.Cecil;

#if !DEBUG
    using System.Threading.Tasks;

#endif

namespace GlobalConfigureAwait
{
    /// <summary>
    /// The entry point of this addin, invoked by fody
    /// </summary>
    public class ModuleWeaver
    {
        // Will log an MessageImportance.High message to MSBuild.
        public Action<string> LogInfo { get; set; }

        // Will log an warning message to MSBuild.
        public Action<string> LogWarning { get; set; }

        // Will log an error message to MSBuild.
        public Action<string> LogError { get; set; }

        // An instance of Mono.Cecil.ModuleDefinition for processing.
        public ModuleDefinition ModuleDefinition { get; set; }

        // An instance of Mono.Cecil.IAssemblyResolver for resolving assembly references.
        public IAssemblyResolver AssemblyResolver { get; set; }

        // A copy of the contents of the $(DefineConstants).
        public string[] DefineConstants { get; set; }

        public void Execute()
        {
            var assemblyLevelSettings = new AssemblyLevelSettings(ModuleDefinition.Assembly);
            var types = ModuleDefinition.GetTypes().ToList();

            var typeProvider = new TypeProvider(AssemblyResolver);
            var typeReferenceProvider = new TypeReferenceProvider(ModuleDefinition, typeProvider);
            LogInfo("Done");

            var asyncIlHelper = new AsyncIlHelper(typeProvider, typeReferenceProvider, ModuleDefinition);

            //the performance improvement with parallel execution will be non existent below 15 types
#if !DEBUG
            if (types.Count > 15)
                Parallel.ForEach(types,
                    typeDefinition => ProcessType(assemblyLevelSettings, typeDefinition, asyncIlHelper));
            else
#endif

            foreach (var typeDefinition in types)
                    ProcessType(assemblyLevelSettings, typeDefinition, asyncIlHelper);

            RemoveReference();
        }

        private void ProcessType(AssemblyLevelSettings assemblySettings, TypeDefinition type, AsyncIlHelper asyncIlHelper)
        {
            if (!type.HasMethods || type.IsCompilerGenerated() && type.IsIAsyncStateMachine())
                return;

            var typeSettings = new TypeLevelSettings(type, assemblySettings);
            foreach (var method in type.Methods)
            {
                var methodSettings = new MethodLevelSettings(method, typeSettings);
                var configureAwait = methodSettings.GetConfigureAwait();
                if (configureAwait == null)
                    continue;

                var asyncStateMachineType = method.GetAsyncStateMachineType();
                if (asyncStateMachineType == null)
                {
                    if (methodSettings.MethodConfigureAwait.HasValue)
                        LogWarning($"ConfigureAwaitAttribue applied to non-async method '{method.FullName}'.");
                    continue;
                }

                asyncIlHelper.AddAwaitConfigToAsyncMethod(asyncStateMachineType, configureAwait.Value);
            }
        }

        private void RemoveReference()
        {
            var publicKeyToken = new byte[] {149, 192, 193, 107, 11, 246, 153, 79};
            var referenceToRemove = ModuleDefinition.AssemblyReferences.FirstOrDefault(x => x.PublicKeyToken.SequenceEqual(publicKeyToken));

            if (referenceToRemove == null)
            {
                LogInfo("\tNo reference to 'GlobalConfigureAwait.dll' found. References not modified.");
                return;
            }

            ModuleDefinition.AssemblyReferences.Remove(referenceToRemove);
            LogInfo("\tRemoving reference to 'ConfigureAwait.dll'.");
        }
    }

    public class WeavingException : Exception
    {
        public WeavingException(string message) : base(message)
        {
        }
    }
}