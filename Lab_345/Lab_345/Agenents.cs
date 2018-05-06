using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_345
{
    class ConstantCountingAgent : Agent
    {
        public ConstantCountingAgent(int Identify) : base(Identify)
        { }

        override public void Update()
        {
            CCA_i++;
            if (CCA_i == 10)
            {
                Console.WriteLine("I counted to 10 and my Id is {0}.", this.ID);
                HasFinished = true;
            }
        }

        private int CCA_i = 0;
        public override void Update(List<IRunnable> listAgents) { }
        public override int GetSum() { return 0; }
    }

    class CountingAgent : Agent
    {
        public CountingAgent(int Identify) : base(Identify)
        { }

        override public void Update()
        {
            CA_i++;
            if (CA_i >= ID)
            {
                Console.WriteLine("I counted to my ID and my Id is {0}.", this.ID);
                HasFinished = true;
            }
        }

        private int CA_i = 0;
        public override void Update(List<IRunnable> listAgents) { }
        public override int GetSum() { return 0; }
    }

    class SineGeneratingAgent : Agent
    {
        public SineGeneratingAgent(int Identify) : base(Identify)
        { }

        public override void Update()
        {
            SGA_i++;
            Output = (float)Math.Sin(this.virtualTimeS);
            if (this.virtualTimeS >= this.ID % 10)
            {              
                Console.WriteLine("I made a sine and my Id is {0}.", this.ID);
                HasFinished = true;
            }
            //Console.WriteLine("I wanna do a sinus!");
        }

        public float Output;
        private int SGA_i = 0;
        public override void Update(List<IRunnable> listAgents) { }
        public override int GetSum() { return 0; }
    }

    class ListCountingAgent : Agent
    {
       public ListCountingAgent(int Identify, List<int> list, int max_range, int range_div) : base (Identify)
        {
            range = new int[2];
            ints = list;
            int mod = max_range % range_div;
            //Console.WriteLine("Modulo: {0}", mod);
            int tmp = (max_range - mod) / range_div;
            range[0] = (tmp - 1) * range_div;
            range[1] = max_range;
            //Console.WriteLine("Zakres 0: {0}", range[0]);
            //Console.WriteLine("Zakres 0: {0}", range[1]);
        }

        public override void Update()
        {
            LCA_i++;
            sum += ints[LCA_i + range[0]];
            Console.WriteLine("I made a sum! My sum is: {0}.", sum);
            HasFinished = true;
        }

        public void Divide(int n)
        {

        }

        public int LCA_i = 0;
        private int[] range;
        public int sum = 0;
        List<int> ints;
        public override void Update(List<IRunnable> listAgents) { }
        public override int GetSum() { return sum; }
    }

    class ListSummingAgent : Agent
    {
        public ListSummingAgent(int Identify) : base(Identify)
        {
        }
        public override void Update() { }
        public override void Update(List<IRunnable> listAgents)
        {
            Console.WriteLine("Utknal?? \n");
            foreach (var ag in listAgents)
            {
                if (ag is ListCountingAgent)
                    if (ag.HasFinished == true)
                        sum += ag.GetSum();

                if (ag is ListCountingAgent && ag.HasFinished == false)
                { 
                    HasFinished = false;
                    Console.WriteLine("Utknal?? \n");
                }
                else
                {
                    HasFinished = true; Console.WriteLine("I made a full sum! My full sum is: {0}.", sum);
                }
            }
        }

        public int LSA_i = 0;
        public int sum;

        public override int GetSum() { return 0; }
    }
}
