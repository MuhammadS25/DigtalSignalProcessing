using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> L = new List<float>();
            float N = InputSignal.Samples.Count;
            for (int k = 0; k < N; k++)
            {
                float value = 0;
                for (int n = 0; n < N; n++)
                {
                    value += (float)(InputSignal.Samples[n] * Math.Cos((Math.PI / (4 * N)) * (2 * n - 1) * (2 * k - 1)));
                }
                value *= (float)Math.Sqrt(2 / N);
                L.Add(value);
            }
            OutputSignal = new Signal(L, false);

        }
    }
}