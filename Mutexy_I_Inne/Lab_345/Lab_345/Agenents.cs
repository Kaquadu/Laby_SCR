using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_345
{
    class ConstantCountingAgent : Agent
    {
        public ConstantCountingAgent(int Identify) : base(Identify)
        { }

        override public void Update()
        {
            CCA_i++;
            if (CCA_i == 10)
            {
                Console.WriteLine("I counted to 10 and my Id is {0}.", this.ID);
                HasFinished = true;
            }
        }

        private int CCA_i = 0;
    }

    class CountingAgent : Agent
    {
        public CountingAgent(int Identify) : base(Identify)
        { }

        override public void Update()
        {
            CA_i++;
            if (CA_i >= ID)
            {
                Console.WriteLine("I counted to my ID and my Id is {0}.", this.ID);
                HasFinished = true;
            }
        }

        private int CA_i = 0;
    }

    class SineGeneratingAgent : Agent
    {
        public SineGeneratingAgent(int Identify) : base(Identify)
        { }

        public override void Update()
        {
            SGA_i++;
            Output = (float)Math.Sin(this.virtualTimeS);
            if (this.virtualTimeS >= this.ID % 10)
            {              
                Console.WriteLine("I made a sine and my Id is {0}.", this.ID);
                HasFinished = true;
            }
            //Console.WriteLine("I wanna do a sinus!");
        }

        public float Output;
        private int SGA_i = 0;
    }

    class ListCountingAgent : Agent
    { 
    
    public ListCountingAgent(int id, int begg, int endd, ref List<int> numbers, int rang_e) : base(id)
    {
        this.numbers = numbers;
        this.begg = begg;
        this.endd = endd;
        this.rang_e = rang_e;
    }

    public override void Update()
    {
        int size = (this.endd - this.begg);
        int range = size / rang_e;

        for (int j = 0; j < rang_e; j++)
        {
            int begg2 = this.begg + (j * range);
            int endd2 = this.begg + ((j + 1) * range);

            if (j == rang_e - 1)
            {
                endd2 = this.begg + size;
            }

            for (int i = begg2; i < endd2; i++)
            {
                this.Suma = this.Suma + this.numbers.ElementAt(i);
            }
                Thread.Sleep(100);
        }

        Console.WriteLine("ID watku: " + ID + ", Suma: " + this.Suma);
        HasFinished = true;
    }
        private List<int> numbers;
        private int begg;
        private int endd;
        private int rang_e;
    }

    class ListsSummingAgent : Agent
    {
        public ListsSummingAgent(int id, List<IRunnable> ags) : base(id)
        {
            this.agents = ags;
        }

        public override void Update()
        {
            //Console.WriteLine("Tu sie wyrzucilem? {0}", ID);
            bool allFinished = false;
            while (!allFinished)
            {
                Console.WriteLine("Tu sie wyrzucilem? {0}", ID);
                int i = 0;
                foreach (Agent ag in agents)
                {
                    if (i < 10)
                    {
                        allFinished = true;
                        if (!ag.HasFinished)
                        {
                            allFinished = false;
                            break;
                        }
                    }
                    if (i >= 10)
                        i = 0;
                    i++;
                }
            }


            foreach (Agent ag in agents)
            {
                sumasum += ag.Suma;
            }
            Console.WriteLine("ID watku: " + ID + ", suma sum: " + this.sumasum);
            HasFinished = true;
        }

        public List<IRunnable> agents = new List<IRunnable>();
        public int sumasum = 0;
    }
    
    class TextDividingAgent : Agent
    {
        public TextDividingAgent(int id, string text, int nSize) : base(id)
        {
            this.text = text;
            this.nSize = nSize;
        }

        public override void Update()
        {
            //tworzenie listy slow
            text = text.StripPunctuation();
            array = text.Split(' ');
            listOfWords = array.ToList();

            //foreach (var w in listOfWords)
            //    Console.WriteLine(w);

            //dzielenie na listy slow dla osobnych watkow
            while (listOfWords.Any())
            {
                listOfLists.Add(listOfWords.Take(nSize).ToList());
                listOfWords = listOfWords.Skip(nSize).ToList();
            }

            HasFinished = true;
        }

        public List<List<string>> listOfLists = new List<List<string>>();
        public List<string> listOfWords = new List<string>();
        public string text;
        public string[] array;
        public int nSize;
    }

    class TextSummingAgent : Agent
    {
        public TextSummingAgent(int id, TextDividingAgent TDAgent, int nSize) : base(id)
        {
            this.TDAgent = TDAgent;
            this.nSize = nSize;
        }

        public override void Update()
        {
            if (TDAgent.HasFinished == true)
            {
                agList = TDAgent.listOfLists[this.ID];

                g = agList.GroupBy(i => i);

                foreach (var grp in g)
                {
                    //Console.WriteLine("{0} {1}", grp.Key, grp.Count());
                }

                //Console.WriteLine("\n\n NEXT AGENT \n\n");
                HasFinished = true;
                //Console.WriteLine("Koniec agenta o ID: " + this.ID);
            }

        }

        public IEnumerable<IGrouping<string, string>> g;
        public TextDividingAgent TDAgent;
        public List<string> agList = new List<string>();
        public int nSize;
    }

    class TextJoiningAgent : Agent
    {
        public TextJoiningAgent(int id, List<TextSummingAgent> ags) : base(id)
        {
            agents = ags;
        }

        public override void Update()
        {
            bool allFinished = false;
            while (!allFinished)
            {
                foreach (Agent ag in agents)
                {
                    allFinished = true;
                    if (!ag.HasFinished)
                    {
                        allFinished = false;
                        break;
                    }
                }
            }

            foreach(var ag in agents)
            {
                wordList.AddRange(ag.agList);
            }

            g = wordList.GroupBy(i => i);

            foreach (var grp in g)
            {
                Console.WriteLine("{0} {1}", grp.Key, grp.Count());
            }

            HasFinished = true;
        }

        public List<string> wordList = new List<string>();
        public List<TextSummingAgent> agents = new List<TextSummingAgent>();
        public IEnumerable<IGrouping<string, string>> g;
    }
}


public static class StringExtension
{
    public static string StripPunctuation(this string s)
    {
        var sb = new StringBuilder();
        foreach (char c in s)
        {
            if (!char.IsPunctuation(c))
                sb.Append(c);
        }
        return sb.ToString();
    }
}