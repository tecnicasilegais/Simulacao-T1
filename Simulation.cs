using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

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
            Arrival(_queue.Arrival); //first arrival
            double lessTime = 0;
            var lessPos = 0;

            while (_rndNumbers.Count != 0)
            {
                if (_queueState == _queue.Capacity && !_queue.IsInfinite)
                {

                    lessTime = _queue.EventList[0].Time;
                    lessPos = 0;
                    for (var i = 0; i < _queue.EventList.Count; i++)
                    {
                        if (_queue.EventList[lessPos].Time > _queue.EventList[i].Time)
                        {
                            lessTime = _queue.EventList[i].Time;
                            lessPos = i;
                        }
                    }

                    if (_queue.EventList[lessPos].Type == EventType.Arrival)
                    {
                        _losses++;

                        _queue.EventList.RemoveAt(lessPos);
                        Arrival(lessTime);
                    }
                    else
                    {
                        Exit(lessTime);
                        _queue.EventList.RemoveAt(lessPos);
                    }

                }
                else
                {
                    lessTime = _queue.EventList[0].Time;
                    lessPos = 0;
                    for (var i = 0; i < _queue.EventList.Count; i++)
                    {
                        if (_queue.EventList[lessPos].Time > _queue.EventList[i].Time)
                        {
                            lessTime = _queue.EventList[i].Time;
                            lessPos = i;
                        }

                    }

                    if (_queue.EventList[lessPos].Type == EventType.Arrival)
                    {

                        Arrival(lessTime);
                        _queue.EventList.RemoveAt(lessPos);

                    }
                    else if (_queue.EventList[lessPos].Type == EventType.Departure)
                    {
                        Exit(lessTime);
                        _queue.EventList.RemoveAt(lessPos);
                    }
                    lessTime = 0;
                    lessPos = 0;
                }
            }

        }
        void Arrival(double time)
        {
            double aux = -1;
            int posFila = _queueState;
            CountTime(time);

            if (_queueState < _queue.Capacity)
            {
                _queueState = posFila + 1;
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
            double aux = -1;
            CountTime(time);
            int posFila = _queueState;
            _queueState = posFila - 1;
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
            _queue.EventList.Add(e);
        }

        void ScheduleExit(double aux)
        {
            double result = _elapsedTime + (((_queue.MaxService - _queue.MinService) * aux)
                    + _queue.MinService);

            var e = new Event(EventType.Departure, result);
            _queue.EventList.Add(e);

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

