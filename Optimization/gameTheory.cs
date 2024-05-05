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

            double[] a = new double[n];  double[] b = new double[n];

            (int index,double value) lower=(0,double.MinValue); (int index, double value) upper = (0, double.MaxValue);

            for (int i = 0; i < n; i++) { a[i]=double.MaxValue; b[i]= double.MinValue; }

            for ( int i = 0; i < n; i++)
            {
                for( int j = 0; j < n; j++)
                {
                    a[i] = a[i] < matrix[i,j] ? a[i] : matrix[i,j]; b[j] = b[j] > matrix[i, j] ? b[j] : matrix[i, j];
                }
            }

            for(int i = 0; i < n; i++)
            {
                lower = lower.value < a[i] ? (i, a[i]): lower; upper = upper.value > b[i] ? (i, b[i]) : upper;
            }

            
            //Вывод
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

            Console.WriteLine($"Нижняя цена= индекс {lower.index} значение {lower.value}");
            Console.WriteLine($"Верхняя цена= индекс {upper.index} значение {upper.value}");

            if(lower.value == upper.value)
            {
                Console.WriteLine($"Точка перегиба: ИНДЕКС {lower.index};{upper.index} значение {upper.value}");
            }
            else Console.WriteLine("Точки перегиба нет");
            
        }



        public static void mixedStrategy()
        {

        }


    }
}
