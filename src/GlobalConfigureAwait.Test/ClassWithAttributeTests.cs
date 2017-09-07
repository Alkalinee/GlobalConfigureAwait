using System.Threading.Tasks;
using GlobalConfigureAwait.Test.Helpers;
using Xunit;

namespace GlobalConfigureAwait.Test
{
    public class ClassWithAttributeNetTests : ClassWithAttributeTests<NetAssemblyWeaver>
    {
    }

    public class ClassWithAttributeNetStandardTests : ClassWithAttributeTests<NetStandardAssemblyWeaver>
    {
    }

    public abstract class ClassWithAttributeTests<TAssemblyToProcess> : TestClassBase<TAssemblyToProcess>
        where TAssemblyToProcess : IAssemblyToProcess, new()
    {
        protected ClassWithAttributeTests() : base("AssemblyToProcess.ClassWithAttribute")
        {
        }

        [Fact]
        public async Task TestAsyncMethod()
        {
            await Test.AsyncMethod(Context);

            Assert.False(Context.Flag);
        }

        [Fact]
        public async Task TestAsyncMethodWithReturn()
        {
            var result = await Test.AsyncMethodWithReturn(Context);

            Assert.False(Context.Flag);
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task TestAsyncGenericMethod()
        {
            await Test.AsyncGenericMethod(Context);

            Assert.False(Context.Flag);
        }

        [Fact]
        public async Task TestAsyncGenericMethodWithReturn()
        {
            var result = await Test.AsyncGenericMethodWithReturn(Context);

            Assert.False(Context.Flag);
            Assert.Equal(10, result);
        }
    }
}