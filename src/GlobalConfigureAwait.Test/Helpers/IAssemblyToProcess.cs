using System;

namespace GlobalConfigureAwait.Test.Helpers
{
    public interface IAssemblyToProcess
    {
        Type GetContextType();
        Type GetClassType(string fullName);
    }
}