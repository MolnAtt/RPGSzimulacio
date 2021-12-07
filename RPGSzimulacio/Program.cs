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
            public int hp;
            protected int mindmg;
            protected int maxdmg;
            protected static Random g = new Random();

            public Lény(string nev, int hp, int mindmg, int maxdmg)
            {
                this.nev = nev;
                this.hp = hp;
                this.mindmg = mindmg;
                this.maxdmg = maxdmg;
            }

            public override string ToString() => $" név: {nev}\n hp:{hp}\n sebzés:{mindmg}-{maxdmg}\n";
            public virtual void Attack(Lény Ellen)
            { 

            }
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
                    Console.WriteLine($"{this.nev} megütötte {ellen.nev}-t! Most viszont elfáradt");
                }
                else
                {
                    Console.WriteLine($"{this.nev} megütné {ellen.nev}-t, de fáradt, úgyhogy inkább pihen.");
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
                Console.WriteLine($"{this.nev} megütötte {ellen.nev}-t!");
            }
        }



        static void Main(string[] args)
        {
            Lény Bence = new Lény("Bence",40,9,20);
            Barbár Gyuszó = new Barbár("Gyuszó", 60, 15, 35);
            Mágus Csege = new Mágus("Csege", 20, 30, 40);

            
            Console.WriteLine(Gyuszó);
            Console.WriteLine(Csege);

            Gyuszó.Attack(Csege);

            Console.WriteLine(Gyuszó);
            Console.WriteLine(Csege);

            Csege.Attack(Gyuszó);

            Console.WriteLine(Gyuszó);
            Console.WriteLine(Csege);

            Gyuszó.Attack(Csege);

            Console.WriteLine(Gyuszó);
            Console.WriteLine(Csege);

            Csege.Attack(Gyuszó);

            Console.WriteLine(Gyuszó);
            Console.WriteLine(Csege);


            Console.ReadKey();
        }
    }
}
