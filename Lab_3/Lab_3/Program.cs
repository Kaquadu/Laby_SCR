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

                int ilosc_iteracji = 10;
                int[] zakres = new int[2];
                zakres[0] = 1000;
                zakres[1] = 10500000;
                int wielkosci_tablic = 75000;

                dataCenter[] ourDatas = new dataCenter[ilosc_iteracji];
                for (int i = 0; i < ilosc_iteracji; i++)
                    ourDatas[i] = new dataCenter(wielkosci_tablic, zakres[0], zakres[1]);

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
                            Iterating(ilosc_iteracji, ourDatas);
                            break;
                        }
                    case 2:
                        {
                            Threading(ilosc_iteracji, ourDatas);
                            break;
                        }
                    case 3:
                        {
                            Iterating(ilosc_iteracji, ourDatas);
                            Threading(ilosc_iteracji, ourDatas);
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }
                Console.ReadLine();
            }
        }

        static void Iterating(int ilosc, dataCenter[] ourDatas)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();


            for (int i = 0; i < ilosc; i++)
            {
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

        static void Threading(int ilosc, dataCenter[] ourDatas)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Threads[] threads = new Threads[ilosc];

            for (int i = 0; i < ilosc; i++)
            {
                threads[i] = new Threads(ourDatas[i], i);
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
    public Threads(dataCenter ourDatas, int i)
    {
        ThreadStart pts = delegate
        {
            generateObject(ourDatas);
        };
        ThreadStart pts2 = delegate
        {
            sortObject(ourDatas);
        };
        ThreadStart pts3 = delegate
        {
            calculateObject(ourDatas);
        };
        IDwatku = i;
        generating = new Thread(pts);
        sorting = new Thread(pts2);
        calculations = new Thread(pts3);
        generating.Start();
    }
    ~Threads()
    {}

    public void generateObject(dataCenter ourDatas)
    {
        Console.WriteLine("Generuje watek: " + IDwatku + "\n");
        ourDatas.arrayGenerator();
        sorting.Start();
    }

    public void sortObject(dataCenter ourDatas)
    {
        Console.WriteLine("Sortuje watek: " + IDwatku + "\n");
        ourDatas.arraySorter();
        calculations.Start();
    }

    public void calculateObject(dataCenter ourDatas)
    {
        //Console.WriteLine("Start watka.");
        //Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)127;
        //ourDatas.arrayGenerator();
        //ourDatas.arraySorter();
        Console.WriteLine("Kalkuluje watek: " + IDwatku + "\n");
        ourDatas.onSqr();
        ourDatas.inSqr();
        ourDatas.difference();
        ourDatas.calculateSet();
        Console.WriteLine("Skonczylem watek: " + IDwatku + "\n");
        //Console.WriteLine("Koniec watku.");
    }

    Thread generating;
    Thread sorting;
    Thread calculations;
    int IDwatku;
}


class dataCenter
{
    public dataCenter(int wielkosc_tab, int Min, int Max)
    {
        randoms = new double[wielkosc_tab];
        inRsA = new double[wielkosc_tab];
        onRsA = new double[wielkosc_tab];
        onRsFIELD = new double[wielkosc_tab];
        inRsFIELD = new double[wielkosc_tab];
        fieldDiff = new double[wielkosc_tab];
        RandFILEDset = new double[wielkosc_tab, 2];

        iteracje = wielkosc_tab;
        MIN_NR = Min;
        MAX_NR = Max;
    }
    ~dataCenter() { }

    public void arrayGenerator()
    {
        for (int i = 0; i < iteracje; i++)
        {
            randoms[i] = rnd.Next(MIN_NR, MAX_NR);
            //Console.WriteLine(randoms[i] + "\n");
        }
        //Console.WriteLine("GENERUJE! \n");
    }

    public void arraySorter()
    {
        Array.Sort(randoms);
    }

    public void onSqr()
    {
        for (int i = 0; i < iteracje; i++)
        {
            onRsA[i] = 2.0 * randoms[i];
            onRsFIELD[i] = onRsA[i] * onRsA[i];
        }
    }

    public void inSqr()
    {
        for (int i = 0; i < iteracje; i++)
        {
            inRsA[i] = (2.0 * randoms[i]) / Math.Sqrt(2);
            inRsFIELD[i] = inRsA[i] * inRsA[i];
        }
    }

    public void difference()
    {
        for (int i = 0; i < iteracje; i++)
        {
            fieldDiff[i] = onRsFIELD[i] - inRsFIELD[i];
        }
    }

    public void calculateSet()
    {
        for (int i = 0; i < iteracje; i++)
        {
            RandFILEDset[i, 0] = randoms[i];
            RandFILEDset[i, 1] = fieldDiff[i];
            //Console.WriteLine(RandFILEDset[i, 0] + " " + RandFILEDset[i, 1] + "\n");
        }
    }

    Random rnd = new Random();
    public int MIN_NR = 0;
    public int MAX_NR = 0;
    public int iteracje = 0;
    public double[] randoms;
    public double[] inRsA;
    public double[] inRsFIELD;
    public double[] fieldDiff;
    public double[,] RandFILEDset;
    public double[] onRsA;
    public double[] onRsFIELD;

}