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
        abstract public void Update(List<IRunnable> listAgents);

        abstract public int GetSum();

        public IEnumerator<float> CoroutineUpdate()
        {
            //float i = 0;
            while (HasFinished != true)
            {
                Update();
                virtualTimeS += 0.1f;
                //System.Threading.Thread.Sleep(100);
                yield return virtualTimeS;
            }
            yield break;
        }

        public void Run()
        {
            while (!HasFinished)
            {
                {
                    Update();
                    virtualTimeS += 0.1f;
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        public void Run(List<IRunnable> listAgents)
        {
            Console.WriteLine("Utknal?? \n");
            while (!HasFinished)
            {
                {
                    Update(listAgents);
                    virtualTimeS += 0.1f;
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        public bool HasFinished { get; set; } = false;


        public int ID;
        double frequency;
        public float virtualTimeS = 0.0f;

    }
}



//__________################____________
//_________##################___________
//_________####[ ]####[ ]####___________
//_________########\/########___________
//_________##################___________