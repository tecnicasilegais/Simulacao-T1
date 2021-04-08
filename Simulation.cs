using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using System.Threading.Channels;

namespace Simulacao_T1
{
    class Simulation
    {
        private LinkedList<double> _rndNumbers;

        int _losses = 0;
        int _queueState = 0;
        double _elapsedTime = 0;
        private Queue _queue;

        public void Simulate()
        {
            ScheduleFirstArrival(_queue.Arrival); //first arrival

            while (_rndNumbers.Count != 0)
            {
                try
                {
                    var currEvent = _queue.EventList.First.Value;

                    if (_queueState == _queue.Capacity && !_queue.IsInfinite) //Queue is at maximum capacity
                    {

                        if(currEvent.Type == EventType.Arrival)//Someone is coming to the queue
                        {
                            _losses++;

                            _queue.EventList.RemoveFirst();
                            Arrival(currEvent.Time);
                        }
                        else
                        {
                            Exit(currEvent.Time);
                            _queue.EventList.RemoveFirst();
                        }

                    }
                    else
                    {
                        if (currEvent.Type == EventType.Arrival)
                        {

                            Arrival(currEvent.Time);
                            _queue.EventList.RemoveFirst();

                        }
                        else if (currEvent.Type == EventType.Departure)
                        {
                            Exit(currEvent.Time);
                            _queue.EventList.RemoveFirst();
                        }
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine("Empty Event List");
                    return;
                }
            }

        }
        void Arrival(double time)
        {
            CountTime(time);
            double aux = -1;

            if (_queueState < _queue.Capacity)
            {
                _queueState++;
                if (_queueState <= _queue.Servers)
                {
                    if (ConsumeRandom(ref aux))
                    {
                        ScheduleExit(aux);
                    }
                }
            }
            if (ConsumeRandom(ref aux))
            {
                ScheduleArrival(aux);
            }
        }
        void Exit(double time)
        {
            CountTime(time);
            double aux = -1;
            _queueState--;
            if (_queueState >= _queue.Servers)
            {
                if (ConsumeRandom(ref aux))
                {
                    ScheduleExit(aux);
                }
            }
        }

        void ScheduleArrival(double aux)
        {
            double result = _elapsedTime + (((_queue.MaxArrival - _queue.MinArrival) * aux) + _queue.MinArrival);
            var e = new Event(EventType.Arrival, result);
            _queue.EventList.SortedInsertion(e);
        }

        void ScheduleFirstArrival(double time)
        {
            var e = new Event(EventType.Arrival, time);
            _queue.EventList.SortedInsertion(e);
        }

        void ScheduleExit(double aux)
        {
            double result = _elapsedTime + (((_queue.MaxService - _queue.MinService) * aux)
                    + _queue.MinService);

            var e = new Event(EventType.Departure, result);
            _queue.EventList.SortedInsertion(e);

        }

        void CountTime(double time)
        {

            int aux = _queueState;

            double tempoAnterior = _elapsedTime;
            _elapsedTime = time;
            double posTemAux = _elapsedTime - tempoAnterior;
            _queue.IncrStateTime(aux, posTemAux);
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

        public Simulation(Queue queue, LinkedList<double> rndNumbers)
        {
            this._queue = queue;
            this._rndNumbers = rndNumbers;
        }

        public Queue Queue => this._queue;

        public int Losses => this._losses;
        public double ElapsedTime => this._elapsedTime;
    }
}

