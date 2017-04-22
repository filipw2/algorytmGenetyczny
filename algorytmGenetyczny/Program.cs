using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorytmGenetyczny
{

    public class Rzeczywista
    {
        public Rzeczywista() { }

        public double rzeczywista(int a)
        {
            return -1.5 + (4.0 * a) / (DWA_22 - 1.5); 
        }

        int DWA_22 = 4194304;
    }

    class Osobnik
    {
        public Osobnik(Random rnd)
        {
            _przystosowanie = 0;

            for (int i = 0; i < 22; i++)
            {
                _chromosom[i] = rnd.Next(2);
                // Console.Write(_chromosom[i]);
            }

            // Console.WriteLine();
        }



        public double PRZYSTOSOWANIE
        {
            get { return Math.Abs(_przystosowanie); }
            set { _przystosowanie = value; }
        }

        public int[] CHROMOSOM
        {
            get { return _chromosom; }
        }


        int[] _chromosom = new int[22];
        double _przystosowanie;
        public double x;
        public double getX() { return x; }
    }

    public class BtoD
    {

        public int zamien(int[] tab)
        {
            int temp = 1;
            int dziesietna = 0;
            for (int i = 0; i < 22; i++)
            {
                if (tab[i] == 1) dziesietna += temp;
                temp *= 2;
            }
            return dziesietna;
        }
    }

    class Populacja
    {
        public Populacja(int wielkoscP, double pMutacji)
        {
            _wielkoscP = wielkoscP;
            _pMutacji = pMutacji;
            createP();
        }
        void createP()
        {
            _osobnik = new Osobnik[_wielkoscP];
            for (int i = 0; i < _wielkoscP; i++)
            {
                _osobnik[i] = new Osobnik(rnd);
            }
        }


        public void setOsobnik(int i, Osobnik o)
        {
            _osobnik[i] = o;
        }
        public Osobnik[] populacja
        {
            get { return _osobnik; }
        }

        int _wielkoscP;
        double _pMutacji;
        Osobnik[] _osobnik;
        Random rnd = new Random();
    }



    class AlgorytmGenetyczny
    {

        public AlgorytmGenetyczny()
        {
            r = new Random();
            o1 = new Osobnik(r);
            o2 = new Osobnik(r);
        }

        void init()
        {
            _p1 = new Populacja(_wielkoscP, _pMutacji);
            _p2 = new Populacja(_wielkoscP, _pMutacji);
        }

        public void oblicz(int wielkoscP, double pMutacji, int iteracje)
        {
            _wielkoscP = wielkoscP;
            _pMutacji = pMutacji;
            _iteracje = iteracje;

            init();
            for (int i = 0; i < _iteracje; i++)
            {
                selekcja();
                krzyzowanie();
                mutuj();
                zamien();

            }

            Console.WriteLine("x: {0}", real.rzeczywista(btod.zamien(_p1.populacja[0].CHROMOSOM)));
            Console.WriteLine(_p1.populacja[0].PRZYSTOSOWANIE);
        }

        void selekcja()
        {

            przystosowanie();
            Array.Sort(_p1.populacja, delegate (Osobnik y, Osobnik x) { return y.PRZYSTOSOWANIE.CompareTo(x.PRZYSTOSOWANIE); });
        }


        void krzyzowanie()
        {
            _i1 = r.Next(_wielkoscP);
            _i2 = r.Next(_wielkoscP);
            _pktp1 = r.Next(20) + 2;
            _pktp2 = r.Next(20) + 2;
            int tmp;

            if (_pktp1 >= _pktp2)
            {
                tmp = _pktp1;
                _pktp1 = _pktp2;
                _pktp2 = tmp;
            }

            for (int j = 0; j < _pktp1; j++)
            {
                o1.CHROMOSOM[j] = _p1.populacja[_i1].CHROMOSOM[j];
                o2.CHROMOSOM[j] = _p1.populacja[_i2].CHROMOSOM[j];
            }

            for (int j = _pktp1; j < _pktp2; j++)
            {
                o1.CHROMOSOM[j] = _p1.populacja[_i2].CHROMOSOM[j];
                o2.CHROMOSOM[j] = _p1.populacja[_i1].CHROMOSOM[j];
            }

            for (int j = _pktp2; j < 22; j++)
            {
                o1.CHROMOSOM[j] = _p1.populacja[_i1].CHROMOSOM[j];
                o2.CHROMOSOM[j] = _p1.populacja[_i2].CHROMOSOM[j];
            }

        }

        void mutuj()
        {
            if (r.Next(10) < _pMutacji * 10) o1.CHROMOSOM[r.Next(22)] = r.Next(2);
            if (r.Next(10) < _pMutacji * 10) o2.CHROMOSOM[r.Next(22)] = r.Next(2);
        }

        void zamien()
        {
            for (int i = 0; i < _wielkoscP - 2; i++)
                _p2.setOsobnik(i, _p1.populacja[i]);
            _p2.setOsobnik(_wielkoscP - 2, o1);
            _p2.setOsobnik(_wielkoscP - 1, o2);
            Populacja temp = _p1;
            _p1 = _p2;
            _p2 = temp;
        }

        void przystosowanie()
        {
            for (int i = 0; i < _wielkoscP; i++)
            {
                x = 0;
                x = real.rzeczywista(btod.zamien(_p1.populacja[i].CHROMOSOM));
                double co = Math.Cos(4.0 * x);
                double wynik = x * x + co * co * co;
                _p1.populacja[i].PRZYSTOSOWANIE = wynik;
            }
        }

        double x;
        Random r;
        Rzeczywista real = new Rzeczywista();
        BtoD btod = new BtoD();
        int _iteracje, _i1, _i2, _pktp1, _pktp2;
        Populacja _p1, _p2;
        Osobnik o1;
        Osobnik o2;
        int _wielkoscP;
        double _pMutacji;
    }


    class Program
    {
        static void Main(string[] args)
        {

            // AlgorytmGenetyczny a = new AlgorytmGenetyczny();
            //  a.oblicz(1024, 0.4, 1000);

            Console.WriteLine("Wyszukiwanie miejsca zerowego funkcji za pomocą algorytmu genetycznego");
            Console.WriteLine("funkcja: x^2+cos(4*x)^3\n");

            AlgorytmGenetycznyBuilder a = new AlgorytmGenetycznyBuilder();
            AlgorytmGenetycznyB a2 = a.dwuEli();
            a2.oblicz(1024, 0.4, 2000);
            a2.oblicz(2048, 0.4, 1000);
            a2.oblicz(1024, 0.4, 1000);
            a2.oblicz(1024, 0.4, 1000);
            // a.oblicz(2048,0.2,500);
            // a.oblicz(2048, 0.2, 3000);
            Console.ReadKey();
        }
    }
}
