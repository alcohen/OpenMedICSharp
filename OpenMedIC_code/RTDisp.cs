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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;

namespace OpenMedIC
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
    //[ToolboxBitmap(typeof(Bitmap))]
    [ToolboxBitmap(typeof(RTGraph),@"RTGraph.ico")]
	public class RTGraph : OpenMedIC.GraphBase
    {
        private IContainer components;

        /// <summary>
        /// Constructor
        /// </summary>
		public RTGraph()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
			cursPen = new Pen(cursColor, cursWidth);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
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
            this.txtXAxisDispMax.Location = new System.Drawing.Point(500, 191);
            this.txtXAxisDispMax.Size = new System.Drawing.Size(40, 16);
            this.txtXAxisDispMax.Text = "20.00";
            // 
            // txtTitle
            // 
            this.txtTitle.Font = new System.Drawing.Font("Arial", 14F);
            this.txtTitle.Location = new System.Drawing.Point(5, 5);
            this.txtTitle.Size = new System.Drawing.Size(535, 22);
            this.txtTitle.Text = "Title Here";
            // 
            // txtYAxisDispMin
            // 
            this.txtYAxisDispMin.Font = new System.Drawing.Font("Arial", 10F);
            this.txtYAxisDispMin.Location = new System.Drawing.Point(15, 175);
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
            this.pnlGraphingDisplay.Size = new System.Drawing.Size(460, 149);
            // 
            // txtXAxisDispMin
            // 
            this.txtXAxisDispMin.Font = new System.Drawing.Font("Arial", 10F);
            this.txtXAxisDispMin.Location = new System.Drawing.Point(40, 191);
            this.txtXAxisDispMin.Size = new System.Drawing.Size(40, 16);
            this.txtXAxisDispMin.Text = "0.00";
            this.txtXAxisDispMin.Visible = false;
            // 
            // RTGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "RTGraph";
            this.Size = new System.Drawing.Size(552, 216);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.RTWaveform_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void RTWaveform_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            Debug.WriteLine("Entering Paint");
            if (AutoRedraw)
            {
                //Waveform
                if (wfThis != null)
                {
                    RedrawGraph();
                }
            }
            Debug.WriteLine("Exiting Paint");
		}

        /// <summary>
		/// Pen used for drawing the cursor
		/// </summary>
		private Pen cursPen; 

		/// <summary>
		/// Width (pixels) of cursor
		/// </summary>
		private int cursWidth = 1;

		/// <summary>
		/// Width (pixels) of cleared area in fron of cursor
		/// </summary>
		private int cursForeWidth = 3;

		/// <summary>
		/// color of cursor
		/// </summary>
		private Color cursColor = Color.Blue; 


        /// <summary>
        ///for passing samples from waveform buffer to this component
        /// </summary>		
        private OpenMedIC.Samples InSamples = new Samples(1000000);

		private long lastAbsIndexDisplayed;

        /// <summary>
        /// The pixel on the graphing surface most recently written to the screen
        /// </summary>
        private Point LatestPixelDisplayed = new Point(-1, -1);
	
		/// <summary>
		/// Width (pixels) of cursor
		/// </summary>
		[Description("Width (pixels) of cursor"),Category("Appearance")] 
		public int CursWidth
		{
			get
			{
				return cursWidth;
			}
			set
			{
				cursWidth = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Width (pixels) of cleared area in fron of cursor
		/// </summary>
		[Description("Width (pixels) of cleared area in fron of cursor"),Category("Appearance")] 
		public int CursForeWidth
		{
			get
			{
				return cursForeWidth;
			}
			set
			{
				cursForeWidth = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// color of cursor
		/// </summary>
		[Description("color of cursor"),Category("Appearance")] 
		public Color CursColor
		{
			get
			{
				return cursPen.Color;
			}
			set
			{
				cursColor = value;
				cursPen.Color = value;
				this.Invalidate();
			}
        }

        /// <summary>
        /// Redraws entire waveform from scratch
        /// </summary>
		public void RedrawGraph()
		{
			long StartIndex;

			lock ( this )
			{
				//get the points we need from the waveform
				StartIndex = wfThis.getPoints(InSamples, this.xAxisDispMax);
				//Debug.WriteLine(StartIndex.ToString() + ", " + InSamples.size.ToString());
                //Set LatestPixelDisplayed so we don't draw a spurious line at the beginning
                LatestPixelDisplayed.X = PixelFromAbsIndex(StartIndex);
                if (StartIndex != 0)
                {
                    LatestPixelDisplayed.Y = PixelFromYVal(InSamples[0].sampleValue);
                }
                else
                {
                    LatestPixelDisplayed.Y = 0;
                }

				NewGraphPoints(StartIndex, InSamples);
			}
		}
		/// <summary>
		/// Draws points acquired since last draw
		/// </summary>
		public void DrawNew()
		{
            if (AutoRedraw)
            {
                long StartIndex;

                lock (this)
                {
                    //get the points we need from the waveform
                    try
                    {
                        if (wfThis == null)
                        {
                            OpenMedICUtils.debugPrint("wfThis null! In RTDisp.DrawNew()");
                        }
                        else if (InSamples == null)
                        {
                            OpenMedICUtils.debugPrint("InSamples null! In RTDisp.DrawNew()");
                        }
                        else
                        {
                            StartIndex = wfThis.getPoints(
                                InSamples,
                                this.xAxisDispMax,
                                lastAbsIndexDisplayed);
                            //Debug.WriteLine(StartIndex.ToString() + ", " + InSamples.size.ToString());

                            NewGraphPoints(StartIndex, InSamples);
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(
                              "Exception: \n"
                            + e.Message
                            + "\nStack trace:\n\n"
                            + e.StackTrace);
                    }
                }
            }
		}

 		private int PixelFromAbsIndex(long AbsIndex)
		{
			if (xAxisDispMax == 0)
				throw new DataException("Cannot process data point position because the X axis display is of size 0");
			return Convert.ToInt32((pnlGraphingDisplay.Width / this.xAxisDispMax) * AbsIndex * wfThis.samplePeriod) 
				% pnlGraphingDisplay.Width;
		}

		private int PixelFromYVal(float YVal)
		{
			if (yAxisDispMax == yAxisDispMin)
				throw new DataException("Cannot process data point position because the Y axis display is of size 0");
			float PixelsPerYUnit = pnlGraphingDisplay.Height / (this.yAxisDispMax - this.yAxisDispMin);
			float YPixelOffset = 0-(PixelsPerYUnit * this.yAxisDispMin);
			try
			{
				return pnlGraphingDisplay.Height - Convert.ToInt32((PixelsPerYUnit * YVal) + YPixelOffset);
			}
			catch (Exception convEx)
			{
				MessageBox.Show("Error converting to Int32: (" + 
									PixelsPerYUnit + " * " + YVal + " ) + " + 
									YPixelOffset + ": \n\n" + 
								convEx.Message);
			}
			return 0;	// Only in case of exception

		}

		//private void ClonePoints(Point[] 

        private void NewDrawLines(Graphics GS, Pen GPen, OpenMedIC.Samples GPoints, long FirstAbsIndex, int StartPoint, int EndPoint)
		{

			//Clear the X columns that we'll be drawing over
			GS.FillRectangle(wfBGBrush, PixelFromAbsIndex(FirstAbsIndex + StartPoint), 0, PixelFromAbsIndex(FirstAbsIndex + EndPoint) - PixelFromAbsIndex(FirstAbsIndex + StartPoint), pnlGraphingDisplay.Height);
			
			for (int i=StartPoint; i != EndPoint; i++)
			{
					GS.DrawLine(GPen, 
						PixelFromAbsIndex(FirstAbsIndex + i), 
						PixelFromYVal(GPoints[i].sampleValue), 
						PixelFromAbsIndex(FirstAbsIndex + i + 1),
						PixelFromYVal(GPoints[i + 1].sampleValue));
			}

			if (EndPoint < (GPoints.size-1))
			{
				//Clear the X columns that we'll be drawing over
				GS.FillRectangle(wfBGBrush, PixelFromAbsIndex(FirstAbsIndex + EndPoint), 0, 
					PixelFromAbsIndex(FirstAbsIndex + EndPoint + 1) + pnlGraphingDisplay.Width, pnlGraphingDisplay.Height);

				//we're going to wrap to beginning of GS
				//so draw another line segment to the next offscreen point
				GS.DrawLine(GPen, 
					PixelFromAbsIndex(FirstAbsIndex + EndPoint), 
					PixelFromYVal(GPoints[EndPoint].sampleValue), 
					PixelFromAbsIndex(FirstAbsIndex + EndPoint + 1) + pnlGraphingDisplay.Width, 
					PixelFromYVal(GPoints[EndPoint + 1].sampleValue));
			}
		}

        private void NewGraphPoints(long StartIndex, OpenMedIC.Samples InSamples)
		{

            Point PixelToDraw = new Point();
			//Graphics GS = pnlGraphingDisplay.CreateGraphics();

			for (int i=0; i != InSamples.size; i++)
			{
				PixelToDraw.X = PixelFromAbsIndex(StartIndex + i);
				PixelToDraw.Y = PixelFromYVal(InSamples[i].sampleValue);

				if ((PixelToDraw.X != LatestPixelDisplayed.X) || (PixelToDraw.Y != LatestPixelDisplayed.Y))
				{
					//if we're moving to a new x column, we need to clear waveform
					if (PixelToDraw.X > LatestPixelDisplayed.X)
					{
						OffscreenDC.FillRectangle(wfBGBrush, 
							LatestPixelDisplayed.X + 1, 0, 
							PixelToDraw.X - LatestPixelDisplayed.X, pnlGraphingDisplay.Height);
					}
					
					if (PixelToDraw.X >= LatestPixelDisplayed.X)
					{
						//test for the first time we're plotting the point
						//if it is the first time, then the LatestPixelDisplayed.Y
						//is a dummy, so simply skip it
						if (LatestPixelDisplayed.Y > -1)
						{
							//draw the segment
                            OffscreenDC.DrawLine(wfPen, 
								LatestPixelDisplayed.X, 
								LatestPixelDisplayed.Y, 
								PixelToDraw.X, 
								PixelToDraw.Y);
						}
						
					}
					else
					{
						//we should wrap back to the beginning of the graph

						//clear the space to the end
                        OffscreenDC.FillRectangle(wfBGBrush, 
							LatestPixelDisplayed.X, 0, 
							(pnlGraphingDisplay.Width - LatestPixelDisplayed.X), pnlGraphingDisplay.Height);

						//draw the line segment that goes off of the screen
                        OffscreenDC.DrawLine(wfPen, 
							LatestPixelDisplayed.X, 
							LatestPixelDisplayed.Y, 
							PixelToDraw.X + pnlGraphingDisplay.Width, 
							PixelToDraw.Y);

						//clear the space at the start
                        OffscreenDC.FillRectangle(wfBGBrush, 
							0, 0, 
							PixelToDraw.X + 1, pnlGraphingDisplay.Height);

						//draw the line segment that starts off of the screen
                        OffscreenDC.DrawLine(wfPen, 
							LatestPixelDisplayed.X - pnlGraphingDisplay.Width, 
							LatestPixelDisplayed.Y, 
							PixelToDraw.X, 
							PixelToDraw.Y);
					}
					//lastAbsIndexDisplayed = PixelFromAbsIndex(StartIndex + InSamples.size);
					LatestPixelDisplayed.X = PixelToDraw.X;
					LatestPixelDisplayed.Y = PixelToDraw.Y;
				}
				lastAbsIndexDisplayed = StartIndex + InSamples.size;
			}
			
			//finally, draw the cursor
            OffscreenDC.DrawLine(cursPen, LatestPixelDisplayed.X + 1, 0, LatestPixelDisplayed.X + 1, pnlGraphingDisplay.Height);
            OffscreenDC.FillRectangle(wfBGBrush, LatestPixelDisplayed.X + 2, 0, cursForeWidth, pnlGraphingDisplay.Height);

            //now bitblt to the Display
            Graphics GS = pnlGraphingDisplay.CreateGraphics();
            GS.DrawImage(OffscreenBitmap, 0, 0);

        }

        /// <summary>
        /// Real-time waveform to be displayed on this graph
        /// </summary>
        protected WaveformBuffer wfThis;

        /// <summary>
        /// Real-time waveform to be displayed on this graph
        /// </summary>
        [Browsable(false)]
        public WaveformBuffer WFThis
        {
            get
            {
                return wfThis;
            }
            set
            {
                wfThis = value;
            }
        }

        override public float XAxisDispMin
        {
            get
            {
                return xAxisDispMin;
            }
        }

        public void Reset()
        {
            lastAbsIndexDisplayed = 0;
        }
	}
}
