using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FringeAnalysis.Controls
{
    class Coefficient
    {
        public int M { get; set; }
        public int N { get; set; }
        public double G1 { get; set; }
        public int GBlurx0 { get; set; }
        public int GBlury0 { get; set; }
        public double GBlurZone0 { get; set; }
        public int GBlurx1 { get; set; }
        public int GBlury1 { get; set; }
        public double GBlurZone1 { get; set; }
        public int GBlurx2 { get; set; }
        public int GBlury2 { get; set; }
        public double GBlurZone2 { get; set; }
        public int normlow { get; set; }
        public int normhigh { get; set; }

        public Coefficient()
        {
            M = 1; 
            N = 25;          
            GBlurx0 = 15;
            GBlury0 = 15;
            GBlurZone0 = 1.8;
            GBlurx1 = 121;
            GBlury1 = 81;
            GBlurZone1 = 28;
            GBlurx2 = 9;
            GBlury2 = 9;
            GBlurZone2 = 1.8;
            normlow = 0;
            normhigh = 255;
            G1 = 0.1;
        }
    }
}
