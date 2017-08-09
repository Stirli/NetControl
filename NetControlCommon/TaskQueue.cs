using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetControlCommon
{
    public class TaskQueue
    {
        private SemaphoreSlim semaphore;
        public TaskQueue(int concurrency)
        {
            semaphore = new SemaphoreSlim(concurrency);
        }

        public async Task<T> Enqueue<T>(Func<Task<T>> taskGenerator)
        {
            await semaphore.WaitAsync();
            try
            {
                return await taskGenerator();
            }
            finally
            {
                semaphore.Release();
            }
        }
        public async Task Enqueue(Func<Task> taskGenerator)
        {
            await semaphore.WaitAsync();
            try
            {
                await taskGenerator();
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
