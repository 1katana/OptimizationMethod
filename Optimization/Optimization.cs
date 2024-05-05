using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace OptimizationMethod
{
    delegate double FunVector(Vector vector);
    public delegate double Fun(double x);

    public enum TypeExtremum
    {
        Minimum,
        Maximum,
        None
    }

    class Optimization
    {



        public static double StepByStep(double xn, double h, double eps, Fun F)
        {
            double k = 0;

            double xt = xn;
            double xs = xt + h;

            double ft = F(xt);
            double fs = F(xs);

            while (Math.Abs(h) > eps)
            {
                k++;

                xs += h;
                fs = F(xs);
                if (fs > ft)
                {
                    h = -(h / 2);


                }
                else
                {
                    h = h * 1.2;
                }
                xt = xs;
                ft = fs;

            }
            return xt;
        }

        public static double GoldenSearch(double a, double b, double eps, Func<double, double> F)
        {
            double v = a + 0.382 * (b - a);
            double w = a + 0.618 * (b - a);
            double fv = F(v);
            double fw = F(w);

            while ((b - a) > eps)
            {
                if (fv < fw)
                {
                    b = w; w = v; fw = fv;
                    v = a + 0.382 * (b - a);
                    fv = F(v);

                }
                else
                {
                    a = v; v = w; fv = fw;
                    w = a + 0.618 * (b - a);
                    fw = F(w);
                }
            }
            return (a + b) / 2;

        }

        public static double Aprox(double x1, double x2, double x3, double eps, Func<double, double> F,ref TypeExtremum type)
        {
            double a, b, c, xOptimal, fOptimal;
            double[] x = { x1, x2, x3 };
            double[] f = { F(x1), F(x2), F(x3) };

            Array.Sort(x, f);

            do
            {
                a = f[0];
                b = (f[1] - f[0]) / (x[1] - x[0]);
                c = (1 / (x[2] - x[1])) * (((f[2] - f[0]) / (x[2] - x[0])) - ((f[1] - f[0]) / (x[1] - x[0])));

                //Console.WriteLine($"a= {a} b= {b} c= {c}");
                xOptimal = ((x[1] + x[0]) / 2) - (b / (2 * c));

                fOptimal = F(xOptimal);
                //Console.WriteLine($"xO= {xOptimal} fO={fOptimal}");

                if (Math.Abs(x[1] - xOptimal) < eps) break;

                if (xOptimal > x[1] && xOptimal < x[2])
                {
                    x[0] = x[1];
                    x[1] = xOptimal;
                    f[0] = f[1];
                    f[1] = fOptimal;

                }

                else if (xOptimal < x[1] && xOptimal > x[0])
                {
                    x[2] = x[1];
                    f[2] = f[1];
                    f[1] = fOptimal;
                    x[1] = xOptimal;

                }
                

                //Console.WriteLine($"{x[0]}-{f[0]} {x[1]}-{f[1]} {x[2]}-{f[2]}");
                //Console.WriteLine();
                //Console.ReadKey();
            } while (true);

            if (c > 0) type = TypeExtremum.Minimum;
            if (c < 0) type = TypeExtremum.Maximum;

            return x[1];
        }

        public static double PolDel(double a, double b, double eps, Func<double, double> f)
        {
            double c, v, w, fv, fw;
            do
            {
                c = (a + b) / 2;
                v = c - eps / 2;
                w = c + eps / 2;
                fv = f(v);
                fw = f(w);
                if (fv > fw)
                {
                    a = w;
                }
                else
                {
                    b = v;
                }




            } while (Math.Abs((b - a)) > eps);

            return ((a + b) / 2);
        }


        public static Vector gradEval(Vector xn, double delta, FunVector F)
        {
            Vector xt;

            Vector grad = new Vector(xn.Size);

            double fpr;

            int n = xn.Size;

            double ft = F(xn);

            for (int i = 0; i < n; i++)
            {
                xt = xn.Copy();
                xt[i] += delta;
                fpr = F(xt);

                grad[i] = (fpr - ft) / delta;

            }

            return grad;
        }

        public static OptimumResult stepGrad(Vector xn, double h, double eps, FunVector F)
        {


            Vector xt = xn;

            double ft = F(xt);

            Vector xs;
            double fs;

            double delta = eps / 2;

            Vector s = new Vector(xn.Size);

            int k = 0;

            while (Math.Abs(h) > eps)
            {
                s = gradEval(xt, eps / 2, F);

                xs = xt - h * s;
                fs = F(xs);
                h = fs < ft ? h * 1.2 : h / 2;
                ft = fs; xt = xs.Copy();
                //Console.WriteLine($"{ft} {xt} {h}");



                k++;
            }
            OptimumResult optimumResult = new OptimumResult();

            optimumResult.fopt = new Vector(new double[] { ft });
            optimumResult.ogr = null;
            optimumResult.xopt = xt;
            optimumResult.CountIterations = k;



            return optimumResult;

        }

        public static OptimumResult randomSearch(Vector xn, double h, double eps, FunVector F)
        {
            Vector xt = xn.Copy();
            double ft = 0;

            int n = xn.Size;

            int m = n * 3;

            Vector xjp = new Vector(n);
            double fjp;

            Vector xjmin = new Vector(n);


            int k = 0;

            while (Math.Abs(h) > eps)
            {
                k++;

                double fjmin = double.MaxValue;

                for (int i = 0; i < m; i++)
                {
                    xjp = xt + h * Vector.NormalizeRandom(n);

                    //Console.WriteLine(xjp);

                    fjp = F(xjp);

                    if (fjp < fjmin)
                    {
                        fjmin = fjp;
                        xjmin = xjp;
                    }

                }
                //Console.ReadLine();
                ft = F(xt);

                if (fjmin < ft)
                {
                    xt = xjmin.Copy();
                    h = h * 1.2;

                }
                else
                {
                    h = h / 2;
                }


            }

            OptimumResult optimumResult = new OptimumResult();

            optimumResult.fopt = new Vector(new double[] { ft });
            optimumResult.ogr = null;
            optimumResult.xopt = xt;
            optimumResult.CountIterations = k;
            //Console.WriteLine(k);
            return optimumResult;
        }

        public static OptimumResult fastestDescent(Vector xn, double h, double eps, FunVector F)
        {


            Vector s = new Vector(xn.Size);

            Vector xt;

            double ft;

            Vector xs = xn;
            double fs = F(xs);

            int k = 0;

            do
            {
                ft = fs; xt = xs.Copy();

                s = gradEval(xt, eps / 2, F);

                h = Optimization.StepByStep(0, h, eps, h => F(xt - h * s));

                xs = xt - h * s;

                fs = F(xs);

                k++;
            } while (Math.Abs((xs - xt).Norma1()) > eps);

            OptimumResult optimumResult = new OptimumResult();

            optimumResult.fopt = new Vector(new double[] { ft });
            optimumResult.ogr = null;
            optimumResult.xopt = xt;
            optimumResult.CountIterations = k;



            return optimumResult;

        }

        public static OptimumResult conjugateDirections(Vector xn, double h, double eps, FunVector F)
        {


            double y;

            Vector grad = new Vector(xn.Size);
            Vector grad_new = new Vector(xn.Size);

            Vector d = new Vector(xn.Size);

            Vector xt = xn.Copy();
            double ft = F(xt);

            Vector xs;
            double fs;

            int k = 0;

            grad = gradEval(xt, eps / 2, F);
            d = -1 * grad;

            while (true)
            {

                h = Optimization.StepByStep(0, h, eps, h => F(xt + h * d));
                xs = xt + h * d;

                grad_new = gradEval(xs, eps / 2, F);

                y = (grad_new * grad_new) / (grad * grad);

                d = -1 * grad_new + y * d;

                fs = F(xs);

                k++;
                if (Math.Abs((xs - xt).Norma1()) < eps)
                {
                    break;
                }
                grad = grad_new.Copy();
                ft = fs; xt = xs.Copy();
            }

            OptimumResult optimumResult = new OptimumResult();

            optimumResult.fopt = new Vector(new double[] { ft });
            optimumResult.ogr = null;
            optimumResult.xopt = xt;
            optimumResult.CountIterations = k;

            return optimumResult;
        }


        public static OptimumResult randomSearchWithOgr(Vector xn, double h, double eps, FunVector F, Restriction[] restriction)
        {
            int n = xn.Size;

            if (n != restriction.Length)
            {
                throw new ArgumentException("n != restriction");
            }

            for (int j = 0; j < n; j++)
            {

                if (restriction[j].IsRegion(xn[j]) == false)
                {
                    xn[j] = restriction[j].Proection(xn[j]);
                }
                Console.WriteLine(xn);

            }


            Vector xt = xn.Copy();
            double ft = 0;

            int m = n * 3;

            Vector xjp = new Vector(n);
            double fjp;

            Vector xjmin = new Vector(n);

            int k = 0;

            while (Math.Abs(h) > eps)
            {

                k++;

                double fjmin = double.MaxValue;

                for (int i = 0; i < m; i++)
                {
                    xjp = xt + h * Vector.NormalizeRandom(n);

                    for (int j = 0; j < n; j++)
                    {

                        if (restriction[j].IsRegion(xjp[j]) == false)
                        {
                            xjp[j] = restriction[j].Proection(xjp[j]);
                        }

                    }

                    //Console.WriteLine(xjp);

                    fjp = F(xjp);

                    if (fjp < fjmin)
                    {
                        fjmin = fjp;
                        xjmin = xjp;
                    }

                }
                //Console.ReadLine();
                ft = F(xt);

                if (fjmin < ft)
                {
                    xt = xjmin.Copy();
                    h = h * 1.2;

                }
                else
                {
                    h = h / 2;
                }


            }

            OptimumResult optimumResult = new OptimumResult();

            optimumResult.fopt = new Vector(new double[] { ft });
            optimumResult.ogr = null;
            optimumResult.xopt = xt;
            optimumResult.CountIterations = k;
            //Console.WriteLine(k);
            return optimumResult;
        }

        public static OptimumResult randomSearchWithOgrLinear(Vector xn, double h, double eps, FunVector F, RestrictionLinear restriction)
        {
            int n = xn.Size;


            if (n != restriction.A.Columns)
            {
                throw new ArgumentException("n != restriction");
            }


            if (restriction.isRegion(xn) == false)
            {
                xn = restriction.Proection(xn);
            }
            //Console.WriteLine(xn);

            Vector xt = xn.Copy();
            double ft = 0;

            int m = n * 3;

            Vector xjp = new Vector(n);
            double fjp;

            Vector xjmin = new Vector(n);

            int k = 0;

            while (Math.Abs(h) > eps)
            {

                k++;

                double fjmin = double.MaxValue;

                for (int i = 0; i < m; i++)
                {
                    xjp = xt + h * Vector.NormalizeRandom(n);

                    //Console.Write($"{xjp}->");
                    if (restriction.isRegion(xjp) == false)
                    {
                        xjp = restriction.Proection(xjp);
                    }

                    //Console.Write(xjp);
                    //Console.WriteLine();
                    fjp = F(xjp);

                    if (fjp < fjmin)
                    {
                        fjmin = fjp;
                        xjmin = xjp;
                    }

                }
                //Console.ReadLine();
                ft = F(xt);

                if (fjmin < ft)
                {
                    xt = xjmin.Copy();
                    h = h * 1.2;

                }
                else
                {
                    h = h / 2;
                }


            }

            OptimumResult optimumResult = new OptimumResult();

            optimumResult.fopt = new Vector(new double[] { ft });
            optimumResult.ogr = null;
            optimumResult.xopt = xt;
            optimumResult.CountIterations = k;
            //Console.WriteLine(k);
            return optimumResult;
        }

        public static OptimumResult RandomFuncOgr(Vector xn, double h, double eps,
            FunVector f, Restriction[] arg, RestrictionFunc[] RestrictionFunc)
        {
            int n = xn.GetSize();
            if (arg.Length != n || RestrictionFunc.Length != n) return null;
            Vector xt = xn.Copy();
            int m = 3 * n;
            int k = 0;
            
            for (int j = 0; j < n; j++)
            {
                if (!RestrictionFunc[j].IsRegion(xt))
                {
                    return null;
                }

            }
            
            for (int i = 0; i < n; i++)
            {
                if (!arg[i].IsRegion(xt[i]))
                {
                    xt[i] = arg[i].Proection(xt[i]);
                }
            }
            double ft = f(xt);
            while (Math.Abs(h) > eps)
            {
                k++;
                double fmin = double.MaxValue;
                Vector xpimin = new Vector(n);

                for (int i = 0; i < m; i++)
                {
                    int flag = 0;
                    Vector R = new Vector(n);
                    Vector xpi = new Vector(n);
                    while (flag == 1)
                    {
                        R = Vector.NormalizeRandom(n);
                        xpi = xt + h * R;
                        for (int j = 0; j < n; j++)
                        {
                            if (!arg[j].IsRegion(xt[j])) {
                                xpi[i] = arg[j].Proection(xpi[j]);
                            }
                                
                        }
                        flag = 1;
                        for (int j = 0; j < n; j++)
                        {
                            if (!RestrictionFunc[j].IsRegion(xpi))
                            {
                                flag = 0;
                                break;
                            }

                        }
                    }
                    double fi = f(xpi);
                    if (fi < fmin)
                    {
                        fmin = fi;
                        xpimin = xpi.Copy();
                    }
                }

                if (fmin < ft)
                {
                    xt = xpimin.Copy();
                    ft = fmin;
                    h *= 1.2;
                }
                else
                {
                    h /= 2;
                }
            }
            Vector func = new Vector(new double[] { ft });
            OptimumResult result = new OptimumResult(xt, null, func, k);
            return result;
        }
    



    // Задаем начальный симплекс
    public static Vector[] InitSimplex(Vector xn, double l, int m, int n)
        {
            Vector[] xv = new Vector[m];
            double r1 = l / (n * Math.Sqrt(2)) * (Math.Sqrt(n + 1) + n - 1);
            double r2 = l / (n * Math.Sqrt(2)) * (Math.Sqrt(n + 1) - 1);
            xv[0] = xn; // Первая вершина симплекса - начальная точка xn

            // Генерация остальных вершин симплекса на основе начальной точки xn
            for (int i = 1; i < m; i++)
            {
                Vector xt = xn.Copy();
                for (int j = 0; j < n; j++)
                {
                    if ((i - 1) == j) { xt[j] += r1; } // Добавление r1 к координате, соответствующей текущей вершине
                    else { xt[j] += r2; } // Добавление r2 к остальным координатам
                }
                xv[i] = xt; // Добавление новой вершины симплекса в массив
            }
            return xv;
        }


        // Сортируем значения функции Bubble Sort
        public static void SortValues(Vector[] x, double[] f, int m)
        {
            int num_of_elems = m;
            bool swappedElements = true;
            while (swappedElements)
            {
                num_of_elems--;
                swappedElements = false;
                for (int i = 0; i < num_of_elems; i++)
                {
                    if (f[i] > (f[i + 1]))
                    {
                        double tmp; tmp = f[i]; f[i] = f[i + 1]; f[i + 1] = tmp;
                        Vector t = x[i]; x[i] = x[i + 1]; x[i + 1] = t;
                        swappedElements = true;
                    }
                }
            }
        }

        // Находим минимум функции
        public static OptimumResult NelderMid(Vector xn, double l, double eps, FunVector f)
        {
            double alpha = 1.0;
            double beta = 0.5;
            double gamma = 2.0;

            int n = xn.GetSize();
            int m = n + 1;
            int k = 0;


            Vector[] x_array = InitSimplex(xn, l, m, n);
            double[] f_array = new double[m];
            for (int i = 0; i < m; i++)
            {
                f_array[i] = f(x_array[i]);
            }
            do
            {
                // Сортировка значений функции
                SortValues(x_array, f_array, m);

                // Находим центр тяжести
                double[] xc_array = new double[n];
                for (int i = 0; i < n; i++) { xc_array[i] = 0; }
                Vector xc = new Vector(xc_array);

                Vector xmax = x_array[m - 1];
                for (int i = 0; i < m; i++) { xc += x_array[i]; }
                xc = (xc - xmax) * (1.0 / n);
                double fc = f(xc);
                // Вычисляем критерий сходимости
                double kr = 0.0;
                for (int i = 0; i < m; i++)
                {
                    kr += (x_array[i] - xc).Norma1();
                }
                kr /= m;

                // Проверяем условие сходимости
                if (kr <= eps)
                {
                    Vector xopt = x_array[0];
                    Vector func = new Vector(new double[] { f(xopt) });
                    OptimumResult result = new OptimumResult(xopt, null, func, k);
                    return result;
                }

                // Поиск значений для дальнейшей работы
                Vector xmin = x_array[0];
                double fmin = f_array[0];
                double fs = f_array[m - 2];
                double fmax = f_array[m - 1];

                // Выполняем операция отражения
                Vector xotr = xc + alpha * (xc - xmax);
                double fotr = f(xotr);

                // Проверяем первое условие
                if (fotr <= fmin)
                {
                    // Выполняем операцию растяжения
                    Vector xrast = xc + gamma * (xotr - xc);
                    double frast = f(xrast);

                    // Заменяем наихудшую точку новой 
                    // в зависимости от выполнения условия
                    if (frast < fmin)
                    {
                        x_array[m - 1] = xrast;
                        f_array[m - 1] = frast;
                    }
                    else
                    {
                        x_array[m - 1] = xotr;
                        f_array[m - 1] = fotr;
                    }
                }
                // Проверяем второе условие
                else if (fs < fotr && fotr <= fmax)
                {
                    // Выполняем операцию сжатия
                    Vector xsjat = xc + beta * (xmax - xc);
                    // Заменяем худшую точку новой
                    x_array[m - 1] = xsjat;
                    double fsjat = f(xsjat);
                    f_array[m - 1] = fsjat;
                }
                // Проверяем третье условие
                else if (fmin < fotr && fotr <= fs)
                {
                    // Заменяем худшую точку новой
                    x_array[m - 1] = xotr;
                    f_array[m - 1] = fotr;
                }
                // Проверяем четвертое условие
                else if (fotr > fmax)
                {
                    // Заменяем все вершины многогранника 
                    for (int i = 0; i < m; i++)
                    {
                        x_array[i] = xmin + 0.5 * (x_array[i] - xmin);
                        f_array[i] = f(x_array[i]);
                    }
                }
                k++;
            } while (true);
        }

        public static OptimumResult OZU(Vector xn, Restriction[] xogr, RestrictionFunc[] J, RestrictionFunc[] Jdesired, double h, double eps)
        {
            int k, n, m;
            Vector xt, xmin;
            double fv, fl, fu, gt, gmin, gp;
            bool flak;

            k = 0;
            n = xn.GetSize();
            m = 3 * n;

            xt = new Vector(n);

            xmin = new Vector(n);

            for (int i = 0; i < n; i++) xt[i] = xogr[i].IsRegion(xn[i]) ? xn[i] : xogr[i].Proection(xn[i]);

            foreach (var item in J)
            {
                if (!item.IsRegion(xt)) throw new Exception("Не удовлетворяет ОДЗ");
            }

            gt = FNorm(xt, Jdesired);
            while (Math.Abs(h) > eps)
            {
                k++;
                gmin = double.MaxValue;
                for (int i = 0; i < m; i++)
                {
                    Vector xp = new Vector(n);
                    flak = false;
                    do
                    {
                        xp = xt + h * Vector.NormalizeRandom(n);
                        for (int j = 0; j < n; j++) xp[j] = xogr[j].IsRegion(xt[j]) ? xp[j] : xogr[j].Proection(xp[j]);
                        flak = false;
                        foreach (var item in J)
                        {

                            if (!item.IsRegion(xp))
                            {
                                flak = true;
                                break;
                            }
                        }

                    }
                    while (flak);
                    gp = FNorm(xp, Jdesired);
                    if (gp < gmin) { gmin = gp; xmin = xp.Copy(); }

                }
                if (gmin < gt) { xt = xmin.Copy(); gt = gmin; h *= 1.2; }
                else { h /= 2; }
                k++;

            }
            int f_length = Jdesired.Length;
            Vector fopt = new Vector(f_length);
            for (int i = 0; i < f_length; i++) fopt[i] = Jdesired[i].GetValue(xt);
            OptimumResult result = new OptimumResult(xt, null, fopt, k);
            return result;

        }



        private static double FNorm(Vector x, RestrictionFunc[] Jdesrired)
        {
            double fv, fl, fu;

            List<double> fnorm = new List<double>();
            RestrictionTypes type;
            for (int i = 0; i < Jdesrired.Length; i++)
            {
                fv = Jdesrired[i].GetValue(x);
                fl = Jdesrired[i].Lower;
                fu = Jdesrired[i].Upper;
                type = Jdesrired[i].Type;

                switch (type)
                {
                    case RestrictionTypes.DoubleOgr:
                        fnorm.Add((fv - fl) / (fu - fl));
                        fnorm.Add((fu - fv) / (fu - fl));
                        continue;
                    case RestrictionTypes.UpperOgr:
                        if (fu > 0) fnorm.Add(fv / fu);
                        else if (fu < 0) fnorm.Add(2 - fv / fu);
                        else fnorm.Add(1 - fv / fu);
                        continue;
                    case RestrictionTypes.LowerOgr:
                        if (fl > 0) fnorm.Add(2 - fv / fl);
                        else if (fl < 0) fnorm.Add(fv / fl);
                        else fnorm.Add(1 - fv / fl);
                        continue;
                }

            }
            double g = fnorm.Max();
            return g;
        }



    }
}