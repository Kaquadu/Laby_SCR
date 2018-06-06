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
                bool passed = false;
                while (!passed)
                {
                    try
                    {
                        _bank._spinlock.Enter(ref _bank.lockTaken);
                        Console.WriteLine("Withdrawing started.");
                        passed = true;
                    }
                    catch { }
                    System.Threading.Thread.Sleep(100);
                }
                if (_bank.lockTaken)
                {
                    _bank.Withdraw(100);
                    _bank._spinlock.Exit();
                    Console.WriteLine("Withdrawing finished.");
                }
        }

        }
    }

