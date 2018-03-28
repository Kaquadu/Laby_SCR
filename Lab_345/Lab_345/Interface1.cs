using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_345
{
    interface IRunnable
    {
        void Run();
        void CoroutineUpdate(float IEnumerator);
        bool HasFinished { get; set; }
    }

    abstract class Agent : IRunnable
    {

        abstract public void Update();

        abstract public void Run();
        abstract public void CoroutineUpdate(float IEnumerator);
        abstract public bool HasFinished { get; set; }

    }

    class ConstantCountingAgent : Agent
    {
        public ConstantCountingAgent(int Identify)
        {
            ID = Identify;
            frequency = 10;
        }
        override public void Update()
        {

        }
        override public void Run() { }
        override public void CoroutineUpdate(float IEnumerator) { }
        override public bool HasFinished { get; set; }

        private int ID;
        private int frequency;
    }

    class CountingAgent : Agent
    {
        public CountingAgent(int Identify)
        {
            ID = Identify;
            frequency = 10;
        }
        override public void Update()
        {
            
        }
        override public void Run() { }
        override public void CoroutineUpdate(float IEnumerator) { }
        override public bool HasFinished { get; set; }

        private int ID;
        private int frequency;
    }

    class SineGeneratingAgent : Agent
    {
        public SineGeneratingAgent(int Identify)
        {
            ID = Identify;
            frequency = 10;
        }
        override public void Update()
        {

        }
        override public void Run() { }
        override public void CoroutineUpdate(float IEnumerator) { }
        override public bool HasFinished { get; set; }

        private int ID;
        private int frequency;
        public double output;
    }
}
