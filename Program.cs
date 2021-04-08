using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Simulacao_T1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("Indicate file to test, must be in data folder");
                Console.WriteLine("Call example: Simulator.exe model.yml");
            }
            TextReader file = new StreamReader($"data/{args[0]}");
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var model = deserializer.Deserialize<Model>(file);

            var sims = new List<Simulation>();
            if (model.Seeds != null) //running using seed
            {
                foreach (var seed in model.Seeds)
                {
                    var generator = new RandNumberGenerator(seed);
                    sims.Add(new Simulation(model.Queues[0], generator.NextNDoubles(model.RndNumbersPerSeed)));
                }
            }
            else //using fixed random numbers given in YML
            {
                var sim = new Simulation(model.Queues[0], model.RndNumbers);
                sims.Add(sim);
            }

            foreach(var sim in sims)
            {
                sim.Simulate();
                model.Queues[0].Restart();
            }

            var report = new SimulationReport(sims);

            Console.WriteLine(report.PrintReport());
        }
    }
}
