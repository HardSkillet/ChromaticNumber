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

    public static void Intersection(this SetInt a, SetInt b)
    {
        a = SetInt.Intersection(a, b);
    }

    public static void Difference(this SetInt a, SetInt b)
    { 
        a = SetInt.Difference(a, b);
    }

    public static void Union(this SetInt a, SetInt b)
    {
        a = SetInt.Union(a, b);
    }

    public static void Union(this SetInt a, int b)
    {
        a = SetInt.Union(a, b);
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
