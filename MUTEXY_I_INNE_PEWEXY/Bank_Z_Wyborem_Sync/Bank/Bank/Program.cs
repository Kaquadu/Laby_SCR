using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Bank
{

    public interface IRunnable
    {
        void Run();
       // Mutex mut { get; set; }
        //bool HasFinished { get; set; }
    }

    public struct Operate
    {
        public string _operation;
        public int _amount;
        public Operate(string op, int am)
        {
            _operation = op;
            _amount = am;
        }
    }

    class Program
    {
        public static Bakery bakery = new Bakery(20);
        public static void GenerateRunnables(List<IRunnable> agents, int choice)
        {
            if (choice != 6)
                agents.Add(new Bank(1000, choice, bakery));
            else agents.Add(new Bank_V(1000));
            int cash = 100;
            bool positive = false;
            for (int i = 0; i < 10; i++)
                agents.Add(new Client(agents[0], agents.Count()-1, choice, cash, positive));
            cash = 300;
            positive = true;
            for (int i = 0; i < 10; i++)
                agents.Add(new Client(agents[0], agents.Count()-1, choice, cash, positive));
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
            Console.WriteLine("Wybierz metode synchronizacji: \n" +
                "1. Bez synchronizacji \n" +
                "2. Mutex \n" +
                "3. Lock \n" +
                "4. Spinlock \n" +
                "5. Interlock \n" +
                "6. Volatile \n" +
                "7. Memory Barier \n" +
                "8. Queue \n" +
                "9. Bakery \n");
            int choice;
            choice = Convert.ToInt32(Console.ReadLine());
            List<IRunnable> agents = new List<IRunnable>();
            GenerateRunnables(agents, choice);
            RunThreads(agents);
            Console.ReadLine();
        }
    }
}
