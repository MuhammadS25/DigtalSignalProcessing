using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float tot = 0;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
                tot += InputSignal.Samples[i];
            tot /= InputSignal.Samples.Count;
            OutputSignal = new Signal(new List<float>(), false);
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                OutputSignal.Samples.Add(InputSignal.Samples[i] - tot);
                OutputSignal.SamplesIndices.Add(i);
            }
        }
    }
}
