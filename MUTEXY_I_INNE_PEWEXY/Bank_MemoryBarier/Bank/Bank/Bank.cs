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
        public int balance;
        private static System.Timers.Timer aTimer = new System.Timers.Timer();

        public Bank(double initial_balance)
        {

            balance = (int)initial_balance;
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
            balance = (int)update_balance;
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
                Interlocked.Add(ref balance, (int)-cash);
                Console.WriteLine("WYJALEM sobie: {0}", cash);
            }
            else Console.WriteLine("Eat jars.");
        }

        public void Add(double cash)
        {
            Interlocked.Add(ref balance, (int)cash);
            Console.WriteLine("DODALEM sobie: {0}", cash);
        }
    }
}
