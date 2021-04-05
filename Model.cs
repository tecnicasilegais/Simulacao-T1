using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace Simulacao_T1
{
    public class Model
    {
        public Decimal Arrivals { get; set; }
        public List<Queue> Queues { get; set; }
        public LinkedList<Decimal> RndNumbers { get; set; }
        public int RndNumbersPerSeed { get; set; }
        public List<double> Seeds { get; set; }

        public class Queue
        {
            private int capacity;
            public int Servers { get; set; }

            public int Capacity
            {
                get => this.capacity;
                set
                {
                    this.capacity = value;
                    if (value > 0)
                    {
                        IsInfinte = false;
                        for (var i = 0; i <= this.capacity; i++)
                            QueueStates.Add(new decimal(0.0));
                    }
                }
            }
            public Decimal MinArrival { get; set; }
            public Decimal MaxArrival { get; set; }
            public Decimal MinService { get; set; }
            public Decimal MaxService { get; set; }
            public List<Event> EventList = new List<Event>();
            public List<Decimal> QueueStates = new List<Decimal>();
            public bool IsInfinte = true;
            public string Name { get; set; }
        }
        public class NetworkItem
        {
            public string Source { get; set; }
            public string Target { get; set; }
            public Decimal Probability { get; set; }
        }
    }
}
