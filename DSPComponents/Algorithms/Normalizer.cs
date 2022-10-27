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
            List<float> S = new List<float>();

            float max = float.MinValue, min = float.MaxValue;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Samples[i] > max)
                {
                    max = InputSignal.Samples[i];
                }
                if (InputSignal.Samples[i] < min)
                {
                    min = InputSignal.Samples[i];
                }
            }

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float normalizedSample =
                    (InputSignal.Samples[i] - min) * (InputMaxRange - InputMinRange) / (max - min)
                    + InputMinRange;
                S.Add(normalizedSample);
            }

            OutputNormalizedSignal = new Signal(S, false);
        }
    }
}
