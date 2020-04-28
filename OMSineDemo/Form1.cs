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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenMedIC;

namespace OMDemo1
{
    public partial class Form1 : Form
    {
        //Declare the OpenMedic Objects
        private SineWaveGen SineMaker;
        private WaveformBuffer WFBuffer;
        private NewDataTrigger DisplayTrigger;

        //Declare sampling frequency
        private const double secPerStep = 1.0 / 500.0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Create the building blocks
            SineMaker = new SineWaveGen ( secPerStep, true, 0.5); //0.5 Hz sine wave generator
            WFBuffer = new WaveformBuffer(10000); //Buffer used by waveform graph
            
            //Delegate to be called when new points are added
            //This delegate will determine when to refresh the graph
            DisplayTrigger = new NewDataTrigger(new NewDataTrigger.refreshDelegate(NewData), true, true);
            
            //connect the building blocks
            
            //WFBufffer follows SineMaker in chain
            SineMaker.addFollower(WFBuffer);
            //DispTrigger follows WFBuff
            WFBuffer.addFollower(DisplayTrigger);
            //Graph display uses WFBuff
            rtgMain.WFThis = WFBuffer;
        }
        private void NewData()
        {
            // Refresh the graph:
            lock (rtgMain)	// We need to lock to avoid a potential collision
            {
                rtgMain.DrawNew();
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (btnStartStop.Text == "Start")
            {
                //start the data flow

                //start data chain that begins 
                ChainInfo acqInfo = new ChainInfo();
                //StartData.patientInfo = new PatientInfo("Johnny", "Q", "Public");
                SineMaker.init(acqInfo);

                //change the button to a pause button
                btnStartStop.Text = "Pause";
            }
            else if (btnStartStop.Text == "Pause")
            {
                SineMaker.pause();
                btnStartStop.Text = "Resume";
            }
            else
            {
                //button text must be "Resume"
                SineMaker.resume();
                btnStartStop.Text = "Pause";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Release stuff:
            SineMaker.Terminate();
            WFBuffer.dropFollower(DisplayTrigger);
            SineMaker.dropFollower(WFBuffer);
        }
    }
}