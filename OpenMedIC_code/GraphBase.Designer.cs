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

namespace OpenMedIC
{
    partial class GraphBase
    {

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.txtXAxisDispMax = new System.Windows.Forms.TextBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.txtYAxisDispMin = new System.Windows.Forms.TextBox();
			this.txtYAxisDispMax = new System.Windows.Forms.TextBox();
			this.pnlGraphingDisplay = new System.Windows.Forms.Panel();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.txtXAxisDispMin = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// txtXAxisDispMax
			// 
			this.txtXAxisDispMax.BackColor = System.Drawing.SystemColors.Control;
			this.txtXAxisDispMax.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtXAxisDispMax.Location = new System.Drawing.Point(294, 169);
			this.txtXAxisDispMax.Name = "txtXAxisDispMax";
			this.txtXAxisDispMax.Size = new System.Drawing.Size(40, 13);
			this.txtXAxisDispMax.TabIndex = 4;
			this.txtXAxisDispMax.Text = "XAxisDispMax";
			this.txtXAxisDispMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtXAxisDispMax.Enter += new System.EventHandler(this.txtXAxisDispMax_Enter);
			this.txtXAxisDispMax.Leave += new System.EventHandler(this.txtXAxisDispMax_Leave);
			this.txtXAxisDispMax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtXAxisDispMax_KeyPress);
			// 
			// txtTitle
			// 
			this.txtTitle.BackColor = System.Drawing.SystemColors.Control;
			this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtTitle.Location = new System.Drawing.Point(44, 23);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtTitle.Size = new System.Drawing.Size(495, 13);
			this.txtTitle.TabIndex = 0;
			this.txtTitle.Text = "Title Text";
			this.txtTitle.WordWrap = false;
			this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
			// 
			// txtYAxisDispMin
			// 
			this.txtYAxisDispMin.BackColor = System.Drawing.SystemColors.Control;
			this.txtYAxisDispMin.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtYAxisDispMin.Location = new System.Drawing.Point(67, 151);
			this.txtYAxisDispMin.Name = "txtYAxisDispMin";
			this.txtYAxisDispMin.Size = new System.Drawing.Size(112, 13);
			this.txtYAxisDispMin.TabIndex = 2;
			this.txtYAxisDispMin.Text = "YAxisDispMin";
			this.txtYAxisDispMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtYAxisDispMin.Enter += new System.EventHandler(this.txtYAxisDispMin_Enter);
			this.txtYAxisDispMin.Leave += new System.EventHandler(this.txtYAxisDispMin_Leave);
			this.txtYAxisDispMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtYAxisDispMin_KeyPress);
			// 
			// txtYAxisDispMax
			// 
			this.txtYAxisDispMax.BackColor = System.Drawing.SystemColors.Control;
			this.txtYAxisDispMax.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtYAxisDispMax.Location = new System.Drawing.Point(75, 63);
			this.txtYAxisDispMax.Name = "txtYAxisDispMax";
			this.txtYAxisDispMax.Size = new System.Drawing.Size(96, 13);
			this.txtYAxisDispMax.TabIndex = 1;
			this.txtYAxisDispMax.Text = "YAxisDispMax";
			this.txtYAxisDispMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtYAxisDispMax.Enter += new System.EventHandler(this.txtYAxisDispMax_Enter);
			this.txtYAxisDispMax.Leave += new System.EventHandler(this.txtYAxisDispMax_Leave);
			this.txtYAxisDispMax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtYAxisDispMax_KeyPress);
			// 
			// pnlGraphingDisplay
			// 
			this.pnlGraphingDisplay.BackColor = System.Drawing.Color.Black;
			this.pnlGraphingDisplay.Location = new System.Drawing.Point(187, 63);
			this.pnlGraphingDisplay.Name = "pnlGraphingDisplay";
			this.pnlGraphingDisplay.Size = new System.Drawing.Size(147, 100);
			this.pnlGraphingDisplay.TabIndex = 6;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// txtXAxisDispMin
			// 
			this.txtXAxisDispMin.BackColor = System.Drawing.SystemColors.Control;
			this.txtXAxisDispMin.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtXAxisDispMin.Location = new System.Drawing.Point(187, 169);
			this.txtXAxisDispMin.Name = "txtXAxisDispMin";
			this.txtXAxisDispMin.Size = new System.Drawing.Size(40, 13);
			this.txtXAxisDispMin.TabIndex = 3;
			this.txtXAxisDispMin.Text = "XAxisDispMin";
			this.txtXAxisDispMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtXAxisDispMin.Enter += new System.EventHandler(this.txtXAxisDispMin_Enter);
			this.txtXAxisDispMin.Leave += new System.EventHandler(this.txtXAxisDispMin_Leave);
			this.txtXAxisDispMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtXAxisDispMin_KeyPress);
			// 
			// GraphBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtXAxisDispMin);
			this.Controls.Add(this.txtXAxisDispMax);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.txtYAxisDispMin);
			this.Controls.Add(this.txtYAxisDispMax);
			this.Controls.Add(this.pnlGraphingDisplay);
			this.Name = "GraphBase";
			this.Size = new System.Drawing.Size(586, 247);
			this.Resize += new System.EventHandler(this.GraphBase_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphBase_Paint);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.TextBox txtXAxisDispMax;
        protected System.Windows.Forms.TextBox txtTitle;
        protected System.Windows.Forms.TextBox txtYAxisDispMin;
        protected System.Windows.Forms.TextBox txtYAxisDispMax;
        protected System.Windows.Forms.Panel pnlGraphingDisplay;
        protected System.Windows.Forms.ErrorProvider errorProvider1;
        protected System.Windows.Forms.TextBox txtXAxisDispMin;
        private System.ComponentModel.IContainer components;
    }
}
