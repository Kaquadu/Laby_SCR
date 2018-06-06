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
            while(true)
            {
                _bank.addToQueue(new Operate("Deposit", 100.0));
                System.Threading.Thread.Sleep(1000);
            }
            
        }
    }
}

