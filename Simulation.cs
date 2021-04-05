﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Simulacao_T1
{
    class Simulation
    {
        private LinkedList<Decimal> _rndNumbers;

        int _losses = 0;
        int _queuePos = 0;
        Decimal _elapsedTime = 0;

        public void Simulate()
        {
            var _queues = Model.Queues;
            Arrival(Model.Arrivals); //first arrival
            Decimal lessTime = 0;
            var lessPos = 0;

            while (_rndNumbers.Count != 0)
            {
                if (_queuePos == Model.Queues[0].Capacity)
                {

                    lessTime = _queues[0].EventList[0].Time;
                    lessPos = 0;
                    for (var i = 0; i < _queues[0].EventList.Count; i++)
                    {
                        if (_queues[0].EventList[lessPos].Time > _queues[0].EventList[i].Time)
                        {
                            lessTime = _queues[0].EventList[i].Time;
                            lessPos = i;
                        }
                    }

                    if (_queues[0].EventList[lessPos].Type == 1)
                    {
                        _losses++;

                        _queues[0].EventList.RemoveAt(lessPos);
                        Arrival(lessTime);
                    }
                    else
                    {
                        Exit(lessTime);
                        _queues[0].EventList.RemoveAt(lessPos);
                    }

                }
                else
                {
                    lessTime = _queues[0].EventList[0].Time;
                    lessPos = 0;
                    for (var i = 0; i < _queues[0].EventList.Count; i++)
                    {
                        if (_queues[0].EventList[lessPos].Time > _queues[0].EventList[i].Time)
                        {
                            lessTime = _queues[0].EventList[i].Time;
                            lessPos = i;
                        }

                    }

                    if (_queues[0].EventList[lessPos].Type == 1)
                    {

                        Arrival(lessTime);
                        _queues[0].EventList.RemoveAt(lessPos);

                    }
                    else if (_queues[0].EventList[lessPos].Type == 0)
                    {
                        Exit(lessTime);
                        _queues[0].EventList.RemoveAt(lessPos);
                    }
                    lessTime = 0;
                    lessPos = 0;
                }
            }

        }
        void Arrival(Decimal time)
        {
            Decimal aux = -1;
            int posFila = _queuePos;
            CountTime(time);

            if (_queuePos < Model.Queues[0].Capacity)
            {
                _queuePos = posFila + 1;
                if (_queuePos <= Model.Queues[0].Servers)
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
        void Exit(Decimal time)
        {
            Decimal aux = -1;
            CountTime(time);
            int posFila = _queuePos;
            _queuePos = posFila - 1;
            if (_queuePos >= Model.Queues[0].Servers)
            {
                if (ConsumeRandom(ref aux))
                {
                    ScheduleExit(aux);
                }
            }
        }

        void ScheduleArrival(Decimal aux)
        {
            Decimal result = _elapsedTime + (((Model.Queues[0].MaxArrival - Model.Queues[0].MinArrival) * aux) + Model.Queues[0].MinArrival);
            var e = new Event(1, result);
            Model.Queues[0].EventList.Add(e);
        }

        void ScheduleExit(Decimal aux)
        {
            Decimal result = _elapsedTime + (((Model.Queues[0].MaxService - Model.Queues[0].MinService) * aux)
                    + Model.Queues[0].MinService);

            var e = new Event(0, result);
            Model.Queues[0].EventList.Add(e);

        }

        void CountTime(Decimal time)
        {

            int aux = _queuePos;

            Decimal tempoAnterior = _elapsedTime;
            _elapsedTime = time;
            Decimal posTemAux = _elapsedTime - tempoAnterior;
            Decimal timeAux = Model.Queues[0].QueueStates[aux] + posTemAux;
            Model.Queues[0].QueueStates[aux] = timeAux;

        }

        bool ConsumeRandom(ref Decimal aux)
        {
            if (_rndNumbers.Count > 0)
            {
                aux = _rndNumbers.First.Value;
                _rndNumbers.RemoveFirst();
                return true;
            }
            return false;
        }

        public Simulation(Model model, LinkedList<Decimal> rndNumbers)
        {
            this.Model = model;
            this._rndNumbers = rndNumbers;
        }

        public Model Model { get; }

        public int Losses => this._losses;
        public Decimal ElapsedTime => this._elapsedTime;
    }
}

