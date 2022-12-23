using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public int Ncoefficents(float factor)
        {
            int N = (int)(Math.Ceiling(factor / (InputTransitionBand / InputFS)));
            return N % 2 == 0 ? N + 1 : N;
        }
        /*--------------------------------------------------------------------------*/
        public float hanning(int range, int N)
        {
            return (float)(0.5f + 0.5f * Math.Cos((float)(2 * Math.PI * range) / N));
        }

        public float hamming(int range, int N)
        {
            return (float)(0.54f + 0.46f * Math.Cos((float)(2 * Math.PI * range) / N));
        }

        public float blackman(int range, int N)
        {
            return (float)(0.42f + 0.5f * Math.Cos((float)(2 * Math.PI * range) / (N - 1))
                + 0.08f * Math.Cos((float)(4 * Math.PI * range) / (N - 1)));
        }

        /*----------------------------------------------------------------------------*/

        public float lowpass(int n, float fc)
        {
            return n == 0 ? 2 * fc :
                (float)(2 * fc * ((Math.Sin(n * 2 * Math.PI * fc))
                / (n * 2 * Math.PI * fc)));
        }
        public float highpass(int n, float fc)
        {
            return n == 0 ? 1 - 2 * fc :
                (float)(-1 * 2 * fc * ((Math.Sin(n * 2 * Math.PI * fc))
                / (n * 2 * Math.PI * fc)));
        }
        public float bandpass(int n, float fc1, float fc2)
        {
            if (n != 0)
            {
                float res1 = (float)(2 * fc1 * ((Math.Sin(n * 2 * Math.PI * fc1))
                    / (n * 2 * Math.PI * fc1)));

                float res2 = (float)(2 * fc2 * ((Math.Sin(n * 2 * Math.PI * fc2))
                    / (n * 2 * Math.PI * fc2)));

                return res2 - res1;
            }
            else
                return 2 * (fc2 - fc1);
        }
        public float bandreject(int n, float fc1, float fc2)
        {
            if (n != 0)
            {
                float res1 = (float)(2 * fc1 * ((Math.Sin(n * 2 * Math.PI * fc1))
                    / (n * 2 * Math.PI * fc1)));

                float res2 = (float)(2 * fc2 * ((Math.Sin(n * 2 * Math.PI * fc2))
                    / (n * 2 * Math.PI * fc2)));

                return res1 - res2;
            }
            else
                return 1 - 2 * (fc2 - fc1);
        }
        public override void Run()
        {
            OutputHn = new Signal(new List<float>(), false);
            OutputHn.SamplesIndices = new List<int>();
            OutputHn.Samples = new List<float>();

            OutputYn = new Signal(new List<float>(), false);
            OutputYn.SamplesIndices = new List<int>();
            OutputYn.Samples = new List<float>();

            if (InputFilterType == FILTER_TYPES.LOW)
            {
                //Adjusting cutoff frequency
                InputCutOffFrequency += InputTransitionBand / 2;
                InputCutOffFrequency /= InputFS;

                if (InputStopBandAttenuation <= 21)
                {
                    int N = Ncoefficents(0.9f);
                    int range = (N - 1) / 2;
                    for (int i = -range; i <= 0; ++i)
                    {
                        OutputHn.Samples.Add(lowpass(i, (float)InputCutOffFrequency));
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 44)
                {
                    int N = Ncoefficents(3.1f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = lowpass(i, (float)InputCutOffFrequency);
                        w = hanning(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 53)
                {
                    int N = Ncoefficents(3.3f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = lowpass(i, (float)InputCutOffFrequency);
                        w = hamming(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 74)
                {
                    int N = Ncoefficents(5.5f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = lowpass(i, (float)InputCutOffFrequency);
                        w = blackman(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                //Adjusting cutoff frequency
                InputCutOffFrequency -= InputTransitionBand / 2;
                InputCutOffFrequency /= InputFS;
                if (InputStopBandAttenuation <= 21)
                {
                    int N = Ncoefficents(0.9f);
                    int range = (N - 1) / 2;
                    for (int i = -range; i <= 0; ++i)
                    {
                        OutputHn.Samples.Add(highpass(i, (float)InputCutOffFrequency));
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 44)
                {
                    int N = Ncoefficents(3.1f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = highpass(i, (float)InputCutOffFrequency);
                        w = hanning(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 53)
                {
                    int N = Ncoefficents(3.3f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = highpass(i, (float)InputCutOffFrequency);
                        w = hamming(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 74)
                {
                    int N = Ncoefficents(5.5f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = highpass(i, (float)InputCutOffFrequency);
                        w = blackman(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                //Adjusting cutoff frequency
                float fc1 = (float)((InputF1 - (InputTransitionBand / 2)) / InputFS);
                float fc2 = (float)((InputF2 + (InputTransitionBand / 2)) / InputFS);
                if (InputStopBandAttenuation <= 21)
                {
                    int N = Ncoefficents(0.9f);
                    int range = (N - 1) / 2;
                    for (int i = -range; i <= 0; ++i)
                    {
                        OutputHn.Samples.Add(bandpass(i, fc1, fc2));
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 44)
                {
                    int N = Ncoefficents(3.1f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = bandpass(i, fc1, fc2);
                        w = hanning(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 53)
                {
                    int N = Ncoefficents(3.3f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = bandpass(i, fc1, fc2);
                        w = hamming(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 74)
                {
                    int N = Ncoefficents(5.5f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = bandpass(i, fc1, fc2);
                        w = blackman(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
            }
            else
            {
                //Adjusting cutoff frequency
                float fc1 = (float)((InputF1 + (InputTransitionBand / 2)) / InputFS);
                float fc2 = (float)((InputF2 - (InputTransitionBand / 2)) / InputFS);
                if (InputStopBandAttenuation <= 21)
                {
                    int N = Ncoefficents(0.9f);
                    int range = (N - 1) / 2;
                    for (int i = -range; i <= 0; ++i)
                    {
                        OutputHn.Samples.Add(bandreject(i, fc1, fc2));
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 44)
                {
                    int N = Ncoefficents(3.1f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = bandreject(i, fc1, fc2);
                        w = hanning(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 53)
                {
                    int N = Ncoefficents(3.3f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = bandreject(i, fc1, fc2);
                        w = hamming(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
                else if (InputStopBandAttenuation <= 74)
                {
                    int N = Ncoefficents(5.5f);
                    int range = (N - 1) / 2;
                    float h, w;
                    for (int i = -range; i <= 0; ++i)
                    {
                        h = bandreject(i, fc1, fc2);
                        w = blackman(i, N);
                        OutputHn.Samples.Add(h * w);
                        OutputHn.SamplesIndices.Add(i);
                    }
                    for (int i = 1; i <= range; ++i)
                    {
                        OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(OutputHn.SamplesIndices[range - i])]);
                        OutputHn.SamplesIndices.Add(i);
                    }
                }
            }
            DirectConvolution dc = new DirectConvolution();
            dc.InputSignal1 = InputTimeDomainSignal;
            dc.InputSignal2 = OutputHn;
            dc.Run();
            OutputYn = dc.OutputConvolvedSignal;
        }
    }
}
