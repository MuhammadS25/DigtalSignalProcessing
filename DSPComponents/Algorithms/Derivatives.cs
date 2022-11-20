using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            FirstDerivative = new Signal(new List<float>(), false);
            SecondDerivative = new Signal(new List<float>(), false);
            FirstDerivative.SamplesIndices = new List<int>();
            FirstDerivative.Samples = new List<float>();
            SecondDerivative.SamplesIndices = new List<int>();
            SecondDerivative.Samples = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count - 1; i++)
            {
                FirstDerivative.Samples.Add
                    (InputSignal.Samples[i + 1] - InputSignal.Samples[i]);

                FirstDerivative.SamplesIndices.Add(i);
            }
            for (int i = 0; i < InputSignal.Samples.Count - 2; i++)
            {
                SecondDerivative.Samples.Add
                    (InputSignal.Samples[i + 2] -
                    2 * InputSignal.Samples[i + 1] + InputSignal.Samples[i]);

                SecondDerivative.SamplesIndices.Add(i);
            }
        }
    }
}
