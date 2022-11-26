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

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            if (InputSignal2 == null)
            {
                InputSignal2 = new Signal(InputSignal1.Samples, InputSignal1.Periodic);
                double Sum1 = 0 , Sum2 = 0 , SumTot = 0;

                for(int i=0; i<InputSignal1.Samples.Count; i++)
                {
                    Sum1 += Math.Pow(InputSignal1.Samples[i], 2);
                }
                Sum2 = Sum1;
                SumTot = Sum1 * Sum2;
                double NrmlFctr = (1 / ((1 / InputSignal1.Samples.Count) * Math.Sqrt(SumTot)));
               
                for(int i=0; i<InputSignal1.Samples.Count; i++)
                {
                    double nonNorm = 0 , Norm = 0;
                    for(int j=0; j<InputSignal1.Samples.Count; j++)
                    {
                        nonNorm += InputSignal1.Samples[j] * InputSignal2.Samples[j];
                    }
                    Norm = NrmlFctr * nonNorm;
                    OutputNonNormalizedCorrelation[i] = (float)nonNorm;
                    OutputNormalizedCorrelation[i] = (float)Norm;
                    if (InputSignal2.Periodic)
                    {
                        float first = InputSignal2.Samples[0];
                        InputSignal2.Samples.RemoveAt(0);
                        InputSignal2.Samples.Add(first);
                    }
                    else
                    {
                        InputSignal2.Samples.RemoveAt(0);
                        InputSignal2.Samples.Add(0);
                    }
                }
            }
            else
            {
                double Sum1 = 0, Sum2 = 0, SumTot = 0;
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    Sum1 += Math.Pow(InputSignal1.Samples[i], 2);
                }
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                {
                    Sum2 += Math.Pow(InputSignal2.Samples[i], 2);
                }
                SumTot = Sum1 * Sum2;
                double NrmlFctr = (1 / ((1 / InputSignal1.Samples.Count) * Math.Sqrt(SumTot)));
            }
            throw new NotImplementedException();
        }
    }
}