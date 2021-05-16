using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
            foreach (var keyValuePair in _queues.Where(q => q.Value.HasOutsideArrival))
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
            var totalTicks = _eventList.Count;
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
                            var tarQueue = _queues[currEvent.Target];
                            _eventList.RemoveFirst();
                            Arrival(currEvent.Source, new(currEvent.Target, tarQueue), currEvent.Time);
                            pbar.Tick();
                            break;
                        }
                        case EventType.Departure: // TODO arrival when it comes from another queue
                        {
                            var srcQueue = _queues[currEvent.Source];
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
            var (_, q) = queue;
            CountTime(time);
            double rnd = -1;

            if (!q.HasSpace())
            {
                q.Losses++;
            } // :( queue is full...
            else
            {
                q.State++;
                if (q.State <= q.Servers)
                {
                    if (ConsumeRandom(ref rnd))
                    {
                        HandleQueueExit(queue, rnd);
                    }
                }
            }

            if (qSource != null)
            {
                return;
            }

            if (ConsumeRandom(ref rnd))
            {
                ScheduleArrival(null, queue, rnd);
            }
        }

        private void Exit(Tuple<string, Queue> queue, double time)
        {
            var (_, q) = queue;
            CountTime(time);
            double rnd = -1;
            q.State--;
            if (q.State < q.Servers) return;
            if (ConsumeRandom(ref rnd))
            {
                HandleQueueExit(queue, rnd);
            }
        }

        private void HandleQueueExit(Tuple<string, Queue> queue, double rnd)
        {
            var (_, q) = queue;

            switch (q.Connections.Count)
            {
                case 0:
                    ScheduleExit(queue, null, rnd);
                    break;
                case 1 when q.Connections[0].Probability == 1:
                    ScheduleExit(queue,q.Connections[0].Target, rnd);
                    break;
                default:
                {
                    double rnd2 = -1;
            
                    if (!ConsumeRandom(ref rnd2)) return;
            
                    var target = ChooseRandomTarget(q, rnd2);
                    ScheduleExit(queue, target, rnd);
                    break;
                }
            }
            
        }

        private void ScheduleArrival(string qSource, Tuple<string, Queue> queue, double rnd)
        {
            var (qName, q) = queue;
            var result = ElapsedTime + ToInterval(q.MaxArrival, q.MinArrival, rnd);
            var e = new Event(EventType.Arrival, result, qName, qSource);
            _eventList.SortedInsertion(e);
        }

        private void ScheduleExit(Tuple<string, Queue> qSource, string qTarget, double rnd)
        {
            var (qName, q) = qSource;
            var result = ElapsedTime + ToInterval(q.MaxService, q.MinService, rnd);

            var e = new Event(EventType.Departure, result, qTarget, qName);
            _eventList.SortedInsertion(e);
        }

        private void CountTime(double time)
        {
            var tempoAnterior = ElapsedTime;
            ElapsedTime = time;
            var posTemAux = ElapsedTime - tempoAnterior;

            foreach (var q in _queues.Values)
            {
                q.IncrStateTime(q.State, posTemAux);
            }
        }

        private bool ConsumeRandom(ref double rnd)
        {
            if (_rndNumbers.Count <= 0) return false;
            rnd = _rndNumbers.First.Value;
            _rndNumbers.RemoveFirst();
            return true;

        }

        private static double ToInterval(double max, double min, double rnd)
        {
            return (max - min) * rnd + min;
        }

        private static string ChooseRandomTarget(Queue q, double rnd)
        {
            double acc = 0;
            string target = null;
            foreach (var conn in q.Connections)
            {
                acc += conn.Probability;
                if (!(rnd <= acc)) continue;
                target = conn.Target;
                break;
            }

            return target;
        }
    }
}