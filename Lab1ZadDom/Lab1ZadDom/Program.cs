using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


class dataCenter
{
    public
    dataCenter(int Min, int Max)
    {
        randoms = new double[Max-Min];
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
        for (int i = 0; i < MAX_NR-MIN_NR; i++)
        {
            randoms[i] = rnd.Next(MIN_NR, MAX_NR);
        }
    }

    public void arraySorter()
    {
        Array.Sort(randoms);
    }

    public void onSqr()
    {
        for (int i = 0; i < MAX_NR-MIN_NR; i++)
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
            //Console.Write(inRsA[i] + "\n");
            //Console.ReadLine();
            inRsFIELD[i] = inRsA[i] * inRsA[i];
            //Console.Write(inRsFIELD[i] + "\n");
        }
    }

    public void difference()
    {
        for (int i = 0; i < MAX_NR - MIN_NR; i++)
        {
            fieldDiff[i] = onRsFIELD[i] - inRsFIELD[i];
            //Console.Write(fieldDiff[i]);
            //Console.ReadLine();
        }
    }

    public void calculateSet()
    {
        for (int i = 0; i < MAX_NR - MIN_NR; i++)
        {
            RandFILEDset[i, 0] = randoms[i];
            RandFILEDset[i, 1] = fieldDiff[i];
            Console.Write(" # {0:0.0000} # ", RandFILEDset[i, 0]);
            Console.Write( "{0:0.0000} # ", RandFILEDset[i, 1]);
            if (i % 4 == 0)
            {
                Console.Write("\n");
            }
        }
        Console.Clear();
    }

    Random rnd = new Random();
    public int MIN_NR;
    public int MAX_NR;
    public double[] randoms;
    public double[] inRsA;
    public double[] inRsFIELD;
    public double[] fieldDiff;
    public double[,] RandFILEDset;
    public double[] onRsA;
    public double[] onRsFIELD;

}




namespace Lab1ZadDom
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            int zak1 = 5000;
            int zak2 = 35000;

            /*Console.Write("Podaj zakres przedzialu: ");
            try
            {
                zak1 = Int32.Parse(Console.ReadLine());
                zak2 = Int32.Parse(Console.ReadLine());
            }
            catch (System.FormatException)
            {
                Console.Write("Bledny przedzial!");
            }*/

            double[] times = new double[7];
            double[] ticks = new double[7];

            if (zak1 <= zak2)
            {
                sw.Start();

                dataCenter ourCenter = new dataCenter(zak1, zak2);

                sw.Stop();
                ticks[0] = sw.ElapsedTicks;
                times[0] = ticks[0] / Stopwatch.Frequency;
                sw.Start();

                ourCenter.arrayGenerator();

                sw.Stop();
                ticks[1] = sw.ElapsedTicks;
                times[1] = ticks[1] / Stopwatch.Frequency;
                sw.Start();

                ourCenter.arraySorter();

                sw.Stop();
                ticks[2] = sw.ElapsedTicks;
                times[2] = ticks[2] / Stopwatch.Frequency;
                sw.Start();

                ourCenter.onSqr();

                sw.Stop();
                ticks[3] = sw.ElapsedTicks;
                times[3] = ticks[3] / Stopwatch.Frequency;
                sw.Start();

                ourCenter.inSqr();

                sw.Stop();
                ticks[4] = sw.ElapsedTicks;
                times[4] = ticks[4] / Stopwatch.Frequency;
                sw.Start();

                ourCenter.difference();

                sw.Stop();
                ticks[5] = sw.ElapsedTicks;
                times[5] = ticks[5] / Stopwatch.Frequency;
                sw.Start();

                ourCenter.calculateSet();

                sw.Stop();
                ticks[6] = sw.ElapsedTicks;
                times[6] = ticks[6] / Stopwatch.Frequency;

                Console.Write("Czas utworzenia obiektu: " + times[0] + " [s], tykniec: " + ticks[0] + ", czestotliwosc: " + Stopwatch.Frequency + " [Hz].\n");
                Console.Write("Czas losowania: " + times[1] + " [s], tykniec: " + ticks[1] + ", czestotliwosc: " + Stopwatch.Frequency + " [Hz].\n");
                Console.Write("Czas sortowania: " + times[2] + " [s], tykniec: " + ticks[2] + ", czestotliwosc: " + Stopwatch.Frequency + " [Hz].\n");
                Console.Write("Czas liczenia na kole: " + times[3] + " [s], tykniec: " + ticks[3] + ", czestotliwosc: " + Stopwatch.Frequency + " [Hz].\n");
                Console.Write("Czas liczenia w kole: " + times[4] + " [s], tykniec: " + ticks[4] + ", czestotliwosc: " + Stopwatch.Frequency + " [Hz].\n");
                Console.Write("Czas liczenia roznicy: " + times[5] + " [s], tykniec: " + ticks[5] + ", czestotliwosc: " + Stopwatch.Frequency + " [Hz].\n");
                Console.Write("Czas tworzenia zbioru: " + times[6] + " [s], tykniec: " + ticks[6] + ", czestotliwosc: " + Stopwatch.Frequency + " [Hz].\n");
                sw.Reset();
            }


            Console.ReadLine();
        }
    }
}
