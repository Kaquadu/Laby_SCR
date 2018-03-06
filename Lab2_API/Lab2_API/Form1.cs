using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2_API
{
    public partial class Form1 : Form
    {
        static dataCenter ourDatas;
        static int lower = 0;
        static int higher = 0;

        static bool isRunning = false;
        bool firstRun = true;

        private static void dataCenterHandling()
        {
            while (isRunning)
            {
                ourDatas = new dataCenter(lower, higher);
                ourDatas.arrayGenerator();
                ourDatas.arraySorter();
                ourDatas.onSqr();
                ourDatas.inSqr();
                ourDatas.difference();
                ourDatas.calculateSet();
                Thread.Sleep(5000);
            }
        }

        Thread dataThread = new Thread(dataCenterHandling); 

        public Form1()
        {
            InitializeComponent();
            ourDatas = new dataCenter(lower, higher);
        }

        

        private void button1_Click(object sender, EventArgs e) //zatwierdz
        {
            ourDatas = new dataCenter(lower, higher);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            lower = Int32.Parse(textBox2.Text);
            Console.WriteLine(lower + " :lower \n");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            higher = Int32.Parse(textBox1.Text);
            Console.WriteLine(higher + " :higher \n");
        }

        private void button2_Click(object sender, EventArgs e) //start
        {
            if (firstRun)
            {
                dataThread.Start();
                firstRun = false;
            }
            if (isRunning == false)
            {
                isRunning = true;
            }

        }

        private void button3_Click(object sender, EventArgs e) //stop
        {
            if (isRunning == false)
                isRunning = true;
            else
                isRunning = false;
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            for (int i = 0; i < ourDatas.MAX_NR - ourDatas.MIN_NR; i++)
            {
                chart1.Series[0].Points.AddXY(ourDatas.RandFILEDset[i, 0], ourDatas.RandFILEDset[i, 1]);
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            }
        }
    }
}


class dataCenter
{
    public
    dataCenter(int Min, int Max)
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
        Console.WriteLine("GENERUJE! \n");
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