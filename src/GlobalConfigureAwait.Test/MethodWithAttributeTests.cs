using System.Threading.Tasks;
using GlobalConfigureAwait.Test.Helpers;
using Xunit;

namespace GlobalConfigureAwait.Test
{
    public class MethodWithAttributeNetTests : MethodWithAttributeTests<NetAssemblyWeaver>
    {
    }

    public class MethodWithAttributeNetStandardTests : MethodWithAttributeTests<NetStandardAssemblyWeaver>
    {
    }

    public abstract class MethodWithAttributeTests<TAssemblyToProcess> : TestClassBase<TAssemblyToProcess>
        where TAssemblyToProcess : IAssemblyToProcess, new()
    {
        protected MethodWithAttributeTests() : base("AssemblyToProcess.MethodWithAttribute")
        {
        }

        [Fact]
        public async Task TestAsyncMethod()
        {
            await Test.AsyncMethod(Context);

            Assert.False(Context.Flag);
        }
    }
}