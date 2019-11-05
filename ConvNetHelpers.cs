using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvNetForms
{
    static class ConvNetHelpers
    {
        public static float AdjustLearnRate(float currErr, float prevErr, float learnRate, float decrErr = 0.98f)
        {
            if(currErr > prevErr)
            {
                return learnRate * decrErr;
            }
           
            return learnRate;
        }
    }
}
