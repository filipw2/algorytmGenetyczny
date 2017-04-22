using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorytmGenetyczny
{
    class AlgorytmGenetycznyBuilder
    {
        protected AlgorytmGenetycznyB algorytmGenetyczny;

        public AlgorytmGenetycznyB AlgorytmGenetyczny { get { return algorytmGenetyczny; } }

        public void createNewAlgorytmGenetyczny()
        {
            algorytmGenetyczny = new AlgorytmGenetycznyB();
        }

        public AlgorytmGenetycznyB dwuEli()
        {
            createNewAlgorytmGenetyczny();
            algorytmGenetyczny.KRZYZOWANIE = new DwuPunktowe();
            algorytmGenetyczny.SELEKCJA = new Elitaryzm();
            return algorytmGenetyczny;
        }

        //public abstract void BuildPopulacje();
        // public abstract void BuildSelekcje();
        // public abstract void BuildKrzyzowanie();
        // public abstract void BuildMutowanie();
    }

    abstract class Krzyzowanie
    {
        public abstract void krzyzuj(Populacja p1);
    }

    class DwuPunktowe : Krzyzowanie {

        
        public override void  krzyzuj(Populacja p1)
        {
            Random r = new Random();

            int _wielkoscP = p1.populacja.Length;
            int _i1, _i2;

            Osobnik o1 = new Osobnik(r);
            Osobnik o2 = new Osobnik(r);

            _i1 = r.Next(_wielkoscP);
            _i2 = r.Next(_wielkoscP);
            int _pktp1 = r.Next(20) + 2;
            int _pktp2 = r.Next(20) + 2;
            int tmp;

            if (_pktp1 >= _pktp2)
            {
                tmp = _pktp1;
                _pktp1 = _pktp2;
                _pktp2 = tmp;
            }

            for (int j = 0; j < _pktp1; j++)
            {
                o1.CHROMOSOM[j] = p1.populacja[_i1].CHROMOSOM[j];
                o2.CHROMOSOM[j] = p1.populacja[_i2].CHROMOSOM[j];
            }

            for (int j = _pktp1; j < _pktp2; j++)
            {
                o1.CHROMOSOM[j] = p1.populacja[_i2].CHROMOSOM[j];
                o2.CHROMOSOM[j] = p1.populacja[_i1].CHROMOSOM[j];
            }

            for (int j = _pktp2; j < 22; j++)
            {
                o1.CHROMOSOM[j] = p1.populacja[_i1].CHROMOSOM[j];
                o2.CHROMOSOM[j] = p1.populacja[_i2].CHROMOSOM[j];
            }
        }
    }


    abstract class Selekcja
    {
        public abstract void selekcja(Populacja p1);
    }

    class Elitaryzm : Selekcja{

        public override void selekcja(Populacja p1)
        {
            int _wielkoscP = p1.populacja.Length;
            double x;
            for (int i = 0; i < _wielkoscP; i++)
            {
                x = real.rzeczywista(btod.zamien(p1.populacja[i].CHROMOSOM));
                double co = Math.Cos(4.0 * x);
                double wynik = x * x + co * co * co;
                p1.populacja[i].PRZYSTOSOWANIE = wynik;
            }

            Array.Sort(p1.populacja, delegate(Osobnik y, Osobnik z) { return y.PRZYSTOSOWANIE.CompareTo(z.PRZYSTOSOWANIE); });

        }
        Rzeczywista real = new Rzeczywista();
        BtoD btod = new BtoD();
    }

    //class DwaElitaryzm : AlgorytmGenetycznyBuilder
    //{
    //    public override void BuildSelekcje()
    //    {
    //        algorytmGenetyczny.SELEKCJA = new Elitaryzm();
    //    }
    //    public override void BuildKrzyzowanie()
    //    {
    //        algorytmGenetyczny.KRZYZOWANIE = new DwuPunktowe();
    //    }
    //}

    class AlgorytmGenetycznyB
    {
        Selekcja selekcja;
        Krzyzowanie krzyzowanie;

        public Selekcja SELEKCJA { set { selekcja = value; } }
        public Krzyzowanie KRZYZOWANIE { set { krzyzowanie = value; } }

        public AlgorytmGenetycznyB()
        {
        r = new Random();
        o1 = new Osobnik(r);
        o2 = new Osobnik(r);
        }
       
        void init()
        {
            _p1 = new Populacja(_wielkoscP,_pMutacji);
            _p2 = new Populacja(_wielkoscP, _pMutacji);
        }

        public void oblicz(int wielkoscP, double pMutacji,int iteracje)
        {
            _wielkoscP = wielkoscP;
            _pMutacji = pMutacji;
            _iteracje = iteracje;

            init();
            for (int i = 0; i < _iteracje; i++)
            {
                selekcja.selekcja(_p1);
                krzyzowanie.krzyzuj(_p1);
                mutuj();
                zamien();
            
            }

            Console.WriteLine("x: {0}",real.rzeczywista(btod.zamien(_p1.populacja[0].CHROMOSOM)));
            Console.WriteLine("przystosowanie: {0}",_p1.populacja[0].PRZYSTOSOWANIE);
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

}
