using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Bank
{
    class Bank : IRunnable
    {
        public double balance;
        private static Timer aTimer = new Timer();

        public Bank(double initial_balance)
        {
            balance = initial_balance;
            aTimer.Interval = 2000;
            aTimer.Elapsed += Update;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
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
    }
}
