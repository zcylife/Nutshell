using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nutshell.QueueWorker.Settings;
using Nutshell.ThreadWorkerPool;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Nutshell.QueueWorker.Test
{
    [TestClass]
    public class MultithreadQueueWorkerTest
    {
        [TestMethod]
        public void TestTryAdd()
        {
            MultithreadQueueWorkerSettings settings = new MultithreadQueueWorkerSettings(1, 1, 1);
            using (MultithreadQueueWorker<int> queue = new MultithreadQueueWorker<int>((data) => { }, settings))
            {
                Assert.IsTrue(queue.TryAdd(1));
                Assert.IsFalse(queue.TryAdd(2));
            }
        }

        [TestMethod]
        public void TestQueueWorking()
        {
            ConcurrentBag<int> workResult = new ConcurrentBag<int>();
            MultithreadQueueWorkerSettings settings = new MultithreadQueueWorkerSettings(100, 1, 4);
            MultithreadQueueWorker<int> queue = new MultithreadQueueWorker<int>((data) =>
            {
                Thread.Sleep(100);
                workResult.Add(data);
            }, settings);

            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    Thread.Sleep(50);
                    Console.WriteLine(i.ToString()+queue.TryAdd(i));
                }
            }).ContinueWith(task => { throw task.Exception; }, TaskContinuationOptions.OnlyOnFaulted);

            Task.Factory.StartNew(() =>
            {
                for (int i = 50; i < 100; i++)
                {
                    Thread.Sleep(10);
                    Console.WriteLine(i.ToString()+queue.TryAdd(i));
                }
            }).ContinueWith(task => { throw task.Exception; }, TaskContinuationOptions.OnlyOnFaulted);

            queue.Start();

            int index = 0;
            while (workResult.Count != 100)
            {
                if (index == 100)
                    break;
                Thread.Sleep(200);
                index++;
            }
            Assert.AreEqual(100, workResult.Count());
            queue.Dispose();
        }
    }
}
