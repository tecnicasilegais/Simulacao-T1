using System;
using System.Collections.Generic;
using System.Text;

namespace Simulacao_T1
{
    public enum EventType
    {
        Arrival,
        Departure
    };
    public class Event
    {
        public int Type { get; set; }
        public Decimal Time { get; set; }

        public Event(int type, Decimal time)
        {
            Type = type;
            Time = time;
        }
    }
}
