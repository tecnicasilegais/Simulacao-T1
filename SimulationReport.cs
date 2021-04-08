using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Simulacao_T1
{
    class SimulationReport
    {
        private readonly List<Simulation> _sims;
        private SimulationData _sd;
        public SimulationReport(List<Simulation> sims)
        {
            _sims = sims;
            GenerateReport();
        }

        private void GenerateReport()
        {
            _sd = new SimulationData()
            {
                Queue = _sims[0].Queue, //takes first simulation's queue data
            };
            for (var i = 1; i < _sims.Count; i++) //sums remaning data
            {
                //_sd.ElapsedTime += _sims[i].ElapsedTime;
                for (var j = 0; j < _sims[i].Queue.StateStats.Count; j++)
                {
                    _sd.Queue.IncrStateTime(j, _sims[i].Queue.GetStateTime(j));
                }
            }

            _sd.Losses = _sims.Sum(s => s.Losses);
            _sd.ElapsedTime = _sd.Queue.StateStats.Sum();
        }

        public String PrintReport()
        {
            var sb = new StringBuilder();
            sb.Append($"Queue: G/G/{_sd.Queue.Servers}");
            if (!_sd.Queue.IsInfinite) { sb.AppendLine($"/{_sd.Queue.Capacity}"); }

            sb.AppendLine();
            sb.AppendLine($" State | {"Time",12}{"",8} | {"Probability",11}");
            sb.AppendLine($" {"".PadLeft(5, '-')} | {"".PadLeft(20, '-')} | {"".PadLeft(11, '-')}");
            for (var i = 0; i <= _sd.Queue.Capacity; i++)
            {

                sb.AppendLine($" {i,3}{"",2} | {_sd.Queue.GetStateTime(i),20:F4} | {_sd.Queue.GetStateTime(i) / _sd.ElapsedTime,9:P}");
            }
            sb.AppendLine();
            sb.AppendLine($"{"Losses:",-20}{_sd.Losses}");
            sb.AppendLine($"{"Simulation Time:",-20}{_sd.ElapsedTime:F4}");

            return sb.ToString();
        }

        class SimulationData
        {
            public Queue Queue { get; set; }
            public double ElapsedTime { get; set; }
            public int Losses { get; set; }
        }
    }
}
