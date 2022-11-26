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

            float Norm = (float)1 / S.Samples.Count * res; //sqrt of the squared value
            return Norm;
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
        }
    }
}