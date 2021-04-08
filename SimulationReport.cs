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
                    _sd.Queue.IncrStateTime(j,_sims[i].Queue.GetStateTime(j));
                }
            }
            
            _sd.Losses = _sims.Sum(s => s.Losses);
            _sd.ElapsedTime = _sd.Queue.StateStats.Sum();
        }

        public String PrintReport()
        {
            var sb = new StringBuilder();
            sb.Append($"Queue: G/G/{_sd.Queue.Servers}");
            if (!_sd.Queue.IsInfinite) { sb.AppendLine($"/{_sd.Queue.Capacity}");}

            sb.AppendLine("\n");
            sb.AppendLine($" State\t {"Time",12}{"",8}\t {"Probability",11}");
            for (var i = 0; i <= _sd.Queue.Capacity; i++)
            {

                sb.AppendLine($" {i,3}\t {_sd.Queue.GetStateTime(i),20:F4}\t {_sd.Queue.GetStateTime(i) / _sd.ElapsedTime,9:P}");
            }
            sb.AppendLine();
            sb.AppendLine($"Losses: {_sd.Losses}");
            sb.AppendLine($"Simulation Time: {_sd.ElapsedTime:F4}");

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
