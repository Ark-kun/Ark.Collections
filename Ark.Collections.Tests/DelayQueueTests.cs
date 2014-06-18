using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ark.Collections.Tests {
    [TestClass]
    public class DelayQueueTests {
        [TestMethod]
        public void TestDelayQueue() {
            var queue = new DelayQueue<int>();
            queue.Enqueue(1, 1);
            queue.Enqueue(2, 2);
            queue.Enqueue(4, 4);
            queue.Enqueue(8, 8);
            queue.Enqueue(16, 16);
            queue.Enqueue(11, 1);

            //0
            Assert.IsFalse(queue.CurrentList.Any());

            //1
            queue.SwitchToNextList();
            Assert.IsTrue(Enumerable.SequenceEqual(queue.CurrentList, new int[] { 1, 11 }));

            //2
            queue.SwitchToNextList();
            Assert.IsTrue(queue.CurrentList.Single() == 2);

            //3
            queue.SwitchToNextList();
            Assert.IsFalse(queue.CurrentList.Any());

            //4
            queue.SwitchToNextList();
            Assert.IsTrue(queue.CurrentList.Single() == 4);

            for (int i = 5; i < 8; i++) {
                queue.SwitchToNextList();
                Assert.IsTrue(!queue.CurrentList.Any());
            }

            //8
            queue.SwitchToNextList();
            Assert.IsTrue(queue.CurrentList.Single() == 8);
            queue.Enqueue(88, 8);

            for (int i = 9; i < 16; i++) {
                queue.SwitchToNextList();
                Assert.IsFalse(queue.CurrentList.Any());
            }

            //16
            queue.SwitchToNextList();
            Assert.IsTrue(Enumerable.SequenceEqual(queue.CurrentList, new int[] { 16, 88 }));

            //for (long i = 0; i <= int.MaxValue; i++) {
            for (long i = 0; i < 100; i++) {
                queue.SwitchToNextList();
                Assert.IsFalse(queue.CurrentList.Any());
            }
        }
    }
}
