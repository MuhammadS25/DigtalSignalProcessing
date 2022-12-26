using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            OutputConvolvedSignal = new Signal(new List<float>(), false);
            OutputConvolvedSignal.SamplesIndices = new List<int>();
            OutputConvolvedSignal.Samples = new List<float>();
            int first = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];
            int last = InputSignal1.SamplesIndices.Max() + InputSignal2.SamplesIndices.Max();

            for (int i = first; i <= last; i++)
            {
                float res = 0;
                for (int k = first; k < InputSignal1.Samples.Count; k++)
                {
                    if ((i - k) >= InputSignal2.Samples.Count() || (i - k) < InputSignal2.SamplesIndices.Min() || (i - k) > InputSignal2.SamplesIndices.Max()
                        || k < InputSignal1.SamplesIndices.Min() || k > InputSignal1.SamplesIndices.Max())
                        continue;
                    res += InputSignal1.Samples[InputSignal1.SamplesIndices.IndexOf(k)] *
                        InputSignal2.Samples[InputSignal2.SamplesIndices.IndexOf(i - k)];

                }
                if (res == 0 && i == last) continue;
                OutputConvolvedSignal.SamplesIndices.Add(i);
                OutputConvolvedSignal.Samples.Add(res);
            }

            //Removing Zeros
            int index = OutputConvolvedSignal.Samples.Count - 1;
            while (OutputConvolvedSignal.Samples[index] == 0) --index;

            OutputConvolvedSignal.Samples.RemoveRange(index + 1, OutputConvolvedSignal.Samples.Count - (index + 1));
            OutputConvolvedSignal.SamplesIndices.RemoveRange(index + 1, OutputConvolvedSignal.SamplesIndices.Count - (index + 1));
        }

    }
}

