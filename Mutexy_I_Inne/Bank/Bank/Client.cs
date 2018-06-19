using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Client :IRunnable
    {
        Bank _bank;


        public Client(IRunnable b) {
            _bank = (Bank)b;
        }

        public void Withdraw(double cash)
        {
            double cur_bal =_bank.getBalance();
            System.Threading.Thread.Sleep(2000);
            if (cur_bal > cash)
                _bank.setBalance(cur_bal - cash);
            else Console.WriteLine("Eat jars.");
        }
    }
}
