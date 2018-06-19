using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Bank
{
    class Client : IRunnable
    {
        Bank _bank;


        public Client(IRunnable b) {
            _bank = (Bank)b;
        }

        public void Update()
        {
            _bank.getMutex().WaitOne();
            Thread.Sleep(1000);
            _bank.Withdraw(100);
            _bank.getMutex().ReleaseMutex();
        }

        public void Run()
        {
            while (_bank.getBalance() >= 0)
                Update();
        }
    }
}
