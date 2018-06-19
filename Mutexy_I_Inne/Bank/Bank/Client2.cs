using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Client2 :IRunnable
    {
        Bank _bank;

        public Client2(IRunnable b)
        {
            _bank = (Bank)b;
        }
        public void Withdraw(double cash)
        {
            double cur_bal = _bank.getBalance();
            System.Threading.Thread.Sleep(5000);
            _bank.setBalance(cur_bal + cash);
            Console.WriteLine("You can eat meat now.");
        }
    }
}
