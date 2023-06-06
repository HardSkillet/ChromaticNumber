using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChromaticNumber;


public class NewPair
{
    public int Count { get; set; } = 0;
    public double SumNovikov { get; set; } = 0;
    public double SumOlemskoy { get; set; } = 0;
}
public static class Test
{
    public static int Count = 0;
    public static Dictionary<double, NewPair> Results { get; set; } = new();
    static Test() 
    {
        for (int i = 0; i < 10; i++)
        {
            var t = (double)i/10;
            Results.Add(t, new());
        }
    }


    public static void Run(int[,] a, int n, bool isWrite = false) 
    {
        var g = new Graph(a, n);
        var m = 0;
        foreach (var r in g.AdjList)
        {
            m += r.Count;
        }
        m/=2;
        var oc = (double)n*n/(n*n - 2 * m);
        int ocenka = (int)Math.Ceiling(oc);

        var nodes = new SetInt();
        var cBest = new List<SetInt>();
        for (int i = 0; i < n; i++)
        {
            nodes.Add(i);
        }

        //Thread.Sleep(10000);
        // Get the elapsed time as a TimeSpan value.
        var p = (double)2*m / (n * (n-1));
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        if (isWrite)
        {
            Console.WriteLine("____________________________________________________________");
            Console.WriteLine("Размер " + n + $" Плотность Графа: {p}");
        }
        var v = new NewAlgorithm(a, n, ocenka);
        v.Start();

        stopWatch.Stop();

        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        if (isWrite)
        {
            Console.WriteLine($"Result:{v.UBest}, RunTime " + elapsedTime);
        }
        var res1 = v.UBest;
        Stopwatch stopWatch1 = new Stopwatch();
        stopWatch1.Start();

        var an = new AlgorithmNovikova(g, n, ocenka);
        an.Start(nodes, new());
        
        var res = an.Best;
        
        
        //Console.WriteLine($"Res:{res}");
        stopWatch1.Stop();
        TimeSpan ts1 = stopWatch1.Elapsed;
        elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts1.Hours, ts1.Minutes, ts1.Seconds,
            ts1.Milliseconds / 10);
        if (isWrite)
        {
            Console.WriteLine($"Result:{res}, RunTime " + elapsedTime);
        }
        if (res != res1)
            Console.WriteLine("WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!WARNING!!!!!!");
        var t = Math.Round(p*10) / 10;
        if (t == 1)
        {
            t = 0.9;
        }
        Results[t].SumOlemskoy += ts.Seconds + (double)ts.Milliseconds/1000;
        Results[t].SumNovikov += ts1.Seconds + (double)ts1.Milliseconds/1000;
        Results[t].Count++;
        Console.WriteLine(Count++);
    }
    public static void Check(int[,] a, int n)
    {
        var r = new Random();
        var road = new SetInt();
        var g = new Graph(a, n);
        var visited = new Dictionary<int, bool>();
        for (int i = 0; i < n; i++)
        { 
            visited.Add(i, false);
        }
        var stack = new Stack<int>();
        var res = new List<int>();
        int current = 0;
        res.Add(current);
        while (res.Count < n)
        {
            visited[current]= true;
            foreach (var t in g.AdjList[current])
            {
                if (!visited[t])
                { 
                    stack.Push(t);
                    visited[t] = true;
                }
            }
            //visited[current] = true;
            if (stack.Count > 0)
                current = stack.Pop();
            else 
            {
                var random = r.Next(0, res.Count-1);
                foreach (var d in visited)
                    if (!d.Value)
                    { 
                        current = d.Key;
                        var temp = res[random];
                        a[temp, current] = 1;
                        break;
                    }
            }
            res.Add(current);
        }
    }
    public static void TrA(int[,] a, int n)
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                if (a[i,j] != a[j,i])
                    a[i,j] = a[j,i] =1;
            }
    }
}

public class Graph
{
    // Список списков для представления списка смежности
    public List<SetInt> AdjList { get; set; } = new();

    // Конструктор
    public Graph(int[,] matrix, int n)
    {
        AdjList = new();
        for (int i = 0; i < n; i++)
        {
            AdjList.Add(new SetInt());
        }

        //for (int i = 0; i < n; i++)
        //{
        //    for (int j = 0; j < n; j++)
        //        Console.Write(matrix[i, j] + " ");
        //    Console.WriteLine();
        //}

        //Console.WriteLine();

        for (int i = 0; i < n; i++)
            for (int j = 0; j <= i; j++)
                if (matrix[i, j] == 1)
                    matrix[j, i] = 1;

        for (int i = 0; i < n; i++)
            for (int j = i+1; j < n; j++)
            {
                if (matrix[i, j] == 1)
                { 
                    AdjList[i].Add(j);
                    AdjList[j].Add(i); 
                }
            }
        //foreach (var t in AdjList)
        //    if (t.Count > 0)
        //        t.Sort(0, t.Count-1);

        //int count = 1;
        //foreach (var t in AdjList)
        //{
        //    Console.Write($"{count++} : ");
        //    for (int i = 0; i < t.Count; i++)
        //    {
        //        Console.Write(t[i]+1 + " ");
        //    }
        //    Console.WriteLine();
        //}

        //for (int i = 0; i < n; i++)
        //{
        //    for (int j = 0; j < n; j++)
        //        Console.Write(matrix[i, j] + " ");
        //    Console.WriteLine();
        //}
    }
}