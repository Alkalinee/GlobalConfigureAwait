using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Fody;

namespace AssemblyToProcess
{
    public class Issue1
    {
        [ConfigureAwait(false)]
        public async Task WithReaderAndWriter(SynchronizationContext context, TextWriter writer, TextReader reader)
        {
            SynchronizationContext.SetSynchronizationContext(context);

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
                await writer.WriteLineAsync(line);
        }
    }

    public sealed class GenericIssueClass<TItem>
    {
        [ConfigureAwait(false)]
        public async Task Method(Task<TItem> itemTask)
        {
            var item = await itemTask;
        }
    }

    public sealed class GenericIssueMethod
    {
        [ConfigureAwait(false)]
        public async Task Method<TItem>(Task<TItem> itemTask)
        {
            var item = await itemTask;
        }
    }
}