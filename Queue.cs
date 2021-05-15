using System.Collections.Generic;

namespace Simulacao_T1
{
    public class Queue
    {
        public readonly List<double> StateStats = new();
        private double _arrival;
        private int _capacity;
        public bool HasOutsideArrival;
        public bool IsInfinite = true;
        public int State { get; set; }
        public int Servers { get; set; }

        public double Arrival
        {
            get => _arrival;
            set
            {
                _arrival = value;
                if (value <= 0)
                {
                    return;
                }

                HasOutsideArrival = true;
            }
        }

        public int Capacity
        {
            get => _capacity;
            set
            {
                _capacity = value;
                if (value <= 0)
                {
                    return;
                }

                IsInfinite = false;
                for (int i = 0; i <= _capacity; i++)
                {
                    StateStats.Add(0);
                }
            }
        }

        public double MinArrival { get; set; }
        public double MaxArrival { get; set; }
        public double MinService { get; set; }
        public double MaxService { get; set; }
        public List<Network> Connections { get; set; }
        public int Losses { get; set; }

        public void Restart()
        {
            State = 0;
        }

        public bool HasSpace()
        {
            return State != Capacity || IsInfinite;
        }

        public void IncrStateTime(int index, double time)
        {
            if (StateStats.Count > index)
            {
                StateStats[index] += time;
            }
            else
            {
                for (int k = StateStats.Count; k < index; k++)
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
