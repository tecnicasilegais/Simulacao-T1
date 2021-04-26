using System.Collections.Generic;

namespace Simulacao_T1
{
    public class Model
    {
        public Dictionary<string, Queue> Queues { get; set; }
        public LinkedList<double> RndNumbers { get; set; }
        public int RndNumbersPerSeed { get; set; }
        public List<double> Seeds { get; set; }
    }
}
