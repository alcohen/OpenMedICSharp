using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMDemo1
{
    class BoxcarFilter
    {
        static float[] filtBuf = new float[5];

        public BoxcarFilter()
        {
            for (int i=0; i<5; i++)
            {
                filtBuf[i] = 0;
            }
        }

        public float nextStep(float nextIn)
        {
            for (int i=4; i>0; i--)
            {
                filtBuf[i] = filtBuf[i - 1];
            }
            filtBuf[0] = nextIn;

            float sum = 0;
            foreach(float f in filtBuf)
            {
                sum += f;
            }
            return sum / 5;
        }
    }
}
