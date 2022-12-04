using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            //Correlation Calculation
            DirectCorrelation dircorr = new DirectCorrelation();
            dircorr.InputSignal1 = InputSignal1;
            dircorr.InputSignal2 = InputSignal2;
            dircorr.Run();

            //Finding Absolute Value
            float abs = dircorr.OutputNormalizedCorrelation.Max();
            //Saving it's Lag(j)
            int lag = dircorr.OutputNormalizedCorrelation.IndexOf(abs);
            //Time Delay = Ts * j
            OutputTimeDelay = InputSamplingPeriod * lag;

        }
    }
}
