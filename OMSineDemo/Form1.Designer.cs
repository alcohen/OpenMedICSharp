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

namespace OMDemo1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.rtgMain = new OpenMedIC.RTGraph();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(25, 260);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 1;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // rtgMain
            // 
            this.rtgMain.AutoRedraw = true;
            this.rtgMain.BorderBevelWidth = 4;
            this.rtgMain.BorderMargin = 5;
            this.rtgMain.CursColor = System.Drawing.Color.Blue;
            this.rtgMain.CursForeWidth = 3;
            this.rtgMain.CursWidth = 1;
            this.rtgMain.GSBackColor = System.Drawing.Color.Black;
            this.rtgMain.GSBevelWidth = 3;
            this.rtgMain.GSBottom = 4;
            this.rtgMain.GSLeft = 4;
            this.rtgMain.GSRight = 4;
            this.rtgMain.GSTop = 4;
            this.rtgMain.Location = new System.Drawing.Point(12, 12);
            this.rtgMain.Name = "rtgMain";
            this.rtgMain.Size = new System.Drawing.Size(604, 216);
            this.rtgMain.TabIndex = 0;
            this.rtgMain.TitleFont = new System.Drawing.Font("Arial", 14F);
            this.rtgMain.TitleText = "Sine Wave Demo";
            this.rtgMain.WFColor = System.Drawing.Color.LimeGreen;
            this.rtgMain.WFThis = null;
            this.rtgMain.WFWidth = 2;
            this.rtgMain.XAxisDispMax = 5F;
            this.rtgMain.XAxisDispMin = 0F;
            this.rtgMain.XAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgMain.XAxisNumberFormat = "f2";
            this.rtgMain.XAxisTicLength = 5;
            this.rtgMain.XAxisTicWidth = 1;
            this.rtgMain.YAxisCaptionFont = new System.Drawing.Font("Arial", 12F);
            this.rtgMain.YAxisCaptionText = "Units";
            this.rtgMain.YAxisDispMax = 1F;
            this.rtgMain.YAxisDispMin = -1F;
            this.rtgMain.YAxisMaxNumTics = 5;
            this.rtgMain.YAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgMain.YAxisNumberFormat = "f2";
            this.rtgMain.YAxisTicLength = 5;
            this.rtgMain.YAxisTicWidth = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 295);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.rtgMain);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenMedIC.RTGraph rtgMain;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnStartStop;
    }
}

