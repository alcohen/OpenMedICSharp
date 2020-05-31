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
            this.rtgHigh = new OpenMedIC.RTGraph();
            this.rtGraph1 = new OpenMedIC.RTGraph();
            this.rtgLow = new OpenMedIC.RTGraph();
            this.rtGraph3 = new OpenMedIC.RTGraph();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnAToggle = new System.Windows.Forms.Button();
            this.btnBToggle = new System.Windows.Forms.Button();
            this.btnCToggle = new System.Windows.Forms.Button();
            this.btnDToggle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
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
            this.rtgHigh.YAxisDispMax = 1300F;
            this.rtgHigh.YAxisDispMin = 950F;
            this.rtgHigh.YAxisMaxNumTics = 5;
            this.rtgHigh.YAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgHigh.YAxisNumberFormat = "f2";
            this.rtgHigh.YAxisTicLength = 5;
            this.rtgHigh.YAxisTicWidth = 1;
            // 
            // rtGraph1
            // 
            this.rtGraph1.AutoRedraw = true;
            this.rtGraph1.BorderBevelWidth = 4;
            this.rtGraph1.BorderMargin = 5;
            this.rtGraph1.CursColor = System.Drawing.Color.Blue;
            this.rtGraph1.CursForeWidth = 3;
            this.rtGraph1.CursWidth = 1;
            this.rtGraph1.GSBackColor = System.Drawing.Color.Black;
            this.rtGraph1.GSBevelWidth = 3;
            this.rtGraph1.GSBottom = 4;
            this.rtGraph1.GSLeft = 4;
            this.rtGraph1.GSRight = 4;
            this.rtGraph1.GSTop = 4;
            this.rtGraph1.Location = new System.Drawing.Point(494, 16);
            this.rtGraph1.Name = "rtGraph1";
            this.rtGraph1.Size = new System.Drawing.Size(451, 216);
            this.rtGraph1.TabIndex = 2;
            this.rtGraph1.TitleFont = new System.Drawing.Font("Arial", 14F);
            this.rtGraph1.TitleText = "P/V";
            this.rtGraph1.WFColor = System.Drawing.Color.LimeGreen;
            this.rtGraph1.WFThis = null;
            this.rtGraph1.WFWidth = 2;
            this.rtGraph1.XAxisDispMax = 20F;
            this.rtGraph1.XAxisDispMin = 0F;
            this.rtGraph1.XAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtGraph1.XAxisNumberFormat = "f2";
            this.rtGraph1.XAxisTicLength = 5;
            this.rtGraph1.XAxisTicWidth = 1;
            this.rtGraph1.YAxisCaptionFont = new System.Drawing.Font("Arial", 12F);
            this.rtGraph1.YAxisCaptionText = "Units";
            this.rtGraph1.YAxisDispMax = 100F;
            this.rtGraph1.YAxisDispMin = 0F;
            this.rtGraph1.YAxisMaxNumTics = 5;
            this.rtGraph1.YAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtGraph1.YAxisNumberFormat = "f2";
            this.rtGraph1.YAxisTicLength = 5;
            this.rtGraph1.YAxisTicWidth = 1;
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
            this.rtgLow.TitleText = "Flow";
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
            this.rtgLow.YAxisDispMax = 1300F;
            this.rtgLow.YAxisDispMin = 950F;
            this.rtgLow.YAxisMaxNumTics = 5;
            this.rtgLow.YAxisNumberFont = new System.Drawing.Font("Arial", 10F);
            this.rtgLow.YAxisNumberFormat = "f2";
            this.rtgLow.YAxisTicLength = 5;
            this.rtgLow.YAxisTicWidth = 1;
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
            this.rtGraph3.Location = new System.Drawing.Point(494, 251);
            this.rtGraph3.Name = "rtGraph3";
            this.rtGraph3.Size = new System.Drawing.Size(451, 216);
            this.rtGraph3.TabIndex = 4;
            this.rtGraph3.TitleFont = new System.Drawing.Font("Arial", 14F);
            this.rtGraph3.TitleText = "F/V";
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
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 487);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(43, 32);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "XXX";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 522);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "FiO2";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(84, 522);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "Tidal Vol";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(84, 487);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(70, 32);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "YYY";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btnAToggle
            // 
            this.btnAToggle.Location = new System.Drawing.Point(494, 487);
            this.btnAToggle.Name = "btnAToggle";
            this.btnAToggle.Size = new System.Drawing.Size(35, 23);
            this.btnAToggle.TabIndex = 9;
            this.btnAToggle.Text = "A";
            this.btnAToggle.UseVisualStyleBackColor = true;
            this.btnAToggle.Click += new System.EventHandler(this.button_Click);
            // 
            // btnBToggle
            // 
            this.btnBToggle.Location = new System.Drawing.Point(535, 487);
            this.btnBToggle.Name = "btnBToggle";
            this.btnBToggle.Size = new System.Drawing.Size(35, 23);
            this.btnBToggle.TabIndex = 10;
            this.btnBToggle.Text = "B";
            this.btnBToggle.UseVisualStyleBackColor = true;
            this.btnBToggle.Click += new System.EventHandler(this.button_Click);
            // 
            // btnCToggle
            // 
            this.btnCToggle.Location = new System.Drawing.Point(576, 487);
            this.btnCToggle.Name = "btnCToggle";
            this.btnCToggle.Size = new System.Drawing.Size(35, 23);
            this.btnCToggle.TabIndex = 11;
            this.btnCToggle.Text = "C";
            this.btnCToggle.UseVisualStyleBackColor = true;
            this.btnCToggle.Click += new System.EventHandler(this.button_Click);
            // 
            // btnDToggle
            // 
            this.btnDToggle.Location = new System.Drawing.Point(617, 487);
            this.btnDToggle.Name = "btnDToggle";
            this.btnDToggle.Size = new System.Drawing.Size(35, 23);
            this.btnDToggle.TabIndex = 12;
            this.btnDToggle.Text = "D";
            this.btnDToggle.UseVisualStyleBackColor = true;
            this.btnDToggle.Click += new System.EventHandler(this.button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 580);
            this.Controls.Add(this.btnDToggle);
            this.Controls.Add(this.btnCToggle);
            this.Controls.Add(this.btnBToggle);
            this.Controls.Add(this.btnAToggle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.rtGraph3);
            this.Controls.Add(this.rtgLow);
            this.Controls.Add(this.rtGraph1);
            this.Controls.Add(this.rtgHigh);
            this.Name = "Form1";
            this.Text = "DzhamVent";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenMedIC.RTGraph rtgHigh;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private OpenMedIC.RTGraph rtGraph1;
        private OpenMedIC.RTGraph rtgLow;
        private OpenMedIC.RTGraph rtGraph3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnAToggle;
        private System.Windows.Forms.Button btnBToggle;
        private System.Windows.Forms.Button btnCToggle;
        private System.Windows.Forms.Button btnDToggle;
    }
}

