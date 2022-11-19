using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DSPAlgorithms.Algorithms;
using DSPAlgorithms.DataStructures;

namespace DSPComponentsUnitTest
{
    [TestClass]
    public class AccumlatorSumTestCases
    {
        [TestMethod]
        public void AccumlatorSumTestMethod1()
        {
            var expectedOutput = new Signal(new List<float>() { 1, 3, 6, 10, 15, 21, 28 }, false);
            List<float> Samples = new List<float> { 1, 2, 3, 4, 5, 6, 7 };
            AccumulationSum AS = new AccumulationSum();
            AS.InputSignal = new Signal(Samples, false);
            AS.Run();
            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.Samples, AS.OutputSignal.Samples));
        }
    }
}
