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
        string trybPracy = "Anuluj";
        bool hasFinished = false;

        public Client2(IRunnable b)
        {
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
                        Console.WriteLine("Adding started 2.");
                        passed = true;
                    }
                    catch { }
                    System.Threading.Thread.Sleep(100);
                }
                if (_bank.lockTaken)
                {
                    _bank.Add(100);
                    _bank._spinlock.Exit();
                    Console.WriteLine("Adding finished 2.");
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
                    _bank.Add(100);
                    _bank._spinlock.Exit();
                    Console.WriteLine("Adding finished.");
                    hasFinished = true;
                }
                else
                    hasFinished = true;
            }

            if (trybPracy == "Cykl")
            {
                try
                {
                    _bank._spinlock.Enter(ref _bank.lockTaken);
                }
                catch
                { }
                if (_bank.lockTaken)
                {
                    _bank.Add(100);
                    _bank._spinlock.Exit();
                    Console.WriteLine("Adding finished.");
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

