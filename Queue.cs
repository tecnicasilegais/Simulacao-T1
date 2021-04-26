using System;
using System.Collections.Generic;

namespace Simulacao_T1
{

    public class Queue
    {
        private int _capacity;
        private double _arrival;
        public int State { get; set; }
        public int Servers { get; set; }

        public double Arrival
        {
            get => this._arrival;
            set
            {
                this._arrival = value;
                if (value <= 0) return;
                HasOutsideArrival = true;
            }
        }        
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
        public  readonly List<double> StateStats = new();
        public bool IsInfinite = true;
        public bool HasOutsideArrival = false;
        public Network Connection { get; set; }
        public int Losses { get; set; }

        public void Restart()
        {
            this.State = 0;
        }

        public bool HasSpace()
        {
            return this.State != this.Capacity || this.IsInfinite;
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
    public class Network
    {
        public string Target { get; set; }
        public double Probability { get; set; }
    }
}