using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Mono.Cecil;

namespace GlobalConfigureAwait.Extensions
{
	public class TypeProvider
	{
		public TypeProvider(IAssemblyResolver assemblyResolver)
		{
			var types = new List<TypeDefinition>(1000);

			var runtime = assemblyResolver.Resolve(new AssemblyNameReference("System.Runtime", null)); //.Net Core 2.0
			if (runtime != null)
				types.AddRange(runtime.MainModule.Types);

			var mscorlib = assemblyResolver.Resolve(new AssemblyNameReference("mscorlib", null)); //.Net Framework
			if (mscorlib != null)
				types.AddRange(mscorlib.MainModule.Types);

			var threadingTasks =
				assemblyResolver.Resolve(new AssemblyNameReference("System.Threading.Tasks", null)); //.Net Core 1.1/.Net Standard
			if (threadingTasks != null)
				types.AddRange(threadingTasks.MainModule.Types);

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