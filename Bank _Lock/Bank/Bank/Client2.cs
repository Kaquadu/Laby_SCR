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
                bool passed = false;
                while (!passed)
                {
                    try
                    {
                        _bank._spinlock.Enter(ref _bank.lockTaken);
                        Console.WriteLine("Adding started.");
                        passed = true;
                    }
                    catch { }
                }
                if (_bank.lockTaken)
                {

                    _bank.Add(100);
                    _bank._spinlock.Exit();
                    Console.WriteLine("Adding finished.");
                }
        }
    }
}

