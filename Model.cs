using System;
using System.Collections.Generic;
using System.Text;

namespace Simulacao_T1
{
    public class Model
    {
        public double Arrivals { get; set; }
        public List<Queue> Queues { get; set; }
        public List<double> RndNumbers { get; set; }
        public int RndNumbersPerSeed { get; set; }
        public List<double> Seeds { get; set; }

        public class Queue
        {
            public int Servers { get; set; }
            public int Capacity { get; set; }
            public double MinArrival { get; set; }
            public double MaxArrival { get; set; }
            public double MinService { get; set; }
            public double MaxService { get; set; }
            public string Name { get; set; }
        }
        public class NetworkItem
        {
            public string Source { get; set; }
            public string Target { get; set; }
            public double Probability { get; set; }
        }
    }
}
