using System.Collections.Generic;
using System.Text;

namespace Simulacao_T1
{
    internal class SimulationReport
    {
        private readonly double _elapsedTime;
        private readonly int _nSimulations;
        private readonly Dictionary<string, Queue> _queues;
        private readonly StringBuilder _sb;

        public SimulationReport(Dictionary<string, Queue> queues, double elapsedTime, int nSimulations)
        {
            this._queues = queues;
            this._elapsedTime = elapsedTime / nSimulations;
            this._nSimulations = nSimulations;
            this._sb = new StringBuilder();
        }

        private void PrintStateStats(Queue q)
        {
            var limiter = q.IsInfinite ? q.StateStats.Count-1 : q.Capacity;
            for (var i = 0; i <= limiter; i++)
            {
                _sb.AppendLine(
                    $" {i,3}{"",2} | {q.GetStateTime(i) / _nSimulations,20:F4} | {q.GetStateTime(i) / _nSimulations / _elapsedTime,9:P}");
            }
        }

        public string PrintReport()
        {
            foreach (var entry in _queues)
            {
                var (k, q) = entry;
                _sb.Append($"Queue {k}: G/G/{q.Servers}");
                _sb.AppendLine( q.IsInfinite? "" : $"/{q.Capacity}");


                if (q.MinArrival > 0 && q.MaxArrival > 0)
                {
                    _sb.AppendLine("Arrival " + q.MinArrival + " .. " + q.MaxArrival);
                }

                _sb.AppendLine("Service " + q.MinService + " .. " + q.MaxService);

                _sb.AppendLine($" State | {"Time",12}{"",8} | {"Probability",11}");
                _sb.AppendLine($" {"".PadLeft(5, '-')} | {"".PadLeft(20, '-')} | {"".PadLeft(11, '-')}");

                PrintStateStats(q);

                _sb.AppendLine();
                _sb.AppendLine($"{"Losses:",-20}{q.Losses / _nSimulations:F0}\n\n");
            }

            _sb.AppendLine($"{"Simulation Time:",-20}{_elapsedTime:F4}");

            return _sb.ToString();
        }
    }
}
