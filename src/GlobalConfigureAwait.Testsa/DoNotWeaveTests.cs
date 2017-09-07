using System;
using System.Threading.Tasks;
using Xunit;

namespace GlobalConfigureAwait.Tests
{
    public class DoNotWeaveTests<TAssemblyToProcess> where TAssemblyToProcess : IAssemblyToProcess, new()
    {
        private readonly dynamic _context;
        private readonly dynamic _test;

        public DoNotWeaveTests()
        {
            var assemblyToProcess = new TAssemblyToProcess();
            _context = Activator.CreateInstance(assemblyToProcess.GetContextType());
            _test = Activator.CreateInstance(assemblyToProcess.GetClassType());
        }

        [Fact]
        public async Task TestAsyncMethod()
        {
            Assert.False(_context.Flag);

            await _test.AsyncMethod(_context);

            Assert.False(_context.Flag);
        }
    }

    public interface IAssemblyToProcess
    {
        Type GetContextType();
        Type GetClassType();
    }
}