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
        Bank_V _bank_V;
        int choice;
        int cash = 100;
        bool positive = true;
        bool hasFinished = false;
        string trybPracy = "Cykl";

        public Client(IRunnable b, int choice, int cash, bool positive) {
            if (choice != 6)
                _bank = (Bank)b;
            else _bank_V = (Bank_V)b;
            this.choice = choice;
            this.cash = cash;
            this.positive = positive;
        }

        //        "1. Bez synchronizacji \n" +
        //        "2. Mutex \n" +
        //        "3. Lock \n" +
        //        "4. Spinlock \n" +
        //        "5. Interlock \n" +
        //        "6. Volatile \n" +
        //        "7. Memory Barier \n" +
        //        "8. Queue \n");


        public void Update()
        {
            if (choice == 1)
                UpNoSync();
            if (choice == 2)
                UpMutex();
            if (choice == 3)
                UpLock();
            if (choice == 4)
                UpSpinlock();
            if (choice == 5)
                UpInterlock();
            if (choice == 6)
                UpVolatile();
            if (choice == 7)
                UpMemoryBarier();
            if (choice == 8)
                UpQueue();
        }

        public void Run()
        {
            if (choice != 6)
                while (_bank.getBalance() >= 0)
                    Update();
            else while (_bank_V.getBalance() >= 0)
                    Update();
        }


        //diffrent updates


        public void UpNoSync()
        {
            if (!positive)
                _bank.Withdraw(cash);
            else
                _bank.Add(cash);
        }

        public void UpMutex()
        {
            _bank.getMutex().WaitOne();

            if (!positive)
                _bank.Withdraw(cash);
            else
                _bank.Add(cash);

            _bank.getMutex().ReleaseMutex();
        }

        public void UpLock()
        {
            lock (_bank.thisLock)
            {
                if (!positive)
                    _bank.Withdraw(cash);
                else
                    _bank.Add(cash);
            }
        }

        public void UpSpinlock()
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
                    if (!positive)
                        _bank.Withdraw(cash);
                    else
                        _bank.Add(cash);
                    try
                    {
                        _bank._spinlock.Exit();
                        Console.WriteLine("Withdrawing finished.");
                    }
                    catch { }
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
                    if (!positive)
                        _bank.Withdraw(cash);
                    else
                        _bank.Add(cash);
                    try
                    {
                        _bank._spinlock.Exit();
                        Console.WriteLine("Withdrawing finished.");
                        hasFinished = true;
                    } catch { }
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
                    if (!positive)
                        _bank.Withdraw(cash);
                    else
                        _bank.Add(cash);
                    try
                    {
                        _bank._spinlock.Exit();
                        Console.WriteLine("Withdrawing finished.");
                    } catch { }
                }

            }
        }

        public void UpInterlock()
        {
            if (!positive)
                _bank.Withdraw(cash, 1);
            else
                _bank.Add(cash, 1);
        }

        public void UpVolatile()
        {
            if (!positive)
                _bank_V.Withdraw(cash);
            else
                _bank_V.Add(cash);
        }

        public void UpMemoryBarier()
        {
            if (!positive)
                _bank.Withdraw(cash, 2);
            else
                _bank.Add(cash, 2);
        }

        public void UpQueue()
        {
            if (!positive)
                _bank.Withdraw(cash, 3);
            else
                _bank.Add(cash, 3);
            System.Threading.Thread.Sleep(5000);
        }
    }
}
