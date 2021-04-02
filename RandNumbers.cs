using System;
using System.Collections.Generic;

namespace Simulacao_T1
{   
    public class RandNumbers
    {
        private const int M = Int32.MaxValue;
        private const int A = 1664525;
        private const int C = 1013904223;
        private double _seed;

        public RandNumbers(double seed) => _seed = seed;
        public RandNumbers() => _seed = 7; //default seed

        public double NextDouble()
        {
            _seed = ((A * _seed) + C) % M;
            return _seed / M;
        }

        public List<double> NextNDoubles(int n)
        {
            var lst = new List<double>();
            for (var i = 0; i < n; i++)
            {
                lst.Add(NextDouble());
            }
            return lst;
        }

        public double NextDouble(double min, double max)
        {
            return ((max - min) * NextDouble()) + min;
        }
        
        public List<double> NextNDoubles(int n, int min, int max)
        {
            var lst = new List<double>();
            for (var i = 0; i < n; i++)
            {
                lst.Add(((max - min) * NextDouble()) + min);
            }
            return lst;
        }
    }
}