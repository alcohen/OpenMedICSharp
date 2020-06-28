using OpenMedIC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;


namespace OMDemo1
{
    static class GlobalVars
    {
        static public WaveformBuffer wfLowBuff;
        static public WaveformBuffer wfHighBuff;
        public static SerialPort gPort = new SerialPort("COM6", 115200, Parity.None, 8, StopBits.One);
        public static Valves gValves = new Valves();
        public static VentStateMachine.BreathStates BreathState = VentStateMachine.BreathStates.None;
        public static float curPBuff;
        public static float curPSys;
        public static float curPAmbientBuf;
        public static float curPAmbientSys;
        public static float PEEP = 5;
        public static float RespRate = 5;
        public static Stopwatch stopwatch = new Stopwatch();
        public static string serialIn = "";
    }
}
