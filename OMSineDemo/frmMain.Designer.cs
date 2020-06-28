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
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.rtGraph3 = new OpenMedIC.RTGraph();
            this.rtgLow = new OpenMedIC.RTGraph();
            this.rtgFlow = new OpenMedIC.RTGraph();
            this.rtgHigh = new OpenMedIC.RTGraph();
            this.tmrStateLoop = new System.Windows.Forms.Timer(this.components);
            this.lblPreVentilation = new System.Windows.Forms.Label();
            this.lblInhalation = new System.Windows.Forms.Label();
            this.lblExhalation = new System.Windows.Forms.Label();
            this.lblTimingPause = new System.Windows.Forms.Label();
            this.lblBufferRefill = new System.Windows.Forms.Label();
            this.grpValveState = new System.Windows.Forms.GroupBox();
            this.btnDToggle = new System.Windows.Forms.Button();
            this.btnCToggle = new System.Windows.Forms.Button();
            this.btnBToggle = new System.Windows.Forms.Button();
            this.btnAToggle = new System.Windows.Forms.Button();
            this.btnZero = new System.Windows.Forms.Button();
            this.btnFlow = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.btnStartCycle = new System.Windows.Forms.Button();
            this.grpValveState.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(957, 349);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(43, 32);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "XXX";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.Visible = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(957, 384);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "FiO2";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(951, 445);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "Tidal Vol";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(951, 410);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(70, 32);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "YYY";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox2.Visible = false;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // rtGraph3
            // 
            this.rtGraph3.AutoRedraw = true;
            this.rtGraph3.BorderBevelWidth = 4;
            this.rtGraph3.BorderMargin = 5;
            this.rtGraph3.CursColor = System.Drawing.Color.Blue;
            this.rtGraph3.CursForeWidth = 3;
            this.rtGraph3.CursWidth = 1;
            this.rtGraph3.GSBackColor = System.Drawing.Color.Black;
            this.rtGraph3.GSBevelWidth = 3;
            this.rtGraph3.GSBottom = 4;
            this.rtGraph3.GSLeft = 4;
            this.rtGraph3.GSRight = 4;
            this.rtGraph3.GSTop = 4;
            this.rtGraph3.Location = new System.Drawing.Point(494, 247);
            this.rtGraph3.Name = "rtGraph3";
            this.rtGraph3.Size = new System.Drawing.Size(451, 220);
            this.rtGraph3.TabIndex = 4;
            this.rtGraph3.TitleFont = new System.Drawing.Font("Arial", 14F);
            this.rtGraph3.TitleText = "F/V";
            this.rtGraph3.Visible = false;
            this.rtGraph3.WFColor = System.Drawing.Color.LimeGreen;
            this.rtGraph3.WFThis = null;
            this.rtGraph3.WFWidth = 2;
            this.rtGraph3.XAxisDispMax = 20F;
            this.rtGraph3.XAxisDispMin = 0F;
            this.rtGraph3.XAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtGraph3.XAxisNumberFormat = "f2";
            this.rtGraph3.XAxisTicLength = 5;
            this.rtGraph3.XAxisTicWidth = 1;
            this.rtGraph3.YAxisCaptionFont = new System.Drawing.Font("Arial", 12F);
            this.rtGraph3.YAxisCaptionText = "Units";
            this.rtGraph3.YAxisDispMax = 100F;
            this.rtGraph3.YAxisDispMin = 0F;
            this.rtGraph3.YAxisMaxNumTics = 5;
            this.rtGraph3.YAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtGraph3.YAxisNumberFormat = "f2";
            this.rtGraph3.YAxisTicLength = 5;
            this.rtGraph3.YAxisTicWidth = 1;
            // 
            // rtgLow
            // 
            this.rtgLow.AutoRedraw = true;
            this.rtgLow.BorderBevelWidth = 4;
            this.rtgLow.BorderMargin = 5;
            this.rtgLow.CursColor = System.Drawing.Color.Blue;
            this.rtgLow.CursForeWidth = 10;
            this.rtgLow.CursWidth = 1;
            this.rtgLow.GSBackColor = System.Drawing.Color.Black;
            this.rtgLow.GSBevelWidth = 3;
            this.rtgLow.GSBottom = 4;
            this.rtgLow.GSLeft = 4;
            this.rtgLow.GSRight = 4;
            this.rtgLow.GSTop = 4;
            this.rtgLow.Location = new System.Drawing.Point(12, 247);
            this.rtgLow.Name = "rtgLow";
            this.rtgLow.Size = new System.Drawing.Size(451, 220);
            this.rtgLow.TabIndex = 3;
            this.rtgLow.TitleFont = new System.Drawing.Font("Arial", 14F);
            this.rtgLow.TitleText = "Pressure Low";
            this.rtgLow.WFColor = System.Drawing.Color.LimeGreen;
            this.rtgLow.WFThis = null;
            this.rtgLow.WFWidth = 3;
            this.rtgLow.XAxisDispMax = 10F;
            this.rtgLow.XAxisDispMin = 0F;
            this.rtgLow.XAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgLow.XAxisNumberFormat = "f2";
            this.rtgLow.XAxisTicLength = 5;
            this.rtgLow.XAxisTicWidth = 1;
            this.rtgLow.YAxisCaptionFont = new System.Drawing.Font("Arial", 12F);
            this.rtgLow.YAxisCaptionText = "Units";
            this.rtgLow.YAxisDispMax = 100F;
            this.rtgLow.YAxisDispMin = 0F;
            this.rtgLow.YAxisMaxNumTics = 5;
            this.rtgLow.YAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgLow.YAxisNumberFormat = "f2";
            this.rtgLow.YAxisTicLength = 5;
            this.rtgLow.YAxisTicWidth = 1;
            // 
            // rtgFlow
            // 
            this.rtgFlow.AutoRedraw = true;
            this.rtgFlow.BorderBevelWidth = 4;
            this.rtgFlow.BorderMargin = 5;
            this.rtgFlow.CursColor = System.Drawing.Color.Blue;
            this.rtgFlow.CursForeWidth = 3;
            this.rtgFlow.CursWidth = 1;
            this.rtgFlow.GSBackColor = System.Drawing.Color.Black;
            this.rtgFlow.GSBevelWidth = 3;
            this.rtgFlow.GSBottom = 4;
            this.rtgFlow.GSLeft = 4;
            this.rtgFlow.GSRight = 4;
            this.rtgFlow.GSTop = 4;
            this.rtgFlow.Location = new System.Drawing.Point(494, 12);
            this.rtgFlow.Name = "rtgFlow";
            this.rtgFlow.Size = new System.Drawing.Size(451, 220);
            this.rtgFlow.TabIndex = 2;
            this.rtgFlow.TitleFont = new System.Drawing.Font("Arial", 14F);
            this.rtgFlow.TitleText = "Flow";
            this.rtgFlow.WFColor = System.Drawing.Color.LimeGreen;
            this.rtgFlow.WFThis = null;
            this.rtgFlow.WFWidth = 2;
            this.rtgFlow.XAxisDispMax = 20F;
            this.rtgFlow.XAxisDispMin = 0F;
            this.rtgFlow.XAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgFlow.XAxisNumberFormat = "f2";
            this.rtgFlow.XAxisTicLength = 5;
            this.rtgFlow.XAxisTicWidth = 1;
            this.rtgFlow.YAxisCaptionFont = new System.Drawing.Font("Arial", 12F);
            this.rtgFlow.YAxisCaptionText = "Units";
            this.rtgFlow.YAxisDispMax = 100F;
            this.rtgFlow.YAxisDispMin = 0F;
            this.rtgFlow.YAxisMaxNumTics = 5;
            this.rtgFlow.YAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgFlow.YAxisNumberFormat = "f2";
            this.rtgFlow.YAxisTicLength = 5;
            this.rtgFlow.YAxisTicWidth = 1;
            // 
            // rtgHigh
            // 
            this.rtgHigh.AutoRedraw = true;
            this.rtgHigh.BorderBevelWidth = 4;
            this.rtgHigh.BorderMargin = 5;
            this.rtgHigh.CursColor = System.Drawing.Color.Blue;
            this.rtgHigh.CursForeWidth = 10;
            this.rtgHigh.CursWidth = 1;
            this.rtgHigh.GSBackColor = System.Drawing.Color.Black;
            this.rtgHigh.GSBevelWidth = 3;
            this.rtgHigh.GSBottom = 4;
            this.rtgHigh.GSLeft = 4;
            this.rtgHigh.GSRight = 4;
            this.rtgHigh.GSTop = 4;
            this.rtgHigh.Location = new System.Drawing.Point(12, 12);
            this.rtgHigh.Name = "rtgHigh";
            this.rtgHigh.Size = new System.Drawing.Size(451, 220);
            this.rtgHigh.TabIndex = 0;
            this.rtgHigh.TitleFont = new System.Drawing.Font("Arial", 14F);
            this.rtgHigh.TitleText = "Pressure High";
            this.rtgHigh.WFColor = System.Drawing.Color.LimeGreen;
            this.rtgHigh.WFThis = null;
            this.rtgHigh.WFWidth = 3;
            this.rtgHigh.XAxisDispMax = 10F;
            this.rtgHigh.XAxisDispMin = 0F;
            this.rtgHigh.XAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgHigh.XAxisNumberFormat = "f2";
            this.rtgHigh.XAxisTicLength = 5;
            this.rtgHigh.XAxisTicWidth = 1;
            this.rtgHigh.YAxisCaptionFont = new System.Drawing.Font("Arial", 12F);
            this.rtgHigh.YAxisCaptionText = "Units";
            this.rtgHigh.YAxisDispMax = 100F;
            this.rtgHigh.YAxisDispMin = 0F;
            this.rtgHigh.YAxisMaxNumTics = 5;
            this.rtgHigh.YAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgHigh.YAxisNumberFormat = "f2";
            this.rtgHigh.YAxisTicLength = 5;
            this.rtgHigh.YAxisTicWidth = 1;
            // 
            // tmrStateLoop
            // 
            this.tmrStateLoop.Interval = 50;
            this.tmrStateLoop.Tick += new System.EventHandler(this.tmrStateLoop_Tick);
            // 
            // lblPreVentilation
            // 
            this.lblPreVentilation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPreVentilation.Location = new System.Drawing.Point(203, 496);
            this.lblPreVentilation.Name = "lblPreVentilation";
            this.lblPreVentilation.Size = new System.Drawing.Size(100, 23);
            this.lblPreVentilation.TabIndex = 15;
            this.lblPreVentilation.Text = "Pre-Ventilation";
            this.lblPreVentilation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInhalation
            // 
            this.lblInhalation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInhalation.Location = new System.Drawing.Point(309, 496);
            this.lblInhalation.Name = "lblInhalation";
            this.lblInhalation.Size = new System.Drawing.Size(100, 23);
            this.lblInhalation.TabIndex = 16;
            this.lblInhalation.Text = "Inhalation";
            this.lblInhalation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExhalation
            // 
            this.lblExhalation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblExhalation.Location = new System.Drawing.Point(415, 496);
            this.lblExhalation.Name = "lblExhalation";
            this.lblExhalation.Size = new System.Drawing.Size(100, 23);
            this.lblExhalation.TabIndex = 17;
            this.lblExhalation.Text = "Exhalation";
            this.lblExhalation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimingPause
            // 
            this.lblTimingPause.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTimingPause.Location = new System.Drawing.Point(521, 496);
            this.lblTimingPause.Name = "lblTimingPause";
            this.lblTimingPause.Size = new System.Drawing.Size(100, 23);
            this.lblTimingPause.TabIndex = 18;
            this.lblTimingPause.Text = "Timing Pause";
            this.lblTimingPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBufferRefill
            // 
            this.lblBufferRefill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBufferRefill.Location = new System.Drawing.Point(415, 522);
            this.lblBufferRefill.Name = "lblBufferRefill";
            this.lblBufferRefill.Size = new System.Drawing.Size(100, 23);
            this.lblBufferRefill.TabIndex = 19;
            this.lblBufferRefill.Text = "Buffer Refill";
            this.lblBufferRefill.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpValveState
            // 
            this.grpValveState.Controls.Add(this.btnDToggle);
            this.grpValveState.Controls.Add(this.btnCToggle);
            this.grpValveState.Controls.Add(this.btnBToggle);
            this.grpValveState.Controls.Add(this.btnAToggle);
            this.grpValveState.Location = new System.Drawing.Point(659, 482);
            this.grpValveState.Name = "grpValveState";
            this.grpValveState.Size = new System.Drawing.Size(168, 58);
            this.grpValveState.TabIndex = 20;
            this.grpValveState.TabStop = false;
            this.grpValveState.Text = "Valve States";
            // 
            // btnDToggle
            // 
            this.btnDToggle.Location = new System.Drawing.Point(129, 19);
            this.btnDToggle.Name = "btnDToggle";
            this.btnDToggle.Size = new System.Drawing.Size(35, 23);
            this.btnDToggle.TabIndex = 16;
            this.btnDToggle.Text = "D";
            this.btnDToggle.UseVisualStyleBackColor = true;
            // 
            // btnCToggle
            // 
            this.btnCToggle.Location = new System.Drawing.Point(88, 19);
            this.btnCToggle.Name = "btnCToggle";
            this.btnCToggle.Size = new System.Drawing.Size(35, 23);
            this.btnCToggle.TabIndex = 15;
            this.btnCToggle.Text = "C";
            this.btnCToggle.UseVisualStyleBackColor = true;
            // 
            // btnBToggle
            // 
            this.btnBToggle.Location = new System.Drawing.Point(47, 19);
            this.btnBToggle.Name = "btnBToggle";
            this.btnBToggle.Size = new System.Drawing.Size(35, 23);
            this.btnBToggle.TabIndex = 14;
            this.btnBToggle.Text = "B";
            this.btnBToggle.UseVisualStyleBackColor = true;
            // 
            // btnAToggle
            // 
            this.btnAToggle.Location = new System.Drawing.Point(6, 19);
            this.btnAToggle.Name = "btnAToggle";
            this.btnAToggle.Size = new System.Drawing.Size(35, 23);
            this.btnAToggle.TabIndex = 13;
            this.btnAToggle.Text = "A";
            this.btnAToggle.UseVisualStyleBackColor = true;
            // 
            // btnZero
            // 
            this.btnZero.Location = new System.Drawing.Point(959, 17);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(113, 23);
            this.btnZero.TabIndex = 21;
            this.btnZero.Text = "Zero Pressures";
            this.btnZero.UseVisualStyleBackColor = true;
            // 
            // btnFlow
            // 
            this.btnFlow.Location = new System.Drawing.Point(959, 46);
            this.btnFlow.Name = "btnFlow";
            this.btnFlow.Size = new System.Drawing.Size(113, 23);
            this.btnFlow.TabIndex = 22;
            this.btnFlow.Text = "Zero Flow";
            this.btnFlow.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(12, 482);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(103, 68);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Run Mode";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(17, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(72, 17);
            this.radioButton2.TabIndex = 26;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Automatic";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(17, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(60, 17);
            this.radioButton1.TabIndex = 25;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Manual";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // btnStartCycle
            // 
            this.btnStartCycle.Location = new System.Drawing.Point(121, 496);
            this.btnStartCycle.Name = "btnStartCycle";
            this.btnStartCycle.Size = new System.Drawing.Size(75, 23);
            this.btnStartCycle.TabIndex = 26;
            this.btnStartCycle.Text = "Start Cycle";
            this.btnStartCycle.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 559);
            this.Controls.Add(this.btnStartCycle);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnFlow);
            this.Controls.Add(this.btnZero);
            this.Controls.Add(this.grpValveState);
            this.Controls.Add(this.lblBufferRefill);
            this.Controls.Add(this.lblTimingPause);
            this.Controls.Add(this.lblExhalation);
            this.Controls.Add(this.lblInhalation);
            this.Controls.Add(this.lblPreVentilation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.rtGraph3);
            this.Controls.Add(this.rtgLow);
            this.Controls.Add(this.rtgFlow);
            this.Controls.Add(this.rtgHigh);
            this.Name = "frmMain";
            this.Text = "DzhamVent";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpValveState.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenMedIC.RTGraph rtgHigh;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private OpenMedIC.RTGraph rtgFlow;
        private OpenMedIC.RTGraph rtgLow;
        private OpenMedIC.RTGraph rtGraph3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Timer tmrStateLoop;
        private System.Windows.Forms.Label lblPreVentilation;
        private System.Windows.Forms.Label lblInhalation;
        private System.Windows.Forms.Label lblExhalation;
        private System.Windows.Forms.Label lblTimingPause;
        private System.Windows.Forms.Label lblBufferRefill;
        private System.Windows.Forms.GroupBox grpValveState;
        private System.Windows.Forms.Button btnDToggle;
        private System.Windows.Forms.Button btnCToggle;
        private System.Windows.Forms.Button btnBToggle;
        private System.Windows.Forms.Button btnAToggle;
        private System.Windows.Forms.Button btnZero;
        private System.Windows.Forms.Button btnFlow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button btnStartCycle;
    }
}

