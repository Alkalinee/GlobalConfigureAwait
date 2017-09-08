using System.Linq;
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
                types.First(x => x.Name == "Task").Methods.First(x => x.Name == "ConfigureAwait");

            GenericTaskDefinition = types.First(x => x.Name == "Task`1");
            GenericTaskConfigureAwaitMethodDefinition =
                GenericTaskDefinition.Methods.First(x => x.Name == "ConfigureAwait");
        }

        public TypeDefinition ConfiguredTaskAwaitableDefinition { get; }
        public TypeDefinition ConfiguredTaskAwaiterDefinition { get; }

        public TypeDefinition GenericConfiguredTaskAwaitableDefinition { get; }
        public TypeDefinition GenericConfiguredTaskAwaiterDefinition { get; }

        public MethodDefinition TaskConfigureAwaitMethodDefinition { get; }
        public TypeDefinition GenericTaskDefinition { get; }
        public MethodReference GenericTaskConfigureAwaitMethodDefinition { get; }
    }

    public class TypeReferenceProvider
    {
        public TypeReferenceProvider(ModuleDefinition moduleDefinition, TypeProvider typeProvider)
        {
            ConfiguredTaskAwaitableReference =
                moduleDefinition.ImportReference(typeProvider.ConfiguredTaskAwaitableDefinition);
            ConfiguredTaskAwaiterReference =
                moduleDefinition.ImportReference(typeProvider.ConfiguredTaskAwaiterDefinition);

            GenericConfiguredTaskAwaitableReference =
                moduleDefinition.ImportReference(typeProvider.GenericConfiguredTaskAwaitableDefinition);
            GenericConfiguredTaskAwaiterReference =
                moduleDefinition.ImportReference(typeProvider.GenericConfiguredTaskAwaiterDefinition);

            TaskConfigureAwaitMethodReference =
                moduleDefinition.ImportReference(typeProvider.TaskConfigureAwaitMethodDefinition);
            GenericTaskReference = moduleDefinition.ImportReference(typeProvider.GenericTaskDefinition);
            GenericTaskConfigureAwaitMethodReference =
                moduleDefinition.ImportReference(typeProvider.GenericTaskConfigureAwaitMethodDefinition);
        }

        public TypeReference ConfiguredTaskAwaitableReference { get; }
        public TypeReference ConfiguredTaskAwaiterReference { get; }

        public TypeReference GenericConfiguredTaskAwaitableReference { get; }
        public TypeReference GenericConfiguredTaskAwaiterReference { get; }

        public MethodReference TaskConfigureAwaitMethodReference { get; set; }
        public TypeReference GenericTaskReference { get; set; }
        public MethodReference GenericTaskConfigureAwaitMethodReference { get; set; }
    }
}