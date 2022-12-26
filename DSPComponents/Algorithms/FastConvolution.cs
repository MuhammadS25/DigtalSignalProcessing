using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int size = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;

            for (int i = InputSignal1.Samples.Count; i < size; i++)
                InputSignal1.Samples.Add(0);

            for (int i = InputSignal2.Samples.Count; i < size; i++)
                InputSignal2.Samples.Add(0);

            DiscreteFourierTransform d1 = new DiscreteFourierTransform();
            d1.InputTimeDomainSignal = InputSignal1;
            d1.Run();

            DiscreteFourierTransform d2 = new DiscreteFourierTransform();
            d2.InputTimeDomainSignal = InputSignal2;
            d2.Run();

            List<float> phaseshift = new List<float>();
            for (int i = 0; i < d1.OutputFreqDomainSignal.FrequenciesPhaseShifts.Count; i++)
                phaseshift.Add(d1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i] + d2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);

            List<float> amps = new List<float>();
            for (int i = 0; i < d2.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
                amps.Add(d1.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * d2.OutputFreqDomainSignal.FrequenciesAmplitudes[i]);

            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            idft.InputFreqDomainSignal = new DSPAlgorithms.DataStructures.Signal
                (true, new List<float>(amps), amps, phaseshift);
            idft.Run();
            OutputConvolvedSignal = new Signal(idft.OutputTimeDomainSignal.Samples, false);

            //Removing Zeros
            int index = OutputConvolvedSignal.Samples.Count - 1;
            while (OutputConvolvedSignal.Samples[index] == 0) --index;

            OutputConvolvedSignal.Samples.RemoveRange(index + 1, OutputConvolvedSignal.Samples.Count - (index + 1));
            OutputConvolvedSignal.SamplesIndices.RemoveRange(index + 1, OutputConvolvedSignal.SamplesIndices.Count - (index + 1));
        }
    }
}
