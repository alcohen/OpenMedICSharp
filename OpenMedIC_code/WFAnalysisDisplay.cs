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

// The following causes us to use the advanced algorithm to determine the best value
// to show for the y-axis cursor value
#define USE_ADVANCED_CURSOR_VALUE_ALGORITHM

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OpenMedIC
{
    public partial class WFAnalysisDisplay : OpenMedIC.GraphBase
    {
        private int firstGraphedIndex;
        private int lastGraphedIndex;

        /// <summary>
        /// Gets or sets the waveform.
        /// </summary>
        /// <value>The waveform.</value>
        public Single[] Waveform
        {
            get { return waveform; }
            set { waveform = value; }
        }

        /// <summary>
        /// Is waveform visible
        /// </summary>
        private bool wfVisible;

        /// <summary>
        /// Gets or sets a value indicating whether [wf visible].
        /// </summary>
        /// <value><c>true</c> if [wf visible]; otherwise, <c>false</c>.</value>
        public bool WfVisible
        {
            get { return wfVisible; }
            set { wfVisible = value; }
        }

        /// <summary>
        /// Start time in seconds (usually 0)
        /// </summary>
        private float wfStartTime = 0;

        /// <summary>
        /// Gets or sets the wf start time in seconds.
        /// </summary>
        /// <value>The wf start time.</value>
        public float WfStartTime
        {
            get { return wfStartTime; }
            set { wfStartTime = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WFAnalysisDisplay"/> class.
        /// </summary>
        public WFAnalysisDisplay()
        {
            InitializeComponent();
        }

        private int borderBevelWidth;
        
        /// <summary>
        /// WaveformBuffer To Display
        /// </summary>
        private Single[] waveform;

        /// <summary>
        /// WaveformBuffer To Display
        /// </summary>
        private Double period;

        public Double Period
        {
          get { return period; }
          set { period = value; }
        }
        
        private void WFAnalysisDisplay_Paint(object sender, PaintEventArgs e)
        {
            //clear the canvas
            OffscreenDC.Clear(GSBackColor);

            if (waveform != null)
            {            
            
                //start by drawing the grid
            
                //Minor X grid
                Pen gridPen = new Pen(XMinorGridColor);
                DrawXGrid(gridPen, XMinorGridInterval);

                //Major X grid
                gridPen.Color = XMajorGridColor;
                DrawXGrid(gridPen, XMajorGridInterval);

                //Minor Y grid
                gridPen.Color = YMinorGridColor;
                DrawYGrid(gridPen, YMinorGridInterval);

                //Major Y grid
                gridPen.Color = YMajorGridColor;
                DrawYGrid(gridPen, YMajorGridInterval);

                //draw the waveform
                Point fromPoint= new Point(0, 0);
                Point toPoint = new Point(0, 0);


                //draw the waveform
                //find the first point to graqph
                firstGraphedIndex = Convert.ToInt32(XAxisDispMin / Period);
                if (firstGraphedIndex < 0)
                    firstGraphedIndex = 0;

                lastGraphedIndex = Convert.ToInt32(XAxisDispMax / Period);
                if (lastGraphedIndex > (Waveform.Length - 1))
                    lastGraphedIndex = Waveform.Length - 1;

				if (lastGraphedIndex > firstGraphedIndex)
				{	// We have something to graph:
					Point[] GraphPoints = new Point[(lastGraphedIndex - firstGraphedIndex) + 1];

					for (int i = firstGraphedIndex; i <= lastGraphedIndex; i++)
					{
						//calculate the point
						GraphPoints[i - firstGraphedIndex].X = PixelFromAbsIndex(i);
						GraphPoints[i - firstGraphedIndex].Y = PixelFromYVal(Waveform[i]);
					}

					//plot the point
					OffscreenDC.DrawLines(wfPen, GraphPoints);
				}


                /*
                for (int i = firstGraphedIndex; i != lastGraphedIndex; i++)
                {
                    //from point
                    fromPoint.X = PixelFromAbsIndex(i);
                    fromPoint.Y = PixelFromYVal(Waveform[i]);

                    //to point
                    toPoint.X = PixelFromAbsIndex(i + 1);
                    toPoint.Y = PixelFromYVal(Waveform[i+1]);

                    //plot the point
                    OffscreenDC.DrawLine(wfPen, fromPoint, toPoint);
                }
                */



                //now bitblt to the Display
                Graphics GS = pnlGraphingDisplay.CreateGraphics();
                GS.DrawImage(OffscreenBitmap, 0, 0);
                //image.DrawImage(OffscreenBitmap, 0, 0);
            }
        }
        private int PixelFromAbsIndex(long AbsIndex)
        {
			if (XAxisDispMax == XAxisDispMin)
			{	// Would cause an error:
				return 0;
			}
            double pixelsPerYval = pnlGraphingDisplay.Width / (XAxisDispMax - XAxisDispMin);
            return Convert.ToInt32(((AbsIndex * Period) - (XAxisDispMin))* pixelsPerYval);
        }

        private int PixelFromYVal(float YVal)
        {
            //double PixelsPerYUnit = pnlGraphingDisplay.Height / (this.yAxisDispMax - this.yAxisDispMin);
            double YPixelOffset = 0 - (PixelsPerYUnit * this.yAxisDispMin);
            return pnlGraphingDisplay.Height - Convert.ToInt32((PixelsPerYUnit * YVal) + YPixelOffset);
        }

        private void txtXAxisDispMax_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtXAxisDispMax_Leave(object sender, EventArgs e)
        {
            /*
            if (Waveform != null)
            {
                //check to see if we're out of bounds
                if (xAxisDispMax > ((Waveform.Length - 1) * Period))
                {
                    txtXAxisDispMax.Text = Convert.ToString((Waveform.Length - 1) * Period);
                    base.txtXAxisDispMax_Leave(this, e);
                }
            }
            */
        }

        private Color cursColor;
        [Description("Cursor color"), Category("Cursor")]
        public Color CursColor
        {
            get { return cursColor; }
            set 
            { 
                cursColor = value;
                cursPen = new Pen(value, cursLineWidth);
            }
        }

        private Pen cursPen = new Pen(Color.White);

        private int cursRadius = 5;

        [Description("Cursor radius"), Category("Cursor")]
        public int CursRadius
        {
            get { return cursRadius; }
            set { cursRadius = value; }
        }

        Single cursX;
        
        public Single CursX
        {
            get { return cursX; }
        }

        Single cursY;

        public Single CursY
        {
            get { return cursY; }
        }

        Point lastCursPos = new Point(-100, -100);

        public Point LastCursPos
        {
            get { return lastCursPos; }
        }

        private void pnlGraphingDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            int isAnEndPoint;   // -1 = at beginning;  1 = at end; 0 = in between
            Point curCursPos = new Point(e.X, e.Y);
            if ((PixelsPerXUnit != 0) && (Period != 0))
            {
                //snap the cursor to the data point below
                //determine the closest X point
                int xIndex = Convert.ToInt32(((e.X / PixelsPerXUnit) + xAxisDispMin) / Period);
                if (xIndex > (Waveform.Length - 1))
                {
                    // beyond the end -- take the last point:
                    isAnEndPoint = 1;
                    curCursPos.X = PixelFromAbsIndex(Waveform.Length - 1);
                    curCursPos.Y = PixelFromYVal(waveform[Waveform.Length - 1]);
                }
                else if (xIndex < 0)
                {
                    // before the start -- take the first point:
                    isAnEndPoint = -1;
                    curCursPos.X = PixelFromAbsIndex(0);
                    curCursPos.Y = PixelFromYVal(waveform[0]);
                }
                else
                {
                    // Somewhere in between:
                    isAnEndPoint = 0;
                    curCursPos.X = PixelFromAbsIndex(xIndex);
                    curCursPos.Y = PixelFromYVal(waveform[xIndex]);
                }

                DrawCursor(curCursPos);

                //update values
                cursX = Convert.ToSingle(curCursPos.X / PixelsPerXUnit) + XAxisDispMin;
#if USE_ADVANCED_CURSOR_VALUE_ALGORITHM
                if (isAnEndPoint == 0)
                {   // Means, we're not at an extremity
                    float[] pixelData;  // Stores the data points for the selected pixel column
                    float sampRate;     // The sampling rate, in Hz
                    float bandwidth;    // The max. pass-band bandwidth, in Hz
                    // We need to:
                    // - determine the relevant pixel column
                    // ...
                    // - determine the values that comprise the pixel column
                    // ...
                    // - Get the relevant y-axis value to display
                    //cursY = GetDispSample(pixelData, sampRate, bandwidth);
                    //OK, JUST GO WITH THE OLD FUNCTIONALITY:
                    cursY = YAxisDispMax - Convert.ToSingle(curCursPos.Y / PixelsPerYUnit);
                }
                else if ( isAnEndPoint == -1 )
                {   // Means, we want the very first sample
                    cursY = waveform[0];
                }
                else
                {   // Means, we want the very last sample
                    cursY = waveform[Waveform.Length - 1];
                }
#else
                cursY = YAxisDispMax - Convert.ToSingle(curCursPos.Y / PixelsPerYUnit);
#endif
            }
            base.OnMouseMove(e);
            
        }

#if USE_ADVANCED_CURSOR_VALUE_ALGORITHM
/* start of USE_ADVANCED_CURSOR_VALUE_ALGORITHM section */

        /// <summary>
        /// Determines which sample in the given set is most relevant to display as
        /// the "value" of the set.
        /// </summary>
        /// <param name="data">The data to be analyzed</param>
        /// <param name="samplingRate">The data acquisition sampling rate (samp./sec.)</param>
        /// <param name="bandwidthMaxF">The high-end pass-band cutoff frequency (samp./sec.)</param>
        /// <returns>The value to be displayed.</returns>
        private static float GetDispSample(float[] data,
                                            float samplingRate,
                                            float bandwidthMaxF)
        {
            // Algorithm:
            // - Determine how many wavelengths there are in the data, based on
            //	 the max. frequency and the sampling rate
            // - Divide the samples in that many bins
            // - Calculate the average for each bin
            // - Determine the correct value:
            //	 - If the first and last bin have the lowest and highest averages,
            //		the value is the average of the whole set
            //	 - If the first or last bin has the highest average, and the other one
            //		does not have the lowest average, then pick the lowest value in 
            //		the set
            //	 - If the first or last bin has the lowest average, and the other one
            //		does not have the highest average, then pick the highest value in 
            //		the set
            float result;
            // Determine the number of bins:
            // - Get the samples per max. wavelength:
            float sampPerWave = samplingRate / bandwidthMaxF;
            // - Calculate the wavelengths in the data set:
            uint numWavelengths = (uint)Math.Truncate(data.Length / sampPerWave);
            // - sanity checks:
            if (numWavelengths < 3)
            {	// Can't do much useful -- we have only 1 or two wave lengths at all!
                // Different approach -- just return the average:
                return GetAverage(data);
            }
            // Reasonable number of samples -- continue
            // - Get the average for each bin:
            //   Note that the last bin contains any additional points that don't 
            //	 fit into a whole bin
            float[] avgs = new float[numWavelengths];
            int sampPerBin = (int)(data.Length / numWavelengths);
            for (int i = 0; i < numWavelengths; i++)
            {
                if (i < numWavelengths - 1)
                    // Get one bin's worth:
                    avgs[i] = GetAverage(data, i, sampPerBin);
                else
                    // All samples from the start of the last bin to the end:
                    avgs[i] = GetAverage(data, i, sampPerBin);
            }
            // - Determine which method to use to pick the result:
            int maxBin = 0;
            float maxBinVal = avgs[0];
            int minBin = 0;
            float minBinVal = avgs[0];
            uint lastBin = numWavelengths - 1;	// This is just for legibility
            for (int i = 1; i < numWavelengths; i++)
            {
                if (maxBinVal < avgs[i])
                {	// New max:
                    maxBin = i;
                    maxBinVal = avgs[i];
                }
                else if (minBinVal > avgs[i])
                {	// New min:
                    minBin = i;
                    minBinVal = avgs[i];
                }
            }
            if ((maxBin == 0 || maxBin == lastBin) &&
                 (minBin == 0 || minBin == lastBin))
            {	// Min and Max are the two extremes -- we have a slope
                // take the average:
                result = GetAverage(data);
            }
            else if (maxBin == 0 || maxBin == lastBin)
            {	// Max is at one of the extremes -- we have a negative-going peak
                // Take the lowest value:
                result = GetLowest(data);
            }
            else if (minBin == 0 || minBin == lastBin)
            {	// Min is at one of the extremes -- we have a positive-going peak
                // Take the highest value:
                result = GetHighest(data);
            }
            else
            {	// Non-predictable waveform -- just take the average:
                result = GetAverage(data);
            }
            // ...
            return result;
        }

        private static float GetAverage(float[] data, int binNum, int sampPerBin)
        {
            float valSum = 0;
            int count = 0;
            for (int i = binNum * sampPerBin; i < (binNum + 1) * sampPerBin; i++)
            {
                if (i >= data.Length)
                    break;
                valSum += data[i];
                count++;
            }
            return valSum / (float)count;
        }

        private static float GetAverage(float[] data)
        {
            float valSum = 0;
            // cycle through the values:
            for (int i = 0; i < data.Length; i++)
            {
                valSum += data[i];
            }
            return valSum / (float)data.Length;
        }

        private static float GetHighest(float[] data)
        {
            float maxVal = data[0];
            // cycle through the values:
            for (int i = 1; i < data.Length; i++)
            {
                if (maxVal < data[i])
                {
                    maxVal = data[i];
                }
            }
            return maxVal;
        }

        private static float GetLowest(float[] data)
        {
            float minVal = data[0];
            // cycle through the values:
            for (int i = 1; i < data.Length; i++)
            {
                if (minVal > data[i])
                {
                    minVal = data[i];
                }
            }
            return minVal;
        }
/* End of USE_ADVANCED_CURSOR_VALUE_ALGORITHM section */
#endif

        private void DrawCursor(Point CursorLocation)
        {
            if (lastCursPos != new Point(-100, -100))
            {
                //delete old cursor by redrawing the waveform
                //Not optimal, but GDI+ does not support an XOR pen!

                //bitblt to the Display
                Graphics GS = pnlGraphingDisplay.CreateGraphics();
                OnscreenDC.DrawImage(OffscreenBitmap, 0, 0);
            }

            //draw the new cursor
            
            //draw the vertical line
            OnscreenDC.DrawLine(cursPen, 
                CursorLocation.X, CursorLocation.Y - CursRadius, 
                CursorLocation.X, CursorLocation.Y + CursRadius);

            //draw the horizontal line
            OnscreenDC.DrawLine(cursPen,
                CursorLocation.X - CursRadius, CursorLocation.Y,
                CursorLocation.X + CursRadius, CursorLocation.Y);
            lastCursPos = CursorLocation;

        }

        private Single xMajorGridInterval = 0.5F;

        public Single XMajorGridInterval
        {
            get { return xMajorGridInterval; }
            set { xMajorGridInterval = value; }
        }
        private Color xMajorGridColor = Color.Red;

        public Color XMajorGridColor
        {
            get { return xMajorGridColor; }
            set { xMajorGridColor = value; }
        }
        private Single xMinorGridInterval = 0.1F;

        public Single XMinorGridInterval
        {
            get { return xMinorGridInterval; }
            set { xMinorGridInterval = value; }
        }
        private Color xMinorGridColor = Color.Salmon;

        public Color XMinorGridColor
        {
            get { return xMinorGridColor; }
            set { xMinorGridColor = value; }
        }
        private Single yMajorGridInterval = 0.5F;

        public Single YMajorGridInterval
        {
            get { return yMajorGridInterval; }
            set { yMajorGridInterval = value; }
        }
        private Color yMajorGridColor = Color.Red;

        public Color YMajorGridColor
        {
            get { return yMajorGridColor; }
            set { yMajorGridColor = value; }
        }
        private Single yMinorGridInterval = 0.1F;

        public Single YMinorGridInterval
        {
            get { return yMinorGridInterval; }
            set { yMinorGridInterval = value; }
        }
        private Color yMinorGridColor = Color.Salmon;

        public Color YMinorGridColor
        {
            get { return yMinorGridColor; }
            set { yMinorGridColor = value; }
		}

		private void DrawXGrid(Pen gridPen, double gridInterval)
		{
			DrawXGrid(gridPen, gridInterval, OffscreenDC);
		}

		private void DrawXGrid(Pen gridPen, double gridInterval, Graphics dc)
		{
			Double firstGrid = (Math.Ceiling(XAxisDispMin / gridInterval)) * gridInterval;
            Double lastGrid = (Math.Floor(XAxisDispMax / gridInterval)) * gridInterval;
            int topPixelRow = 0;
            int botPixelRow = OffscreenBitmap.Height - 1;
            int XPixelPos;

            for (Double gridPos = firstGrid; gridPos <= lastGrid; gridPos += gridInterval)
            {
                XPixelPos = Convert.ToInt32((gridPos - XAxisDispMin) * PixelsPerXUnit);
                dc.DrawLine(gridPen,
                    XPixelPos, 0,
                    XPixelPos, botPixelRow);
            }
        }

		private void DrawYGrid(Pen gridPen, double gridInterval)
		{
			DrawYGrid(gridPen, gridInterval, OffscreenDC);
		}

		private void DrawYGrid(Pen gridPen, double gridInterval, Graphics dc)
        {
            Double firstGrid = (Math.Ceiling(YAxisDispMin / gridInterval)) * gridInterval;
            Double lastGrid = (Math.Floor(YAxisDispMax / gridInterval)) * gridInterval;
            int leftPixelRow = 0;
            int rightPixelRow = OffscreenBitmap.Width - 1;
            int YPixelPos;

            for (Double gridPos = firstGrid; gridPos <= lastGrid; gridPos += gridInterval)
            {
                YPixelPos = Convert.ToInt32((gridPos - YAxisDispMin) * PixelsPerYUnit);
                OffscreenDC.DrawLine(gridPen,
                    leftPixelRow, YPixelPos,
                    rightPixelRow, YPixelPos);
            }
        }

        private int cursLineWidth = 2;
        
        [Description("Cursor line width"), Category("Cursor")]
        public int CursLineWidth
        {
            get { return cursLineWidth; }
            set { cursLineWidth = value; }
        }

        public Bitmap DisplayBitmap
        {
            get
            {
                //Bitmap bm = new Bitmap(pnlGraphingDisplay.Width, pnlGraphingDisplay.Height);
                //Rectangle rect = new Rectangle(0, 0, pnlGraphingDisplay.Width, pnlGraphingDisplay.Height);
                //pnlGraphingDisplay.DrawToBitmap(bm, rect);
                //return bm;
                return OffscreenBitmap;
            }
        }

    }
}
