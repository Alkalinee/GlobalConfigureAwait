using System;
using System.Threading.Tasks;
using GlobalConfigureAwait.Test.Helpers;
using Xunit;

namespace GlobalConfigureAwait.Test
{
    public class CatchAndFinallyTestsNetTests : CatchAndFinallyTestsTests<NetAssemblyWeaver>
    {
    }

    public class CatchAndFinallyTestsNetStandardTests : CatchAndFinallyTestsTests<NetStandardAssemblyWeaver>
    {
    }

    public abstract class CatchAndFinallyTestsTests<TAssemblyToProcess> : TestClassBase<TAssemblyToProcess>
        where TAssemblyToProcess : IAssemblyToProcess, new()
    {
        protected CatchAndFinallyTestsTests() : base("AssemblyToProcess.CatchAndFinally")
        {
        }

        [Fact]
        public async Task TestCatch1()
        {
            await Test.Catch1(Context);

            Assert.True(Context.Flag);
        }

        [Fact]
        public async Task TestCatch2()
        {
            await Test.Catch2(Context);

            Assert.False(Context.Flag);
        }

        [Fact]
        public async Task TestCatch3()
        {
            await Test.Catch3(Context);

            Assert.False(Context.Flag);
        }

        [Fact]
        public async Task TestFinally1()
        {
            await Assert.ThrowsAsync<NotImplementedException>(() => Test.Finally1(Context));
            Assert.True(Context.Flag);
        }

        [Fact]
        public async Task TestFinally2()
        {
            await Assert.ThrowsAsync<NotImplementedException>(() => Test.Finally2(Context));
            Assert.False(Context.Flag);
        }

        [Fact]
        public async Task TestFinally3()
        {
            await Assert.ThrowsAsync<NotImplementedException>(() => Test.Finally3(Context));
            Assert.False(Context.Flag);
        }
    }
}