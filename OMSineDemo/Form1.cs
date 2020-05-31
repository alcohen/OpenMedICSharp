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

namespace OMDemo1
{
    public partial class Form1 : Form
    {
        //Declare the OpenMedic Objects
        private SineWaveGen SineMaker;
        //private WaveformBuffer WFBuffer;
        private NewDataTrigger DisplayTrigger;

        //Declare sampling frequency
        private const float secPerStep =  (float)(1.0 / 10.0);


        private Timer updateGraphTimer;
        enum ReadingState {NothingYet, DelimiterFound, ReadingValue, }
        SerialPort port;
        //WaveformBuffer wfBuff;

        //Regex regexLow;
        MatchCollection matchCollectionHigh;
        MatchCollection matchCollectionLow;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Globals.wfLowBuff = new WaveformBuffer(10000);
            Globals.wfLowBuff.stepPeriod = secPerStep;
            Globals.wfHighBuff = new WaveformBuffer(10000);
            Globals.wfHighBuff.stepPeriod = secPerStep;

            //regexLow = new Regex(@"^[-+]?[0-9]*\.[0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            rtgHigh.WFThis = Globals.wfHighBuff;
            rtgLow.WFThis = Globals.wfLowBuff;
            updateGraphTimer = new Timer();
            updateGraphTimer.Interval = 100;
            updateGraphTimer.Tick += new EventHandler(UpdateGraph);
            updateGraphTimer.Enabled = true;
            port = new SerialPort("COM9", 115200, Parity.None, 8, StopBits.One);
            //port.ReadTimeout = 10;
            port.Handshake = Handshake.RequestToSendXOnXOff;
            port.DtrEnable = true;
            port.RtsEnable = true;
            port.ReadTimeout = 0;
            port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            port.Open();
            port.DiscardInBuffer();
        }

        private void FetchNewData()
        {
            /*
            // Refresh the graph:
            lock (rtgMain)	// We need to lock to avoid a potential collision
            {
                rtgMain.DrawNew();
            }
            */
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
        }

        private void UpdateGraph(Object sender, EventArgs e)
        {
            rtgHigh.DrawNew();
            rtgLow.DrawNew();
        }

        private static void DataReceivedHandler(
            object sender,
            SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            try
            {
                string indata = sp.ReadLine();
                Debug.Write(indata);
                //Sample s = new Sample(Convert.ToSingle(indata));
                Regex regex = new Regex(@"^([HL]\s)[-+]?[0-9]*(\.[0-9]+)?\r$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matchCollection = regex.Matches(indata);
                if (matchCollection.Count > 0)
                {
                    string sampType = indata.ToString().Substring(0, 1);
                    float sampNum = Convert.ToSingle(indata.ToString().Substring(2));
                    Sample s = new Sample(sampNum);
                    
                    if (sampType == "H")
                    {
                        Globals.wfHighBuff.addValue(s);
                    }
                    else
                    {
                        Globals.wfLowBuff.addValue(s);
                    }
                }
            }
            catch (TimeoutException ex)
            {
                Debug.Write("----------------> nothin");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Release stuff:
             port.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {
            port.Write(((Button)sender).Text.ToLower());
        }
    }
}