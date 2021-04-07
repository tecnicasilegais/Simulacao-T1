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
    public class Event : IComparable<Event>
    {
        public double Time { get; set; }
        public EventType Type { get; set; }

        public Event(EventType type, double time)
        {
            Type = type;
            Time = time;
        }

        public int CompareTo(Event other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Time.CompareTo(other.Time);
        }
        
    }
}
