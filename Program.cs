using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Simulacao_T1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Queue Simulator");
            Console.WriteLine("Eduardo Andrade, Marcelo Heredia, Michael Rosa");


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

            Console.WriteLine("Loading Settings");

            if (model.Seeds != null) //running using seed
            {
                foreach (var seed in model.Seeds)
                {
                    var generator = new RandNumberGenerator(seed);
                    sims.Add(new Simulation(model.Queues, generator.NextNDoubles(model.RndNumbersPerSeed)));
                }
            }
            else //using fixed random numbers given in YML
            {
                var sim = new Simulation(model.Queues, model.RndNumbers);
                sims.Add(sim);
            }

            double elapsedTime = 0;
            var i = 0;
            foreach (var sim in sims)
            {
                if (model.Seeds != null)
                {
                    Console.WriteLine("Running simulation {0} using Seed: {1}", i + 1, model.Seeds[i]);
                    i++;
                }

                sim.Simulate();
                elapsedTime += sim.ElapsedTime;
                foreach (var q in model.Queues.Values)
                {
                    q.Restart();
                }
            }

            var nSimulations = model.Seeds?.Count ?? 1;
            var report = new SimulationReport(model.Queues, elapsedTime, nSimulations);

            Console.WriteLine(report.PrintReport());
        }
    }
}
