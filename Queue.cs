using System;
using System.Collections.Generic;

namespace Simulacao_T1
{

    public class Queue
    {
        private int _capacity;
        public double Arrival { get; set; }        
        public int State { get; set; }
        public int Servers { get; set; }

        public int Capacity
        {
            get => this._capacity;
            set
            {
                this._capacity = value;
                if (value <= 0) return;
                IsInfinite = false;
                for (var i = 0; i <= this._capacity; i++)
                    StateStats.Add(0);
            }
        }
        public double MinArrival { get; set; }
        public double MaxArrival { get; set; }
        public double MinService { get; set; }
        public double MaxService { get; set; }
        public  LinkedList<Event> EventList = new();
        public  readonly List<double> StateStats = new();
        public bool IsInfinite = true;
        public string Name { get; set; }

        public void Restart()
        {
            this.EventList = new();
        }

        public void IncrStateTime(int index, double time)
        {
            if (StateStats.Count > index)
            {
                StateStats[index] += time;
            }
            else
            {
                for(var k=StateStats.Count; k<index; k++)
                {
                    StateStats.Add(0);
                }
            
                StateStats.Add(time);
            }
        }

        public double GetStateTime(int index)
        {
            return StateStats.Count > index ? StateStats[index] : 0;
        }
    }
}