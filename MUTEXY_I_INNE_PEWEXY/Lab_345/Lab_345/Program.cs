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
        //static int CCA_number = 0;
        //static int CA_number = 0;
        //static int SGA_number = 0;
        //static int LCA_number = 10;
        //static int LCA_e_number = 5;
        //static int LCA_list_size = 10060;
        //static int LCA_range = LCA_list_size / LCA_number;
        static int TSA_number = 9;

        static void Main(string[] args)
        {
            Threads threds = new Threads(GenerateRunnables());

            RunFibers(GenerateRunnables());
            
            Console.ReadLine();
        }

        static List<IRunnable> GenerateRunnables()
        {
            var agents = new List<IRunnable>();
            //for (int i = 0; i < CCA_number; i++)
            //{
            //    agents.Add(new ConstantCountingAgent(i));
            //}
            //for (int i = 0; i < CA_number; i++)
            //{
            //    agents.Add(new CountingAgent(i + CA_number));
            //}
            //for (int i = 0; i < SGA_number; i++)
            //{
            //    agents.Add(new SineGeneratingAgent(i + CA_number + CCA_number));
            //}

            //List<int> args = new List<int>();
            //Random rnd = new Random();
            //for (int i = 0; i < LCA_list_size; i++)
            //{
            //    int rand = rnd.Next(1, 100);
            //    args.Add(rand);
            //}

            //for (int id = 0; id < LCA_number; id++)
            //{
            //    int beginn = id * LCA_range;
            //    int endd = ((id + 1) * LCA_range);

            //    if (id == LCA_number - 1)
            //    {
            //        endd = LCA_list_size;
            //    }

            //    agents.Add(new ListCountingAgent(id, beginn, endd, ref args, LCA_e_number));
            //}

            //agents.Add(new ListsSummingAgent(agents.Count(), agents));

            string text = System.IO.File.ReadAllText("mojText.txt");

            agents.Add(new TextDividingAgent(agents.Count()-1, text, TSA_number));
            for (int i = 0; i < TSA_number; i++)
                agents.Add(new TextSummingAgent(agents.Count()-1, (TextDividingAgent)agents[0], TSA_number));

            var agentss = new List<TextSummingAgent>();
            foreach (var a in agents)
            {
                if (a is TextSummingAgent)
                    agentss.Add((TextSummingAgent)a);
            }

            agents.Add(new TextJoiningAgent(agents.Count() - 1, agentss));

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

            //int sum = 0;
            //foreach (Agent agent in agents)
            //{
            //    sum = sum + agent.Suma;
            //}
            //Console.WriteLine("Suma wlokien: " + sum);

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
                    //Console.WriteLine("Odpalam watek \n");
                    var t = new Thread(ag.Run);
                    threads.Add(t);
                    t.Start();
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

            //int sum = 0;
            //foreach (Agent agent in agents)
            //{
            //    sum = sum + agent.Suma;
            //}
            //Console.WriteLine("Suma watkow: " + sum);

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
