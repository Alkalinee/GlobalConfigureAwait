using System;
using System.Threading;
using System.Threading.Tasks;
using Fody;

namespace AssemblyToProcess
{
    public class CatchAndFinally
    {
        public async Task Catch1(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                await Task.Delay(1);
            }
        }

        [ConfigureAwait(false)]
        public async Task Catch2(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                await Task.Delay(1);
            }
        }

        public async Task Catch3(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                await Task.Delay(1).ConfigureAwait(false);
            }
        }

        public async Task Finally1(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                await Task.Delay(1);
            }
        }

        [ConfigureAwait(false)]
        public async Task Finally2(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                await Task.Delay(1);
            }
        }

        public async Task Finally3(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                await Task.Delay(1).ConfigureAwait(false);
            }
        }
    }
}