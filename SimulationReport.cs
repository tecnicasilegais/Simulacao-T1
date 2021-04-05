using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Simulacao_T1
{
    class SimulationReport
    {
        private List<Simulation> _sims;
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
                Queue = _sims[0].Model.Queues[0], //takes first simulation's queue data
                ElapsedTime = _sims[0].ElapsedTime,
                Losses = _sims[0].Losses
            };
            for (var i = 1; i < _sims.Count; i++) //sums remaning data
            {
                _sd.ElapsedTime += _sims[i].ElapsedTime;
                _sd.Losses += _sims[i].Losses;
                for (var j = 0; j < _sims[i].Model.Queues[0].QueueStates.Count; j++)
                {
                    _sd.Queue.QueueStates[j] += _sims[i].Model.Queues[0].QueueStates[j];
                }
            }
        }

        public String PrintReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine($" State\t {"Time",12}{"",8}\t {"Probability",11}");
            for (var i = 0; i <= _sd.Queue.Capacity; i++)
            {

                sb.AppendLine($" {i,3}\t {_sd.Queue.QueueStates[i],20:F4}\t {_sd.Queue.QueueStates[i] / _sd.ElapsedTime,9:P}");
            }
            sb.AppendLine();
            sb.AppendLine($"Losses: {_sd.Losses}");

            return sb.ToString();
        }

        class SimulationData
        {
            public Model.Queue Queue { get; set; }
            public Decimal ElapsedTime { get; set; }
            public int Losses { get; set; }
        }
    }
}
