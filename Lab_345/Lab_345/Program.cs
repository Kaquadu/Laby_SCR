using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Lab_345
{

    class Program
    {
        static int CCA_number = 10;
        static int CA_number = 10;
        static int SGA_number = 10;

        static void Main(string[] args)
        {
            List<IRunnable> agents = new List<IRunnable>();
            agents = GenerateRunnables();
            //RunThreads(agents);
            RunFibers(agents);
            Console.ReadLine();
        }

        static List<IRunnable> GenerateRunnables()
        {
            var agents = new List<IRunnable>();
            for (int i = 0; i < CCA_number; i++)
            {
                agents.Add(new ConstantCountingAgent(i));
            }
            for (int i = 0; i < CA_number; i++)
            {
                agents.Add(new CountingAgent(i + CA_number));
            }
            for (int i = 0; i < SGA_number; i++)
            {
                agents.Add(new SineGeneratingAgent(i + CA_number + SGA_number));
            }

            return agents;
        }

        static void RunThreads(IEnumerable<IRunnable> agents)
        {
            var threads = new List<Thread>(agents.Count());

            foreach (Agent ag in agents)
            {
                var t = new Thread(ag.Run);
                threads.Add(t);
                t.Start();
            }

            bool allFinished = false;
            while (!allFinished)
            {
                Thread.Sleep(100);
                allFinished = !agents.Any(r => !r.HasFinished);
                //Console.WriteLine("Not finished! \n");  
            }
            Console.WriteLine("### Finished! ### \n");
        }

        static void RunFibers(IEnumerable<IRunnable> agents)
        {
            var enumerators = agents.Select(r => r.CoroutineUpdate());
            var timeStep = 0.0f;
            bool allFinished = false;
            while (!allFinished)
            {
                foreach (var enumer in enumerators)
                {
                    if (enumer.MoveNext())
                    {
                        timeStep = enumer.Current;
                    }
                }
                allFinished = !agents.Any(r => !r.HasFinished);
                Thread.Sleep(100);
            }
        }
    }
}
