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



        public void Run()
        {
            lock (_bank.thisLock)
            {
                Console.WriteLine("Withdrawing started.");
                _bank.Withdraw(100);
                Console.WriteLine("Withdrawing finished.");
            }
        }

        }
    }

