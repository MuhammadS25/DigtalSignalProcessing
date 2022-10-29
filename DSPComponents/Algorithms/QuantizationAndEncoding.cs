using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();
            OutputIntervalIndices = new List<int>();
            OutputQuantizedSignal = new Signal(new List<float>(), false);
            if (InputLevel == 0)
                InputLevel = (int)Math.Pow(2, InputNumBits);

            if (InputNumBits == 0)
                InputNumBits = (int)Math.Log(InputLevel, 2);

            float delta = (InputSignal.Samples.Max() - InputSignal.Samples.Min()) / InputLevel;

            List<float> midpoints = new List<float>(InputLevel);
            for (int i = 0; i < InputLevel; i++)
                midpoints.Add
                    ((InputSignal.Samples.Min() + delta * i + InputSignal.Samples.Min() + delta * (i + 1)) / 2);

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Samples[i] == InputSignal.Samples.Max())
                    OutputQuantizedSignal.Samples.Add(midpoints[midpoints.Count - 1]);
                else
                    OutputQuantizedSignal.Samples.Add
                        ((float)Math.Round
                        ((midpoints[(int)((InputSignal.Samples[i] - InputSignal.Samples.Min()) / delta)]), 3));

                OutputIntervalIndices.Add
                    ((int)((InputSignal.Samples[i] - InputSignal.Samples.Min()) / delta) + 1);
                if (OutputIntervalIndices[i] > InputLevel) OutputIntervalIndices[i]--;

                OutputEncodedSignal.Add
                    (Convert.ToString((OutputIntervalIndices[i] - 1), 2));

                if (OutputEncodedSignal[i].Length != InputNumBits)
                    for (int j = OutputEncodedSignal[i].Length; j < InputNumBits; j++)
                        OutputEncodedSignal[i] = "0" + OutputEncodedSignal[i];
            }

            for (int i = 0; i < InputSignal.Samples.Count; i++)
                OutputSamplesError.Add
                    ((float)Math.Round(OutputQuantizedSignal.Samples[i] - InputSignal.Samples[i], 3));

        }
    }
}
