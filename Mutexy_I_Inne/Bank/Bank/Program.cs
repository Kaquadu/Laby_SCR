﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Bank
{

    public interface IRunnable
    {
        void Run();
        //bool HasFinished { get; set; }
    }

    class Program
    {
        public static void GenerateRunnables(List<IRunnable> agents)
        {
            agents.Add(new Bank(1000.0));
            agents.Add(new Client(agents[0]));
            agents.Add(new Client2(agents[0]));
        }

        static void RunThreads(List<IRunnable> agents)
        {
            List<Thread> threads = new List<Thread>();

            foreach (IRunnable agent in agents)
            {
                threads.Add(new Thread(new ThreadStart(agent.Run)));
            }
            Console.WriteLine("Zakonczono wywolywanie watkow");

            foreach (Thread thr in threads)
            {
                thr.Start();
            }
        }

        static void Main(string[] args)
        {
            Bank bank = new Bank(1000.0);
            Console.ReadLine();
        }
    }
}
