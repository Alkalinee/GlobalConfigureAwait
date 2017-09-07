using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GlobalConfigureAwait.Test.Helpers;
using Xunit;

namespace GlobalConfigureAwait.Test
{
    public class IssueTestsNetTests : IssueTests<NetAssemblyWeaver>
    {
    }

    public class IssueTestsNetStandardTests : IssueTests<NetStandardAssemblyWeaver>
    {
    }

    public abstract class IssueTests<TAssemblyToProcess> where TAssemblyToProcess : IAssemblyToProcess, new()
    {
        private readonly IAssemblyToProcess _assemblyToProcess;

        protected IssueTests()
        {
            _assemblyToProcess = new TAssemblyToProcess();
        }

        [Fact]
        public async Task TestIssue1WithReaderAndWriter()
        {
            dynamic context = Activator.CreateInstance(_assemblyToProcess.GetContextType());
            dynamic test = Activator.CreateInstance(_assemblyToProcess.GetClassType("AssemblyToProcess.Issue1"));

            var testString = "HELLO\r\nWORLD\r\nTHIS\r\n IS AWESOME\r\nGuid: {B693D5EA-F185-461D-ADB7-57F106D5378A}";
            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(testString)))
            using (var outputStream = new MemoryStream())
            using (var textReader = new StreamReader(memoryStream))
            using (var textWriter = new StreamWriter(outputStream))
            {
                await test.WithReaderAndWriter(context, textWriter, textReader);
                textWriter.Flush();
                Assert.Equal(testString, Encoding.UTF8.GetString(outputStream.ToArray()).TrimEnd());
            }

            Assert.False(context.Flag);
        }

        [Fact]
        public async Task TestGenericIssueClass()
        {
            var classType = _assemblyToProcess.GetClassType("AssemblyToProcess.GenericIssueClass`1");
            dynamic test = Activator.CreateInstance(classType.MakeGenericType(typeof(long)));
            var task = Task.Run(async () =>
            {
                await Task.Delay(100);
                return 100L;
            });

            await test.Method(task);
            Assert.True(task.IsCompleted);
        }

        [Fact]
        public async Task TestGenericIssueMethod()
        {
            dynamic test =
                Activator.CreateInstance(_assemblyToProcess.GetClassType("AssemblyToProcess.GenericIssueMethod"));
            var task = Task.Run(async () =>
            {
                await Task.Delay(100);
                return 100L;
            });

            await test.Method<long>(task);
            Assert.True(task.IsCompleted);
        }
    }
}