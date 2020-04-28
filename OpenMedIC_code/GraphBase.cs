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
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace OpenMedIC
{
    public abstract partial class GraphBase : UserControl
	{

		#region declarations

		// Declare delegate for AxisChanged events
        public delegate void XAxisChangedHandler();
        public delegate void YAxisChangedHandler();

        protected enum BevelDirection { Outward, Inward };

        protected enum BevelStyle { Sunk, Raised };

        // Declare the events, which is associated with our
        [Category("Action")]
        [Description("Fires when the X Axis is changed.")]
        public event XAxisChangedHandler XAxisChanged;
        [Category("Action")]
        [Description("Fires when the Y Axis is changed.")]
        public event YAxisChangedHandler YAxisChanged;

        private bool autoRedraw = true;
        /// <summary>
        /// Width (pixels) of bevel surrounding the control
        /// </summary>
        protected int borderBevelWidth = 4;

        /// <summary>
        /// Margin (pixels) between contents of control and outer bevel
        /// </summary>
        protected int borderMargin = 5;

        /// <summary>
        /// Bevel width (pixels) of graphing surface
        /// </summary>
        protected int gsBevelWidth = 3;

        /// <summary>
        /// Width (pixels) between bottom edge of graphing surface and bottom edge of component
        /// </summary>
        protected int gsBottom = 4;

        /// <summary>
        /// Width (pixels) between left edge of graphing surface and left edge of component
        /// </summary>
        protected int gsLeft = 4;

        /// <summary>
        /// Width (pixels) between right edge of graphing surface and right edge of component
        /// </summary>
        protected int gsRight = 4;

        /// <summary>
        /// Width (pixels) between top edge of graphing surface and top edge of component
        /// </summary>
        protected int gsTop = 4;

        /// <summary>
        /// Used to double buffer waveform display
        /// </summary>
        protected Bitmap OffscreenBitmap;

        /// <summary>
        /// Used to double buffer waveform display
        /// </summary>
        protected Graphics OffscreenDC;

        /// <summary>
        /// Used to double buffer waveform display
        /// </summary>
        protected Graphics OnscreenDC;

        protected double pixelsPerXUnit = 0;
        protected double pixelsPerYUnit = 0;
        protected string prevXAxisDispMax;
       
        protected string prevXAxisDispMin;
        protected string prevYAxisDispMax;
        protected string prevYAxisDispMin;

        /// <summary>
        /// Font of title displayed on text
        /// </summary>
        protected Font titleFont = new Font("Arial", 14);

        /// <summary>
        /// Title displayed on graph
        /// </summary>
        protected string titleText = "Title Here";

        /// <summary>
        /// Brush used for erasing background
        /// </summary>
        protected SolidBrush wfBGBrush;

        /// <summary>
        /// Waveform color
        /// </summary>
        protected Color wfColor = Color.LimeGreen;

        /// <summary>
        /// Pen used for drawing the waveform
        /// </summary>
        protected Pen wfPen;

        /// <summary>
        /// Waveform width (pixels)
        /// </summary>
        protected int wfWidth = 2;

        /// <summary>
        /// Maximum value displayed on X axis
        /// </summary>
        protected float xAxisDispMax = 20;

        /// <summary>
        /// Minimum value displayed on X axis
        /// </summary>
        protected float xAxisDispMin = 0;
        
        /// <summary>
        /// Font of Y Axis Numbers
        /// </summary>
        protected Font xAxisNumberFont = new Font("Arial", 10);

        /// <summary>
        /// Format of Y Axis Numbers
        /// </summary>
        protected string xAxisNumberFormat = "f2";

        /// <summary>
        /// Length (pixels) of X Axis Tics
        /// </summary>
        protected int xAxisTicLength = 5;

        /// <summary>
        /// Width (pixels) of X Axis Tics
        /// </summary>
        protected int xAxisTicWidth = 1;

        /// <summary>
        /// Font of Y Axis Caption
        /// </summary>
        protected Font yAxisCaptionFont = new Font("Arial", 12);

        protected int yAxisCaptionHeight;

        /// <summary>
        /// Caption displayed for the Y Axis (unit of measurement)
        /// </summary>
        protected string yAxisCaptionText = "Units";

        protected int yAxisCaptionWidth;

        /// <summary>
        /// Maximum value displayed on Y axis
        /// </summary>
        protected float yAxisDispMax = 100;

        /// <summary>
        /// Minimum value displayed on Y axis
        /// </summary>
        protected float yAxisDispMin = 0;

        /// <summary>
        /// Maximum Number of Y Axis Tics
        /// </summary>
        protected int yAxisMaxNumTics = 5;

        /// <summary>
        /// Font of Y Axis Numbers
        /// </summary>
        protected Font yAxisNumberFont = new Font("Arial", 10);

        /// <summary>
        /// Format of Y Axis Numbers
        /// </summary>
        protected string yAxisNumberFormat = "f2";

        /// <summary>
        /// Maximum width (pixels) of a Y Axis number, if it goes to scientific notation
        /// </summary>
        protected int yAxisNumberMaxWidth;

        /// <summary>
        /// Length (pixels) of Y Axis Tics
        /// </summary>
        protected int yAxisTicLength = 5;

        /// <summary>
        /// Width (pixels) of Y Axis Tics
        /// </summary>
        protected int yAxisTicWidth = 1;

		#endregion


		#region constructors

		public GraphBase()
        {
            InitializeComponent();
            // TODO: Add any initialization after the InitForm call
            wfPen = new Pen(wfColor, wfWidth);
            OffscreenBitmap = new Bitmap(pnlGraphingDisplay.Width, pnlGraphingDisplay.Height);
            OffscreenDC = Graphics.FromImage(OffscreenBitmap);
            OnscreenDC = pnlGraphingDisplay.CreateGraphics();
		}

		#endregion


		#region private methods

		private void GraphBase_Paint(object sender, PaintEventArgs e)
        {
            if (AutoRedraw)
            {
                //Phase 1: Populate child controls
                txtTitle.Font = titleFont;
                txtTitle.Text = titleText;

                txtYAxisDispMax.Font = yAxisNumberFont;
                txtYAxisDispMax.Text = yAxisDispMax.ToString(yAxisNumberFormat);

                txtYAxisDispMin.Font = yAxisNumberFont;
                txtYAxisDispMin.Text = yAxisDispMin.ToString(yAxisNumberFormat);

                txtXAxisDispMax.Font = xAxisNumberFont;
                txtXAxisDispMax.Text = xAxisDispMax.ToString(xAxisNumberFormat);

                txtXAxisDispMin.Font = xAxisNumberFont;
                txtXAxisDispMin.Text = xAxisDispMin.ToString(xAxisNumberFormat);

                //Phase 2: Calculate some numbers
                Graphics CS = this.CreateGraphics();
                //Graphics GS = OffscreenDC.CreateGraphics();

                yAxisNumberMaxWidth = Convert.ToInt32(CS.MeasureString("-88e-88", yAxisNumberFont).Width) + 5; //add a buffer of 5
                txtYAxisDispMax.Width = yAxisNumberMaxWidth;
                txtYAxisDispMin.Width = yAxisNumberMaxWidth;

                yAxisCaptionWidth = Convert.ToInt32(Math.Ceiling(yAxisCaptionFont.Size) / 2); //caption will be turned by 90 degrees
                //yAxisCaptionWidth = yAxisCaptionFont.Size;
                yAxisCaptionHeight = Convert.ToInt32(CS.MeasureString(yAxisCaptionText, yAxisCaptionFont).Width); //add a buffer of 5

                //Phase 3: Move controls into place

                //the graphing surface
                pnlGraphingDisplay.Left = borderBevelWidth
                    + borderMargin
                    + yAxisCaptionWidth
                    + yAxisNumberMaxWidth
                    + yAxisTicLength
                    + gsBevelWidth;

                pnlGraphingDisplay.Top = borderBevelWidth
                    + borderMargin
                    + txtTitle.Height
                    + gsBevelWidth;

                pnlGraphingDisplay.Width = (this.Width
                    - (borderBevelWidth
                    + borderMargin
                    + gsBevelWidth))
                    - pnlGraphingDisplay.Left;

                pnlGraphingDisplay.Height = (this.Height
                    - (
                    borderBevelWidth
                    + borderMargin
                    + gsBevelWidth
                    + txtXAxisDispMax.Height
                    + XAxisTicLength)
                    )
                    - pnlGraphingDisplay.Top;

                //Resize the Device Context
                OnscreenDC = pnlGraphingDisplay.CreateGraphics();


                //resize the "behend-the scenes" graphing panel
                OffscreenBitmap = new Bitmap(pnlGraphingDisplay.Width, pnlGraphingDisplay.Height);
                OffscreenDC = Graphics.FromImage(OffscreenBitmap);

                //resize text boxes
                txtTitle.Width = this.Width - 20;

                //reset the scaling factors
                pixelsPerXUnit = pnlGraphingDisplay.Width / (XAxisDispMax - XAxisDispMin);
                pixelsPerYUnit = pnlGraphingDisplay.Height / (YAxisDispMax - YAxisDispMin);

                //the time scale
                //X Axis max
                txtXAxisDispMax.Left = pnlGraphingDisplay.Right - txtXAxisDispMax.Width;
                txtXAxisDispMax.Top = this.Height
                    - (borderBevelWidth + borderMargin + txtXAxisDispMax.Height);

                txtXAxisDispMin.Left = pnlGraphingDisplay.Left - txtXAxisDispMin.Width;
                txtXAxisDispMin.Top = this.Height
                    - (borderBevelWidth + borderMargin + txtXAxisDispMin.Height);

                //Title
                txtTitle.Left = BorderMargin;
                txtTitle.Top = BorderMargin;

                //Y axis max and min
                txtYAxisDispMax.Left = borderBevelWidth
                    + borderMargin
                    + yAxisCaptionWidth
                    + yAxisNumberMaxWidth
                    - txtYAxisDispMax.Width;
                txtYAxisDispMin.Top = pnlGraphingDisplay.Bottom - (txtYAxisDispMax.Height / 2);

                txtYAxisDispMin.Left = borderBevelWidth
                    + borderMargin
                    + yAxisCaptionWidth
                    + yAxisNumberMaxWidth
                    - txtYAxisDispMin.Width;
                txtYAxisDispMax.Top = pnlGraphingDisplay.Top - (txtYAxisDispMin.Height / 2);

                //Phase 4: Draw various items

                //Outer bevel
                Rectangle ControlRect = new Rectangle(0, 0, this.Width, this.Height);
                DrawBevel(CS, ControlRect, borderBevelWidth, BevelStyle.Raised, BevelDirection.Inward);

                //Graphing surface bevel
                DrawBevel(CS, pnlGraphingDisplay.Bounds, gsBevelWidth, BevelStyle.Sunk, BevelDirection.Outward);

                //Y axis caption
                CS.TranslateTransform(borderBevelWidth,
                    pnlGraphingDisplay.Top + ((pnlGraphingDisplay.Height + yAxisCaptionHeight) / 2));
                CS.RotateTransform(-90);
                StringFormat Format = new StringFormat();
                CS.DrawString(YAxisCaptionText, YAxisCaptionFont, Brushes.Black, 0, 0, Format);
                CS.ResetTransform();

                //X axis tics
                int TicLeft;
                Pen TicPen = new Pen(Color.Black, xAxisTicWidth);
				if (xAxisDispMax <= xAxisDispMin)
				{
					// Nothing to draw!
				}
				else
				{
					for (int sec = 0; sec <= (xAxisDispMax - xAxisDispMin) + 1; sec++)
					{
						//draw a tic
						TicLeft = pnlGraphingDisplay.Left + Convert.ToInt16(sec * (pnlGraphingDisplay.Width / (xAxisDispMax - xAxisDispMin)));
						CS.DrawLine(TicPen,
							TicLeft, pnlGraphingDisplay.Bottom + gsBevelWidth,
							TicLeft, pnlGraphingDisplay.Bottom + gsBevelWidth + XAxisTicLength);
					}
				}
            }
        }

        private void GraphBase_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
		}

		private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtXAxisDispMax_Enter(object sender, System.EventArgs e)
        {
            txtXAxisDispMax.SelectAll();
            prevXAxisDispMax = txtXAxisDispMax.Text;
        }

        private void txtXAxisDispMax_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtXAxisDispMax_Leave(this, null);
            }
        }

        private void txtXAxisDispMax_TextChanged(object sender, System.EventArgs e)
        {

        }
      
        private void txtXAxisDispMin_Enter(object sender, EventArgs e)
        {
            txtXAxisDispMin.SelectAll();
            prevXAxisDispMin = txtXAxisDispMin.Text;
        }

        private void txtXAxisDispMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtXAxisDispMin_Leave(this, null);
            }
        }

        private void txtXAxisDispMin_Leave(object sender, EventArgs e)
        {
            try
            {
                //convert to correct format
                float tmp = Convert.ToSingle(txtXAxisDispMin.Text.ToString());  //store as entered
                txtXAxisDispMin.Text = tmp.ToString(xAxisNumberFormat);  //convert displayed text to correct format
                xAxisDispMin = Convert.ToSingle(txtXAxisDispMin.Text);  //convert stored number to correct format
                //save in case we need to restore
                prevXAxisDispMin = txtXAxisDispMin.Text;
                this.Invalidate();
                if (XAxisChanged != null)
                {
                    XAxisChanged();  // Notify Subscribers
                }
            }
            catch
            {
                //not a valid number
                //txtXAxisDispMin.Text = xAxisDispLength.ToString();
                txtXAxisDispMin.Text = prevXAxisDispMin;
            }

        }

        private void txtYAxisDispMax_Enter(object sender, EventArgs e)
        {
            txtYAxisDispMax.SelectAll();
            prevYAxisDispMax = txtYAxisDispMax.Text;
        }

        private void txtYAxisDispMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtYAxisDispMax_Leave(this, null);
            }
        }

        private void txtYAxisDispMax_Leave(object sender, EventArgs e)
        {
            try
            {
                //convert to correct format
                float tmp = Convert.ToSingle(txtYAxisDispMax.Text.ToString());  //store as entered
                txtYAxisDispMax.Text = tmp.ToString(yAxisNumberFormat);  //convert displayed text to correct format
                yAxisDispMax = Convert.ToSingle(txtYAxisDispMax.Text);  //convert stored number to correct format
                //save in case we need to restore
                prevYAxisDispMax = txtYAxisDispMax.Text;
                this.Invalidate();
                if (YAxisChanged != null)
                {
                    YAxisChanged();  // Notify Subscribers
                }
            }
            catch
            {
                txtYAxisDispMax.Text = prevYAxisDispMax;
            }

        }

        private void txtYAxisDispMin_Enter(object sender, EventArgs e)
        {
            txtYAxisDispMin.SelectAll();
            prevYAxisDispMin = txtYAxisDispMin.Text;
        }

        private void txtYAxisDispMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtYAxisDispMin_Leave(this, null);
            }
        }

        private void txtYAxisDispMin_Leave(object sender, EventArgs e)
        {
            try
            {
                //convert to correct format
                float tmp = Convert.ToSingle(txtYAxisDispMin.Text.ToString());  //store as entered
                txtYAxisDispMin.Text = tmp.ToString(yAxisNumberFormat);  //convert displayed text to correct format
                yAxisDispMin = Convert.ToSingle(txtYAxisDispMin.Text);  //convert stored number to correct format
                //save in case we need to restore
                prevYAxisDispMin = txtYAxisDispMin.Text;
                this.Invalidate();
                if (YAxisChanged != null)
                {
                    YAxisChanged();  // Notify Subscribers
                }
            }
            catch
            {
                txtYAxisDispMin.Text = prevYAxisDispMin;
            }

        }

        private void ztxtYAxisDispMax_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {

        }

        private void ztxtYAxisDispMax_Leave(object sender, System.EventArgs e)
        {
        }

        private void ztxtYAxisDispMin_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtYAxisDispMin_Leave(this, null);
            }
        }

        private void ztxtYAxisDispMin_Leave(object sender, System.EventArgs e)
        {
            try
            {
                yAxisDispMin = Convert.ToSingle(txtYAxisDispMin.Text.ToString());
                // Overflow management:
                while (yAxisDispMin >= yAxisDispMax)
                {
                    yAxisDispMin -= 0.001F;
                }
                this.Invalidate();
            }
            catch
            {
                txtYAxisDispMin.Text = yAxisDispMin.ToString();
            }
        }

		#endregion


		#region protected methods

		/// <summary>
		/// Draws a bevel around a rectangle
		/// </summary>
		/// <param name="GS">Graphics surface to draw on</param>
		/// <param name="ReferenceBorder">Rectangle representing the starting border of the bevel</param>
		/// <param name="Width">Border width (pixels)</param>
		/// <param name="Style">Sunk or raised</param>
		/// <param name="Direction">Direction to draw, referenced to ReferenceBorder</param>
		protected void DrawBevel(Graphics GS, Rectangle ReferenceBorder, int Width, BevelStyle Style, BevelDirection Direction)
		{
			int Inflator; //number of pixels to inflate ReferenceBorder
			//draw appropriate number of 1-pixel thick rectangles
			for (int i = 0; i != Width; i++)
			{
				Pen TLPen;
				Pen BRPen;

				//select the proper colors
				if (Style == BevelStyle.Raised)
				{
					TLPen = new Pen(SystemColors.ControlLight, 1);
					BRPen = new Pen(SystemColors.ControlDark, 1);
				}
				else
				{
					TLPen = new Pen(SystemColors.ControlDark, 1);
					BRPen = new Pen(SystemColors.ControlLight, 1);
				}

				if (Direction == BevelDirection.Outward)
				{
					Inflator = i;
				}
				else
				{
					Inflator = -i;
				}

				//draw the borders
				GS.DrawLine(TLPen,
					ReferenceBorder.Left - Inflator,
					ReferenceBorder.Bottom + Inflator,
					ReferenceBorder.Left - Inflator,
					ReferenceBorder.Top - Inflator);

				GS.DrawLine(TLPen,
					ReferenceBorder.Left - Inflator,
					ReferenceBorder.Top - Inflator,
					ReferenceBorder.Right + Inflator,
					ReferenceBorder.Top - Inflator);

				GS.DrawLine(BRPen,
					ReferenceBorder.Right + Inflator,
					ReferenceBorder.Top - Inflator,
					ReferenceBorder.Right + Inflator,
					ReferenceBorder.Bottom + Inflator);

				GS.DrawLine(BRPen,
					ReferenceBorder.Right + Inflator,
					ReferenceBorder.Bottom + Inflator,
					ReferenceBorder.Left - Inflator,
					ReferenceBorder.Bottom + Inflator);
			}
		}

		protected void txtXAxisDispMax_Leave(object sender, System.EventArgs e)
		{
			try
			{
				//convert to correct format
				float tmp = Convert.ToSingle(txtXAxisDispMax.Text.ToString());  //store as entered
				txtXAxisDispMax.Text = tmp.ToString(xAxisNumberFormat);  //convert displayed text to correct format
				xAxisDispMax = Convert.ToSingle(txtXAxisDispMax.Text);  //convert stored number to correct format
				//save in case we need to restore
				prevXAxisDispMax = txtXAxisDispMax.Text;
				this.Invalidate();
				if (XAxisChanged != null)
				{
					XAxisChanged();  // Notify Subscribers
				}
			}
			catch
			{
				//not a valid number
				txtXAxisDispMax.Text = prevXAxisDispMax;
				//txtXAxisDispMax.Text = xAxisDispLength.ToString();
			}
		}

		#endregion


		#region public methods

		public void Reset()
		{
		}

        /// <summary>
        /// Gets or sets a value indicating whether [auto redraw].
        /// </summary>
        /// <value><c>true</c> if [auto redraw]; otherwise, <c>false</c>.</value>
        public bool AutoRedraw
        {
            get { return autoRedraw; }
            set 
            { 
                autoRedraw = value;
                if (autoRedraw)
                {
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Width (pixels) of bevel surrounding the control
        /// </summary>
        [Description("Width (pixels) of bevel surrounding the control"), Category("Appearance")]
        public int BorderBevelWidth
        {
            get
            {
                return borderBevelWidth;
            }
            set
            {
                borderBevelWidth = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the border margin.
        /// </summary>
        /// <value>The border margin.</value>
        [Description("Margin (pixels) between contents of control and outer bevel"), Category("Appearance")]
        //[DefaultValue(5)] 
        public int BorderMargin
        {
            get
            {
                return borderMargin;
            }
            set
            {
                borderMargin = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Irrelevant base class property
        /// </summary>
        [Browsable(false)]
        public new Font Font
        {
            get
            {
                return null; //always null
            }
            set
            {
            }
        }

        /// <summary>
        /// Irrelevant base class property
        /// </summary>
        [Browsable(false)]
        public new Color ForeColor
        {
            get
            {
                return Color.Black; //always null
            }
            set
            {
            }
        }

        /// <summary>
        /// Background color of graphing surface
        /// </summary>
        [Description("Background color of graphing surface"), Category("Appearance")]
        public Color GSBackColor
        {
            get
            {
                return pnlGraphingDisplay.BackColor;
            }
            set
            {
                //paint the OffscreenDC
                pnlGraphingDisplay.BackColor = value;
                wfBGBrush = new SolidBrush(value);
                /*
                OffscreenDC.FillRectangle(wfBGBrush, 0, 0,
                    OffscreenBitmap.Width, OffscreenBitmap.Height);
                this.Invalidate();
                */
            }
        }

        /// <summary>
        /// Bevel width (pixels) of graphing surface
        /// </summary>
        [Description("Bevel width (pixels) of graphing surface"), Category("Appearance")]
        public int GSBevelWidth
        {
            get
            {
                return gsBevelWidth;
            }
            set
            {
                gsBevelWidth = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Width (pixels) between bottom edge of graphing surface and bottom edge of component
        /// </summary>
        [Description("Width (pixels) between bottom edge of graphing surface and bottom edge of component"), Category("Appearance")]
        public int GSBottom
        {
            get
            {
                return gsBottom;
            }
            set
            {
                gsBottom = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Width (pixels) between left edge of graphing surface and left edge of component
        /// </summary>
        [Description("Width (pixels) between left edge of graphing surface and left edge of component"), Category("Appearance")]
        public int GSLeft
        {
            get
            {
                return gsLeft;
            }
            set
            {
                gsLeft = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Width (pixels) between right edge of graphing surface and right edge of component
        /// </summary>
        [Description("Width (pixels) between right edge of graphing surface and right edge of component"), Category("Appearance")]
        public int GSRight
        {
            get
            {
                return gsRight;
            }
            set
            {
                gsRight = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Width (pixels) between top edge of graphing surface and bottom of title
        /// </summary>
        [Description("Width (pixels) between top edge of graphing surface and bottom of title"), Category("Appearance")]
        public int GSTop
        {
            get
            {
                return gsTop;
            }
            set
            {
                gsTop = value;
                this.Invalidate();
            }
        }

        public double PixelsPerXUnit
        {
            get { return pixelsPerXUnit; }
        }

        public double PixelsPerYUnit
        {
            get { return pixelsPerYUnit; }
        }

        /// <summary>
        /// Irrelevant base class property
        /// </summary>
        [Browsable(false)]
        public new bool RightToLeft
        {
            get
            {
                return false; //always null
            }
            set
            {
            }
        }

        /// <summary>
        /// Font of title displayed on text
        /// </summary>
        [Description("Font of title displayed on text"), Category("Appearance")]
        public Font TitleFont
        {
            get
            {
                return titleFont;
            }
            set
            {
                titleFont = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Title displayed on graph
        /// </summary>
        [Description("Title displayed on graph"), Category("Appearance")]
        public string TitleText
        {
            get
            {
                return titleText;
            }
            set
            {
                titleText = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Waveform color
        /// </summary>
        [Description("Waveform color"), Category("Appearance")]
        public Color WFColor
        {
            get
            {
                return wfColor;
            }
            set
            {
                wfColor = value;
                wfPen.Color = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Waveform width (pixels)
        /// </summary>
        [Description("Waveform width (pixels)"), Category("Appearance")]
        public int WFWidth
        {
            get
            {
                return wfWidth;
            }
            set
            {
                wfWidth = value;
                wfPen.Width = value;
                this.Invalidate();
            }
        }
        
        /// <summary>
        /// Maximum displayed on X Axis
        /// </summary>
        [Description("Maximum displayed on X Axis"), Category("Axes")]
        public float XAxisDispMax
        {
            get { return xAxisDispMax; }
            set { xAxisDispMax = value; }
        }

        /// <summary>
        /// Minimum displayed on X Axis
        /// </summary>
        [Description("Minimum displayed on X Axis"), Category("Axes")]
        public virtual float XAxisDispMin
        {
            get { return xAxisDispMin; }
            set { xAxisDispMin = value; }
        }

        /// <summary>
        /// Font of X Axis Numbers
        /// </summary>
        [Description("Font of X Axis Numbers"), Category("Axes")]
        public Font XAxisNumberFont
        {
            get
            {
                return xAxisNumberFont;
            }
            set
            {
                xAxisNumberFont = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Format of X Axis Numbers
        /// </summary>
        [Description("Format of X Axis Numbers"), Category("Axes")]
        public string XAxisNumberFormat
        {
            get
            {
                return xAxisNumberFormat;
            }
            set
            {
                xAxisNumberFormat = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Length (pixels) of X Axis Tics
        /// </summary>
        [Description("Length (pixels) of X Axis Tics"), Category("Axes")]
        public int XAxisTicLength
        {
            get
            {
                return xAxisTicLength;
            }
            set
            {
                xAxisTicLength = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Width (pixels) of X Axis Tics
        /// </summary>
        [Description("Width (pixels) of X Axis Tics"), Category("Axes")]
        public int XAxisTicWidth
        {
            get
            {
                return xAxisTicWidth;
            }
            set
            {
                xAxisTicWidth = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Font of Y Axis Caption
        /// </summary>
        [Description("Font of Y Axis Caption"), Category("Axes")]
        public Font YAxisCaptionFont
        {
            get
            {
                return yAxisCaptionFont;
            }
            set
            {
                yAxisCaptionFont = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Caption displayed for the Y Axis (unit of measurement)
        /// </summary>
        [Description("Caption displayed for the Y Axis (unit of measurement)"), Category("Axes")]
        public string YAxisCaptionText
        {
            get
            {
                return yAxisCaptionText;
            }
            set
            {
                yAxisCaptionText = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Maximum value displayed on Y axis
        /// </summary>
        [Description("Maximum value displayed on Y axis"), Category("Axes")]
        public float YAxisDispMax
        {
            get
            {
                return yAxisDispMax;
            }
            set
            {
                yAxisDispMax = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Minimum value displayed on Y axis
        /// </summary>
        [Description("Minimum value displayed on Y axis"), Category("Axes")]
        public float YAxisDispMin
        {
            get
            {
                return yAxisDispMin;
            }
            set
            {
                yAxisDispMin = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Maximum Number of Y Axis Tics
        /// </summary>
        [Description("Maximum Number of Y Axis Tics"), Category("Axes")]
        public int YAxisMaxNumTics
        {
            get
            {
                return yAxisMaxNumTics;
            }
            set
            {
                yAxisMaxNumTics = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Font of Y Axis Numbers
        /// </summary>
        [Description("Font of Y Axis Numbers"), Category("Axes")]
        public Font YAxisNumberFont
        {
            get
            {
                return yAxisNumberFont;
            }
            set
            {
                yAxisNumberFont = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Format of Y Axis Numbers
        /// </summary>
        [Description("Format of Y Axis Numbers"), Category("Axes")]
        public string YAxisNumberFormat
        {
            get
            {
                return yAxisNumberFormat;
            }
            set
            {
                yAxisNumberFormat = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Length (pixels) of Y Axis Tics
        /// </summary>
        [Description("Length (pixels) of Y Axis Tics"), Category("Axes")]
        public int YAxisTicLength
        {
            get
            {
                return yAxisTicLength;
            }
            set
            {
                yAxisTicLength = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Width (pixels) of Y Axis Tics
        /// </summary>
        [Description("Width (pixels) of Y Axis Tics"), Category("Axes")]
        public int YAxisTicWidth
        {
            get
            {
                return yAxisTicWidth;
            }
            set
            {
                yAxisTicWidth = value;
                this.Invalidate();
            }
		}


		/// <summary>
		/// Save the image currently in the display to a file with the specified
		/// path and file name, and with an extension of .BMP (any extension
		/// specified in the file name will be treated as part of the file name).
		/// If the file already exists, it is overwritten.
		/// </summary>
		/// <param name="filePathName">A valid path and file name, WITHOUT the
		///			extension, to which to save the current bitmap</param>
		public void SaveToFile(string filePathName)
		{
			// Create a stream to write it to file:
			string fullName = filePathName + ".BMP";
			FileStream fs = new FileStream(	fullName, 
											FileMode.Create,	// Overwrites if existing
											FileAccess.Write);
			// Now write the latest bitmap to that file:
			OffscreenBitmap.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
			fs.Close();
		}

		#endregion

	}
}
