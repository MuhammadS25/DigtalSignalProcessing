﻿using System;
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

        public static List<Complex> ComputeHarmonics(Complex reverse, List<Complex> S)
        {
            List<Complex> harmonics = new List<Complex>(S.Count);
            for (int k = 0; k < S.Count; ++k)
            {
                harmonics.Add(new Complex(0, 0));
                for (int n = 0; n < S.Count; ++n)
                {
                    harmonics[k] +=
                        S[n] * Complex.Pow
                        (Math.E, reverse * k * 2 * Math.PI * n / S.Count);
                }
            }
            return harmonics;
        }
        public override void Run()
        {
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();
            OutputFreqDomainSignal.Frequencies = new List<float>();
            List<Complex> S = new List<Complex>();

            for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++)
                S.Add(new Complex(InputTimeDomainSignal.Samples[i], 0));

            var harmonics = ComputeHarmonics(new Complex(0, -1), S);

            for (int i = 0; i < harmonics.Count; ++i)
            {
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add
                    ((float)(Math.Sqrt(harmonics[i].Real * harmonics[i].Real + harmonics[i].Imaginary * harmonics[i].Imaginary)));

                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add
                    ((float)(Math.Atan2(harmonics[i].Imaginary, harmonics[i].Real)));
            }
            //Computing X-axis
            for (int i = 1; i <= InputTimeDomainSignal.Samples.Count; ++i)
                OutputFreqDomainSignal.Frequencies.Add
                    ((float)Math.Round(
                    (2 * Math.PI * InputSamplingFrequency * i / InputTimeDomainSignal.Samples.Count), 1));

        }
    }
}
