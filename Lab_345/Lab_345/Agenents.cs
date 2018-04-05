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
                this.HasFinished = true;
            }
        }

        private int CCA_i = 0;
    }

    class CountingAgent : Agent
    {
        public CountingAgent(int Identify) : base(Identify)
        { }

        override public void Update()
        {
            CA_i++;
            if (CA_i == ID)
            {
                Console.WriteLine("I counted to my ID and my Id is {0}.", this.ID);
                this.HasFinished = true;
            }
        }

        private int CA_i = 0;
    }

    class SineGeneratingAgent : Agent
    {
        public SineGeneratingAgent(int Identify) : base(Identify)
        { }

        override public void Update()
        {
            SGA_i++;
            if ((this.virtualTimeS / 1000) == (this.ID % 10))
            {
                Output = (float)Math.Sin((double)(this.virtualTimeS));
                Console.WriteLine("I made a sine and my Id is {0}.", this.ID);
                this.HasFinished = true;
            }
        }

        public float Output;
        private int SGA_i = 0;
    }
}
