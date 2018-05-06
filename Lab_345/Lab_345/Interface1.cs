using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_345
{
    interface IRunnable
    {
        void Run();
        IEnumerator<float> CoroutineUpdate();
        bool HasFinished { get; set; }
        int GetSum();
    }
}
