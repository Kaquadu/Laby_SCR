using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Collections;

namespace Bank
{
    class Bank : IRunnable
    {
        public int balance;
        int choice;

        private static System.Timers.Timer aTimer = new System.Timers.Timer();

        public static Mutex mut = new Mutex();

        public Object thisLock = new Object();

        public SpinLock _spinlock = new SpinLock();
        public bool lockTaken = false;

        public static Queue _queue = new Queue();
        public Queue syncQueue = Queue.Synchronized(_queue);

        public Mutex getMutex()
        {
            return mut;
        }

        public Bank(int initial_balance, int choice)
        {
            this.choice = choice;
            balance = initial_balance;
        }

        public void UpdateT(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (choice == 7)
                Thread.MemoryBarrier();
            Console.WriteLine("Obecny stan KONTA: {0}.", balance);
            if (choice == 7)
                Thread.MemoryBarrier();
        }

        public void UpdateQueue(Operate _op)
        {
            switch (_op._operation)
            {
                case "Deposit":
                    {
                        //Console.WriteLine("DEPO started");
                        System.Threading.Thread.Sleep(1000);
                        balance += _op._amount;
                        Console.WriteLine("DODALEM sobie: {0}, stan KONTA: {1}", _op._amount, balance);
                        break;
                    }
                case "Withdraw":
                    {
                        //Console.WriteLine("WITHDRAW started");
                        System.Threading.Thread.Sleep(3000);
                        balance -= _op._amount;
                        Console.WriteLine("WYJALEM sobie: {0}, stan KONTA: {1}", _op._amount, balance);
                        break;
                    }
                default: break;
            }
        }

        public int getBalance()
        {
            return balance;
        }

        public void setBalance(int update_balance)
        {
            balance = update_balance;
        }

        public void Run()
        {
            aTimer.Interval = 2000;
            aTimer.Elapsed += UpdateT;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            Thread.Sleep(500);

            while (true)
            {
                lock (syncQueue.SyncRoot) //  Some operation on the collection, which is now thread safe.
                {
                    UpdateQueue((Operate)syncQueue.Dequeue());
                    Console.WriteLine("W kolejce pozostalo {0} klientow. ", syncQueue.Count);
                }
                System.Threading.Thread.Sleep(2000);
            }
        }

        public void Withdraw(int cash)
        {
            Thread.Sleep(1000);
            if (balance > cash)
            {
                balance -= cash;
                Console.WriteLine("WYJALEM sobie: {0}", cash);
            }
            else Console.WriteLine("Eat jars.");
        }

        public void Add(int cash)
        {
            Thread.Sleep(3000);
            balance += cash;
            Console.WriteLine("DODALEM sobie: {0}, stan KONTA: {1}", cash, balance);
        }

        public void AddToQueue(Operate op)
        {
            syncQueue.Enqueue(op);
            Console.WriteLine("Dodano klienta do KOLEJKI.");
        }

        public void Withdraw(int cash, int method)
        {
            if (balance > cash)
            {
                if (method == 1)
                {
                    Thread.Sleep(1000);
                    Interlocked.Add(ref balance, -cash);
                    Console.WriteLine("WYJALEM sobie: {0}, stan KONTA: {1}", cash, balance);
                }
                if (method == 2)
                {
                    Thread.Sleep(1000);
                    Thread.MemoryBarrier();
                    balance -= cash;
                    Thread.MemoryBarrier();
                    Console.WriteLine("WYJALEM sobie: {0}, stan KONTA: {1}", cash, balance);
                }
                if (method == 3)
                {
                    AddToQueue(new Operate("Withdraw", cash));
                }
            }
            else Console.WriteLine("Eat jars.");
        }



        public void Add(int cash, int method)
        {
            if (method == 1)
            {
                Thread.Sleep(3000);
                Interlocked.Add(ref balance, cash);
                Console.WriteLine("DODALEM sobie: {0}, stan KONTA: {1}", cash, balance);
            }
            if (method == 2)
            {
                Thread.Sleep(3000);
                Thread.MemoryBarrier();
                balance += cash;
                Thread.MemoryBarrier();
                Console.WriteLine("DODALEM sobie: {0}, stan KONTA: {1}", cash, balance);
            }
            if (method == 3)
            {
                AddToQueue(new Operate("Deposit", cash));
            }
        }
    }
}
