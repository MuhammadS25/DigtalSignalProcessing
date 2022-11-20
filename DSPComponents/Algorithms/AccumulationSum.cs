using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);
            OutputSignal.SamplesIndices = new List<int>();
            OutputSignal.Samples = new List<float>();
            OutputSignal = InputSignal;
            for (int i = 1; i < InputSignal.SamplesIndices.Count; i++)
                OutputSignal.Samples[i] = OutputSignal.Samples[i] + OutputSignal.Samples[i - 1];

        }
    }
}
