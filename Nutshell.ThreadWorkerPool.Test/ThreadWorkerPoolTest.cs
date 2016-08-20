using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nutshell.Test.Support;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;


namespace Nutshell.ThreadWorkerPool.Test
{
    [TestClass]
    public class ThreadWorkerPoolTest
    {
        [TestMethod]
        public void TestMinThreadWorkerCountSetting()
        {
            ThreadWorkerPoolSettings settings = ThreadWorkerPoolSettings.Default;
            settings.MinThreadWorkerCount = 10;

            using (var pool = ThreadWorkerPoolFactory.Create(() => { }, settings))
            {
                Assert.AreEqual(settings.MinThreadWorkerCount, pool.IdleThreadWorkerCount);
            }
        }

        [TestMethod]
        public void TestIdleTimeSetting()
        {
            ThreadWorkerPoolSettings settings = ThreadWorkerPoolSettings.Default;
            settings.MinThreadWorkerCount = 1;
            settings.IdleTime = 1000;

            using (var pool = ThreadWorkerPoolFactory.Create(() => { }, settings))
            {
                IThreadWorker worker1;
                IThreadWorker worker2;
                pool.TryTake(out worker1);
                pool.TryTake(out worker2);
                Assert.AreEqual(0, pool.IdleThreadWorkerCount);
                Assert.AreEqual(2, pool.TotalThreadWorkerCount);

                worker1.Work();
                worker2.Work();

                Thread.Sleep(settings.IdleTime + 10);

                Assert.AreEqual(1, pool.IdleThreadWorkerCount);
                Assert.AreEqual(1, pool.TotalThreadWorkerCount);
            }
        }

        [TestMethod]
        public void TestTryTakeTimeOut()
        {
            ThreadWorkerPoolSettings settings = ThreadWorkerPoolSettings.Default;
            settings.MinThreadWorkerCount = 0;
            settings.MaxThreadWorkerCount = 1;

            using (var pool = ThreadWorkerPoolFactory.Create(() => { }, settings))
            {

                int timeout = 1000;
                IThreadWorker worker;

                Assert.IsTrue(pool.TryTake(timeout, out worker));
                Assert.IsNotNull(worker);

                bool isTakeOne = false;
                Thread thread = new Thread(() =>
                {
                    isTakeOne = pool.TryTake(timeout, out worker);
                });
                thread.Start();

                Thread.Sleep(timeout + 10);

                Assert.IsFalse(isTakeOne);
                Assert.IsNull(worker);
            }
        }

        [TestMethod]
        public void TestWork()
        {
            ThreadWorkerPoolSettings settings = ThreadWorkerPoolSettings.Default;
            settings.MinThreadWorkerCount = 3;
            settings.MaxThreadWorkerCount = 20;
            settings.IdleTime = 1000;
            System.Collections.Concurrent.ConcurrentBag<int> result = new System.Collections.Concurrent.ConcurrentBag<int>();
            using (var pool = ThreadWorkerPoolFactory.Create<int>((data) =>
            {
                try
                {
                    result.Add(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }, settings))
            {

                ParameterizedThreadStart testWork = (obj) =>
                {
                    IThreadWorker<int> worker;
                    int value = (int)obj;
                    for (int index = 0; index < 100; index++)
                    {
                        if (pool.TryTake(out worker))
                        {
                            worker.Work(value + index);
                        }
                    }
                };
                Thread[] threadList = new Thread[3];
                threadList[0] = new Thread(testWork);
                threadList[1] = new Thread(testWork);
                threadList[2] = new Thread(testWork);
                for (int index = 0; index < threadList.Length; index++)
                {
                    threadList[index].Start(index * 100 + 1);
                }

                for (int index = 0; index < threadList.Length; index++)
                {
                    threadList[index].Join();
                }

                while (pool.IdleThreadWorkerCount != pool.TotalThreadWorkerCount)
                {
                    Thread.Sleep(100);
                }

                Assert.AreEqual(threadList.Length * 100, result.Distinct().Count());

                Thread.Sleep(pool.Settings.IdleTime + 100);
                Assert.AreEqual(pool.Settings.MinThreadWorkerCount, pool.TotalThreadWorkerCount);
            }
        }
    }
}
