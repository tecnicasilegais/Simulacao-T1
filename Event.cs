﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Simulacao_T1
{
    internal class Event
    {
        public int Type { get; set; }
        public double Time { get; set; }

        public Event(int type, double time)
        {
            Type = type;
            Time = time;
        }
    }
}