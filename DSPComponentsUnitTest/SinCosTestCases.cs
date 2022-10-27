using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DSPAlgorithms.Algorithms;
using DSPAlgorithms.DataStructures;

namespace DSPComponentsUnitTest
{
    [TestClass]
    public class SinCosTestCases
    {
        [TestMethod]
        public void SinCosTestMethod1()
        {
            SinCos obj= new SinCos();
            obj.type = "sin";
            obj.A=3;
            obj.AnalogFrequency=360;
            obj.SamplingFrequency=720;
            obj.PhaseShift = 1.96349540849362f;

            obj.Run();

            var expectedOutput = UnitTestUtitlities.LoadSignal("TestingSignals/Sin.ds");

            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.Samples, obj.samples));

        }
        [TestMethod]
        public void SinCosTestMethod2()
        {
            SinCos obj = new SinCos();
            obj.type = "cos";
            obj.A = 3;
            obj.AnalogFrequency = 200;
            obj.SamplingFrequency = 500;
            obj.PhaseShift = 2.35619449019235f;

            obj.Run();
            var expectedOutput = UnitTestUtitlities.LoadSignal("TestingSignals/Cos.ds");
            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.Samples, obj.samples));


        }
    }
}
