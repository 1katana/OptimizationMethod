using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OptimizationMethod
{

    class Matrix
    {
        protected int rows, columns;
        protected double[,] data;

        
        public Matrix(int r, int c)
        {
            this.rows = r; this.columns = c;
            data = new double[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++) data[i, j] = 0;
        }
        public Matrix(double[,] mm)
        {
            this.rows = mm.GetLength(0); this.columns = mm.GetLength(1);
            data = new double[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    data[i, j] = mm[i, j];
        }
        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }

        public double this[int i, int j]
        {
            get
            {
                if (i < 0 && j < 0 && i >= rows && j >= columns)
                {
                    Console.WriteLine(" Индексы вышли за пределы матрицы ");
                    return Double.NaN;
                }
                else
                    return data[i, j];
            }
            set
            {
                if (i < 0 && j < 0 && i >= rows && j >= columns)
                {
                    Console.WriteLine(" Индексы вышли за пределы матрицы ");
                }
                else
                    data[i, j] = value;
            }
        }

        public static Matrix IdentityMatrix(int n)
        {
            Matrix matrix = new Matrix(n,n);

            for(int i = 0; i < n; i++)
            {
                matrix[i, i] = 1;
            }
            return matrix;
        }

        public Vector GetRow(int r)
        {
            if (r >= 0 && r < rows)
            {
                Vector row = new Vector(columns);
                for (int j = 0; j < columns; j++) row[j] = data[r, j];
                return row;
            }
            return null;
        }
        public Vector GetColumn(int c)
        {
            if (c >= 0 && c < columns)
            {
                Vector column = new Vector(rows);
                for (int i = 0; i < rows; i++) column[i] = data[i, c];
                return column;
            }
            return null;
        }
        public bool SetRow(int index, Vector r)
        {
            if (index < 0 || index > rows) return false;
            if (r.Size != columns) return false;
            for (int k = 0; k < columns; k++) data[index, k] = r[k];
            return true;
        }
        public bool SetColumn(int index, Vector c)
        {
            if (index < 0 || index > columns) return false;
            if (c.Size != rows) return false;
            for (int k = 0; k < rows; k++) data[k, index] = c[k];
            return true;
        }
        public void SwapRows(int r1, int r2)
        {
            if (r1 < 0 || r2 < 0 || r1 >= rows || r2 >= rows || (r1 == r2)) return;
            Vector v1 = GetRow(r1);
            Vector v2 = GetRow(r2);
            SetRow(r2, v1);
            SetRow(r1, v2);
        }
        public Matrix Copy()
        {
            Matrix r = new Matrix(rows, columns);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++) r[i, j] = data[i, j];
            return r;
        }
        public Matrix Trans()
        {
            Matrix transposeMatrix = new Matrix(columns, rows);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    transposeMatrix.data[j, i] = data[i, j];
                }
            }
            return transposeMatrix;
        }
        //Сложение матриц
        public static Matrix operator +(Matrix m1, Matrix m2)     //Сложение матриц
        {
            if (m1.rows != m2.rows || m1.columns != m2.columns)
            {
                throw new Exception("Матрицы не совпадают по размерности");
            }
            Matrix result = new Matrix(m1.rows, m1.columns);

            for (int i = 0; i < m1.rows; i++)
            {
                for (int j = 0; j < m2.columns; j++)
                {
                    result[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return result;
        }
        //Вычитание матриц
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.rows != m2.rows || m1.columns != m2.columns)
            {
                throw new Exception("Матрицы не совпадают по размерности");
            }
            Matrix result = new Matrix(m1.rows, m1.columns);
            for (int i = 0; i < m1.rows; i++)
            {
                for (int j = 0; j < m2.columns; j++)
                {
                    result[i, j] = m1[i, j] - m2[i, j];
                }
            }
            return result;
        }
        //Произведение матриц


        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.columns != m2.rows)
            {
                throw new Exception("Столбцы 1 матрицы не совпадает со строкой 2 матрицы");
            }
            Matrix result = new Matrix(m1.rows, m2.columns);


            int resultRows = 0;
            int resultColums = 0;
            while (resultColums < result.Columns)
            {

                while (resultRows < result.Rows)
                {
                    double sum = 0.0;

                    for (int i = 0; i < m1.columns; i++)
                    {
                        sum = sum + m1[resultRows, i] * m2[i, resultColums];


                    }
                    if (Math.Abs(sum) < 0.000001) sum = 0;
                    result[resultRows, resultColums] = sum;
                    resultRows++;

                }

                resultColums++;
                resultRows = 0;

            }

            return result;
        }



        // произведение на число
        public static Matrix operator *(Matrix m1, double ch)
        {
            Matrix result = new Matrix(m1.rows, m1.columns);

            if (m1 == null || ch == double.NaN)
            {
                throw new Exception("Матрица пустая или число==Nan");
            }

            for (int i = 0; i < m1.rows; i++)
            {
                for (int j = 0; j < m1.columns; j++)
                {

                    result[i, j] = m1[i, j] * ch;
                }
            }


            return result;
        }

        //Произведение на вектор
        public static Vector operator *(Matrix m1, Vector vec)
        {
            Vector result = new Vector(m1.rows);

            if (m1.Columns != vec.Size)
            {
                throw new Exception("Длина столбца матрицы не совпадает с длиной вектора");
            }

            for (int i = 0; i < m1.rows; i++)
            {
                result[i] = 0;
                for (int j = 0; j < m1.columns; j++)
                {
                    result[i] += m1[i, j] * vec[j];
                }
            }
            return result;
        }

        public static Vector LowerTriangle(Matrix A, Vector B)
        {
            if (A.Rows != A.Columns) return null;
            if (A.Rows != B.Size) return null;

            int num_of_rows = A.Rows;
            double eps = 0.0000000001;
            for (int i = 0; i < num_of_rows; i++)
            {
                if (Math.Abs(A[i, i]) < eps)
                {

                    return null;
                }
                for (int j = i + 1; j < num_of_rows; j++)
                {
                    if (Math.Abs(A[i, j]) > eps)
                    {

                        return null;
                    }
                }
            }


            Vector x = new Vector(num_of_rows);
            x[0] = B[0] / A[0, 0];
            for (int i = 1; i < num_of_rows; i++)
            {
                double summa = 0;
                for (int j = 0; j < i; j++)
                {
                    summa += A[i, j] * x[j];

                }
                x[i] = (B[i] - summa) / A[i, i];
            }
            return x;
        }

        public static Vector UpTriangle(Matrix A, Vector B)
        {
            if (A.Rows != A.Columns) return null;
            if (A.Rows != B.Size) return null;

            int num_of_rows = A.Rows;
            double eps = 0.0000000001;
            for (int i = num_of_rows - 1; i <= 0; i--)
            {
                if (Math.Abs(A[i, i]) < eps)
                {

                    return null;
                }
                for (int j = 0; j < i; j++)
                {
                    if (Math.Abs(A[i, j]) > eps)
                    {

                        return null;
                    }
                }
            }


            Vector x = new Vector(num_of_rows);
            x[num_of_rows - 1] = B[num_of_rows - 1] / A[num_of_rows - 1, num_of_rows - 1];
            for (int i = num_of_rows - 2; i >= 0; i--)
            {
                double summa = 0;
                for (int j = i + 1; j < num_of_rows; j++)
                {
                    summa += A[i, j] * x[j];

                }
                x[i] = (B[i] - summa) / A[i, i];
            }
            return x;
        }


        public static Vector GaussMethod(Matrix A, Vector B)
        {
            Matrix copyA = A.Copy();
            Vector copyB = B.Copy();
            int rows = A.rows;
            int columns = A.columns;
            double eps = 0.00000000001;
            if (rows != columns) return null;
            if (rows != B.Size) return null;
            int max;
            double tmp;
            for (int j = 0; j < columns; j++)
            {
                max = j;
                for (int i = j + 1; i < rows; i++)
                {
                    if (Math.Abs(copyA[i, j]) > Math.Abs(copyA[max, j])) { max = i; };
                }
                if (max != j)
                {
                    Vector temp = copyA.GetRow(max); copyA.SetRow(max, copyA.GetRow(j)); copyA.SetRow(j, temp);
                    tmp = copyB[max]; copyB[max] = copyB[j]; copyB[j] = tmp;
                }
                if (Math.Abs(copyA[j, j]) < eps) return null;
                for (int i = j + 1; i < rows; i++)
                {
                    double multiplier = -copyA[j, j] / copyA[i, j];
                    for (int k = 0; k < columns; k++)
                    {
                        copyA[i, k] *= multiplier;
                        copyA[i, k] += copyA[j, k];
                    }
                    copyB[i] *= multiplier;
                    copyB[i] += copyB[j];
                }
            }


            return UpTriangle(copyA, copyB);


        }



        public static Matrix inverseMatrix(Matrix A)
        {
            int rows = A.rows;
            int columns = A.columns;
            // матрица должна быть квадртаной
            if (rows != columns) return null;
            Matrix copied_A = A.Copy();

            Matrix result = new Matrix(rows, columns);
            // формирование единичной матриы
            Matrix E = new Matrix(rows, columns);
            for (int i = 0; i < rows; i++)
            {
                E[i, i] = 1;
            }

            double eps = 0.000000001;
            int max;
            for (int j = 0; j < columns; j++)
            {
                // в каждом столбце находим максимальный элементы
                // по модулю
                max = j;
                for (int i = j + 1; i < rows; i++)
                {
                    if (Math.Abs(copied_A[i, j]) > Math.Abs(copied_A[max, j])) { max = i; };
                }

                // если максимальный элементы не находится
                // на главной диагонали, то меняем строки местами так,
                // чтобы максимальный элемент оказался на главной диагонали
                if (max != j)
                {
                    Vector temp = copied_A.GetRow(max); copied_A.SetRow(max, copied_A.GetRow(j)); copied_A.SetRow(j, temp);
                    Vector tmp = E.GetRow(max); E.SetRow(max, E.GetRow(j)); E.SetRow(j, tmp);
                }

                // на главной диагонали не должны
                // находиться нули
                if (Math.Abs(copied_A[j, j]) < eps) return null;

                // приводим матрицу к виду
                // верхней треугольной матрицы
                // с помощью элементарных преобразований
                // (ед. матрица изменяется одновременно)
                for (int i = j + 1; i < rows; i++)
                {
                    double multiplier = -copied_A[j, j] / copied_A[i, j];
                    for (int k = 0; k < columns; k++)
                    {
                        copied_A[i, k] *= multiplier;
                        copied_A[i, k] += copied_A[j, k];
                        E[i, k] *= multiplier;
                        E[i, k] += E[j, k];
                    }
                }
            }
            // находим столбцы обратной матрицы
            // каждый столбец обратной матрицы есть решение
            // СЛАУ с верхней треугольной матрицей,
            // где вектор свободных коэффициентов
            // есть соответсвующий столбец единичной матрицы
            for (int i = 0; i < columns; i++)
            {
                result.SetColumn(i, UpTriangle(copied_A, E.GetColumn(i)));
            }

            return result;

        }

        private static double algebDop(int iA, int jA, Matrix A)
        {
            double a = Math.Pow(-1, iA + jA);

            int n = A.Columns;

            Matrix b = new Matrix(n - 1, n - 1);
            List<double> temp = new List<double>();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (iA != i && jA != j)
                    {
                        temp.Add(A[i, j]);
                    }
                }

            }
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    int t = i * (n - 1) + j;
                    b[i, j] = temp[t];
                }

            }
            Console.WriteLine(b);
            Console.WriteLine(a * det(b));
            return a * det(b);

        }

        public static double det(Matrix A)
        {
            Matrix copyA = A.Copy();

            double det = 1;

            int rows = A.rows;
            int columns = A.columns;
            double eps = 0.00000000001;
            if (rows != columns) return double.NaN;

            int max;
            double tmp;
            for (int j = 0; j < columns; j++)
            {
                max = j;
                for (int i = j + 1; i < rows; i++)
                {
                    if (Math.Abs(copyA[i, j]) > Math.Abs(copyA[max, j])) { max = i; };
                }
                if (max != j)
                {
                    Vector temp = copyA.GetRow(max); copyA.SetRow(max, copyA.GetRow(j)); copyA.SetRow(j, temp);

                }
                if (Math.Abs(copyA[j, j]) < eps) return 0;
                for (int i = j + 1; i < rows; i++)
                {
                    double multiplier = -copyA[j, j] / copyA[i, j];
                    for (int k = 0; k < columns; k++)
                    {
                        copyA[i, k] *= multiplier;
                        copyA[i, k] += copyA[j, k];
                    }

                }

            }
            for (int i = 0; i < columns; i++)
            {
                det *= copyA[i, i];
            }
            return det;
        }

        public override string ToString()
        {
            string result = "|";
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {


                    result = result + $"{this.data[i, j]} ";

                }
                result = result + "|";
                result = result + "\n";

                if (i != this.Rows - 1)
                {
                    result = result + "|";
                }
            }
            return result;
        }

        public static Vector GrammaShmidta(Matrix matrix, Vector vec)
        {
            //Ортогонализация
            Matrix OrtoMatrix = new Matrix(matrix.Rows, matrix.Columns);

            Vector x = vec.Copy();
            OrtoMatrix.SetColumn(0, matrix.GetColumn(0));

            //Верхнетреуг. матрица с коэфиц.
            Matrix T = new Matrix(matrix.Rows, matrix.Columns);
            //единицы в глав. диаг
            for (int i = 0; i < matrix.rows; i++)
            {
                T[i, i] = 1;
            }

            for (int i = 1; i < matrix.Rows; i++)
            {
                int n = 0;
                //1
                OrtoMatrix.SetColumn(i, matrix.GetColumn(i));
                //остальные
                while (n < i)
                {
                    T[n, i] = (matrix.GetColumn(i) * OrtoMatrix.GetColumn(n)) / (OrtoMatrix.GetColumn(n) * OrtoMatrix.GetColumn(n));
                    OrtoMatrix.SetColumn(i, OrtoMatrix.GetColumn(i) - OrtoMatrix.GetColumn(n) * T[n, i]);
                    n++;
                }
            }
            //диагональная матрица
            Matrix Diagl = OrtoMatrix.Trans() * OrtoMatrix;

            // обратная матрица матрице D
            for (int i = 0; i < Diagl.Columns; i++) Diagl[i, i] = 1 / Diagl[i, i];

            x = Diagl * (OrtoMatrix.Trans() * vec);

            return UpTriangle(T, x);
        }

        public static Vector Progonki(Matrix a, Vector b)
        {
            int n = a.rows;
            //Проверки

            if (n < 3)
            {
                return null;
            }
            if (n != b.Size) return null;
            for (int i = 0; i < n; i++)
            {
                if (a[i, i] == 0) return null;
                if (i < n - 1) { if (a[i, i + 1] == 0) return null; }
                if (i != 0) { if (a[i, i - 1] == 0) return null; }
                for (int j = 0; j < i - 1; j++) { if (a[i, j] != 0) return null; }
                for (int j = i + 2; j < n; j++) { if (a[i, j] != 0) return null; }
            }

            Vector x = b.Copy();
            //Прямой ход
            Vector v = new Vector(n);
            Vector u = new Vector(n);
            //первая строка матрицы
            v[0] = a[0, 1] / (-a[0, 0]);
            u[0] = (-b[0] / (-a[0, 0]));
            //остальные
            for (int i = 1; i < n - 1; i++)
            {
                double zn = (-a[i, i] - a[i, i - 1] * v[i - 1]);

                v[i] = a[i, i + 1] / zn;
                u[i] = (a[i, i - 1] * u[i - 1] - b[i]) / zn;
            }
            //последняя
            v[n - 1] = 0;
            u[n - 1] = (a[n - 1, n - 2] * u[n - 2] - b[n - 1]) / (-a[n - 1, n - 1] - a[n - 1, n - 2] * v[n - 2]);

            //обратный ход
            x[n - 1] = u[n - 1];
            for (int i = n - 1; i > 0; i--)
                x[i - 1] = v[i - 1] * x[i] + u[i - 1];
            return x;
        }
        public static Vector ProgonkiVector(Vector N, Vector S, Vector V, Vector b)
        {
            int n = S.Size;
            // проверка
            if (b.Size != n) return null;
            Vector x = new Vector(n);
            // прогоночные коэффициенты
            Vector v = new Vector(n);
            Vector u = new Vector(n);

            for (int i = 0; i < n; i++) if (S[i] == 0) return null;

            //первая строка матрицы
            v[0] = -V[0] / S[0];
            u[0] = b[0] / S[0];
            //остальные
            for (int i = 1; i < n - 1; i++)
            {
                double zn = -S[i] - N[i - 1] * v[i - 1];

                u[i] = V[i - 1] / zn;
                v[i] = (N[i - 1] * u[i - 1] - b[i]) / zn;
            }
            //последняя
            v[n - 1] = 0;
            u[n - 1] = (N[n - 2] * u[n - 2] - b[n - 1]) / (-S[n - 1] - N[n - 2] * v[n - 2]);
            //обратный ход
            x[n - 1] = u[n - 1];
            for (int i = n - 1; i > 0; i--)
                x[i - 1] = v[i - 1] * x[i] + u[i - 1];
            return x;
        }


        public static Vector posPrib(Matrix matrix, Vector vec)
        {
            //Проверки
            if (matrix.Rows != matrix.Columns) return null;
            if (matrix.Rows != vec.Size) return null;

            int n = matrix.Rows;
            for (int i = 0; i < n; i++)
            {
                if (matrix[i, i] == 0) return null;
            }

            double Eps = 0.001;

            Matrix a = new Matrix(n, n);

            Vector b = vec.Copy();


            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        b[i] = vec[i] / matrix[i, i];
                        a[i, j] = -(matrix[i, j] / matrix[i, i]);
                    }
                    else
                    {
                        a[i, j] = 0;
                    }

                }
            }
            Vector x = b.Copy();
            Vector xLast = b.Copy();

            Vector delta = x.Copy();

            do
            {
                xLast = x.Copy();
                x = a * xLast + b;
                delta = x - xLast;

            } while (Math.Abs(delta.Norma1()) > Eps);
            return x;
        }
    }
    class Vector
    {
        protected int size;

        
        protected double[] data;
        static Random rnd = new Random();

        [JsonIgnore]
        public int Size { get { return size; } }
        
        public double[] Data { get { return data; } }

        public Vector(int size)
        {
            this.size = size;
            data = new double[size];
        }

        
        public double[] GetElements()
        {
            return data;
        }
        public Vector(double[] v)
        {
            this.size = v.Length;
            data = new double[size];
            for (int i = 0; i < size; i++) data[i] = v[i];
        }
        
        public double this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        public int GetSize() { return size; }
        public bool SetElement(double el, int index)
        {
            if (index < 0 || index >= size) return false;
            data[index] = el;
            return true;
        }
        public double GetElement(int index)
        {
            if (index < 0 || index >= size) return default(double);
            return data[index];
        }
        public Vector Copy()
        {
            Vector rez = new Vector(data);
            return rez;
        }
        public override string ToString()
           => $"{{{string.Join(";", this.data)}}}";

        public double Norma1()
        {
            double s = 0;
            for (int i = 0; i < size; i++)
                s += data[i] * data[i];
            return Math.Sqrt(s);
        }
        public double Norma2()
        {
            double s = 0;
            for (int i = 0; i < size; i++)
                if (Math.Abs(data[i]) > s) s = Math.Abs(data[i]);
            return s;
        }
        public double Norma3()
        {
            double s = 0;
            for (int i = 0; i < size; i++)
                s += Math.Abs(data[i]);
            return s;
        }
        public double ScalarMultiply(Vector b)
        {
            if (size != b.size) return 0;
            double s = 0;
            for (int i = 0; i < size; i++)
                s += data[i] * b.data[i];
            return s;
        }
        public Vector MultiplyScalar(double c)
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++) rez.data[i] = data[i] * c;
            return rez;
        }
        public Vector Normalize()
        {
            Vector rez = new Vector(size);
            double d = Norma1();
            for (int i = 0; i < size; i++)
            {

                if (d != 0) rez.data[i] = data[i] / d; else rez.data[i] = data[i];
            }
            return rez;
        }
        public static Vector NormalizeRandom(int size)
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++)
                rez.data[i] = (rnd.NextDouble() - 0.5) * 2.0;
            return rez.Normalize();
        }

        public Vector UMinus()
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++) rez.data[i] = -data[i];
            return rez;
        }
        public Vector Add(Vector c)
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++) rez.data[i] = data[i] + c.data[i];
            return rez;
        }
        public Vector Minus(Vector c)
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++) rez.data[i] = data[i] - c.data[i];
            return rez;
        }
        public static Vector operator +(Vector a, Vector b)
        {
            if (a.size == b.size)
            {
                Vector c = new Vector(a.size);
                for (int i = 0; i < a.size; i++)
                    c[i] += a[i] + b[i];
                return c;
            }
            return null;
        }
        public static Vector operator -(Vector a, Vector b)
        {
            if (a.size == b.size)
            {
                Vector c = new Vector(a.size);
                for (int i = 0; i < a.size; i++)
                    c[i] += a[i] - b[i];
                return c;
            }
            return null;
        }
        public static Vector operator *(Vector a, double c)
        {
            Vector r = new Vector(a.size);
            for (int i = 0; i < a.size; i++)
                r[i] = a[i] * c;
            return r;
        }
        public static Vector operator *(double c, Vector a)
        {
            Vector r = new Vector(a.size);
            for (int i = 0; i < a.size; i++)
                r[i] = a[i] * c;
            return r;
        }
        public static Vector operator *(int c, Vector a)
        {
            Vector r = new Vector(a.size);
            for (int i = 0; i < a.size; i++)
                r[i] = a[i] * c;
            return r;
        }

        public static double operator *(Vector a, Vector b)
        {
            if (a.size == b.size)
            {
                double s = 0.0;
                for (int i = 0; i < a.size; i++)
                    s += a[i] * b[i];
                return s;
            }
            return Double.NaN;
        }
    }
}
