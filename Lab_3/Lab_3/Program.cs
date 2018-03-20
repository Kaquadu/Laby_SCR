using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Lab_3
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                int ilosc_iteracji = 20;
                Console.WriteLine("Wybierz metode: ");
                Console.WriteLine("1. Iteracyjnie");
                Console.WriteLine("2. Watkowo");
                Console.WriteLine("3. Oba po kolei");
                int wybor;
                wybor = Int32.Parse(Console.ReadLine());

                switch (wybor)
                {
                    case 1:
                        {
                            Iterating(ilosc_iteracji);
                            break;
                        }
                    case 2:
                        {
                            Threading(ilosc_iteracji);
                            break;
                        }
                    case 3:
                        {
                            Iterating(ilosc_iteracji);
                            Threading(ilosc_iteracji);
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }
                //Console.ReadLine();
            }
        }

        static void Iterating(int ilosc)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            dataCenter[] ourDatas = new dataCenter[ilosc];
            for (int i = 0; i < ilosc; i++)
            {
                ourDatas[i] = new dataCenter(1000, 350000);
                ourDatas[i].arrayGenerator();
                ourDatas[i].arraySorter();
                ourDatas[i].onSqr();
                ourDatas[i].inSqr();
                ourDatas[i].difference();
                ourDatas[i].calculateSet();
            }

            sw.Stop();
            float ticks = sw.ElapsedTicks;
            float elapsedTime = ticks / Stopwatch.Frequency;
            Console.WriteLine("Czas wykonywania iteracyjnego: " + elapsedTime + "s");

        }

        static void Threading(int ilosc)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Threads[] threads = new Threads[ilosc];
            for (int i = 0; i < ilosc; i++)
            {
                threads[i] = new Threads();
            }

            sw.Stop();
            float ticks = sw.ElapsedTicks;
            float elapsedTime = ticks / Stopwatch.Frequency;
            Console.WriteLine("Czas wykonywania watkowego: " + elapsedTime + "s");
        }
    }

}


class Threads
{
    public Threads()
    {
        thread = new Thread(handleObject);
        thread.Start();
    }
    ~Threads()
    {

    }

    public void handleObject()
    {
        //Console.WriteLine("Start watka.");
        Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)127;
        ourDatas = new dataCenter(1000, 350000);
        ourDatas.arrayGenerator();
        ourDatas.arraySorter();
        ourDatas.onSqr();
        ourDatas.inSqr();
        ourDatas.difference();
        ourDatas.calculateSet();
        //Console.WriteLine("Koniec watka.");
    }

    Thread thread;
    dataCenter ourDatas;
}


class dataCenter
{
    public dataCenter(int Min, int Max)
    {
        randoms = new double[Max - Min];
        inRsA = new double[Max - Min];
        onRsA = new double[Max - Min];
        onRsFIELD = new double[Max - Min];
        inRsFIELD = new double[Max - Min];
        fieldDiff = new double[Max - Min];
        RandFILEDset = new double[Max - Min, 2];

        MIN_NR = Min;
        MAX_NR = Max;
    }
    ~dataCenter() { }

    public void arrayGenerator()
    {
        for (int i = 0; i < MAX_NR - MIN_NR; i++)
        {
            randoms[i] = rnd.Next(MIN_NR, MAX_NR);
        }
        //Console.WriteLine("GENERUJE! \n");
    }

    public void arraySorter()
    {
        Array.Sort(randoms);
    }

    public void onSqr()
    {
        for (int i = 0; i < MAX_NR - MIN_NR; i++)
        {
            onRsA[i] = 2.0 * randoms[i];
            onRsFIELD[i] = onRsA[i] * onRsA[i];
        }
    }

    public void inSqr()
    {
        for (int i = 0; i < MAX_NR - MIN_NR; i++)
        {
            inRsA[i] = (2.0 * randoms[i]) / Math.Sqrt(2);
            inRsFIELD[i] = inRsA[i] * inRsA[i];
        }
    }

    public void difference()
    {
        for (int i = 0; i < MAX_NR - MIN_NR; i++)
        {
            fieldDiff[i] = onRsFIELD[i] - inRsFIELD[i];
        }
    }

    public void calculateSet()
    {
        for (int i = 0; i < MAX_NR - MIN_NR; i++)
        {
            RandFILEDset[i, 0] = randoms[i];
            RandFILEDset[i, 1] = fieldDiff[i];
        }
    }

    Random rnd = new Random();
    public int MIN_NR = 0;
    public int MAX_NR = 0;
    public double[] randoms;
    public double[] inRsA;
    public double[] inRsFIELD;
    public double[] fieldDiff;
    public double[,] RandFILEDset;
    public double[] onRsA;
    public double[] onRsFIELD;

}