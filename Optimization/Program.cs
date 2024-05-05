using OptimizationMethod;
using System.ComponentModel;




class Program
{
    static void Main(string[] args)
    {
        test.game();



        

    }
}



class test
{
    
    public static void Opt()
    {
        Console.WriteLine("StepByStep = {0}", Optimization.StepByStep(0, 0.1, 0.00001, x => x * x + x));

        Console.WriteLine("GoldenSech = {0}", Optimization.GoldenSearch(-3, 3, 0.00001, x => x * x + x));
        Console.WriteLine($"PolDel = {Optimization.PolDel(-3, 3, 0.00001, x => 2 * x * x * x)}");
        TypeExtremum type = TypeExtremum.None;
        Console.WriteLine($"Aprox = {Optimization.Aprox(-5, 1, 5, 0.0001, x => x * x + x, ref type)} {type}");



        OptimumResult stepGrad = Optimization.stepGrad(new Vector(new double[] { 2, 3 }), 0.01, 0.001, x => (1 - x[0]) * (1 - x[0]) + 10 * (x[1] - x[0] * x[0]) * (x[1] - x[0] * x[0]));

        Console.WriteLine($"stepGrad={stepGrad}");


        OptimumResult randomSearch = Optimization.randomSearch(new Vector(new double[] { 2, 3 }), 1, 0.0001, x => (1 - x[0]) * (1 - x[0]) + 10 * (x[1] - x[0] * x[0]) * (x[1] - x[0] * x[0]));


        Console.WriteLine($"randomSearch={randomSearch}");

        OptimumResult fastestDescent = Optimization.fastestDescent(new Vector(new double[] { 2, 3 }), 0.01, 0.0001, x => (1 - x[0]) * (1 - x[0]) + 10 * (x[1] - x[0] * x[0]) * (x[1] - x[0] * x[0]));





        Console.WriteLine($"fastestDescent={fastestDescent}");


        OptimumResult conjugateDirections = Optimization.conjugateDirections(new Vector(new double[] { 2, 3 }), 0.01, 0.0001, x => (1 - x[0]) * (1 - x[0]) + 10 * (x[1] - x[0] * x[0]) * (x[1] - x[0] * x[0]));


        Console.WriteLine($"conjugateDirections={conjugateDirections}");


        //stepGrad.ToJson("C:\\Users\\Katana\\Desktop\\Текстовый документ.txt");

        OptimumResult randomSearchWithOgr = Optimization.randomSearchWithOgr(new Vector(new double[] { 2, 3 }), 1, 0.0001, x => (1 - x[0]) * (1 - x[0]) + 10 * (x[1] - x[0] * x[0]) * (x[1] - x[0] * x[0]), new Restriction[2] { new Restriction(100, -100, RestrictionTypes.DoubleOgr), new Restriction(100, -100, RestrictionTypes.DoubleOgr) });

        Console.WriteLine($"randomSearchWithOgr={randomSearchWithOgr}");



        Console.WriteLine("\nЛинейное ограничение");

        Matrix AA = new Matrix(new double[,] { { 2, 1 } });
        Vector BB = new Vector(new double[] { 2 });

        RestrictionLinear ogr_2 = new RestrictionLinear(AA, BB);

        OptimumResult randomSearchWithOgrLinear = Optimization.randomSearchWithOgrLinear(new Vector(new double[] { 1, 1 }), 0.1, 0.0001, x => x[0] * x[0] + x[1] * x[1], ogr_2);
        Console.WriteLine(randomSearchWithOgrLinear);



        Restriction[] ogr = new Restriction[2]; ogr[0] = new Restriction(2, -2, RestrictionTypes.DoubleOgr);
        RestrictionFunc[] f_ogr = new RestrictionFunc[1]; f_ogr[0] = new RestrictionFunc(-2, 2,
            RestrictionTypes.DoubleOgr, x => x[0] + x[1]);

        ogr[1] = new Restriction(-2, 2, RestrictionTypes.DoubleOgr);
        OptimumResult RandomFuncOgr = Optimization.RandomFuncOgr(new Vector(new double[] { 0.0, 0.0 }), 0.01, 0.1, x => (1 - x[0]) * (1 - x[0]) + 10 * (x[1] - x[0] * x[0]) * (x[1] - x[0] * x[0]), ogr, f_ogr);
        Console.WriteLine("RandomFuncOgr");
        Console.WriteLine(RandomFuncOgr);

        Console.WriteLine("\nНахождение минимума функции методом Нелдера-Мида");

        Vector xn2 = new Vector(new double[] { 0, 0 });

        OptimumResult result_6 = Optimization.NelderMid(xn2, 0.005, 0.003, x => x[0] * x[0] + x[0] * x[1] + x[1] * x[1] - 6 * x[0] - 9 * x[1]);
        Console.WriteLine(result_6);

        Console.WriteLine("\nЗадача ОЗУ");


        Vector xn = new Vector(new double[] { 1.5, -0.3 });
        Restriction[] xogr = new Restriction[2]; xogr[0] = new Restriction(1.75, 0, RestrictionTypes.DoubleOgr);
        xogr[1] = new Restriction(1.5, -0.5, RestrictionTypes.DoubleOgr);

        RestrictionFunc[] f2 = new RestrictionFunc[2]; f2[0] = new RestrictionFunc(1, 0, RestrictionTypes.UpperOgr,
            x => (x[0] - 1) * (x[0] - 1) + (x[1] - 1) * (x[1] - 1));
        f2[1] = new RestrictionFunc(2, 1, RestrictionTypes.DoubleOgr,
            x => 2 * x[0] + x[1]);

        RestrictionFunc[] ph = new RestrictionFunc[1]; ph[0] = new RestrictionFunc(4, 1, RestrictionTypes.DoubleOgr,
            x => x[0] * x[0] + x[1] * x[1]);


        Console.WriteLine("ph1 = x1 ^2 + x2 ^ 2, 1 <= f1 <= 4");
        Console.WriteLine("f1 = (x1 ^ 2 - 1) ^ 2 + (x2 ^ 2 - 1) ^ 2, f1 <= 1");
        Console.WriteLine("f2 = 2 * x1, 1 <= f2 <= 2");
        Console.WriteLine("xn = {1.5, 0.3}");
        Console.WriteLine("0 <= x1 <= 1.75");
        Console.WriteLine("-0.5 <= x2 <= 1.5");
        Console.WriteLine("h = 0.01, eps = 0.0001");
        Console.WriteLine(Optimization.OZU(xn, xogr, ph, f2, 0.01, 0.0001));
    }
    public static void trans()
    {
        Console.WriteLine();

        Vector a = new Vector(new double[] { 50, 60, 40 });
        Vector b = new Vector(new double[] { 30, 50, 25, 45 });
        Matrix c = new Matrix(new double[,] { { 5,4,3,2 }, { 2,7,3,6}, { 5,4,3,8 } });

        TransportTask transport = new TransportTask(a, b, c);

        Matrix plan = transport.northwestCorner(a, b);

        Console.WriteLine(plan);

        transport.findPotencial(c, plan);

        Console.WriteLine(transport.sumOfExpenses(c, plan));

        Console.WriteLine(transport.calcDelta(c));
    }

    public static void dinamyc()
    {

        Matrix Z1 = new Matrix(new double[,] { { 0.0, 0.0, 0.0, 0.0 },
                                              { 10.0, 9.0, 13.0, 12.0 },
                                              { 20.0, 21.0, 18.0, 21.0 },
                                              { 30.0, 30.0, 29.0, 28.0 },
                                              { 40.0, 41.0, 40.0, 41.0 } });

        Matrix Z = new Matrix(new double[,] { { 0.0, 0.0, 0.0, 0.0 }, 
                                              { 10.0, 12.0, 13.0, 11.0 }, 
                                              { 21.0, 20.0, 21.0, 22.0 }, 
                                              { 30.0, 26.0, 31.0, 32.0 }, 
                                              { 38.0, 36.0, 40.0, 39.0 } });
        Matrix Z2 = new Matrix(new double[,] { { 0.0, 0.0, 0.0, 0.0 },
                                               {12.0, 15.0, 11.0, 9.0},
                                               {21.0, 23.0, 20.0, 22.0 },
                                               {35.0, 32.0, 34.0, 36.0 },
                                               {45.0, 41.0, 44.0, 47.0 },
                                               {57.0, 55.0, 54.0, 56.0 }});



        dynamicProgrammingResources resourse = new dynamicProgrammingResources(Z2);

        Matrix[] answer = resourse.calculate();

        foreach(Matrix a in answer)
        {
            Console.WriteLine(a);
        }

        Console.WriteLine("Скобки");

        int[] P2 = new int[6] { 6, 8, 7, 5, 12, 4 };

        int[] P1 = new int[6] { 4,7,2,5,3,6 };

        int[] P= new int[7] {30,35,15,5,10,20,25};

        dynamicProgrammingScobki scobki = new dynamicProgrammingScobki(P2);

        Matrix[] answerScobki = scobki.calc();

        

        foreach (Matrix a in answerScobki)
        {
            Console.WriteLine(a);
        }

        Console.WriteLine(scobki.Matrix_Chain_Multiply(0, 4));

        Console.WriteLine("Рюкзак");

        /*
        Rykzak rykzak = new Rykzak();

        rykzak.add(new Item(6,2));
        rykzak.add(new Item(8, 4));
        rykzak.add(new Item(7, 5));
        rykzak.add(new Item(9, 3));
        rykzak.add(new Item(3, 2));

        rykzak.W = W;
        Console.WriteLine(rykzak.W);
        Console.WriteLine(rykzak.K);

        Console.WriteLine(rykzak.calc());
        */
        int W1 = 15;

        Item[] item = new Item[6] { new Item(10, 4), new Item(7, 3), new Item(5, 2), new Item(6, 4), new Item(4, 2), new Item(8, 3) };

        Rykzak rykzak = new Rykzak(item, W1);
        Console.WriteLine(rykzak.calc());

        rykzak.Approximate_Format();

        Console.WriteLine();

        rykzak.Print(6,15);
    }

    public static void game()
    {
        Matrix n = new Matrix(new double[4,4] { { 3, 1, 2, 5 },{ 2,0,0,3},{ -3,-5,-5,-2},{ 0,-2,-2,1} });

        gameTheory.clearStrategy(n);
    }
}