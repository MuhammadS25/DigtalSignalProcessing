using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            OutputFoldedSignal = new Signal(new List<float>(), !InputSignal.Periodic);
            OutputFoldedSignal.SamplesIndices = new List<int>();
            OutputFoldedSignal.Samples = new List<float>();

            for (int i = 0; i < InputSignal.SamplesIndices.Count; i++)
            {
                OutputFoldedSignal.Samples.Add
                    (InputSignal.Samples[InputSignal.Samples.Count - (i + 1)]);

                OutputFoldedSignal.SamplesIndices.Add
                    (-1 * InputSignal.SamplesIndices[InputSignal.Samples.Count - (i + 1)]);

            }
        }
    }
}
