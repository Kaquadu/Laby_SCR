using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

class newClass
{
    public newClass(int ile, int zakres1, int zakres2)
    {
        iloscLiczb = ile;
        times = new double[3];
        ticks = new double[3];
        zakres = new int[2];
        sw = new Stopwatch();
        tablica = new int[iloscLiczb];

        zakres[0] = zakres1;
        zakres[1] = zakres2;
    }
    ~newClass()
    {

    }

    public void Generator()
    {
        sw.Start();
        for (int i = 0; i < iloscLiczb; i++)
            tablica[i] = rnd.Next(zakres[0], zakres[1]);
        sw.Stop();
        ticks[0] = sw.ElapsedTicks;
        times[0] = ticks[0] / Stopwatch.Frequency;
        sw.Reset();
    }

    public void Sortownik()
    {
        sw.Start();
        Array.Sort(tablica);
        sw.Stop();
        ticks[1] = sw.ElapsedTicks;
        times[1] = ticks[1] / Stopwatch.Frequency;
        sw.Reset();
    }

    public void Wypisywacz()
    {
        sw.Start();
        for (int i = 0; i < iloscLiczb; i++)
            Console.Write(tablica[i] + " \n");
        sw.Stop();
        ticks[2] = sw.ElapsedTicks;
        times[2] = ticks[2] / Stopwatch.Frequency;
        sw.Reset();
    }

    public double[] times;
    public double[] ticks;
    public int iloscLiczb;
    public int[] zakres;

    private Stopwatch sw;

    private Random rnd = new Random();

    private int[] tablica;
}

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            int ILOSC_OBJ;

            Console.Write("Podaj ilosc objektow: ");
            ILOSC_OBJ = Int32.Parse(Console.ReadLine());

            Stopwatch sw = new Stopwatch();

            newClass[] myObjs = new newClass[ILOSC_OBJ];

            int[ , ] zakresy = new int[ILOSC_OBJ, 2];
            double[] tickiCalosciowe = new double[ILOSC_OBJ];
            double[] czasyCalosciowe = new double[ILOSC_OBJ];

            for (int i = 0; i < ILOSC_OBJ; i++)
            {
                Console.Write("Podaj zakres dla " + i + " obiektu:");
                zakresy[i, 0] = Int32.Parse(Console.ReadLine());
                zakresy[i, 1] = Int32.Parse(Console.ReadLine());
                myObjs[i] = new newClass(1000, zakresy[i, 0], zakresy[i, 1]);
            }

            for (int i = 0; i < ILOSC_OBJ; i++)
            {
                sw.Start();
                myObjs[i].Generator();
                myObjs[i].Sortownik();
                myObjs[i].Wypisywacz();
                sw.Stop();
                sw.Reset();
                tickiCalosciowe[i] = sw.ElapsedTicks;
                czasyCalosciowe[i] = tickiCalosciowe[i] / Stopwatch.Frequency;
            }

                Console.Clear();

            for (int i = 0; i < ILOSC_OBJ; i++)
            {
                Console.Write("Czas calosciowy " + i + "'tego obiektu: " + czasyCalosciowe[i] + "\n");
                Console.Write("Generator: " + myObjs[i].times[0] + "\n");
                Console.Write("Sortownik: " + myObjs[i].times[1] + "\n");
                Console.Write("Wypisywacz: " + myObjs[i].times[2] + "\n");
                Console.Write("Zakres obiektu: " + zakresy[i, 0] + ", " + zakresy[i, 1] + ".\n\n");
            }

            Console.ReadLine();
        }
    }
}