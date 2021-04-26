using System.Collections.Generic;
using System.Text;

namespace Simulacao_T1
{
    internal class SimulationReport
    {
        private readonly double elapsedTime;
        private readonly int nSimulations;
        private readonly Dictionary<string, Queue> queues;

        public SimulationReport(Dictionary<string, Queue> queues, double elapsedTime, int nSimulations)
        {
            this.queues = queues;
            this.elapsedTime = elapsedTime / nSimulations;
            this.nSimulations = nSimulations;
        }

        public string PrintReport()
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, Queue> entry in queues)
            {
                (string k, Queue q) = entry;
                sb.Append($"Queue {k}: G/G/{q.Servers}");
                if (!q.IsInfinite) { sb.AppendLine($"/{q.Capacity}"); }

                sb.AppendLine();
                sb.AppendLine($" State | {"Time",12}{"",8} | {"Probability",11}");
                sb.AppendLine($" {"".PadLeft(5, '-')} | {"".PadLeft(20, '-')} | {"".PadLeft(11, '-')}");
                for (int i = 0; i <= q.Capacity; i++)
                {
                    sb.AppendLine(
                        $" {i,3}{"",2} | {q.GetStateTime(i) / nSimulations,20:F4} | {q.GetStateTime(i) / nSimulations / elapsedTime,9:P}");
                }

                sb.AppendLine();
                sb.AppendLine($"{"Losses:",-20}{q.Losses / nSimulations:F0}");
            }

            sb.AppendLine($"{"Simulation Time:",-20}{elapsedTime:F4}");

            return sb.ToString();
        }
    }
}
