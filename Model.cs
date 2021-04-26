using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace Simulacao_T1
{
    public class Model
    {
        public Dictionary<String, Queue> Queues { get; set; }
        public LinkedList<double> RndNumbers { get; set; }
        public int RndNumbersPerSeed { get; set; }
        public List<double> Seeds { get; set; }
    }
}
