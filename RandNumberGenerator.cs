using System.Collections.Generic;

namespace Simulacao_T1
{
    public class RandNumberGenerator
    {
        private const int M = int.MaxValue;
        private const int A = 1664525;
        private const int C = 1013904223;
        private double _seed;

        public RandNumberGenerator(double seed)
        {
            _seed = seed;
        }

        public RandNumberGenerator()
        {
            _seed = 7; //default seed
        }

        public double NextDouble()
        {
            _seed = (A * _seed + C) % M;
            return _seed / M;
        }

        public LinkedList<double> NextNDoubles(int n)
        {
            var lst = new LinkedList<double>();
            for (int i = 0; i < n; i++)
            {
                lst.AddLast(NextDouble());
            }

            return lst;
        }

        public double NextDouble(double min, double max)
        {
            return (max - min) * NextDouble() + min;
        }

        public LinkedList<double> NextNDoubles(int n, int min, int max)
        {
            var lst = new LinkedList<double>();
            for (int i = 0; i < n; i++)
            {
                lst.AddLast((max - min) * NextDouble() + min);
            }

            return lst;
        }

        public LinkedList<decimal> NextNDecimals(int n)
        {
            var lst = new LinkedList<decimal>();
            for (int i = 0; i < n; i++)
            {
                lst.AddLast(new decimal(NextDouble()));
            }

            return lst;
        }

        public LinkedList<decimal> NextNDecimals(int n, int min, int max)
        {
            var lst = new LinkedList<decimal>();
            for (int i = 0; i < n; i++)
            {
                lst.AddLast(new decimal((max - min) * NextDouble() + min));
            }

            return lst;
        }
    }
}
