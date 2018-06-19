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
        string trybPracy = "Cykl";
        bool hasFinished = false;

        public Client(IRunnable b) {
            _bank = (Bank)b;
        }

        public void Update()
        {

            if (trybPracy == "Oczekuj")
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

            if (trybPracy == "Anuluj")
            {
                try
                {
                    _bank._spinlock.Enter(ref _bank.lockTaken);
                }
                catch
                { }
                if (_bank.lockTaken)
                {
                    _bank.Withdraw(100);
                    _bank._spinlock.Exit();
                    Console.WriteLine("Withdrawing finished.");
                    hasFinished = true;
                }
                else hasFinished = true;
            }

            if (trybPracy == "Cykl")
            {
                try
                {
                    _bank._spinlock.Enter(ref _bank.lockTaken);
                    Console.WriteLine("Withdrawing started.");
                }
                catch
                { }
                if (_bank.lockTaken)
                {
                    _bank.Withdraw(100);
                    _bank._spinlock.Exit();
                    Console.WriteLine("Withdrawing finished.");
                }

            }
        }

        public void Run()
        {
            while (hasFinished != true)
            {
                Thread.Sleep(100);
                Update();
            }
        }

        }
    }

