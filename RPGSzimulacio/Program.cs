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

            public bool Él { get => 0 < hp; }
            public Lény(string nev, int hp, int mindmg, int maxdmg)
            {
                this.nev = nev;
                this.maxhp = hp;
                this.hp = this.maxhp;
                this.mindmg = mindmg;
                this.maxdmg = maxdmg;
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
        }

        class Barbár:Lény
        {
            bool kipihent;

            public Barbár(string nev, int hp, int mindmg, int maxdmg):base(nev, hp, mindmg, maxdmg)
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
        }

        class Mágus : Lény
        {
            public Mágus(string nev, int hp, int mindmg, int maxdmg) : base(nev, hp, mindmg, maxdmg)
            {

            }

            public override string ToString() => $"kaszt: Mágus\n" + base.ToString();
            public override void Attack(Lény ellen)
            {
                ellen.hp -= g.Next(mindmg, maxdmg);
                //Console.WriteLine($"{this.nev} megütötte {ellen.nev}-t!");
            }
        }



        static void Main(string[] args)
        {
            //Lény Bence = new Lény("Bence",40,9,20);
            Barbár Gyuszó = new Barbár("Gyuszó", 60, 15, 35);
            Mágus Csege = new Mágus("Csege", 20, 30, 40);

            int csegewon = 0;
            int gyuszowon = 0;

            for (int i = 0; i < 5000; i++)
            {
                while (Gyuszó.Él && Csege.Él) // Mortal Kombat
                {
                    Lény.Randomsorrend();
                    Lény.lista[0].Attack(Lény.lista[1]);
                    //Console.WriteLine(Lény.lista[1]);
                    if (Lény.lista[1].Él)
                    {
                        Lény.lista[1].Attack(Lény.lista[0]);
                        //Console.WriteLine(Lény.lista[0]);
                    }
                }

                if (Gyuszó.Él)
                {
                    //Console.WriteLine("Gyuszó won.");
                    gyuszowon++;
                }
                else
                {
                    //Console.WriteLine("Csege won.");
                    csegewon++;
                }

                foreach (Lény lény in Lény.lista)
                {
                    lény.Feltámad();
                }
            }
            Console.WriteLine($"Csege:{csegewon}, Gyuszó: {gyuszowon}");
            Console.ReadKey();
        }
    }
}
