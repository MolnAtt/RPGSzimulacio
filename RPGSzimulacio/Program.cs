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

            public static Lény Pick(IEnumerable<Lény> lista) => lista.ElementAt(g.Next(lista.Count()));


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

            public List<Lény> Másikcsapat() => Játékosok.Contains(this) ? Ellen : Játékosok;
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

            public override Lény RandomEllenfél() => Lény.Pick(Másikcsapat().Where(x => x.Él));
            
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
            public override Lény RandomEllenfél() => Lény.Pick(Másikcsapat().Where(x => x.Él));

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
            public override Lény RandomEllenfél()
            {
                List<Lény> ezacsapat = Játékosok.Contains(this) ?  Játékosok: Ellen;
                return Lény.Pick(ezacsapat.Where(x => x.Él));
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
            public override Lény RandomEllenfél() => Lény.Pick(Másikcsapat().Where(x => x.Él));

        }

        static void Main(string[] args)
        {
            //Lény Bence = new Lény("Bence",40,9,20);
            Barbár Matyi = new Barbár("Matyi", 70, 30, 45, "PC");
            Barbár Levente = new Barbár("Levente", 70, 35, 40, "PC");
            Barbár Kókusz = new Barbár("Kókusz", 100, 35, 45, "PC");
            Barbár Gyuszó = new Barbár("Gyuszó", 60, 15, 35, "PC");
            Barbár Zsolt = new Barbár("Zsolt", 40, 25, 40, "PC");
            Mágus Csege = new Mágus("Csege", 25, 30, 40, "PC");
            Mágus Máté = new Mágus("Máté", 25, 30, 40, "PC");
            Healer Balu = new Healer("Balu", 35, 10, 20, "PC"); // ez ennyit gyógyít
            Healer Farkas = new Healer("Farkas", 35, 10, 20, "PC"); // ez ennyit gyógyít
            BBB Minotaurusz = new BBB("Minotaurusz", 200, 20, 50, "NPC");
            BBB Sziszi = new BBB("Sziszi", 200, 40, 40, "NPC");
            BBB Vince = new BBB("Vince", 200, 25, 40, "NPC");

            /** /
            for (int i = 0; i < 50; i++)
                new BBB("goblin", 15, 0, 2, "NPC");
            /**/
            int npcwon = 0;
            int pcwon = 0;

            int N = 30;

            for (int db = 0; db < N; db++)
            {
                while (Lény.Játékosok.Any(x=> x.Él) && Lény.Ellen.Any(x => x.Él))
                {
                    Lény.Randomsorrend();
                    foreach (Lény lény in Lény.lista)
                    {
                        if (lény.Él && lény.Másikcsapat().Any(x=>x.Él))
                        {
                            Lény ellenfél = lény.RandomEllenfél();
                            lény.Attack(ellenfél);
                            if (!ellenfél.Él)
                            {
                                //Console.WriteLine($"{ellenfél.nev} elesett.");
                            }
                        }
                    }
                }
                if (Lény.Játékosok.Any(x => x.Él))
                {
                    pcwon++;
                }
                else
                {
                    npcwon++;
                }
                foreach (Lény lény in Lény.lista)
                {
                    lény.Feltámad();
                }
            }

            Console.WriteLine($"Játékosok:{pcwon}, Ellen: {npcwon}, tehát az arány: {(double)pcwon/N}");
            Console.ReadKey();
        }
    }
}
