using System;

namespace Simulacao_T1
{
    public enum EventType
    {
        Arrival,
        Departure
    }

    public class Event : IComparable<Event>
    {
        public Event(EventType type, double time)
        {
            Type = type;
            Time = time;
        }

        public Event(EventType type, double time, string target, string source)
        {
            Type = type;
            Time = time;
            Target = target;
            Source = source;
        }

        public double Time { get; set; }
        public EventType Type { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }

        public int CompareTo(Event other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return Time.CompareTo(other.Time);
        }
    }
}
