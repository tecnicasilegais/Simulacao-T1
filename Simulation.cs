using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using YamlDotNet.Serialization;

namespace Simulacao_T1
{
    internal class Simulation
    {
        private LinkedList<double> _rndNumbers;

        private double _elapsedTime;
        private readonly Dictionary<string, Queue> _queues;
        private readonly LinkedList<Event> _eventList;
        public Simulation(Dictionary<string, Queue> queues, LinkedList<double> rndNumbers)
        {
            this._queues = queues;
            this._rndNumbers = rndNumbers;
            this._elapsedTime = 0;
            this._eventList = new LinkedList<Event>();
            this.Initialize();
        }

        private void Initialize()
        {
            _queues.Where(q => q.Value.HasOutsideArrival)
                .AsParallel().ForAll(q => ScheduleFirstArrival(q.Key, q.Value.Arrival));
            void ScheduleFirstArrival(string target, double time)
            {
                var e = new Event(EventType.Arrival, time, target, null);
                _eventList.SortedInsertion(e);
            }
        }
        
        public void Simulate()
        {

            while (_rndNumbers.Count != 0)
            {
                try
                {
                    var currEvent = _eventList.First.Value;

                    switch (currEvent.Type)
                    {
                        case EventType.Arrival:
                        {
                            var tarQueue = _queues[currEvent.Target];
                            _eventList.RemoveFirst();
                            Arrival(currEvent.Source, new(currEvent.Target, tarQueue), currEvent.Time);
                            break;
                        }
                        case EventType.Departure: // TODO arrival when it comes from another queue
                        {
                            var srcQueue = _queues[currEvent.Source];
                            Exit(new(currEvent.Source, srcQueue), currEvent.Time);
                            if (currEvent.Target != null)
                            {
                                Arrival(currEvent.Source, new(currEvent.Target, _queues[currEvent.Target]), currEvent.Time);
                            }
                            _eventList.RemoveFirst();
                            break;
                        }
                        default:
                            Console.WriteLine("Something unexpected happened in the EventList");
                            Environment.Exit(2);
                            break;
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine("The initial arrivals must be declared!");
                    Environment.Exit(1);
                }
            }

        }

        private void Arrival(string qSource, Tuple<string,Queue> queue, double time)
        {
            var (qName, q) = queue;
            CountTime(q, time);
            double aux = -1;

            if (!q.HasSpace()){q.Losses++;} // :( queue is full...
            else
            {
                q.State++;
                if (q.State <= q.Servers)
                {
                    if (ConsumeRandom(ref aux))
                    {
                        var qt = GetTarget(q);
                        ScheduleExit(queue, qt?.Item1, aux);
                    }
                }
            }

            if (qSource != null) return;
            if (ConsumeRandom(ref aux))
            {
                ScheduleArrival(null, queue, aux);
            }
        }

        private void Exit(Tuple<string,Queue> queue, double time)
        {
            var (qName, q) = queue;
            CountTime(q, time); 
            double aux = -1;
            q.State--;
            if (q.State >= q.Servers)
            {
                if (ConsumeRandom(ref aux))
                {
                    var qt = GetTarget(q);
                    ScheduleExit(queue, qt?.Item1, aux);
                }
            }
        }

        private void ScheduleArrival(string qSource, Tuple<string,Queue> queue, double aux)
        {
            var (qName, q) = queue;
            double result = _elapsedTime + ToInterval(q.MaxArrival, q.MinArrival, aux);
            var e = new Event(EventType.Arrival, result, qName, qSource);
            _eventList.SortedInsertion(e);
        }

        void ScheduleExit(Tuple<string, Queue> qSource, string qTarget, double aux)
        {
            var (qName, q) = qSource;
            double result = _elapsedTime + ToInterval(q.MaxService, q.MinService, aux);

            var e = new Event(EventType.Departure, result, qTarget, qName);
            _eventList.SortedInsertion(e);
        }

        private void CountTime(Queue q, double time)
        {

            int aux = q.State;

            double tempoAnterior = _elapsedTime;
            _elapsedTime = time;
            double posTemAux = _elapsedTime - tempoAnterior;
            q.IncrStateTime(aux, posTemAux);
        }

        bool ConsumeRandom(ref double aux)
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

        private Tuple<string,Queue> GetTarget(Queue q)
        {
            if (q.Connection == null)
            {
                return null;
            }
            var tName = q.Connection.Target;
            return tName != null ? new Tuple<string, Queue>(tName, _queues[tName]) : null;
        }
    }
}

