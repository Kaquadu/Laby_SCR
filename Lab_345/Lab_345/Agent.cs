using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_345
{
    abstract class Agent : IRunnable
    {
        public Agent(int Identify)
        {
            ID = Identify;
            frequency = 100;
        }

        abstract public void Update();

        public IEnumerator<float> CoroutineUpdate()
        {
            float i = 0;
            while (HasFinished != true)
            {
                this.Update();
                this.virtualTimeS += 100;
                System.Threading.Thread.Sleep(100);
                yield return i;
            }
            yield break;
        }

        public void Run()
        {
            while (!HasFinished)
            {
                {
                    this.Update();
                    this.virtualTimeS += 100;
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        public bool HasFinished { get; set; } = false;
        public int ID;
        double frequency;
        protected float virtualTimeS = 0.0f;

    }
}
