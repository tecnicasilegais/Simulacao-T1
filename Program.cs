using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Simulacao_T1
{
    class Program
    {
        //O resultado da média de 5 execuções
        static void Main(string[] args)
        {
            TextReader file = new StreamReader(@"data\model.yml");
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var model = deserializer.Deserialize<Model>(file);

            foreach (double modelRndNumber in model.RndNumbers)
            {
                Console.WriteLine(modelRndNumber);
            }
        }

    }
}
