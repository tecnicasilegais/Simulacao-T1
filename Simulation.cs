using System;
using System.Collections.Generic;
using System.Linq;
using ShellProgressBar;

namespace Simulacao_T1
{
    internal class Simulation
    {
        private readonly LinkedList<Event> _eventList;

        private readonly Dictionary<string, Queue> _queues;

        private readonly LinkedList<double> _rndNumbers;

        public Simulation(Dictionary<string, Queue> queues, LinkedList<double> rndNumbers)
        {
            _queues = queues;
            _rndNumbers = rndNumbers;
            ElapsedTime = 0;
            _eventList = new LinkedList<Event>();
            Initialize();
        }

        public double ElapsedTime { get; private set; }

        private void Initialize()
        {
            foreach (KeyValuePair<string, Queue> keyValuePair in _queues.Where(q => q.Value.HasOutsideArrival))
            {
                ScheduleFirstArrival(keyValuePair.Key, keyValuePair.Value.Arrival);
            }

            void ScheduleFirstArrival(string target, double time)
            {
                var e = new Event(EventType.Arrival, time, target, null);
                _eventList.SortedInsertion(e);
            }
        }

        public void Simulate()
        {
            int totalTicks = _eventList.Count;
            var options = new ProgressBarOptions
            {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true
            };
            using var pbar = new ProgressBar(totalTicks, " - simulating queues", options);
            while (_rndNumbers.Count != 0)
            {
                try
                {
                    Event currEvent = _eventList.First.Value;

                    switch (currEvent.Type)
                    {
                        case EventType.Arrival:
                        {
                            Queue tarQueue = _queues[currEvent.Target];
                            _eventList.RemoveFirst();
                            Arrival(currEvent.Source, new(currEvent.Target, tarQueue), currEvent.Time);
                            pbar.Tick();
                            break;
                        }
                        case EventType.Departure: // TODO arrival when it comes from another queue
                        {
                            Queue srcQueue = _queues[currEvent.Source];
                            Exit(new(currEvent.Source, srcQueue), currEvent.Time);
                            if (currEvent.Target != null)
                            {
                                Arrival(currEvent.Source, new(currEvent.Target, _queues[currEvent.Target]),
                                    currEvent.Time);
                            }

                            pbar.Tick();
                            _eventList.RemoveFirst();
                            break;
                        }
                        default:
                            Console.WriteLine("Something unexpected happened in the EventList");
                            Environment.Exit(2);
                            break;
                    }
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("The initial arrivals must be declared!");
                    Environment.Exit(1);
                }
            }
        }

        private void Arrival(string qSource, Tuple<string, Queue> queue, double time)
        {
            (_, Queue q) = queue;
            CountTime(time);
            double aux = -1;

            if (!q.HasSpace())
            {
                q.Losses++;
            } // :( queue is full...
            else
            {
                q.State++;
                if (q.State <= q.Servers)
                {
                    if (ConsumeRandom(ref aux))
                    {
                        Tuple<string, Queue> qt = GetTarget(q);
                        ScheduleExit(queue, qt?.Item1, aux);
                    }
                }
            }

            if (qSource != null) { return; }

            if (ConsumeRandom(ref aux))
            {
                ScheduleArrival(null, queue, aux);
            }
        }

        private void Exit(Tuple<string, Queue> queue, double time)
        {
            (_, Queue q) = queue;
            CountTime(time);
            double aux = -1;
            q.State--;
            if (q.State >= q.Servers)
            {
                if (ConsumeRandom(ref aux))
                {
                    Tuple<string, Queue> qt = GetTarget(q);
                    ScheduleExit(queue, qt?.Item1, aux);
                }
            }
        }

        private void ScheduleArrival(string qSource, Tuple<string, Queue> queue, double aux)
        {
            (string qName, Queue q) = queue;
            double result = ElapsedTime + ToInterval(q.MaxArrival, q.MinArrival, aux);
            var e = new Event(EventType.Arrival, result, qName, qSource);
            _eventList.SortedInsertion(e);
        }

        private void ScheduleExit(Tuple<string, Queue> qSource, string qTarget, double aux)
        {
            (string qName, Queue q) = qSource;
            double result = ElapsedTime + ToInterval(q.MaxService, q.MinService, aux);

            var e = new Event(EventType.Departure, result, qTarget, qName);
            _eventList.SortedInsertion(e);
        }

        private void CountTime(double time)
        {
            double tempoAnterior = ElapsedTime;
            ElapsedTime = time;
            double posTemAux = ElapsedTime - tempoAnterior;

            foreach (Queue q in _queues.Values)
            {
                q.IncrStateTime(q.State, posTemAux);
            }
        }

        private bool ConsumeRandom(ref double aux)
        {
            if (_rndNumbers.Count > 0)
            {
                aux = _rndNumbers.First.Value;
                _rndNumbers.RemoveFirst();
                return true;
            }

            return false;
        }

        private static double ToInterval(double max, double min, double aux)
        {
            return (max - min) * aux + min;
        }

        private Tuple<string, Queue> GetTarget(Queue q)
        {
            string tName = q.Connection?.Target;
            return tName != null ? new Tuple<string, Queue>(tName, _queues[tName]) : null;
        }
    }
}
