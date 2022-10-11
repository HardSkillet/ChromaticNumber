using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromaticNumber;

public static class Extension
{
    public static void Intersection(this Set<Pair> a, Set<Pair> b)
    {
        a = Set<Pair>.Intersection(a, b);
    }
    public static void Union(this Set<int> a, Set<int> b)
    {
        a = Set<int>.Union(a, b);
    }

    public static void Union(this Set<int> a, int b)
    {
        a = Set<int>.Union(a, b);
    }

    public static void Union(this Set<Pair> a, Set<Pair> b)
    {
        a = Set<Pair>.Union(a, b);
    }

    public static void Union(this Set<Pair> a, Pair b)
    {
        a = Set<Pair>.Union(a, b);
    }
}
