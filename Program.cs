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
            TextReader file = new StreamReader(@"data\model.yml");
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var model = deserializer.Deserialize<Model>(file);

            var sims = new List<Simulation>();
            if (model.Seeds != null) //running using seed
            {
                for (var i = 0; i < model.Seeds.Count; i++)
                {
                    var generator = new RandNumberGenerator(model.Seeds[i]);
                    sims.Add(new Simulation(model, generator.NextNDoubles(model.RndNumbersPerSeed)));
                }
            }
            else //using fixed random numbers given in YML
            {
                var sim = new Simulation(model, model.RndNumbers);
                sims.Add(sim);
            }

            foreach(var sim in sims)
            {
                sim.Simulate();
            }

            var report = new SimulationReport(sims);

            Console.WriteLine(report.PrintReport());
        }
    }
}
