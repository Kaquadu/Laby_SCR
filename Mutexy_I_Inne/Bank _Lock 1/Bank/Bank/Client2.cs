using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Bank
{
    class Client2 : IRunnable
    {
        Bank _bank;
        public Client2(IRunnable b)
        {
            _bank = (Bank)b;

        }

        public void Run()
        {
            lock (_bank.thisLock)
            {
                Console.WriteLine("Adding started.");
                _bank.Add(100);
                Console.WriteLine("Adding finished.");
            }
        }
    }
}

