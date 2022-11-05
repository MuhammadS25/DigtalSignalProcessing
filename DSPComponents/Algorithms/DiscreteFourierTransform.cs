using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public List<Complex> ComputeHarmonics(Complex reverse, Signal S)
        {
            List<Complex> harmonics = new List<Complex>(InputTimeDomainSignal.Samples.Count);
            for (int k = 0; k < S.Samples.Count; ++k)
            {
                harmonics.Add(new Complex(0, 0));
                for (int n = 0; n < S.Samples.Count; ++n)
                {
                    harmonics[k] +=
                        S.Samples[n] * Complex.Pow
                        (Math.E, reverse * k * 2 * Math.PI * n / InputTimeDomainSignal.Samples.Count);
                }
            }
            return harmonics;
        }
        public override void Run()
        {
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();

            var harmonics = ComputeHarmonics(new Complex(0, -1), InputTimeDomainSignal);

            for (int i = 0; i < harmonics.Count; ++i)
            {
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add
                    ((float)(Math.Sqrt(harmonics[i].Real * harmonics[i].Real + harmonics[i].Imaginary * harmonics[i].Imaginary)));

                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add
                    ((float)(Math.Atan2(harmonics[i].Imaginary, harmonics[i].Real)));
            }

        }
    }
}
