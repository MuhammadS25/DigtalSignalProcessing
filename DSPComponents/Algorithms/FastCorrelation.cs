using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public float Normalization(Signal S)
        {
            float res = 0;
            for (int i = 0; i < S.Samples.Count; i++)
                res += S.Samples[i] * S.Samples[i];

            return res;
        }
        public override void Run()
        {
            OutputNormalizedCorrelation = new List<float>();
            OutputNonNormalizedCorrelation = new List<float>();
            if (InputSignal2 == null)
            {
                DiscreteFourierTransform d1 = new DiscreteFourierTransform();
                d1.InputTimeDomainSignal = InputSignal1;
                d1.Run();

                List<Complex> S = new List<Complex>();
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    S.Add(new Complex(InputSignal1.Samples[i], 0));

                var harmonics = DiscreteFourierTransform.ComputeHarmonics(new Complex(0, -1), S);
                for (int i = 0; i < harmonics.Count; i++)
                    harmonics[i] = Complex.Conjugate(harmonics[i]);
                List<float> amps = new List<float>();
                List<float> phases = new List<float>();
                for (int i = 0; i < harmonics.Count; ++i)
                {
                    amps.Add
                        ((float)(Math.Sqrt(harmonics[i].Real * harmonics[i].Real + harmonics[i].Imaginary * harmonics[i].Imaginary)));
                    amps[i] *= d1.OutputFreqDomainSignal.FrequenciesAmplitudes[i];

                    phases.Add
                        ((float)(Math.Atan2(harmonics[i].Imaginary, harmonics[i].Real)));
                    phases[i] += d1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i];
                }
                InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
                idft.InputFreqDomainSignal = new DSPAlgorithms.DataStructures.Signal
                    (true, new List<float>(amps), amps, phases);
                idft.Run();
                float norm = Normalization(InputSignal1);
                norm = (float)1 / InputSignal1.Samples.Count * (float)Math.Sqrt(norm * norm);
                for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count; ++i)
                {
                    OutputNonNormalizedCorrelation.Add(idft.OutputTimeDomainSignal.Samples[i] / InputSignal1.Samples.Count);
                    OutputNormalizedCorrelation.Add(idft.OutputTimeDomainSignal.Samples[i] / InputSignal1.Samples.Count / norm);
                }
            }
            else
            {
                if (InputSignal1.Samples.Count != InputSignal2.Samples.Count)
                {
                    for (int i = 0; i < InputSignal2.Samples.Count - 1; i++)
                        InputSignal1.Samples.Add(0);
                    for (int i = 0; i < InputSignal1.Samples.Count - 1; i++)
                        InputSignal2.Samples.Add(0);
                }

                DiscreteFourierTransform d1 = new DiscreteFourierTransform();
                d1.InputTimeDomainSignal = InputSignal2;
                d1.Run();

                List<Complex> S = new List<Complex>();
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    S.Add(new Complex(InputSignal1.Samples[i], 0));

                var harmonics = DiscreteFourierTransform.ComputeHarmonics(new Complex(0, -1), S);
                for (int i = 0; i < harmonics.Count; i++)
                    harmonics[i] = Complex.Conjugate(harmonics[i]);
                List<float> amps = new List<float>();
                List<float> phases = new List<float>();
                for (int i = 0; i < harmonics.Count; ++i)
                {
                    amps.Add
                        ((float)(Math.Sqrt(harmonics[i].Real * harmonics[i].Real + harmonics[i].Imaginary * harmonics[i].Imaginary)));
                    amps[i] *= d1.OutputFreqDomainSignal.FrequenciesAmplitudes[i];

                    phases.Add
                        ((float)(Math.Atan2(harmonics[i].Imaginary, harmonics[i].Real)));
                    phases[i] += d1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i];
                }

                InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
                idft.InputFreqDomainSignal = new DSPAlgorithms.DataStructures.Signal
                    (true, new List<float>(amps), amps, phases);
                idft.Run();

                float norm1 = Normalization(InputSignal1);
                float norm2 = Normalization(InputSignal2);
                float norm = (float)1 / InputSignal1.Samples.Count * (float)Math.Sqrt(norm1 * norm2);

                for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count; ++i)
                {
                    OutputNonNormalizedCorrelation.Add(idft.OutputTimeDomainSignal.Samples[i] / InputSignal1.Samples.Count);
                    OutputNormalizedCorrelation.Add(idft.OutputTimeDomainSignal.Samples[i] / InputSignal1.Samples.Count / norm);
                }
            }
        }
    }
}