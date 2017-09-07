using System;

namespace GlobalConfigureAwait.Test.Helpers
{
    public abstract class TestClassBase<TAssemblyToProcess> where TAssemblyToProcess : IAssemblyToProcess, new()
    {
        protected readonly dynamic Context;
        protected readonly dynamic Test;

        protected TestClassBase(string className)
        {
            var assemblyToProcess = new TAssemblyToProcess();
            Context = Activator.CreateInstance(assemblyToProcess.GetContextType());
            Test = Activator.CreateInstance(assemblyToProcess.GetClassType(className));
        }
    }
}