using System.Threading.Tasks;
using GlobalConfigureAwait.Test.Helpers;
using Xunit;

namespace GlobalConfigureAwait.Test
{
    public class ExampleTestsNetTests : ExampleTestsTests<NetAssemblyWeaver>
    {
    }

    public class ExampleTestsNetStandardTests : ExampleTestsTests<NetStandardAssemblyWeaver>
    {
    }

    public abstract class ExampleTestsTests<TAssemblyToProcess> : TestClassBase<TAssemblyToProcess>
        where TAssemblyToProcess : IAssemblyToProcess, new()
    {
        protected ExampleTestsTests() : base("AssemblyToProcess.Example")
        {
        }

        [Fact]
        public async Task TestAsyncMethod1()
        {
            await Test.AsyncMethod1(Context);
            Assert.True(Context.Flag);
        }

        [Fact]
        public async Task TestAsyncMethod10()
        {
            var result = await Test.AsyncMethod10(Context);
            Assert.False(Context.Flag);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestAsyncMethod11()
        {
            var result = await Test.AsyncMethod11(Context);
            Assert.False(Context.Flag);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestAsyncMethod12()
        {
            var result = await Test.AsyncMethod12(Context);
            Assert.False(Context.Flag);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestAsyncMethod2()
        {
            await Test.AsyncMethod2(Context);
            Assert.False(Context.Flag);
        }

        [Fact]
        public async Task TestAsyncMethod3()
        {
            await Test.AsyncMethod3(Context);
            Assert.False(Context.Flag);
        }

        [Fact]
        public async Task TestAsyncMethod4()
        {
            var result = await Test.AsyncMethod4(Context);
            Assert.False(Context.Flag);
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task TestAsyncMethod5()
        {
            var result = await Test.AsyncMethod5(Context);
            Assert.False(Context.Flag);
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task TestAsyncMethod6()
        {
            var result = await Test.AsyncMethod6(Context);
            Assert.False(Context.Flag);
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task TestAsyncMethod7()
        {
            var result = await Test.AsyncMethod7(Context);
            Assert.True(Context.Flag);
            Assert.Equal(45, result);
        }

        [Fact]
        public async Task TestAsyncMethod8()
        {
            var result = await Test.AsyncMethod8(Context);
            Assert.False(Context.Flag);
            Assert.Equal(45, result);
        }

        [Fact]
        public async Task TestAsyncMethod9()
        {
            var result = await Test.AsyncMethod9(Context);
            Assert.False(Context.Flag);
            Assert.Equal(45, result);
        }
    }
}