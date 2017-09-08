using System.Linq;
using System.Runtime.CompilerServices;
using Mono.Cecil;

namespace GlobalConfigureAwait.Extensions
{
    public class TypeProvider
    {
        public TypeProvider(IAssemblyResolver assemblyResolver)
        {
            var tasksAssembly = assemblyResolver.Resolve(new AssemblyNameReference("mscorlib", null));
            if (tasksAssembly == null)
            {
                tasksAssembly = assemblyResolver.Resolve(new AssemblyNameReference("System.Threading.Tasks", null));
                if (tasksAssembly == null)
                    throw new WeavingException(
                        "Assemblies 'System.Threading.Tasks' and 'mscorlib' were not found. Resolving failed.");
            }

            var types = tasksAssembly.MainModule.Types;

            ConfiguredTaskAwaitableDefinition = types.First(x => x.Name == "ConfiguredTaskAwaitable");
            ConfiguredTaskAwaiterDefinition = ConfiguredTaskAwaitableDefinition.NestedTypes[0];

            GenericConfiguredTaskAwaitableDefinition = types.First(x => x.Name == "ConfiguredTaskAwaitable`1");
            GenericConfiguredTaskAwaiterDefinition = GenericConfiguredTaskAwaitableDefinition.NestedTypes[0];

            TaskConfigureAwaitMethodDefinition =
                types.First(x => x.FullName == "System.Threading.Tasks.Task").Methods
                    .First(x => x.Name == "ConfigureAwait");

            GenericTaskDefinition = types.First(x => x.FullName == "System.Threading.Tasks.Task`1");
            GenericTaskConfigureAwaitMethodDefinition =
                GenericTaskDefinition.Methods.First(x => x.Name == "ConfigureAwait");

            //in case the nested types change
            Assert.IsType(ConfiguredTaskAwaiterDefinition, typeof(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter));
            Assert.IsType(GenericConfiguredTaskAwaiterDefinition,
                typeof(ConfiguredTaskAwaitable<>.ConfiguredTaskAwaiter));
        }

        public TypeDefinition ConfiguredTaskAwaitableDefinition { get; }
        public TypeDefinition ConfiguredTaskAwaiterDefinition { get; }

        public TypeDefinition GenericConfiguredTaskAwaitableDefinition { get; }
        public TypeDefinition GenericConfiguredTaskAwaiterDefinition { get; }

        public MethodDefinition TaskConfigureAwaitMethodDefinition { get; }
        public TypeDefinition GenericTaskDefinition { get; }
        public MethodReference GenericTaskConfigureAwaitMethodDefinition { get; }
    }
}