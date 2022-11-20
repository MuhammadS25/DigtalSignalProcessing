using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }

        public override void Run()
        {
            OutputAverageSignal = new Signal(new List<float>(), false);
            OutputAverageSignal.SamplesIndices = new List<int>();
            OutputAverageSignal.Samples = new List<float>();
            float res = 0;
            for (int i = 0; i <= InputSignal.Samples.Count - InputWindowSize; ++i)
            {
                for (int j = 0; j < InputWindowSize; ++j)
                {
                    res += InputSignal.Samples[j + i];
                    if (i + j + 1 == InputSignal.Samples.Count) break;
                }
                OutputAverageSignal.Samples.Add(res / InputWindowSize);
                OutputAverageSignal.SamplesIndices.Add(i);
                res = 0;
            }
        }
    }
}
