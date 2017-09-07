using System.Threading.Tasks;
using GlobalConfigureAwait.Test.Helpers;
using Xunit;

namespace GlobalConfigureAwait.Test
{
    public class DoNotWeaveNetTests : DoNotWeaveTests<NetAssemblyWeaver>
    {
    }

    public class DoNotWeaveNetStandardTests : DoNotWeaveTests<NetStandardAssemblyWeaver>
    {
    }

    public abstract class DoNotWeaveTests<TAssemblyToProcess> : TestClassBase<TAssemblyToProcess>
        where TAssemblyToProcess : IAssemblyToProcess, new()
    {
        protected DoNotWeaveTests() : base("AssemblyToProcess.DoNotWeave")
        {
        }

        [Fact]
        public async Task TestAsyncMethod()
        {
            await Test.AsyncMethod(Context);

            Assert.True(Context.Flag);
        }

        [Fact]
        public async Task TestAsyncMethodWithReturn()
        {
            var result = await Test.AsyncMethodWithReturn(Context);

            Assert.True(Context.Flag);
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task TestAsyncGenericMethod()
        {
            await Test.AsyncGenericMethod(Context);
            Assert.True(Context.Flag);
        }

        [Fact]
        public async Task TestAsyncGenericMethodWithReturn()
        {
            var result = await Test.AsyncGenericMethodWithReturn(Context);

            Assert.True(Context.Flag);
            Assert.Equal(10, result);
        }
    }
}