using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeqMaster.JsonAction
{
    internal class DataSaved
    {
        public string Formula { get; set; }

        public int minRange { get; set; }
        public int maxRange { get; set; }

        public string titleGraph { get; set; }

        public string Xtitle { get; set; }

        public string Ytitle { get; set; }

        public OxyColor Pointcolor { get; set; }

        public double pointThinkness {  get; set; }

        public bool isRecursive { get; set; }

        public string firstTermRecur {  get; set; }

    }
}
