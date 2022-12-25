using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public Signal filtering(Signal input)
        {
            FIR fir = new FIR();
            fir.InputFilterType = FILTER_TYPES.LOW;
            fir.InputFS = 8000;
            fir.InputStopBandAttenuation = 50;
            fir.InputCutOffFrequency = 1500;
            fir.InputTransitionBand = 500;
            fir.InputTimeDomainSignal = input;
            fir.Run();
            return fir.OutputYn;
        }

        public Signal upSampling(Signal input)
        {
            Signal tmp = new Signal(new List<float>(), false);
            tmp.Samples = new List<float>();
            tmp.SamplesIndices = new List<int>();
            for (int i = 0; i < input.Samples.Count; i++)
            {
                tmp.Samples.Add(input.Samples[i]);
                for (int j = 0; j < L - 1; j++)
                {
                    tmp.Samples.Add(0);
                }
            }
            int start = input.SamplesIndices[0];
            for (int i = 0; i < tmp.Samples.Count; i++)
                tmp.SamplesIndices.Add(start + i);
            return tmp;
        }

        public Signal downSampling(Signal input)
        {
            Signal tmp = new Signal(new List<float>(), false);
            tmp.Samples = new List<float>();
            tmp.SamplesIndices = new List<int>();
            for (int i = 0; i < input.Samples.Count; i += M)
            {
                tmp.Samples.Add(input.Samples[i]);
                tmp.SamplesIndices.Add(input.SamplesIndices[i / M]);
            }
            return tmp;
        }
        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);
            OutputSignal.SamplesIndices = new List<int>();
            OutputSignal.Samples = new List<float>();
            //UPSAMPLING
            if (M == 0 && L != 0)
            {
                OutputSignal = upSampling(InputSignal);
                OutputSignal = filtering(OutputSignal);
            }
            //DOWNSAMPLING
            else if (M != 0 && L == 0)
            {
                OutputSignal = filtering(InputSignal);
                OutputSignal = downSampling(OutputSignal);
            }
            //RATIOSAMPLING
            else
            {
                OutputSignal = upSampling(InputSignal);
                OutputSignal = filtering(OutputSignal);
                OutputSignal = downSampling(OutputSignal);
            }

        }
    }

}