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

        public Bakery bakery;

        private static System.Timers.Timer aTimer = new System.Timers.Timer();

        public static Mutex mut = new Mutex();

        public Object thisLock = new Object();

        public SpinLock _spinlock = new SpinLock(true);

        public static Queue _queue = new Queue();
        public Queue syncQueue = Queue.Synchronized(_queue);

        public Mutex getMutex()
        {
            return mut;
        }

        public Bank(int initial_balance, int choice, Bakery bakery)
        {
            this.choice = choice;
            this.bakery = bakery;
            balance = initial_balance;
        }

        public void UpdateT(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (choice == 2)
            {
                mut.WaitOne();
                Console.WriteLine("Obecny stan KONTA: {0}.", balance);
                mut.ReleaseMutex();
            }
            else if (choice == 4)
            {
                bool lockTaken = false;
                try
                {
                    _spinlock.Enter(ref lockTaken);
                    Console.WriteLine("Obecny stan KONTA: {0}.", balance);
                }
                finally
                {
                    if (lockTaken)
                    {
                        Console.WriteLine("Obecny stan KONTA: {0}.", balance);
                        _spinlock.Exit(false);
                    }
                }
            }
            else if (choice == 5)
                Console.WriteLine("Obecny stan KONTA: {0}.", Interlocked.CompareExchange(ref balance, 0, 0));
            else if (choice == 3)
            {
                lock (thisLock)
                {
                    Console.WriteLine("Obecny stan KONTA: {0}.", balance);
                }
            }
            else if (choice == 9)
            {
                bakery.Lock(0);
                Console.WriteLine("Obecny stan KONTA: {0}.", balance);
                bakery.Unlock(0);
            }
            else if (choice == 7)
            {
                Thread.MemoryBarrier();
                Console.WriteLine("Obecny stan KONTA: {0}.", balance);
                Thread.MemoryBarrier();
            }
            else Console.WriteLine("Obecny stan KONTA: {0}.", balance);
        }

        public void UpdateQueue(Operate _op)
        {
            switch (_op._operation)
            {
                case "Deposit":
                    {
                        //Console.WriteLine("DEPO started");
                        Add(_op._amount);
                        //Console.WriteLine("DODALEM sobie: {0}, stan KONTA: {1}", _op._amount, balance);
                        break;
                    }
                case "Withdraw":
                    {
                        //Console.WriteLine("WITHDRAW started");
                        Withdraw(_op._amount);
                        //Console.WriteLine("WYJALEM sobie: {0}, stan KONTA: {1}", _op._amount, balance);
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

            if (choice == 8)
            {
                while (true)
                {
                    lock (syncQueue.SyncRoot) //  Some operation on the collection, which is now thread safe.
                    {
                        try
                        { UpdateQueue((Operate)syncQueue.Dequeue()); }
                        catch { /*Console.WriteLine("Brak klientow w kolejce.");*/ }
                        Console.WriteLine("W kolejce pozostalo {0} klientow. ", syncQueue.Count);
                    }
                }
            }
        }

        public void Withdraw(int cash)
        {
            Thread.Sleep(100);
            if (balance > cash)
            {
                balance -= cash;
                Console.WriteLine("WYJALEM sobie: {0}, stan KONTA: {1}", cash, balance);
            }
            else Console.WriteLine("Eat jars.");
        }

        public void Add(int cash)
        {
            Thread.Sleep(300);
            balance += cash;
            Console.WriteLine("DODALEM sobie: {0}, stan KONTA: {1}", cash, balance);
        }

        public void AddToQueue(Operate op)
        {
            syncQueue.Enqueue(op);
            //Console.WriteLine("Dodano klienta do KOLEJKI.");
        }

        public void Withdraw(int cash, int method)
        {
            if (balance > cash)
            {
                if (method == 1)
                {
                    Thread.Sleep(100);
                    Interlocked.Add(ref balance, -cash);
                    Console.WriteLine("WYJALEM sobie: {0}, stan KONTA: {1}", cash, balance);
                }
                if (method == 2)
                {
                    Thread.Sleep(100);
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
                Thread.Sleep(300);
                Interlocked.Add(ref balance, cash);
                Console.WriteLine("DODALEM sobie: {0}, stan KONTA: {1}", cash, balance);
            }
            if (method == 2)
            {
                Thread.Sleep(300);
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

        public void Withdraw(int cash, int method, int id)
        {
            bakery.Lock(id);
            if (balance > cash)
            {
                balance -= cash;
                Console.WriteLine("WYJALEM sobie: {0}, stan KONTA: {1}", cash, balance);
            }
            bakery.Unlock(id);
        }

        public void Add(int cash, int method, int id)
        {
            bakery.Lock(id);
            balance += cash;
            Console.WriteLine("DODALEM sobie: {0}, stan KONTA: {1}", cash, balance);
            bakery.Unlock(id);
        }
    }
}
