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
        static int ilosc_agentow = 10;
        static ConstantCountingAgent[] constantCountingAgent = new ConstantCountingAgent[ilosc_agentow];
        static CountingAgent[] countingAgent = new CountingAgent[ilosc_agentow];
        static SineGeneratingAgent[] sineGeneratingAgent = new SineGeneratingAgent[ilosc_agentow];

        static void Main(string[] args)
        {
            GenerateRunnables();
            RunThreads();
        }

        static void GenerateRunnables()
        {     
            for (int i = 0; i < ilosc_agentow; i++)
            {
                constantCountingAgent[i] = new ConstantCountingAgent(i);
                countingAgent[i] = new CountingAgent(i + (ilosc_agentow));
                sineGeneratingAgent[i] = new SineGeneratingAgent(i + (2 * (ilosc_agentow)));
            }
        }

        static void RunThreads()
        {
            Thread[] threads = new Thread[ilosc_agentow * 3];

            for (int i = 0; i < ilosc_agentow; i++)
            {
                threads[i] = new Thread(constantCountingAgent[i].Run);
                threads[i+ilosc_agentow] = new Thread(countingAgent[i].Run);
                threads[i+(2*ilosc_agentow)] = new Thread(sineGeneratingAgent[i].Run);
            }

        }
    }
}
