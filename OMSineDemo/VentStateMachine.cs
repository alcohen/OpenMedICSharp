﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OMDemo1
{
    static class VentStateMachine
    {
        public enum BreathStates { None, PreVentilation, PreVentilated, Inhalation, ExhalationFill, Exhalation, TimingPause };
        public enum GasStates { NotFilling, O2Filling, AirFIlling };

        private static long millisStart = 0;
        private static long millisToRun = 0;

        public static void StateMachine()
        {
            switch (GlobalVars.BreathState)
            {
                case BreathStates.None:
                    ToPreVentilation();
                    break;

                case BreathStates.PreVentilation:
                    if (!DoPreVentilation())
                    {
                        ToPreVentilated();
                    }
                    break;

                case BreathStates.PreVentilated:
                    if (!DoPreVentilated())
                    {
                        ToInhalation();
                    }
                    break;

        
                case BreathStates.Inhalation:
                    if (!DoInhalation())
                    {
                        ToExhalationFill();
                    }
                    break;

                case BreathStates.ExhalationFill:
                    GlobalVars.stopwatch.Start();
                    if (!DoExhalationFill())
                    {
                        ToExhalation();
                    }
                    break;

                case BreathStates.Exhalation:
                    if (!DoExhalation())
                    {
                        ToTimingPause();
                    }
                    break;

                case BreathStates.TimingPause:
                    if (!DoTimingPause())
                    {
                        ToInhalation();
                    }
                    break;

                default:
                    throw new System.Exception("Somehow we're in an unexpected state in breath cycle");
            }
            
        }

        private static long GetMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private static bool DoTimedState()
        {
            Debug.Print(((millisStart + millisToRun) - GetMillis()).ToString());
            if (GetMillis() < millisStart + millisToRun)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ToPreVentilation()
        {
            GlobalVars.gValves.Clear();
            GlobalVars.gValves.SetValves(true, false, false, false);
            //millisStart = GetMillis();
            //millisToRun = 3000;
            GlobalVars.BreathState = BreathStates.PreVentilation;
        }

        public static bool DoPreVentilation()
        {
            //return DoTimedState();
            if (GlobalVars.curPBuff >= 800)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void ToPreVentilated()
        {
            GlobalVars.gValves.Clear();
            //GlobalVars.gValves.SetValves(false, false, false, false);
            //millisStart = GetMillis();
            //millisToRun = 3000;
            millisStart = GetMillis();
            millisToRun = 5000;
            GlobalVars.BreathState = BreathStates.PreVentilated;
        }

        public static bool DoPreVentilated()
        {
            return DoTimedState();
        }

        public static void ToInhalation()
        {
            GlobalVars.gValves.Clear();
            GlobalVars.gValves.SetValves(false, false, true, false);
            //millisStart = GetMillis();
            //millisToRun = 3000;
            GlobalVars.BreathState = BreathStates.Inhalation;
        }

        public static bool DoInhalation()
        {
            if (GlobalVars.curPSys >= 75)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void ToExhalationFill()
        {
            GlobalVars.gValves.Clear();
            GlobalVars.gValves.SetValves(true, false, false, true);
            GlobalVars.BreathState = BreathStates.ExhalationFill;
        }

        public static bool DoExhalationFill()
        {
            if (GlobalVars.curPBuff >= 800)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void ToExhalation()
        {
            GlobalVars.gValves.Clear();
            GlobalVars.gValves.SetValves(false, false, false, true);
            //millisStart = GetMillis();
            //millisToRun = 3000;
            GlobalVars.BreathState = BreathStates.Exhalation;
        }

        public static bool DoExhalation()
        {
            if (GlobalVars.curPSys <= GlobalVars.PEEP)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void ToTimingPause()
        {
            GlobalVars.gValves.Clear();
            //GlobalVars.gValves.SetValves(true, false, false, false);
            GlobalVars.stopwatch.Stop();
            TimeSpan ts = GlobalVars.stopwatch.Elapsed;
            double secondsSoFar = ts.TotalSeconds;
            double secondsToGo = (60 / GlobalVars.RespRate);
            double millisToGo = secondsToGo * 1000;
            millisStart = GetMillis();
            millisToRun = Convert.ToInt64(millisToGo);
            GlobalVars.BreathState = BreathStates.TimingPause;
        }

        public static bool DoTimingPause()
        {
            return DoTimedState();
        }

    }
}
