using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using YamlDotNet.Core;
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

            foreach (double modelRndNumber in model.RndNumbers)
            {
                Console.WriteLine(modelRndNumber);
            }

            Console.WriteLine("Hello World!");
            var lixo = new RandNumbers(10);
            Console.WriteLine(string.Join(',', lixo.NextNDoubles(1000)));
        }

    }
}
