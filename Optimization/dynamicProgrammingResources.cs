using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethod
{
    class dynamicProgrammingResources
    {
        int m;
        int n;
        Matrix Z;
        Matrix F;
        

        public dynamicProgrammingResources(Matrix Z)
        {
            this.n = Z.Columns;
            this.m = Z.Rows;
            this.Z = Z;
            this.F=new Matrix(m,n);
            //Console.WriteLine($"{m},{n}");
            //Console.WriteLine(Z);
            //Console.WriteLine(F);
        }

        public Matrix[] calculate()
        {
            Matrix matrixK = new Matrix(m,n);

            for(int i = 0; i < this.n; i++)
            {
                for(int j = 0; j < this.m; j++)
                {
                    //Console.WriteLine($"{i},{j}");


                    
                    double maxValue=double.MinValue;

                    
                    if (j != 0)
                    {
                        if (i == 0)
                        {
                            F[j, i] = this.Z[j, i];
                        }
                        else
                        {
                            int k = 0;
                            while (k <= j)
                            {
                                double Fij = F[k, i - 1] + Z[j - k, i];

                                //Console.WriteLine($"{F[k, i - 1]}+{Z[j - k, i]}");

                                if (maxValue < Fij)
                                {
                                    maxValue = Fij;

                                    matrixK[j, i] = k;
                                }

                                F[j, i] = maxValue;



                                //Console.WriteLine($"{k},{maxValue}");

                                //Console.WriteLine(F);

                                k++;

                            }
                            //Console.WriteLine(matrixK);

                        }

                        
                    }
                    
                    //Console.WriteLine(F);
                }
            }

            n--;
            m--;
            string pr = $"max-{F[m,n]} => ";

            
            
            while(n >= 0)
            {
                int kobr = Convert.ToInt16(matrixK[m, n]);
                //Console.WriteLine($"{m -= Convert.ToInt16(matrixK[m, n])} {n}");

                pr = pr + $"pr{n}-{this.Z[m-kobr, n]} ";
                //Console.WriteLine(pr);
                m= kobr;
                n--;
            }

            Console.WriteLine(pr);
            return new Matrix[2] {this.F,matrixK};
        }


    }
}
