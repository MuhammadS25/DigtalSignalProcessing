using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
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

            //Auto
            if (InputSignal2 == null)
            {

                if (!InputSignal1.Periodic)
                {
                    float norm = Normalization(InputSignal1);
                    norm = (float)1 / InputSignal1.Samples.Count * (float)Math.Sqrt(norm * norm);
                    int shifts = 0;
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        float corr = 0;
                        for (int j = 0; j < InputSignal1.Samples.Count - shifts; j++)
                            corr += (InputSignal1.Samples[j] * InputSignal1.Samples[j + shifts]);

                        corr /= InputSignal1.Samples.Count;
                        OutputNonNormalizedCorrelation.Add(corr);
                        OutputNormalizedCorrelation.Add(corr / norm);
                        ++shifts;
                    }
                }
                else
                {
                    float norm = Normalization(InputSignal1);
                    norm = (float)1 / InputSignal1.Samples.Count * (float)Math.Sqrt(norm * norm);
                    Signal input11 = new Signal(new List<float>(), new List<int>(), InputSignal1.Periodic);
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                        input11.Samples.Add(InputSignal1.Samples[i]);
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        float corr = 0;
                        for (int j = 0; j < InputSignal1.Samples.Count; j++)
                            corr += (InputSignal1.Samples[j] * input11.Samples[j]);

                        corr /= InputSignal1.Samples.Count;
                        OutputNonNormalizedCorrelation.Add(corr);
                        OutputNormalizedCorrelation.Add(corr / norm);
                        //Shifting The Signal .......
                        float Shifted = input11.Samples[0];
                        input11.Samples.Remove(input11.Samples[0]);
                        input11.Samples.Add(Shifted);
                    }
                }
            }
            else
            {
                if (InputSignal1.Periodic)
                {
                    if (InputSignal1.Samples.Count != InputSignal2.Samples.Count)
                    {
                        for (int i = 0; i < InputSignal2.Samples.Count - 1; i++)
                            InputSignal1.Samples.Add(0);
                        for (int i = 0; i < InputSignal1.Samples.Count - 1; i++)
                            InputSignal2.Samples.Add(0);
                    }

                    float norm1 = Normalization(InputSignal1);
                    float norm2 = Normalization(InputSignal2);
                    float norm = (float)1 / InputSignal1.Samples.Count * (float)Math.Sqrt(norm1 * norm2);

                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        float corr = 0;
                        for (int j = 0; j < InputSignal1.Samples.Count; j++)
                            corr += (InputSignal1.Samples[j] * InputSignal2.Samples[j]);

                        corr /= InputSignal1.Samples.Count;

                        OutputNonNormalizedCorrelation.Add(corr);
                        OutputNormalizedCorrelation.Add(corr / norm);

                        float Shifted = InputSignal2.Samples[0];
                        InputSignal2.Samples.Remove(InputSignal2.Samples[0]);
                        InputSignal2.Samples.Add(Shifted);
                    }
                }
            }
        }
    }
}