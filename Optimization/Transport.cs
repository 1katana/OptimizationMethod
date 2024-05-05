using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethod
{
    class TransportTask{


        Vector b;Vector a; Matrix C;
        Matrix X; Matrix delta; double J;
        int m;
        int n;
        Vector u;
        Vector v;
        
        
        public Vector U {
            get { return u; }
        }

        public Vector V
        {
            get { return v; }
        }

        public TransportTask(Vector a, Vector b, Matrix C)
        {
            this.a = a;
            this.b = b;
            this.C = C;
            this.n = b.Size;
            this.m = a.Size;

            this.X = new Matrix(m,n);

        }

        public Matrix northwestCorner(Vector a, Vector b)
        {
            double[] bsum = new double[n];
            for ( int i=0; i<m; i++)
            {
                double asum = 0;
                for (int j=0; j<n; j++)
                {
                    double ostA = a[i] - asum;
                    double ostB = b[j] - bsum[j];
                    if (ostA>0 & ostB>0)
                    {
                        X[i, j] = (ostA > 0 & ostB > 0)? Math.Min(ostA,ostB):0;
                       
                    }
                    asum += X[i,j];
                    bsum[j] += X[i, j];
                }
            }
            return X;
        }

        public Matrix calcDelta(Matrix C)
        {
            delta = new Matrix(m, n);

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    delta[i,j] = C[i, j] - u[i] - v[j];
                }
            }

            return delta;
        }


        public double sumOfExpenses(Matrix C, Matrix X)
        {
            double J = 0.0;
            
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    J += C[i, j] * X[i,j];
                }
            }
            return J;
        }
        
        public void findPotencial(Matrix C,Matrix X)
        {
            u = new Vector(m);
            v = new Vector(n);

            u[0] = 0.0;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (X[i, j] != 0.0)
                    {
                        if (v[j] == 0)
                        {
                            v[j] = C[i, j] - u[i];
                            //Console.WriteLine($"v{j}={C[i, j]}-{u[i]}");
                        }
                        else
                        {
                            if(i!=0) u[i] = C[i, j] - v[j];
                            //Console.WriteLine(v[j]);

                            //Console.WriteLine($"u{i}={C[i, j]}-{v[j]}");

                        }
                    }
                }
            }

            for (int j = 0; j < n; j++)
            {
                Console.WriteLine($"{v[j]} v{j}");
            }

            for (int i = 0; i < m; i++)
            {
                Console.WriteLine($"{u[i]} u{i}");
            }

        }
        
    }
}
