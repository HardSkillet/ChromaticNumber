using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Math;

namespace ChromaticNumber;

public class NewAlgorithm
{
    private int[,] A { get; init; }
    private SetInt I { get; set; } = new();
    private List<SetInt> H { get; set; } = new();
    private List<SetInt> U { get; set; } = new();
    private List<List<Dictionary<Pair, SetInt>>> D { get; set; } = new();
    private List<SetInt> JBest { get; set; } = new();
    private List<List<SetInt>> W { get; set; } = new();

    private List<List<SetInt>> F { get; set; } = new();

    private List<List<Set<Pair>>> G { get; set; } = new();

    private List<List<Set<Pair>>> Q { get; set; } = new();

    private List<List<Pair>> Alpha { get; set; } = new();

    private List<List<SetInt>> Psi { get; set; } = new();

    private List<SetInt> B { get; set; } = new();

    private List<SetInt> J { get; set; } = new();
    private Set<Pair> P { get; set; } = new();
    public int UBest { get; set; }

    private int s { get; set; }
    private int j { get; set; }
    private int Ocenka { get; set; }

    public NewAlgorithm(int[,] a, int n, int ocenka)
    {
        A= a;
        for (int i = 0; i < n; i++)
            I.Add(i);
        Ocenka = ocenka;
        Size = n;
    }

    private void Choise(int num) 
    {
        while (num  != -1)
        {
            switch (num)
            {
                case 1:
                    num = p1();
                    break;
                case 2:
                    num = p2();
                    break;
                case 3:
                    num = p3();
                    break;
                case 4:
                    num = p4();
                    break;
                case 5:
                    num = p5();
                    break;
                case 6:
                    num = p6();
                    break;
                case 7:
                    num = p7();
                    break;
                case 8:
                    num = p8();
                    break;
                default:
                    return;
            }
        }
    }
    private void UpdateD(SetInt nodes)
    {
        if (D.Count <= j)
            D.Add(new());

        if (D[j].Count <= s)
            D[j].Add(new());

        D[j][s].Clear();
        foreach (var p in P)
        {
            var i = p.First;
            var j1 = p.Second;
            var temp = SetInt.Intersection(H[i], H[j1]);
            var temp2 = SetInt.Intersection(U[i], U[j1]);
            var temp3 = SetInt.Intersection(temp, temp2);
            var temp4 = SetInt.Intersection(temp3, nodes);

            if (temp4.Contains(i) && temp4.Contains(j1))
            {
                var key = new Pair(i, j1);
                D[j][s].Add(key, temp4);
            }
        }

        //Console.WriteLine();
        //Console.WriteLine($"D[{j+1}][{s+1}]");
        //int k = 1;
        //foreach (var d in D[j][s])
        //{
        //    Console.Write($"{k++}. ({d.Key.First+1}, {d.Key.Second+1}):");
        //    foreach (var t in d.Value)
        //    {
        //        Console.Write(t + 1+ " ");
        //    }
        //    Console.WriteLine();
        //}
    }
    private Pair FindMax(Set<Pair> pairs)
    {
        var max = 0;
        var pair = new Pair(0, 0);
        foreach (var current in pairs)
        {
            if (D[j][s][current].Count > max)
            {
                max = D[j][s][current].Count;
                pair = current;
            }
        }
        return pair;
    }


    public void Start()
    {
        s = 0;
        j = 0;

        foreach (var q in I)
        {
            H.Add(new());
            U.Add(new());
            foreach (var r in I)
            {
                if (A[q, r] == 0)
                    H[q].Add(r);
                
                if (A[r, q] == 0)
                    U[q].Add(r);
            }
        }

        //foreach (var t in H)
        //{
        //    Console.WriteLine();
        //    foreach (var t1 in t)
        //        Console.Write(t1 + " ");
        //}

        //foreach (var t in U)
        //{
        //    Console.WriteLine();
        //    foreach (var t1 in t)
        //        Console.Write(t1 + " ");
        //}

        for (int i = 0; i < Size; i++)
        {
            for (int j = i+1; j < Size; j++)
            {
                var temp = SetInt.Intersection(H[i], H[j]);
                var temp2 = SetInt.Intersection(U[i], U[j]);
                var temp3 = SetInt.Intersection(temp, temp2);
                var t1 = temp3.Contains(i);
                var t2 = temp3.Contains(j);
                if (temp3.Contains(i) && temp3.Contains(j))
                {
                    P.Add(new(i, j));
                }
            }
        }

        foreach (var q in I)
        {
            var temp = new SetInt();
            temp.Add(q);
            JBest.Add(temp);
            //J.Add(temp);
        }
        UBest = I.Count;

        if (Q.Count <= j)
            Q.Add(new());

        if (Q[j].Count <= s)
            Q[j].Add(new());

        if (G.Count <= j)
            G.Add(new());

        if (G[j].Count <= s)
            G[j].Add(new());

        if (F.Count <= j)
            F.Add(new());

        if (F[j].Count <= s)
            F[j].Add(new());

        Choise(1);
    }

    private int p1()
    {
        if (W.Count <= j)
            W.Add(new());
        if (W[j].Count <= s)
            W[j].Add(new());


        var temp = I;
        for (int i = 0; i < j; i++)
        {
            temp = SetInt.Difference(temp, J[i]);
        }
        W[j][s] = temp;

        UpdateD(temp);

        return 2;
        //p2();
    }

    private int p2()
    {
        if (W[j][s].Count <= 0)
            if (s == 0)
            {
                //p8();
                return 8;
            }
            else 
            { 
                //p5();
                return 5;
            }
        return 3;
        //p3();
    }

    private int p3()
    {
        if (G.Count <= j)
            G.Add(new());

        if (G[j].Count <= s)
            G[j].Add(new());

        if (Q.Count <= j)
            Q.Add(new());

        if (Q[j].Count <= s)
            Q[j].Add(new());
        
        G[j][s].Clear();
        foreach (var key in P)
        {
            var first = key.First;
            var second = key.Second;

            if (W[j][s].Contains(first) && W[j][s].Contains(second))
                G[j][s] = Set<Pair>.Union(G[j][s], key);
        }
        //p4();
        return 4;
    }

    private int p4()
    {
        var temp = Set<Pair>.Difference(G[j][s], Q[j][s]);
        var d = D[j][s];
        if (temp.Count > 0)
        {
            if (Alpha.Count <= j)
                Alpha.Add(new());

            if (Alpha[j].Count <= s)
                Alpha[j].Add(new());

            
            Alpha[j][s] = FindMax(temp);
            //Console.WriteLine($"A[{j+1}][{s+1}]: {Alpha[j][s].First + 1}, {Alpha[j][s].Second + 1}");

            var ro = SetInt.Intersection(d[Alpha[j][s]], W[j][s]).Count;

            //Check A
            if (s == 0 && j != 0)
            {
                var k = j + Floor((double)W[j][0].Count/ro);
                if (k >= UBest)
                {
                    j--;
                    s = J[j].Count/2 -1;
                    if (J[j].Count % 2 > 0 || s == -1)
                        s++;
                    //p4();
                    return 4;
                }
            }

            //Check B
            if (j == 0 && s != 0)
            {
                var k = 2*s + ro;
                if (k <= Size/(double)UBest)
                {
                    s--;
                    //p4();
                    return 4;
                }
            }
            //Check C
            if (j == UBest - 2 && j != 0)
            {
                var k = 2*s + ro;
                if (k != W[j][0].Count)
                {
                    j--;
                    s = J[j].Count/2 -1;
                    if (J[j].Count % 2 > 0 || s == -1)
                        s++;
                    //p4();
                    return 4;
                }
            }


            Q[j][s] = Set<Pair>.Union(Q[j][s], Alpha[j][s]);

            if (W[j].Count <= s + 1)
                W[j].Add(new());

            var tmp = new SetInt();
            if (!d.ContainsKey(Alpha[j][s]) == false)
                tmp = SetInt.Intersection(d[Alpha[j][s]], W[j][s]);
            W[j][s+1] = SetInt.Difference(tmp, Alpha[j][s]);          

            s++;
            UpdateD(W[j][s]);

            if (Q.Count <= j)
                Q.Add(new());

            if (Q[j].Count <= s)
                Q[j].Add(new());
            Q[j][s].Clear();

            while (F.Count <= j)
                F.Add(new());

            while (F[j].Count <= s)
                F[j].Add(new());
            F[j][s].Clear();

            //p2();
            return 2;
        }
        else
        {
            var tmp = SetInt.Difference(W[j][s], F[j][s]);
            if (tmp.Count > 0)
            {
                LastBeta = tmp.First();
                
                var ro = 1;

                //Check A
                if (s == 0 && j != 0)
                {
                    var k = j + Floor((double)W[j][0].Count/ro);
                    if (k >= UBest)
                    {
                        j--;
                        s = J[j].Count/2 -1;
                        if (J[j].Count % 2 > 0 || s == -1)
                            s++;
                        //p4();
                        return 4;
                    }
                }

                //Check B
                if (j == 0 && s != 0)
                {
                    var k = 2*s + ro;
                    if (k <= Size/(double)UBest)
                    {
                        s--;
                        //p4();
                        return 4;
                    }
                }
                //Check C
                if (j == UBest - 2 && j != 0)
                {
                    var k = 2*s + ro;
                    if (k != W[j][0].Count)
                    {
                        j--;
                        s = J[j].Count/2 -1;
                        if (J[j].Count % 2 > 0 || s == -1)
                            s++;
                        //p4();
                        return 4;
                    }
                }

                F[j][s] = SetInt.Union(F[j][s], LastBeta);
                //p5();
                return 5;
            }
            else {
                if (s == 0 && j == 0)
                {
                    return -1;
                }
                if (s == 0)
                    j--;
                else 
                    s--;
                if (j < 0)
                    return -1;
                //p4();
                return 4;
            }
        }

    }

    private int p5() 
    {
        while (J.Count <= j)
            J.Add(new());

        J[j].Clear();
        for(int i=0; i < s; i++)
        {
            J[j] = SetInt.Union(J[j], Alpha[j][i]);
        }
        if (W[j][s].Count > 0)
        {
            J[j] = SetInt.Union(J[j], LastBeta);
        }

        //p6();
        return 6;
    }

    private int p6()
    {
        var k = J[j].Count/2 -1;
        if (J[j].Count % 2 == 1 || k == -1)
            k++;
        for (int ss = 0; ss <= k; ss++)
        { 
            while (Psi.Count <= j)
                Psi.Add(new());

            while (Psi[j].Count <= ss)
                Psi[j].Add(new());

            Psi[j][ss] = J[j];

            for (int i = 0; i < ss; i++)
            {
                Psi[j][ss] = SetInt.Difference(Psi[j][ss], Alpha[j][i]);
            }
            var temp = Set<Pair>.Difference(G[j][ss], Q[j][ss]);
            var Z = new Set<Pair>();

            foreach(var t in temp)
            {
                var count = Psi[j][ss].Count;
                var tmp = SetInt.Difference(Psi[j][ss], t);
                var temp2 = SetInt.Difference(D[j][ss][t], Psi[j][ss]).Count;
                if (count - 2== tmp.Count && temp2 == 0)
                {
                    Z = Set<Pair>.Union(Z, t);
                }
            }

            Q[j][ss] = Set<Pair>.Union(Q[j][ss], Z);

        }
        if (j == 0 && s != 0)
        {
            foreach (var b in B)
            {
                var tmp = SetInt.Intersection(b, W[0][0]);
                if (tmp.Count == W[0][0].Count && tmp.Count == b.Count)
                { 
                    s = J[j].Count/2 -1;
                    if (J[j].Count % 2 > 0 || s == -1)
                        s++;
                    //p3();
                    return 3;
                }
            }
            B.Add(Psi[0][0]);
        }

        //p7();
        return 7;
    }

    private int p7() 
    {
        j++;
        s = 0;
        while (G.Count <= j)
            G.Add(new());

        while (F.Count <= j)
            F.Add(new());

        if (G[j].Count <= s)
            G[j].Add(new());
        G[j][0].Clear();

        if (F[j].Count <= s)
            F[j].Add(new());
        F[j][0].Clear();

        while (Q.Count <= j)
            Q.Add(new());

        if(Q[j].Count <= s)
            Q[j].Add(new());
        Q[j][0].Clear();

        //p1();
        return 1;
    }

    private int p8()
    {
        var res = new SetInt();
        for (int i = 0; i < j; i++)
        {
            res = SetInt.Union(res, J[i]);
        }

        if (W[j][0].Count == 0 && res.Count == Size)
        {
            if (j < UBest)
            {
                JBest= J;
                UBest = j;
            }

            var temp = Set<Pair>.Difference(G[0][0], Q[0][0]);
            int ro = 0;
            if (temp.Count == 0)
                ro = 1;
            else ro = SetInt
                    .Intersection(
                        D[0][0][Alpha[0][0]], 
                        W[0][0])
                    .Count;

            var cel = W[0][0].Count/ro;
            var ost = W[0][0].Count%ro;
            if (ost > 0)
                cel++;
            if (cel == UBest)
                return -1;

            if (UBest == Ocenka)
                return -1;

            j--;
            s = J[j].Count/2 -1;
            if (J[j].Count % 2 > 0 || s == -1)
                s++;
            //p4();
            return 4;
        }
        return -1;
    }

    private int Size { get; init; }

    private int LastBeta = new();

}
