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
        private volatile int balance;
        private static System.Timers.Timer aTimer = new System.Timers.Timer();

        public Bank(int initial_balance)
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

        public void setBalance(int update_balance)
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

        public void Withdraw(int cash)
        {
            if (balance > cash)
            {
                balance -= cash;
                Console.WriteLine("WYJALEM sobie: {0}", cash);
            }
            else Console.WriteLine("Eat jars.");
        }

        public void Add(int cash)
        {
            balance += cash;
            Console.WriteLine("DODALEM sobie: {0}", cash);
        }
    }
}
