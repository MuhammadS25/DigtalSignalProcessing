using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            OutputTimeDomainSignal = new Signal(new List<float>(), false);
            List<Complex> S = new List<Complex>();

            for (int i = 0; i < InputFreqDomainSignal.FrequenciesPhaseShifts.Count; i++)
            {
                S.Add(
                    new Complex
                    (InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[i])
                    , InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[i])));
            }
            var harmonics = DiscreteFourierTransform.ComputeHarmonics(new Complex(0, 1), S);
            for (int i = 0; i < harmonics.Count; i++)
                harmonics[i] /= harmonics.Count;

            for (int i = 0; i < harmonics.Count; i++)
                OutputTimeDomainSignal.Samples.Add((float)harmonics[i].Real);
        }
    }
}
