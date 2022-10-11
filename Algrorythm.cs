using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromaticNumber;

public class Algrorythm
{
    private int Size { get; init; }
    private int V { get; set; } = 0;
    private int[,] Matrix { get; init; }
    private Set<int> I { get; set; } = new();
    private Dictionary<Pair, Set<int>> D { get; set; } = new();
    private List<List<Set<int>>> W { get; set; } = new();
    private List<List<Set<int>>> F { get; set; } = new();
    private List<List<Set<Pair>>> G { get; set; } = new();
    private List<List<Set<Pair>>> Q { get; set; } = new();
    private List<List<Set<int>>> Alpha { get; set; } = new();
    private List<List<Set<int>>> Psi { get; set; } = new();
    private Set<int> B { get; set; } = new();
    private List<List<Set<Pair>>> Z { get; set; } = new();
    private List<List<int>> Ro { get; set; } = new();
    private List<Set<int>> J { get; set; } = new();
    private int j { get; set; } = 0;
    private int s { get; set; } = 0;
    public List<int> R { get; set; } = new();
    public Algrorythm(int[,] matrix)
    {
        Matrix = matrix;
        Size = matrix.GetLength(0);
        for (int i = 0; i < Size; i++)
        {
            I.Add(i);
        }
        FillD();
        foreach (var p in D)
        {
            Console.Write($"({p.Key.First + 1}, {p.Key.Second + 1}) -> ");
            for (int i = 0; i < p.Value.Count; i++)
            {
                Console.Write(p.Value[i] + 1 + " ");
            }
            Console.WriteLine();
            //var f = w[1][2];
        }
        V = Size;
        Console.WriteLine();

        var t = 0;
    }
    public void Start()
    {
        PointOne();
    }
    private void InitializeF()
    {
        if (F.Count <= j)
            F.Add(new());

        if (F[j].Count <= s)
            F[j].Add(new());
    }
    private void InitializeQ()
    {
        if (Q.Count == j)
            Q.Add(new());

        if (Q[j].Count == s)
            Q[j].Add(new());
    }
    private void CalculateW()
    {
        if (W.Count <= j)
            W.Add(new());

        if (W[j].Count <= s)
            W[j].Add(new());
        else return;

        W[j][s].Clear();

        var result = I;

        for (int i = 0; i < j; i++)
        {
            var temp = J[i];
            temp.Sort(0, temp.Count-1);
            result = Set<int>.Difference(result, temp);
        }

        W[j][s] = result;
    }
    private void NextSCalculateW()
    {
        W[j].Add(new());
        Pair pair = new(Alpha[j][s][0], Alpha[j][s][1]);

        Set<int> result = Set<int>.Difference(
            Set<int>.Intersection(W[j][s], D[pair]),
            Alpha[j][s]);
        W[j][s + 1] = result;
    }
    private void CalculateG()
    {
        if (G.Count <= j)
            G.Add(new());

        if (G[j].Count <= s)
            G[j].Add(new());

        G[j][s].Clear();

        foreach (var d in D)
        {
            Set<int> set = new();
            set.Add(d.Key.First);
            set.Add(d.Key.Second);
            var t = Set<int>.Intersection(set, W[j][s]);
            if (t.Count == 2)
            {
                var setTwo = Set<int>.Intersection(W[j][s], d.Value);
                if (setTwo.Count > 0)
                {
                    Pair pair = new(d.Key.First, d.Key.Second, setTwo.Count);
                    G[j][s].Add(pair);
                }
            }
        }

        G[j][s].Sort(0, G[j][s].Count - 1);
    }
    private void CalculateAlpha(Pair pair)
    {
        if (Alpha.Count <= j)
            Alpha.Add(new());

        if (Alpha[j].Count <= s)
            Alpha[j].Add(new());

        Alpha[j][s].Add(pair.First);
        Alpha[j][s].Add(pair.Second);
    }
    private void CalculatePsi(int r)
    {
        while (Psi.Count <= j)
            Psi.Add(new());

        while (Psi[j].Count <= r)
            Psi[j].Add(new());

        Set<int> set = new();
        for (int i = 0; i < r; i++)
        {
            set = Set<int>.Union(set, Alpha[j][i]);
        }

        set.Sort(0, set.Count - 1);

        Psi[j][r] = Set<int>.Difference(J[j], set);
        Psi[j][r].Sort(0, Psi[j][r].Count - 1);
    }
    private void CalculateZ(int r)
    {
        while (Z.Count <= j)
            Z.Add(new());

        while (Z[j].Count <= r)
            Z[j].Add(new());
        var t = Set<Pair>.Difference(G[j][r], Q[j][r]);

        for (int i = 0; i < t.Count; i++)
            t[i] = new(t[i].First, t[i].Second);

        t.Sort(0, t.Count - 1);

        var temp = Set<Pair>
            .Intersection(t, GeneratePairSet(r));

        if (r != 0)
            Z[j][r] = temp;

        else
        {
            Set<Pair> result = new();

            for (int i = 0; i < temp.Count; i++)
                if (Set<int>.Difference(Psi[j][r], D[temp[i]]).Count == 0
                    && Set<int>.Difference(D[temp[i]], Psi[j][r]).Count == 0)
                    result.Add(temp[i]);


            Z[j][r] = result;
        }
    }
    private void CalculateB()
    {
        B = Set<int>.Union(B, Psi[0][0]);
    }
    private void CalculateRo()
    {
        if (Ro.Count <= j)
            Ro.Add(new());

        Set<Pair> set = Set<Pair>.Difference(G[0][0], Q[0][0]);
        if (Ro[0].Count <= s)
            if (set.Count > 0)
                Ro[0].Add(set[0].Power);
            else Ro[0].Add(1);
    }
    private bool CheckA()
    {
        if (j + (double)W[j][0].Count / Ro[j][0] >= V)
            return false;

        return true;
    }
    private bool CheckB()
    {
        if (2 * s + Ro[0][s] < D.Count / V)
            return true;

        return false;
    }
    private bool CheckC()
    {
        if (2 * s + Ro[j][s] == W[j][0].Count)
            return true;

        return false;
    }

    private Set<Pair> GeneratePairSet(int r)
    {
        Set<Pair> result = new();


        for (int i = 0; i < Psi[j][r].Count - 1; i++)
        {
            for (int k = i + 1; k < Psi[j][r].Count; k++)
            {
                result.Add(new(Psi[j][r][i], Psi[j][r][k]));
            }
        }
        result.Sort(0, result.Count - 1);
        return result;
    }
    private void PointOne()
    {
        CalculateW();
        InitializeF();
        PointTwo();
    }
    private void PointTwo()
    {
        if (W[j][s].Count == 0)
        {
            if (s == 0) PointEight();
            else PointFive(true);
        }
        else PointThree();
    }
    private void PointThree()
    {
        CalculateG();
        InitializeQ();

        PointFour();
    }
    private void PointFour()
    {
        Set<Pair> set = Set<Pair>.Difference(
            G[j][s],
            Q[j][s]);

        if (set.Count > 0)
        {
            CalculateAlpha(set[0]);
            Q[j][s] = Set<Pair>.Union(Q[j][s], set[0]);
            NextSCalculateW();
            s++;
            PointTwo();
        }
        else
        {
            var beta = Set<int>.Difference(W[j][s], F[j][s]);

            if (beta.Count > 0)
            {
                F[j][s] = Set<int>.Union(F[j][s], beta[0]);
                PointFive(false);
            }
            else
            {
                s--;
                PointFour();
            }
        }
    }
    private void PointFive(bool isEven)
    {
        if (R.Count <= j)
            R.Add(s);

        if (J.Count <= j)
            J.Add(new());

        for (int i = 0; i < s; i++)
        {
            J[j].Add(Alpha[j][i][0]);
            J[j].Add(Alpha[j][i][1]);
        }

        if (!isEven)
        {
            var beta = Set<int>.Difference(W[j][s], F[j][s]);
            J[j] = Set<int>.Union(J[j], beta[0]);
        }
        else R[j] = s - 1;

        PointSix();
    }
    private void PointSix()
    {
        var tempR = R[j];
        while (tempR >= 0)
        {
            CalculatePsi(tempR);
            CalculateZ(tempR);

            Q[j][tempR] = Set<Pair>.Union(Q[j][tempR], Z[j][tempR]);

            tempR--;
        }

        CalculateB();

        PointSeven();
    }
    private void PointSeven()
    {
        Q[j][0] = new();
        F[j][0] = new();

        j++;
        s = 0;

        PointOne();
    }
    private void PointEight()
    {
        V = V < J.Count ? V : J.Count;
        CalculateRo();
        var g = W[0][0].Count % Ro[0][0];
        int y = W[0][0].Count / Ro[0][0];
        if (g == 0 && y == V || g != 0 && y == V - 1)
        {
            Console.WriteLine(V);
            return; 
        }
        else {  s = R[--j]; PointFour(); }
    }


    private Set<int> H(int q)
    {
        Set<int> result = new();

        for (int r = 0; r < Size; r++)
        {
            if (Matrix[q, r] == 0) result.Add(r);
        }

        return result;
    }

    private Set<int> U(int r)
    {
        Set<int> result = new();

        for (int q = 0; q < Size; q++)
        {
            if (Matrix[q, r] == 0) result.Add(q);
        }

        return result;
    }

    private void FillD()
    {
        for (int i = 0; i < Size - 1; i++)
        {
            for (int j = i + 1; j < Size; j++)
            {

                var set = Set<int>.Intersection(
                    Set<int>.Intersection(H(i), H(j)),
                    Set<int>.Intersection(U(i), U(j)));

                var count = set.Count;

                if (set != null
                    && set.Find(i, 0, count - 1) >= 0
                    && set.Find(j, 0, count - 1) >= 0)
                {

                    D.Add(
                        new Pair(i, j),
                        set);
                }
            }
        }
    }
}
