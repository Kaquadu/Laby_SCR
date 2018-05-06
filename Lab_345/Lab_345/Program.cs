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
        static int CCA_number = 1;
        static int CA_number = 1;
        static int SGA_number = 1;
        static int LCA_number = 4;
        static int LCA_list_size = 1006;
        static int LCA_list_modulo = LCA_list_size % LCA_number;
        static int LCA_range = (LCA_list_size - LCA_list_modulo) / LCA_number;
        

        static void Main(string[] args)
        {
            Threads threds = new Threads(GenerateRunnables());

            RunFibers(GenerateRunnables());
            
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
                agents.Add(new SineGeneratingAgent(i + CA_number + CCA_number));
            }

            List<int> args = new List<int>();
            Random rnd = new Random();
            for (int i = 0; i < LCA_list_size; i++)
            {
                int rand = rnd.Next(1, 1000);
                args.Add(rand);
            }

            for (int i = 0; i < LCA_number; i++)
            {
                if(i + 1 != LCA_number)
                    agents.Add(new ListCountingAgent(agents.Count + 1, args, (i+1)*LCA_range, LCA_range));
                if(i + 1 == LCA_number)
                    agents.Add(new ListCountingAgent(agents.Count + 1, args, (i+1)*LCA_range + LCA_list_modulo, LCA_range));
            }

            agents.Add(new ListSummingAgent(agents.Count + 1));

            return agents;
        }

        

        

        static void RunFibers(IEnumerable<IRunnable> agents)
        {

            Console.WriteLine("### Run fibers! ### \n");
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
            Console.WriteLine("### Finished fibers! ### \n");
        }
    }

    class Threads
    {
        public Threads(List<IRunnable> agents)
        {
            Console.WriteLine("### Run threads! ### \n");
            var threads = new List<Thread>(agents.Count());

            foreach (Agent ag in agents)
            {
                if (!(ag is ListSummingAgent))
                {
                    var t = new Thread(ag.Run);
                    threads.Add(t);
                    t.Start();
                }
                else
                {
                    ThreadStart pts = delegate
                    {
                        ag.Run(agents);
                    };
                    var t = new Thread(ag.Run);
                    threads.Add(t);
                    t.Start();
                }
            }

            bool allFinished = false;
            while (!allFinished)
            {
                Thread.Sleep(100);
                foreach (var ag in agents)
                {
                    allFinished = true;
                    if (!ag.HasFinished)
                    {
                        allFinished = false;
                        break;
                    }
                }
            }
            Console.WriteLine("### Finished threads! ### \n");
        }

        //static void RunThreads(IEnumerable<IRunnable> agents)
        //{
        //    Console.WriteLine("### Run threads! ### \n");
        //    var threads = new List<Thread>(agents.Count());

        //    foreach (Agent ag in agents)
        //    {
        //        var t = new Thread(ag.Run);
        //        threads.Add(t);
        //        t.Start();
        //    }

        //    bool allFinished = false;
        //    while (!allFinished)
        //    {
        //        Thread.Sleep(100);
        //        foreach (var ag in agents)
        //        {


        //            allFinished = true;
        //            if (!ag.HasFinished)
        //            {
        //                allFinished = false;
        //                break;
        //            }
        //        }
        //    }
        //    Console.WriteLine("### Finished threads! ### \n");
        //}
    }
}
