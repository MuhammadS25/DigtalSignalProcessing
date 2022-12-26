using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            Signal InputSignal = LoadSignal(SignalPath);
            //Signals Folder
            String path = "C:/Users/MoSabry25/source/repos/DSPToolbox/Saved Signals/";

            /*---------------------------------------------------------*/

            //FIR
            FIR fir = new FIR();
            fir.InputTimeDomainSignal = InputSignal;
            fir.InputFilterType = FILTER_TYPES.BAND_PASS;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputFS = Fs;
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;
            fir.Run();
            saveSignal(fir.OutputYn, path + "fir.ds");

            /*---------------------------------------------------------*/

            //Sampling
            Sampling sampling = new Sampling();
            sampling.OutputSignal = new Signal(new List<float>(), false);
            sampling.OutputSignal.Samples = new List<float>();
            if (newFs >= 2 * maxF)
            {
                sampling.InputSignal = fir.OutputYn;
                sampling.L = L;
                sampling.M = M;
                sampling.Run();
                saveSignal(sampling.OutputSignal, path + "SampledSignal.ds");
            }

            /*---------------------------------------------------------*/

            //Removing DC Component
            DC_Component dc = new DC_Component();
            dc.InputSignal = sampling.OutputSignal.Samples.Count == 0 ? fir.OutputYn : sampling.OutputSignal;
            dc.Run();
            saveSignal(dc.OutputSignal, path + "DCRemoved.ds");

            /*--------------------------------------------------------*/

            //Normalization -1 to 1
            Normalizer normalizer = new Normalizer();
            normalizer.InputSignal = dc.OutputSignal;
            normalizer.InputMinRange = -1;
            normalizer.InputMaxRange = 1;
            normalizer.Run();
            saveSignal(normalizer.OutputNormalizedSignal, path + "NormalizedSignal.ds");

            /*--------------------------------------------------------*/

            //DFT
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = normalizer.OutputNormalizedSignal;
            dft.InputSamplingFrequency = Fs;
            dft.Run();
            OutputFreqDomainSignal = dft.OutputFreqDomainSignal;
            saveSignal(OutputFreqDomainSignal, path + "FreqDomainSignal.ds");
        }
        //Saving Signals from test Utilities
        public void saveSignal(Signal signal, String SignalPath)
        {
            using (StreamWriter writer = File.CreateText(SignalPath))
            {
                if (signal.Frequencies == null || signal.Frequencies.Count == 0)
                    writer.WriteLine(0);
                else
                    writer.WriteLine(1);
                if (signal.Periodic == false)
                    writer.WriteLine(0);
                else
                    writer.WriteLine(1);
                if (signal.Frequencies == null || signal.Frequencies.Count == 0)
                {
                    writer.WriteLine(signal.Samples.Count);
                }
                else writer.WriteLine(signal.Frequencies.Count);
                if (signal.Frequencies == null || signal.Frequencies.Count == 0)
                {
                    for (int i = 0; i < signal.Samples.Count; i++)
                    {
                        writer.Write(signal.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.WriteLine(signal.Samples[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < signal.Frequencies.Count; i++)
                    {
                        writer.Write(signal.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(signal.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.WriteLine(signal.FrequenciesPhaseShifts[i]);
                    }
                }

                writer.Flush();
                writer.Close();
            }
        }
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
