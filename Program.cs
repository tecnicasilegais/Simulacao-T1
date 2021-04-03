using System;
using System.Collections.Generic;
using System.IO;
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
			List<double> rndNumbers = model.RndNumbers;
			List<double> queueState = new List<double>();
			List<Event> eventList = new List<Event>();
			double elapsedTime = 0;
			int losses = 0;
			int queuePos = 0;

			for (int i = 0; i < model.Queues[0].Capacity + 1; i++)
            {
                queueState.Add(0.0);
            }

			filaSimples();

			for (int i = 0; i <= model.Queues[0].Capacity; i++)
			{

				Console.WriteLine("State " + i + " Time: " + queueState[i]);
			}
			Console.WriteLine("Total Time: " + elapsedTime);
			Console.WriteLine("Losses: " + losses);

			void filaSimples()
			{
				elapsedTime = 0;
				arrival(model.Arrivals);
				double lessTime = 0;
				int lessPos = 0;

				while (rndNumbers.Count != 0)
				{
					if (queuePos == model.Queues[0].Capacity)
					{

						lessTime = eventList[0].getTime();
						lessPos = 0;
						for (int i = 0; i < eventList.Count; i++)
						{
							if (eventList[lessPos].getTime() > eventList[i].getTime())
							{
								lessTime = eventList[i].getTime();
								lessPos = i;
							}
						}

						if (eventList[lessPos].getType() == 1)
						{
							losses++;

							eventList.RemoveAt(lessPos);
							arrival(lessTime);
						}
						else
						{
							exit(lessTime);
							eventList.RemoveAt(lessPos);
						}

					}
					else
					{
						lessTime = eventList[0].getTime();
						lessPos = 0;
						for (int i = 0; i < eventList.Count; i++)
						{
							if (eventList[lessPos].getTime() > eventList[i].getTime())
							{
								lessTime = eventList[i].getTime();
								lessPos = i;
							}

						}

						if (eventList[lessPos].getType() == 1)
						{

							arrival(lessTime);
							eventList.RemoveAt(lessPos);

						}
						else if (eventList[lessPos].getType() == 0)
						{
							exit(lessTime);
							eventList.RemoveAt(lessPos);
						}
						lessTime = 0;
						lessPos = 0;
					}
				}

			}
			void arrival(double time)
			{

				int posFila = queuePos;
				countTime(time);

				if (queuePos < model.Queues[0].Capacity)
				{
					queuePos = posFila + 1;
					if (queuePos <= model.Queues[0].Servers)
					{
						scheduleExit();                    }
				}
				scheduleArrival();
			}
			void exit(double time)
			{

				countTime(time);
				int posFila = queuePos;
				queuePos = posFila - 1;
				if (queuePos >= model.Queues[0].Servers)
				{
					scheduleExit();
				}
			}

			void scheduleArrival()
			{
				double aux = rndNumbers[0];
				rndNumbers.RemoveAt(0);
				double result = elapsedTime
						+ (((model.Queues[0].MaxArrival - model.Queues[0].MinArrival) * aux) + model.Queues[0].MinArrival);
				Event e = new Event(1, result);
				eventList.Add(e);
			}

			void scheduleExit()
			{
				double aux = rndNumbers[0];
				rndNumbers.RemoveAt(0);
				double result = elapsedTime + (((model.Queues[0].MaxService - model.Queues[0].MinService) * aux)
						+ model.Queues[0].MinService);

				Event e = new Event(0, result);
				eventList.Add(e);

			}

			void countTime(double time) { 

				int aux = queuePos;

				double tempoAnterior = elapsedTime;
				elapsedTime = time;
				double posTemAux = elapsedTime - tempoAnterior;
				double timeAux = queueState[aux] + posTemAux;
				queueState[aux] = timeAux;

			}
		}

	}
}
