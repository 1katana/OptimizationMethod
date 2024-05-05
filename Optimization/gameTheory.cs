using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OptimizationMethod
{
    static class gameTheory
    {

        public static void clearStrategy(Matrix matrix)
        {
            int n = matrix.Columns;

            double[] a = new double[n];
            double[] b = new double[n];

            (int index,double value) lower=(0,0);
            
            (int index,double value) upper= (0, 0);
            

            for (int i = 0; i < n; i++) { a[i]=double.MaxValue; b[i]= double.MinValue; }


            for ( int i = 0; i < n; i++)
            {
                for( int j = 0; j < n; j++)
                {
                    a[i] = a[i] < matrix[i,j] ? a[i] : matrix[i,j];

                    b[j] = b[j] > matrix[i, j] ? b[j] : matrix[i, j];

                }
            }

            for(int i = 0; i < n-1; i++)
            {
                lower = a[i] > a[i + 1] ? (i, a[i]):(i+1,a[i+1]);

                upper = b[i] <  b[i + 1] ? (i, b[i]) : (i + 1, b[i + 1]);
            }

            
            foreach(double d in a)
            {
                Console.Write($"{d} ");
            }
            Console.WriteLine();
            
            foreach (double d in b)
            {
                Console.Write($"{d} ");
            }
            Console.WriteLine();

            Console.WriteLine($"Нижняя цена={lower}");
            Console.WriteLine($"Верхняя цена={upper}");

            bool f = false;
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if (a[i] == b[j])
                    {
                        Console.WriteLine($"Седловая точка: A{i}={a[i]}, B{j}={b[j]} ");
                        f = true;
                    }
                }
            }


            if (f == false)
            {
                Console.WriteLine("Нет седловой точки");
            }
            
        }

        public static void mixedStrategy()
        {

        }


    }
}
