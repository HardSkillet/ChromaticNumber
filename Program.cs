// See https://aka.ms/new-console-template for more information
using ChromaticNumber;

var plotnost = 0.45;
var n = 16;
for (int y = 0; y < 12; y++)
{
    plotnost+= 0.05;
    for (int op = 0; op < 50; op++)
    {
        //Console.WriteLine("Введите размер матрицы");
        //var n1 = Console.ReadLine();
        //var n = Convert.ToInt32(n1);

        int[,] a = new int[n, n];
        //for (int i = 0; i < n; i++)
        //{
        //    for (int j = 0; j < n; j++)
        //    {
        //        if (i % 2 == j % 2)
        //        {
        //            a[i, j] = 0;

        //        }
        //        else a[i, j] = 1;
        //    }
        //}


        var r1 = new Random();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j< n; j++)
            {
                if (i == j)
                    continue;

                var temp = r1.NextDouble();
                if (temp < plotnost)
                    a[i, j] = 1;
            }
        }
        //for (int i = 0; i < n; i++)
        //{
        //    for (int j = 0; j < n; j++)
        //    {
        //        Console.Write(a[i, j] + " ");
        //    }
        //    Console.WriteLine();
        //}
        //Console.WriteLine();
        Test.TrA(a, n);
        //for (int i = 0; i < n; i++)
        //{
        //    for (int j = 0; j < n; j++)
        //    {
        //        Console.Write(a[i, j] + " ");
        //    }
        //    Console.WriteLine();
        //}
        //Console.WriteLine();
        Test.Check(a, n);
        //a = new int[7, 7] {
        //    { 0, 0, 0, 0, 1, 0, 1},
        //    { 0, 0, 1, 1, 1, 0, 0},
        //    { 0, 1, 0, 1, 0, 0, 1},
        //    { 0, 1, 1, 0, 0, 1, 1},
        //    { 1, 1, 0, 0, 0, 1, 1},
        //    { 0, 0, 0, 1, 1, 0, 0},
        //    { 1, 0, 1, 1, 1, 0, 0}
        //};
        //a = new int[,]{
        //    { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, },
        //    { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, },
        //    { 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, },
        //    { 1, 0, 1, 0, 0, 0, 1, 1, 0, 0, },
        //    { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, },
        //    { 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, },
        //    { 0, 1, 0, 0, 1, 1, 0, 0, 0, 1, },
        //    { 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, },
        //    { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, },
        //    { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, }
        //};
        ////a = new int[,]
        ////{
        ////    { 0, 0, 0, 0, 1, 1},
        ////    { 0, 0, 1, 1, 0, 0},
        ////    { 0, 1, 0, 1, 0, 1},
        ////    { 0, 1, 1, 0, 1, 1},
        ////    { 1, 0, 0, 1, 0, 1},
        ////    { 1, 0, 1, 1, 1, 0},
        ////};
        //n = 10;

        //var algh = new NewAlgorithm(a, n);
        //algh.Start();
        //var res2 = algh.UBest;
        //Console.WriteLine("ANSWER!!! - " + res2);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(a[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Test.Run(a, n, true);
        //for (int i = 0; i < n; i++)
        //{
        //    for (int j = 0; j < n; j++)
        //    {
        //        Console.Write(a[i, j] + " ");
        //    }
        //    Console.WriteLine();
        //}
        //for (int i = 0; i < n; i++)
        //{
        //    Console.WriteLine($"Введите {i+1} строку матрицы через пробел");
        //    n1 = Console.ReadLine();
        //    var t = n1.Split(' ');
        //    int j = 0;
        //    foreach (var t1 in t)
        //        a[i, j++] = Convert.ToInt32(t1);
        //}
        //int[,] a = {
        //    { 0, 1},
        //    { 1, 0}
        //};
        //int[,] a = {
        //    { 0, 0, 0, 1 },
        //    { 0, 0, 1, 0},
        //    { 0, 1, 0, 0},
        //    { 1, 0, 0, 0}
        //};

    }
}
//int kl = 0;

//var test = new Test(a, 10);
//var s = test.Start();
//for (int q1 = 0; q1 < s.Count; q1++)
//{ Console.Write(s[q1] + " "); }
//Console.WriteLine();
//var t = new Graph(a, 10);
//int[,] a = new int[10,10];


//List<List<int>> h = new();
//for (int i = 0; i < 10; i++) 
//{    
//    h.Add(new List<int>());

//    for (int j = 0; j < 10; j++) 
//    {
//        if (a[i, j] == 0) 
//            h[i].Add(j);
//    }
//}

//List<List<int>> u = new();

//for (int j = 0; j < 10; j++)
//{
//    u.Add(new List<int>());
//    for (int i = 0; i < 10; i++)
//    {
//        if (a[i, j] == 0) u[j].Add(i);
//    }
//}

//foreach (var l in h)
//{
//    foreach (var e in l)
//    {
//        Console.Write(e + " ");
//    }
//    Console.WriteLine("\n");
//}
//Console.WriteLine("\n");
//foreach (var l in u)
//{
//    foreach (var e in l)
//    {
//        Console.Write(e + " ");
//    }
//    Console.WriteLine("\n");
//}


//Dictionary<(int, int),List<int>> d = new();
//for (int i = 0; i < 10; i++) {
//    List<int> d1 = new(); 
//    for (int j = i + 1; j < 10; j++) 
//    {
//        d1 = Intersection(Intersection(h[i], u[j]), Intersection(h[j], u[i]));
//        if (d1.Contains(i) && d1.Contains(j)) d.Add((i+1, j+1), d1);
//    }
//}

//Dictionary<(int, int), int> g = new();
//foreach (var kvp in d) {
//    g.Add(kvp.Key, kvp.Value.Count);
//}


//foreach (var l in d) {
//    Console.Write($"({l.Key.Item1}, {l.Key.Item2}) |{l.Value.Count}| -> ");
//    foreach (var e in l.Value) {
//        Console.Write(e + 1 + " ");
//    }
//    Console.WriteLine();
//}
System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
Console.WriteLine(n);
foreach (var t in Test.Results)
{
    if (t.Value.Count == 0)
        continue;
    Console.Write(t.Key + "\t");
}
Console.WriteLine();
foreach (var t in Test.Results)
{
    if (t.Value.Count == 0)
        continue;
    Console.Write(t.Value.Count + "\t");
}
Console.WriteLine("Olemskoy");
Console.Write("y = [");
foreach (var t in Test.Results)
{
    if (t.Value.Count == 0)
        continue;
    Console.Write(t.Value.SumOlemskoy / t.Value.Count + ", ");
}
Console.WriteLine("]");
//Console.WriteLine("Novikov");
Console.Write("z = [");
foreach (var t in Test.Results)
{
    if (t.Value.Count == 0)
        continue;
    Console.Write(t.Value.SumNovikov / t.Value.Count + ", ");
}
Console.WriteLine("]");
Console.ReadKey();
