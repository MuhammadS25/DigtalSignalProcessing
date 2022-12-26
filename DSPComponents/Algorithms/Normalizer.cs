using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            OutputNormalizedSignal = new Signal(new List<float>(), false);
            OutputNormalizedSignal.Samples = new List<float>();
            OutputNormalizedSignal.SamplesIndices = new List<int>();

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float normalizedSample =
                    (InputSignal.Samples[i] - InputSignal.Samples.Min()) * (InputMaxRange - InputMinRange)
                    / (InputSignal.Samples.Max() - InputSignal.Samples.Min())
                    + InputMinRange;

                OutputNormalizedSignal.Samples.Add(normalizedSample);
                OutputNormalizedSignal.SamplesIndices.Add(i);
            }
        }
    }
}
