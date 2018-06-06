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
        public double balance;
        private static System.Timers.Timer aTimer = new System.Timers.Timer();
        public static Queue _queue = new Queue();
        public Queue syncQueue = Queue.Synchronized(_queue);

        public Bank(double initial_balance)
        {
            balance = initial_balance;
        }

        public void Update(Operate _op)
        {
            double cur_bal;
            switch (_op._operation)
            {
                case "Deposit" :
                    {
                        Console.WriteLine("DEPO started");
                        cur_bal = getBalance();
                        System.Threading.Thread.Sleep(3000);
                        setBalance(cur_bal + _op._amount);
                        Console.WriteLine("DEPO finished.");
                        break;
                    }
                case "Withdraw":
                    {
                        Console.WriteLine("WITHDRAW started");
                        cur_bal = getBalance();
                        System.Threading.Thread.Sleep(3000);
                        setBalance(cur_bal - _op._amount);
                        Console.WriteLine("WITHDRAW finished.");
                        break;
                    }
                default: break;
            }
        }

        public void UpdateTime(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("The current bank balance is {0}.", balance);
        }

        public double getBalance()
        {
            return balance;
        }

        public void addToQueue(Operate op)
        {
            syncQueue.Enqueue(op);
            Console.WriteLine("Dodano klienta do KOLEJKA.");
        }

        public void setBalance(double update_balance)
        {
            Console.WriteLine("Balance updated");
            balance = update_balance;
        }

        public void Run()
        {
            aTimer.Interval = 2000;
            aTimer.Elapsed += UpdateTime;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            while(true)
            {
                lock (syncQueue.SyncRoot) //  Some operation on the collection, which is now thread safe.
                {
                    Update((Operate)syncQueue.Dequeue());
                    Console.WriteLine("W kolejce pozostalo {0} klientow. ", syncQueue.Count);
                }
                System.Threading.Thread.Sleep(2000);
            }    
        }
    }
}
