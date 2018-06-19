using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace Bank
{
    class Bank : IRunnable
    {
        public double balance;
        private static System.Timers.Timer aTimer = new System.Timers.Timer();
        public static Mutex mut = new Mutex();

        public Mutex getMutex()
        {
            return mut;
        }

        public Bank(double initial_balance)
        {

            balance = initial_balance;
        }

        public void Update(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("The current bank balance is {0}.", balance);
        }

        public double getBalance()
        {
            return balance;
        }

        public void setBalance(double update_balance)
        {
            balance = update_balance;
        }

        public void Run()
        {
            aTimer.Interval = 2000;
            aTimer.Elapsed += Update;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void Withdraw(double cash)
        {
            if (balance > cash)
            {
                balance -= cash;
                Console.WriteLine("WYJALEM sobie: {0}", cash);
            }
            else Console.WriteLine("Eat jars.");
        }

        public void Add(double cash)
        {
            balance += cash;
            Console.WriteLine("DODALEM sobie: {0}", cash);
        }
    }
}
