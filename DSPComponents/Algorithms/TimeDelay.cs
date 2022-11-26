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

            float abs = float.MinValue;
            int lag = 0;
            for (int i = 0; i < dircorr.OutputNormalizedCorrelation.Count(); i++)
            {
                //Finding Absolute Value
                if (Math.Abs(dircorr.OutputNormalizedCorrelation[i]) > abs)
                {
                    abs = Math.Abs(dircorr.OutputNormalizedCorrelation[i]);
                    //Saving it's Lag(j)
                    lag = i;
                }
            }
            //Time Delay = Ts * j
            OutputTimeDelay = InputSamplingPeriod * lag;

        }
    }
}
