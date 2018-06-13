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

        public void Update()
        {
            _bank.getMutex().WaitOne();
            Thread.Sleep(3000);
            _bank.Add(300);
            _bank.getMutex().ReleaseMutex();
        }

        public void Run()
        {
                while (_bank.getBalance() >= 0)
                    Update();
        }
    }
}
