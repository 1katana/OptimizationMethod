
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OptimizationMethod
{
    class dynamicProgrammingScobki
    {
        int[] P;

        Matrix m;

        Matrix matrixK;

        int n;

        public dynamicProgrammingScobki(int[] P)
        {
            this.P = P;

            n=P.Length-1;

            m = new Matrix(n,n);


        }

        

        public Matrix[] calc()
        {
            this.matrixK = new Matrix(n, n);

            for (int l = 2; l <= n; l++)
            {
                for(int i = 1; i <= n-l+1; i++)
                {
                    int j = i + l - 1;

                    int im=i-1;
                    int jm = j - 1;



                    double minValue = double.MaxValue;
                    int k = i;
                    while (k < j)
                    {
                        /*Console.WriteLine($"i-{i} j-{j} k-{k} l={l}");*/
                        int km = k - 1;

                        double q = m[im, km] + m[km + 1, jm] + P[i - 1] * P[k] * P[j];

                        /*Console.WriteLine($"{m[im, km]} {m[km + 1, jm]} {P[i - 1]} {P[k]} {P[j]}");*/

                        if (q < minValue)
                        {
                            minValue = q;
                            m[im, jm] = q;
                            matrixK[im, jm] = km;
                            /*Console.WriteLine(m);*/
                        }

                        k++;
                    }
                    /*
                    for(int k = i; i < j; k++)
                    {
                        
                    }*/
                }
            }

            return new Matrix[2] {m,matrixK};

            
        }

        public string Matrix_Chain_Multiply(int i,int j)
        {
            

            if (j > i)
            {
                string X = Matrix_Chain_Multiply(i, Convert.ToInt16(matrixK[i, j]));
                string Y= Matrix_Chain_Multiply(Convert.ToInt16(matrixK[i, j])+1, j);


                /*Console.WriteLine($"({X}*{Y})");*/
                return $"({X}*{Y})";
            }
            else
            {
                /*Console.WriteLine($"A{i+1}");*/

                return $"A{i+1}"; 
            }

        }
    }
}
