/* --- GPL ---
 *
 * Copyright (C) 2004-2006 Duke-River Engineering Company.
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 * --- GPL --- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;
using OpenMedIC;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace OMDemo1
{
    public partial class frmMain : Form
    {
        //Declare sampling frequency
        private const float secPerStepLow =  (float)(0.100);
        private const float secPerStepHigh = (float)(0.100);

        private Timer updateGraphTimer;
        //SerialPort gPort;
        Color outStateBackColor = Color.LightGray;
        Color inStateBackColor = Color.Lime;

        AccurateTimer mTimer1;
        Stopwatch TotalStopwatch = new Stopwatch();

        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GlobalVars.wfLowBuff = new WaveformBuffer(10000);
            GlobalVars.wfLowBuff.stepPeriod = secPerStepLow;
            GlobalVars.wfHighBuff = new WaveformBuffer(10000);
            GlobalVars.wfHighBuff.stepPeriod = secPerStepHigh;

            //declare ambient pressure offsets
            GlobalVars.curPAmbientBuf = 1000;
            GlobalVars.curPAmbientSys = 1005;

            //regexLow = new Regex(@"^[-+]?[0-9]*\.[0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);            
            rtgHigh.WFThis = GlobalVars.wfHighBuff;
            rtgLow.WFThis = GlobalVars.wfLowBuff;
            updateGraphTimer = new Timer();
            updateGraphTimer.Interval = 100;
            updateGraphTimer.Tick += new EventHandler(UpdateGraph);
            updateGraphTimer.Enabled = true;

            //GlobalVars.gPort.Handshake = Handshake.RequestToSendXOnXOff;
            //GlobalVars.gPort.DtrEnable = true;
            //GlobalVars.gPort.RtsEnable = true;
            GlobalVars.gPort.ReadTimeout = 1;
            GlobalVars.gPort.ReadBufferSize = 2000;
            GlobalVars.gPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            GlobalVars.gPort.Open();
            GlobalVars.gPort.DiscardInBuffer();
            GlobalVars.gValves.Clear();

            ClearStateIndicators();

            TotalStopwatch.Start();
            int delay = 100;   // In milliseconds. 10 = 1/100th second.
            mTimer1 = new AccurateTimer(this, new Action(TimerReturn), delay);
            GlobalVars.gValves.SetValves(false, false, false, false); //close all valves
            tmrStateLoop.Enabled = true;
        }

        private void TimerReturn()
        {
            //Debug.WriteLine("AT=" + TotalStopwatch.ElapsedMilliseconds);
            GlobalVars.wfLowBuff.addValue(new Sample(GlobalVars.curPSys));
            GlobalVars.wfHighBuff.addValue(new Sample(GlobalVars.curPBuff));
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {

        }

        private void UpdateGraph(Object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            rtgHigh.DrawNew();
            rtgLow.DrawNew();
            stopwatch.Stop();
            //Debug.WriteLine("UpdateGraph: " + stopwatch.ElapsedMilliseconds);
        }

        private static void DataReceivedHandler(
            object sender,
            SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            try
            {
                GlobalVars.serialIn += sp.ReadExisting();
            }
            catch (TimeoutException ex)
            {
                Debug.Write("----------------> nothin");
            }

            string[] splitted = GlobalVars.serialIn.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            GlobalVars.serialIn = splitted[splitted.Length - 1];

            for (int i = 0; i < splitted.Length-1; i++)
            {
                //string pattern = @"^([HL]=)[-+]?[0-9]*(\.[0-9]+)?\r$";
                string pattern = @"^([HL]=)[-+]?[0-9]*(\.[0-9]+)?$";
                if (Regex.IsMatch(splitted[i], pattern))
                {
                    string sampType = splitted[i].ToString().Substring(0, 1);
                    float sampNum = Convert.ToSingle(splitted[i].ToString().Substring(2));
                    if (sampType == "L")
                    {
                        GlobalVars.curPSys = sampNum - GlobalVars.curPAmbientSys;
                    }
                    else
                    {
                        GlobalVars.curPBuff = sampNum - GlobalVars.curPAmbientBuf;
                    }
                }
            }
         }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Release stuff:
            GlobalVars.gValves.Clear();
            GlobalVars.gPort.Close();
            mTimer1.Stop();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_ClickOld(object sender, EventArgs e)
        {
            //port.Write(((Button)sender).Text.ToLower());
            GlobalVars.gValves.SetValve(Valve.A, true);
        }

        private void btnAToggle_Click(object sender, EventArgs e)
        {
            //GlobalVars.gValves.SetValve(Valve.A, !GlobalVars.gValves.ValveStates[(int)Valve.A]);
            GlobalVars.gValves.SetValve(Valve.A, !GlobalVars.gValves.ValveStates[(int)Valve.A]);
        }

        private void btnBToggle_Click(object sender, EventArgs e)
        {
            GlobalVars.gValves.SetValve(Valve.B, !GlobalVars.gValves.ValveStates[(int)Valve.B]);
        }

        private void btnCToggle_Click(object sender, EventArgs e)
        {
            GlobalVars.gValves.SetValve(Valve.C, !GlobalVars.gValves.ValveStates[(int)Valve.C]);
        }

        private void btnDToggle_Click(object sender, EventArgs e)
        {
            GlobalVars.gValves.SetValve(Valve.D, !GlobalVars.gValves.ValveStates[(int)Valve.D]);
        }

        private void tmrStateLoop_Tick(object sender, EventArgs e)
        {
            VentStateMachine.StateMachine();
            SetStateIndicatorLights();
        }

        void SetStateIndicatorLights()
        {
            ClearStateIndicators();
            switch (GlobalVars.BreathState)
            {
                case VentStateMachine.BreathStates.PreVentilation:
                    lblPreVentilation.BackColor = inStateBackColor;
                    break;

                case VentStateMachine.BreathStates.PreVentilated:
                    lblPreVentilation.BackColor = inStateBackColor;
                    break;

                case VentStateMachine.BreathStates.Inhalation:
                    lblInhalation.BackColor = inStateBackColor;
                    break;

                case VentStateMachine.BreathStates.Exhalation:
                    lblExhalation.BackColor = inStateBackColor;
                    break;

                case VentStateMachine.BreathStates.TimingPauseFill:
                    lblTimingPause.BackColor = inStateBackColor;
                    lblBufferRefill.BackColor = inStateBackColor;
                    break;

                case VentStateMachine.BreathStates.TimingPause:
                    lblTimingPause.BackColor = inStateBackColor;
                    break;
                
                default:
                    break;
            }

        }

        void ClearStateIndicators()
        {
            lblPreVentilation.BackColor = outStateBackColor;
            lblInhalation.BackColor = outStateBackColor;
            lblExhalation.BackColor = outStateBackColor;
            lblTimingPause.BackColor = outStateBackColor;
            lblBufferRefill.BackColor = outStateBackColor;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ckbAny_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
             {
                GlobalVars.gValves.SetValve(Valve.A, true);                
            }
        }
    }
}
