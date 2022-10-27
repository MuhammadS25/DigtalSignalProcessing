using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> S = new List<float>();
            for (int i = 0; i + 1 < InputSignals.Count; i++)
                for (int j = 0; j < InputSignals[0].Samples.Count; j++)
                {
                    float sample = InputSignals[i].Samples[j] + InputSignals[i + 1].Samples[j];
                    S.Add(sample);
                }

            OutputSignal = new Signal(S, false);
        }
    }
}