using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSzimulacio
{
    class Program
    {

        class Lény
        {
            public string nev;
            private int maxhp;
            public int hp;
            protected int mindmg;
            protected int maxdmg;
            protected static Random g = new Random();
            public static List<Lény> lista = new List<Lény>();
            public static List<Lény> Játékosok = new List<Lény>();
            public static List<Lény> Ellen = new List<Lény>();


            public bool Él { get => 0 < hp; }
            public Lény(string nev, int hp, int mindmg, int maxdmg, string csapat)
            {
                this.nev = nev;
                this.maxhp = hp;
                this.hp = this.maxhp;
                this.mindmg = mindmg;
                this.maxdmg = maxdmg;
                if (csapat=="NPC")
                    Lény.Ellen.Add(this);
                else if (csapat == "PC")
                    Lény.Játékosok.Add(this);
                Lény.lista.Add(this);
            }

            public override string ToString() => $" név: {nev}\n hp:{hp}\n sebzés:{mindmg}-{maxdmg}\n";
            public virtual void Attack(Lény Ellen)
            { 

            }




            public static void Randomsorrend() // Fisher-Yates algoritmus?
            {
                for (int i = 0; i < Lény.lista.Count-1; i++)
                {
                    int r = g.Next(i + 1, lista.Count);
                    (lista[i], lista[r]) = (lista[r], lista[i]);
                }
            }

            public void Feltámad() => hp = maxhp;

            public virtual Lény RandomEllenfél()
            {
                return null;
            }
        }

        class Barbár:Lény
        {
            bool kipihent;

            public Barbár(string nev, int hp, int mindmg, int maxdmg, string csapat) : base(nev, hp, mindmg, maxdmg, csapat)
            {
                kipihent = true;
            }

            public override string ToString() => $"kaszt: Barbár\n" + base.ToString();
            public override void Attack(Lény ellen)
            {
                if (kipihent)
                {
                    ellen.hp -= g.Next(mindmg, maxdmg);
                    kipihent = false;
                   // Console.WriteLine($"{this.nev} megütötte {ellen.nev}-t! Most viszont elfáradt");
                }
                else
                {
                   // Console.WriteLine($"{this.nev} megütné {ellen.nev}-t, de fáradt, úgyhogy inkább pihen.");
                    kipihent = true;
                }
            }

            public virtual Lény RandomEllenfél() 
            {
                List<Lény> másikcsapat = Játékosok.Contains(this)?Ellen:Játékosok;

            }
        }

        class Mágus : Lény
        {
            public Mágus(string nev, int hp, int mindmg, int maxdmg, string csapat) : base(nev, hp, mindmg, maxdmg, csapat)
            {

            }

            public override string ToString() => $"kaszt: Mágus\n" + base.ToString();
            public override void Attack(Lény ellen)
            {
                ellen.hp -= g.Next(mindmg, maxdmg);
                //Console.WriteLine($"{this.nev} megütötte {ellen.nev}-t!");
            }
        }

        class Healer : Lény
        {
            public Healer(string nev, int hp, int mindmg, int maxdmg, string csapat) : base(nev, hp, mindmg, maxdmg, csapat)
            {

            }
            public override string ToString() => $"kaszt: Kuruzsló\n" + base.ToString();

            public override void Attack(Lény ellen) // Így gyógyít
            {
                ellen.hp += g.Next(mindmg, maxdmg);
            }

        }

        class BBB : Lény
        {
            public BBB(string nev, int hp, int mindmg, int maxdmg, string csapat) : base(nev, hp, mindmg, maxdmg, csapat)
            {

            }
            public override string ToString() => $"kaszt: Big Bad Boss\n" + base.ToString();

            public override void Attack(Lény ellen) // Így gyógyít
            {
                ellen.hp -= g.Next(mindmg, maxdmg);
            }
        }

        static void Main(string[] args)
        {
            //Lény Bence = new Lény("Bence",40,9,20);
            Barbár Gyuszó = new Barbár("Gyuszó", 60, 15, 35, "PC");
            Mágus Csege = new Mágus("Csege", 25, 30, 40, "PC");
            Healer Farkas = new Healer("Farkas", 30, 10, 20, "PC"); // ez ennyit gyógyít
            BBB Sziszi = new BBB("Sziszi", 500, 15, 30, "NPC"); // ez ennyit gyógyít

            int npcwon = 0;
            int pcwon = 0;

            for (int db = 0; db < 5000; db++)
            {
                Lény.Randomsorrend();
                foreach (Lény lény in Lény.lista)
                {
                    if (lény.Él)
                        lény.Attack(lény.RandomEllenfél());
                }
            }

            Console.WriteLine($"Játékosok:{pcwon}, Sziszi: {npcwon}, tehát az arány: {(double)pcwon/5000}");
            Console.ReadKey();
        }
    }
}
