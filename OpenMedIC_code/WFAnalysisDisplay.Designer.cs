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
    partial class WFAnalysisDisplay
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtXAxisDispMax
            // 
            this.txtXAxisDispMax.Font = new System.Drawing.Font("Arial", 10F);
            this.txtXAxisDispMax.Location = new System.Drawing.Point(587, 181);
            this.txtXAxisDispMax.Size = new System.Drawing.Size(40, 16);
            this.txtXAxisDispMax.Text = "20.00";
            this.txtXAxisDispMax.Leave += new System.EventHandler(this.txtXAxisDispMax_Leave);
            this.txtXAxisDispMax.TextChanged += new System.EventHandler(this.txtXAxisDispMax_TextChanged);
            // 
            // txtTitle
            // 
            this.txtTitle.Font = new System.Drawing.Font("Arial", 14F);
            this.txtTitle.Location = new System.Drawing.Point(5, 5);
            this.txtTitle.Size = new System.Drawing.Size(304, 22);
            this.txtTitle.Text = "Title Here";
            // 
            // txtYAxisDispMin
            // 
            this.txtYAxisDispMin.Font = new System.Drawing.Font("Arial", 10F);
            this.txtYAxisDispMin.Location = new System.Drawing.Point(15, 165);
            this.txtYAxisDispMin.Size = new System.Drawing.Size(57, 16);
            this.txtYAxisDispMin.Text = "0.00";
            // 
            // txtYAxisDispMax
            // 
            this.txtYAxisDispMax.Font = new System.Drawing.Font("Arial", 10F);
            this.txtYAxisDispMax.Location = new System.Drawing.Point(15, 26);
            this.txtYAxisDispMax.Size = new System.Drawing.Size(57, 16);
            this.txtYAxisDispMax.Text = "100.00";
            // 
            // pnlGraphingDisplay
            // 
            this.pnlGraphingDisplay.Location = new System.Drawing.Point(80, 34);
            this.pnlGraphingDisplay.Size = new System.Drawing.Size(547, 139);
            this.pnlGraphingDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlGraphingDisplay_MouseMove);
            // 
            // txtXAxisDispMin
            // 
            this.txtXAxisDispMin.Font = new System.Drawing.Font("Arial", 10F);
            this.txtXAxisDispMin.Location = new System.Drawing.Point(40, 181);
            this.txtXAxisDispMin.Size = new System.Drawing.Size(40, 16);
            this.txtXAxisDispMin.Text = "0.00";
            // 
            // WFAnalysisDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "WFAnalysisDisplay";
            this.Size = new System.Drawing.Size(639, 206);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.WFAnalysisDisplay_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

    }
}
