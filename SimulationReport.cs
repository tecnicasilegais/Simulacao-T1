using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Simulacao_T1
{
    class SimulationReport
    {

        private Dictionary<string, Queue> queues;
        private double elapsedTime;
        public SimulationReport(Dictionary<String,Queue> queues, double elapsedTime)
        {
            this.queues = queues;
            this.elapsedTime = elapsedTime;
        }

        public String PrintReport()
        {
            var sb = new StringBuilder();
            foreach(KeyValuePair<string, Queue> entry in queues)
            {
                var (k, q) = entry;
                sb.Append($"Queue {k}: G/G/{q.Servers}");
                if (!q.IsInfinite) { sb.AppendLine($"/{q.Capacity}"); }
            sb.AppendLine();
            sb.AppendLine($" State | {"Time",12}{"",8} | {"Probability",11}");
            sb.AppendLine($" {"".PadLeft(5, '-')} | {"".PadLeft(20, '-')} | {"".PadLeft(11, '-')}");
            for (var i = 0; i <= q.Capacity; i++)
            {

                sb.AppendLine($" {i,3}{"",2} | {q.GetStateTime(i),20:F4} | {q.GetStateTime(i) / elapsedTime,9:P}");
            }
            sb.AppendLine();
            sb.AppendLine($"{"Losses:",-20}{q.Losses}");
            }
            sb.AppendLine($"{"Simulation Time:",-20}{this.elapsedTime:F4}");

            return sb.ToString();
        }

    }
}
