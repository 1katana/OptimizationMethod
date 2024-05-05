using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethod
{
    class Item
    {
        private int p;
        private int w;
        private double specific_p;

        public Item(int p, int w)
        {
            this.p = p;
            this.w = w;
            if(p!=0 || w!=0) specific_p = (double) p/w ;
        }

        public int P { get { return p; } set { p = value; specific_p = p / w; } }

        public int W { get { return w; } set { w = value; specific_p = p / w; } }

        public double specificP { get { return specific_p; } }

        public override string ToString()
        {
            return $"(p={p}  w={w} p/w={specific_p})";
        }
    }

    class Rykzak
    {
        Matrix A;
        List<Item> item=new List<Item> {new Item(0,0)};

        private int k;
        private int w;
        
        public int K { get { return k; }  }

        public int W { get { return w; } set { w = value; } }


        public Rykzak(Item[] item, int W)
        {


            this.item.InsertRange(1,item);

            this.w = W;
            this.k = item.Length;


        }

        public Rykzak()
        {


        }

        public void add(Item i)
        {
            item.Add(i);
            this.k = this.item.Count-1;
        }

        public Matrix calc()
        {
            this.A = new Matrix(k + 1, W + 1);

            for (int n = 0; n <= W; ++n) {     // Заполняем нулевую строчку

                A[0,n] = 0;
            }

            for (int s = 1; s <= k; ++s)
            {
                for(int n=0; n <= W; ++n)
                {

                    A[s, n] = A[s - 1, n];
                    if (n >= item[s].W && (A[s - 1, n - item[s].W]+ item[s].P > A[s,n]))
                    {
                        A[s,n]=A[s - 1,n - item[s].W] + item[s].P;
                    }
                }
            }
            return A;
        }
        

        public void Print(int s, int n)
        {
            if (A[s, n] == 0)
            {
                return;
            }
            else if (A[s - 1,n] == A[s,n])
            {
                Print(s - 1, n);
            }
            else
            {
                Print(s-1, n - item[s].W);
                Console.WriteLine(item[s]);
            }
        }

        public void Approximate_Format()
        {
            //Сортировка по удельной стоимости
            bool changes_made = true;
            Item tmp;
            List<Item> tmp_p = new List<Item>();

            tmp_p.InsertRange(0,item.GetRange(1,k));

            while (changes_made)
            {
                changes_made = false;
                for (int i = 0; i < k - 1; i++)
                {
                    if (tmp_p[i].specificP < tmp_p[i + 1].specificP)
                    {
                        tmp = tmp_p[i];
                        tmp_p[i] = tmp_p[i + 1];
                        tmp_p[i + 1] = tmp;
                        changes_made = true;
                    }
                }
            }
            //Складываем элементы в рюкзак
            double summ = 0;
            foreach (Item item in tmp_p)
            {
                if (summ + item.W > W) continue;
                summ += item.W;
                Console.WriteLine(item);
                if (summ == W) break;
            }
        }
   



    }
}
