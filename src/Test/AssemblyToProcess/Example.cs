using System.Threading;
using System.Threading.Tasks;
using Fody;

namespace AssemblyToProcess
{
    public class Example
    {
        public async Task AsyncMethod1(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            await Task.Delay(1);
        }

        [ConfigureAwait(false)]
        public async Task AsyncMethod2(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            await Task.Delay(1);
        }

        public async Task AsyncMethod3(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            await Task.Delay(1).ConfigureAwait(false);
        }

        public async Task<int> AsyncMethod4(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            var result = await Task.FromResult(10);
            return result;
        }

        [ConfigureAwait(false)]
        public async Task<int> AsyncMethod5(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            var result = await Task.FromResult(10);
            return result;
        }

        public async Task<int> AsyncMethod6(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            var result = await Task.FromResult(10).ConfigureAwait(false);
            return result;
        }

        public async Task<int> AsyncMethod7(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            var count = await Task.FromResult(10);
            var sum = 0;
            for (int i = 0; i < count; i++)
            {
                await Task.Delay(1);
                sum += await Task.FromResult(i);
            }
            return sum;
        }

        [ConfigureAwait(false)]
        public async Task<int> AsyncMethod8(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            var count = await Task.FromResult(10);
            var sum = 0;
            for (int i = 0; i < count; i++)
            {
                await Task.Delay(1);
                sum += await Task.FromResult(i);
            }
            return sum;
        }

        public async Task<int> AsyncMethod9(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            var count = await Task.FromResult(10).ConfigureAwait(false);
            var sum = 0;
            for (int i = 0; i < count; i++)
            {
                await Task.Delay(1).ConfigureAwait(false);
                sum += await Task.FromResult(i).ConfigureAwait(false);
            }
            return sum;
        }

        public async Task<Example> AsyncMethod10(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            var result = await Task.FromResult(new Example());
            return result;
        }

        [ConfigureAwait(false)]
        public async Task<Example> AsyncMethod11(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            var result = await Task.FromResult(new Example());
            return result;
        }

        public async Task<Example> AsyncMethod12(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            var result = await Task.FromResult(new Example()).ConfigureAwait(false);
            return result;
        }
    }
}