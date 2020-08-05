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
            this.tabMainTabSet = new System.Windows.Forms.TabControl();
            this.tbpMain = new System.Windows.Forms.TabPage();
            this.rtGraph3 = new OpenMedIC.RTGraph();
            this.rtgFlow = new OpenMedIC.RTGraph();
            this.rtgHigh = new OpenMedIC.RTGraph();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblBufferRefill = new System.Windows.Forms.Label();
            this.lblTimingPause = new System.Windows.Forms.Label();
            this.lblExhalation = new System.Windows.Forms.Label();
            this.lblInhalation = new System.Windows.Forms.Label();
            this.lblPreVentilation = new System.Windows.Forms.Label();
            this.btnStartCycle = new System.Windows.Forms.Button();
            this.grpValveState = new System.Windows.Forms.GroupBox();
            this.btnDToggle = new System.Windows.Forms.Button();
            this.btnCToggle = new System.Windows.Forms.Button();
            this.btnBToggle = new System.Windows.Forms.Button();
            this.btnAToggle = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rtgLow = new OpenMedIC.RTGraph();
            this.tbpCal = new System.Windows.Forms.TabPage();
            this.btnCalComplete = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblBank = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnCalZero = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tmrStateLoop = new System.Windows.Forms.Timer(this.components);
            this.tbpTesting = new System.Windows.Forms.TabPage();
            this.ckbA = new System.Windows.Forms.CheckBox();
            this.ckbD = new System.Windows.Forms.CheckBox();
            this.ckbC = new System.Windows.Forms.CheckBox();
            this.ckbB = new System.Windows.Forms.CheckBox();
            this.grpbValveStates = new System.Windows.Forms.GroupBox();
            this.tabMainTabSet.SuspendLayout();
            this.tbpMain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpValveState.SuspendLayout();
            this.tbpCal.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tbpTesting.SuspendLayout();
            this.grpbValveStates.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMainTabSet
            // 
            this.tabMainTabSet.Controls.Add(this.tbpMain);
            this.tabMainTabSet.Controls.Add(this.tbpCal);
            this.tabMainTabSet.Controls.Add(this.tbpTesting);
            this.tabMainTabSet.Location = new System.Drawing.Point(12, 12);
            this.tabMainTabSet.Name = "tabMainTabSet";
            this.tabMainTabSet.SelectedIndex = 0;
            this.tabMainTabSet.Size = new System.Drawing.Size(1094, 589);
            this.tabMainTabSet.TabIndex = 27;
            // 
            // tbpMain
            // 
            this.tbpMain.Controls.Add(this.rtGraph3);
            this.tbpMain.Controls.Add(this.rtgFlow);
            this.tbpMain.Controls.Add(this.rtgHigh);
            this.tbpMain.Controls.Add(this.groupBox1);
            this.tbpMain.Controls.Add(this.textBox1);
            this.tbpMain.Controls.Add(this.lblBufferRefill);
            this.tbpMain.Controls.Add(this.lblTimingPause);
            this.tbpMain.Controls.Add(this.lblExhalation);
            this.tbpMain.Controls.Add(this.lblInhalation);
            this.tbpMain.Controls.Add(this.lblPreVentilation);
            this.tbpMain.Controls.Add(this.btnStartCycle);
            this.tbpMain.Controls.Add(this.grpValveState);
            this.tbpMain.Controls.Add(this.label2);
            this.tbpMain.Controls.Add(this.textBox2);
            this.tbpMain.Controls.Add(this.label1);
            this.tbpMain.Controls.Add(this.rtgLow);
            this.tbpMain.Location = new System.Drawing.Point(4, 22);
            this.tbpMain.Name = "tbpMain";
            this.tbpMain.Padding = new System.Windows.Forms.Padding(3);
            this.tbpMain.Size = new System.Drawing.Size(1086, 563);
            this.tbpMain.TabIndex = 1;
            this.tbpMain.Text = "Main";
            this.tbpMain.UseVisualStyleBackColor = true;
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
            this.rtGraph3.Location = new System.Drawing.Point(488, 241);
            this.rtGraph3.Name = "rtGraph3";
            this.rtGraph3.Size = new System.Drawing.Size(451, 220);
            this.rtGraph3.TabIndex = 30;
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
            this.rtgFlow.Location = new System.Drawing.Point(488, 6);
            this.rtgFlow.Name = "rtgFlow";
            this.rtgFlow.Size = new System.Drawing.Size(451, 220);
            this.rtgFlow.TabIndex = 28;
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
            this.rtgHigh.Location = new System.Drawing.Point(6, 6);
            this.rtgHigh.Name = "rtgHigh";
            this.rtgHigh.Size = new System.Drawing.Size(451, 220);
            this.rtgHigh.TabIndex = 27;
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
            this.rtgHigh.YAxisDispMax = 1000F;
            this.rtgHigh.YAxisDispMin = 0F;
            this.rtgHigh.YAxisMaxNumTics = 5;
            this.rtgHigh.YAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgHigh.YAxisNumberFormat = "f2";
            this.rtgHigh.YAxisTicLength = 5;
            this.rtgHigh.YAxisTicWidth = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(6, 476);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(103, 68);
            this.groupBox1.TabIndex = 43;
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
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(951, 343);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(43, 32);
            this.textBox1.TabIndex = 31;
            this.textBox1.Text = "XXX";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.Visible = false;
            // 
            // lblBufferRefill
            // 
            this.lblBufferRefill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBufferRefill.Location = new System.Drawing.Point(409, 516);
            this.lblBufferRefill.Name = "lblBufferRefill";
            this.lblBufferRefill.Size = new System.Drawing.Size(100, 23);
            this.lblBufferRefill.TabIndex = 39;
            this.lblBufferRefill.Text = "Buffer Refill";
            this.lblBufferRefill.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimingPause
            // 
            this.lblTimingPause.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTimingPause.Location = new System.Drawing.Point(515, 490);
            this.lblTimingPause.Name = "lblTimingPause";
            this.lblTimingPause.Size = new System.Drawing.Size(100, 23);
            this.lblTimingPause.TabIndex = 38;
            this.lblTimingPause.Text = "Timing Pause";
            this.lblTimingPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExhalation
            // 
            this.lblExhalation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblExhalation.Location = new System.Drawing.Point(409, 490);
            this.lblExhalation.Name = "lblExhalation";
            this.lblExhalation.Size = new System.Drawing.Size(100, 23);
            this.lblExhalation.TabIndex = 37;
            this.lblExhalation.Text = "Exhalation";
            this.lblExhalation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInhalation
            // 
            this.lblInhalation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInhalation.Location = new System.Drawing.Point(303, 490);
            this.lblInhalation.Name = "lblInhalation";
            this.lblInhalation.Size = new System.Drawing.Size(100, 23);
            this.lblInhalation.TabIndex = 36;
            this.lblInhalation.Text = "Inhalation";
            this.lblInhalation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPreVentilation
            // 
            this.lblPreVentilation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPreVentilation.Location = new System.Drawing.Point(197, 490);
            this.lblPreVentilation.Name = "lblPreVentilation";
            this.lblPreVentilation.Size = new System.Drawing.Size(100, 23);
            this.lblPreVentilation.TabIndex = 35;
            this.lblPreVentilation.Text = "Pre-Ventilation";
            this.lblPreVentilation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStartCycle
            // 
            this.btnStartCycle.Location = new System.Drawing.Point(115, 490);
            this.btnStartCycle.Name = "btnStartCycle";
            this.btnStartCycle.Size = new System.Drawing.Size(75, 23);
            this.btnStartCycle.TabIndex = 44;
            this.btnStartCycle.Text = "Start Cycle";
            this.btnStartCycle.UseVisualStyleBackColor = true;
            // 
            // grpValveState
            // 
            this.grpValveState.Controls.Add(this.btnDToggle);
            this.grpValveState.Controls.Add(this.btnCToggle);
            this.grpValveState.Controls.Add(this.btnBToggle);
            this.grpValveState.Controls.Add(this.btnAToggle);
            this.grpValveState.Location = new System.Drawing.Point(653, 476);
            this.grpValveState.Name = "grpValveState";
            this.grpValveState.Size = new System.Drawing.Size(168, 58);
            this.grpValveState.TabIndex = 40;
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
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(945, 439);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 18);
            this.label2.TabIndex = 34;
            this.label2.Text = "Tidal Vol";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(945, 404);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(70, 32);
            this.textBox2.TabIndex = 33;
            this.textBox2.Text = "YYY";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox2.Visible = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(951, 378);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 23);
            this.label1.TabIndex = 32;
            this.label1.Text = "FiO2";
            this.label1.Visible = false;
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
            this.rtgLow.Location = new System.Drawing.Point(6, 241);
            this.rtgLow.Name = "rtgLow";
            this.rtgLow.Size = new System.Drawing.Size(451, 220);
            this.rtgLow.TabIndex = 29;
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
            this.rtgLow.YAxisDispMax = 30F;
            this.rtgLow.YAxisDispMin = 0F;
            this.rtgLow.YAxisMaxNumTics = 5;
            this.rtgLow.YAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgLow.YAxisNumberFormat = "f2";
            this.rtgLow.YAxisTicLength = 5;
            this.rtgLow.YAxisTicWidth = 1;
            // 
            // tbpCal
            // 
            this.tbpCal.Controls.Add(this.btnCalComplete);
            this.tbpCal.Controls.Add(this.tableLayoutPanel1);
            this.tbpCal.Controls.Add(this.textBox3);
            this.tbpCal.Location = new System.Drawing.Point(4, 22);
            this.tbpCal.Name = "tbpCal";
            this.tbpCal.Padding = new System.Windows.Forms.Padding(3);
            this.tbpCal.Size = new System.Drawing.Size(1086, 563);
            this.tbpCal.TabIndex = 0;
            this.tbpCal.Text = "Calibrate";
            this.tbpCal.UseVisualStyleBackColor = true;
            // 
            // btnCalComplete
            // 
            this.btnCalComplete.Location = new System.Drawing.Point(204, 260);
            this.btnCalComplete.Name = "btnCalComplete";
            this.btnCalComplete.Size = new System.Drawing.Size(75, 23);
            this.btnCalComplete.TabIndex = 2;
            this.btnCalComplete.Text = "Complete";
            this.btnCalComplete.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblBank, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.button1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.button2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.button3, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnCalZero, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 56);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(315, 198);
            this.tableLayoutPanel1.TabIndex = 1;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // lblBank
            // 
            this.lblBank.BackColor = System.Drawing.Color.LightCyan;
            this.lblBank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBank.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBank.Location = new System.Drawing.Point(160, 3);
            this.lblBank.Margin = new System.Windows.Forms.Padding(3);
            this.lblBank.Name = "lblBank";
            this.lblBank.Size = new System.Drawing.Size(152, 33);
            this.lblBank.TabIndex = 10;
            this.lblBank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDown3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown3.Location = new System.Drawing.Point(3, 164);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(151, 26);
            this.numericUpDown3.TabIndex = 9;
            this.numericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown3.ThousandsSeparator = true;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown2.Location = new System.Drawing.Point(3, 120);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(151, 26);
            this.numericUpDown2.TabIndex = 8;
            this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown2.ThousandsSeparator = true;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 42);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 33);
            this.label4.TabIndex = 3;
            this.label4.Text = "Zero";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightCyan;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 33);
            this.label3.TabIndex = 2;
            this.label3.Text = "Flow";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown1.Location = new System.Drawing.Point(3, 84);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(150, 26);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.ThousandsSeparator = true;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.Location = new System.Drawing.Point(198, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Sample";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button2.Location = new System.Drawing.Point(198, 125);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Sample";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button3.Location = new System.Drawing.Point(198, 165);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Sample";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btnCalZero
            // 
            this.btnCalZero.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCalZero.Location = new System.Drawing.Point(198, 47);
            this.btnCalZero.Name = "btnCalZero";
            this.btnCalZero.Size = new System.Drawing.Size(75, 23);
            this.btnCalZero.TabIndex = 1;
            this.btnCalZero.Text = "Sample";
            this.btnCalZero.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(6, 6);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(315, 44);
            this.textBox3.TabIndex = 0;
            this.textBox3.Text = "Calibrate by sampling known flow at zero plus three other points";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tmrStateLoop
            // 
            this.tmrStateLoop.Interval = 50;
            // 
            // tbpTesting
            // 
            this.tbpTesting.Controls.Add(this.grpbValveStates);
            this.tbpTesting.Location = new System.Drawing.Point(4, 22);
            this.tbpTesting.Name = "tbpTesting";
            this.tbpTesting.Padding = new System.Windows.Forms.Padding(3);
            this.tbpTesting.Size = new System.Drawing.Size(1086, 563);
            this.tbpTesting.TabIndex = 2;
            this.tbpTesting.Text = "Testing";
            this.tbpTesting.UseVisualStyleBackColor = true;
            // 
            // ckbA
            // 
            this.ckbA.AutoSize = true;
            this.ckbA.Location = new System.Drawing.Point(24, 32);
            this.ckbA.Name = "ckbA";
            this.ckbA.Size = new System.Drawing.Size(90, 17);
            this.ckbA.TabIndex = 0;
            this.ckbA.Text = "Valve A open";
            this.ckbA.UseVisualStyleBackColor = true;
            this.ckbA.CheckedChanged += new System.EventHandler(this.ckbA_CheckedChanged);
            // 
            // ckbD
            // 
            this.ckbD.AutoSize = true;
            this.ckbD.Location = new System.Drawing.Point(24, 101);
            this.ckbD.Name = "ckbD";
            this.ckbD.Size = new System.Drawing.Size(91, 17);
            this.ckbD.TabIndex = 1;
            this.ckbD.Text = "Valve D open";
            this.ckbD.UseVisualStyleBackColor = true;
            // 
            // ckbC
            // 
            this.ckbC.AutoSize = true;
            this.ckbC.Location = new System.Drawing.Point(24, 78);
            this.ckbC.Name = "ckbC";
            this.ckbC.Size = new System.Drawing.Size(90, 17);
            this.ckbC.TabIndex = 2;
            this.ckbC.Text = "Valve C open";
            this.ckbC.UseVisualStyleBackColor = true;
            // 
            // ckbB
            // 
            this.ckbB.AutoSize = true;
            this.ckbB.Location = new System.Drawing.Point(24, 55);
            this.ckbB.Name = "ckbB";
            this.ckbB.Size = new System.Drawing.Size(90, 17);
            this.ckbB.TabIndex = 3;
            this.ckbB.Text = "Valve B open";
            this.ckbB.UseVisualStyleBackColor = true;
            // 
            // grpbValveStates
            // 
            this.grpbValveStates.Controls.Add(this.ckbA);
            this.grpbValveStates.Controls.Add(this.ckbB);
            this.grpbValveStates.Controls.Add(this.ckbD);
            this.grpbValveStates.Controls.Add(this.ckbC);
            this.grpbValveStates.Location = new System.Drawing.Point(40, 32);
            this.grpbValveStates.Name = "grpbValveStates";
            this.grpbValveStates.Size = new System.Drawing.Size(139, 141);
            this.grpbValveStates.TabIndex = 4;
            this.grpbValveStates.TabStop = false;
            this.grpbValveStates.Text = "Valve States";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1119, 614);
            this.Controls.Add(this.tabMainTabSet);
            this.Name = "frmMain";
            this.Text = "DzhamVent";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabMainTabSet.ResumeLayout(false);
            this.tbpMain.ResumeLayout(false);
            this.tbpMain.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpValveState.ResumeLayout(false);
            this.tbpCal.ResumeLayout(false);
            this.tbpCal.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tbpTesting.ResumeLayout(false);
            this.grpbValveStates.ResumeLayout(false);
            this.grpbValveStates.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMainTabSet;
        private System.Windows.Forms.TabPage tbpCal;
        private System.Windows.Forms.TabPage tbpMain;
        private OpenMedIC.RTGraph rtGraph3;
        private OpenMedIC.RTGraph rtgFlow;
        private OpenMedIC.RTGraph rtgHigh;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblBufferRefill;
        private System.Windows.Forms.Label lblTimingPause;
        private System.Windows.Forms.Label lblExhalation;
        private System.Windows.Forms.Label lblInhalation;
        private System.Windows.Forms.Label lblPreVentilation;
        private System.Windows.Forms.Button btnStartCycle;
        private System.Windows.Forms.GroupBox grpValveState;
        private System.Windows.Forms.Button btnDToggle;
        private System.Windows.Forms.Button btnCToggle;
        private System.Windows.Forms.Button btnBToggle;
        private System.Windows.Forms.Button btnAToggle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private OpenMedIC.RTGraph rtgLow;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer tmrStateLoop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCalZero;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btnCalComplete;
        private System.Windows.Forms.Label lblBank;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabPage tbpTesting;
        private System.Windows.Forms.CheckBox ckbA;
        private System.Windows.Forms.GroupBox grpbValveStates;
        private System.Windows.Forms.CheckBox ckbB;
        private System.Windows.Forms.CheckBox ckbD;
        private System.Windows.Forms.CheckBox ckbC;
    }
}

