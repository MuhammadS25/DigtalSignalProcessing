using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSPAlgorithms.Algorithms;
using System.Collections.Generic;

namespace DSPComponentsUnitTest
{
    [TestClass]
    public class DFTTestCases
    {
        [TestMethod]
        public void DFTTestMethod1()
        {
            DiscreteFourierTransform DFT = new DiscreteFourierTransform();
            // test case 2
            List<float> Samples = new List<float> { 1, 3, 5, 7, 9, 11, 13, 15 };
            DFT.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(Samples, false);
            DFT.InputSamplingFrequency = 4;

            var FrequenciesAmplitudes = new List<float> { 64, 20.9050074380220f, 11.3137084989848f, 8.65913760233915f, 8, 8.65913760233915f, 11.3137084989848f, 20.9050074380220f };
            var FrequenciesPhaseShifts = new List<float> { 0, 1.96349540849362f, 2.35619449019235f, 2.74889357189107f, -3.14159265358979f, -2.74889357189107f, -2.35619449019235f, -1.96349540849362f };
            var Frequencies = new List<float> { 0, 1, 2, 3, 4, 5, 6, 7 };

            DFT.Run();

            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(FrequenciesAmplitudes, DFT.OutputFreqDomainSignal.FrequenciesAmplitudes)
                && UnitTestUtitlities.SignalsPhaseShiftsAreEqual(FrequenciesPhaseShifts, DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts));

        }
    }
}
