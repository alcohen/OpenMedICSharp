using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OMDemo1
{
    public enum Valve { A, B, C, D };

    public class Valves
    {
        public bool[] ValveStates = new bool[4];

        public void SetValves (bool A, bool B, bool C, bool D)
        {
            SetValve(Valve.A, A);
            SetValve(Valve.B, B);
            SetValve(Valve.C, C);
            SetValve(Valve.D, D);
        }

        public void SetValve(Valve Valve, bool NewState)
        {
            string message;
            if (NewState)
            {
                message = Valve.ToString().ToUpper();                 
            }
            else
            {
                message = Valve.ToString().ToLower();
            }
            GlobalVars.gPort.WriteLine(message);
            ValveStates[(int)Valve] = NewState;
        }

        public void Clear()
        {
            foreach (Valve V in (Valve[])Enum.GetValues(typeof(Valve)))
            {
                SetValve(V, false);
            }
        }

        class GasMix
        {
            float O2Percent { set; get; }
            float AirPercent { get { return 1 - O2Percent; } }
        }
    }
}
