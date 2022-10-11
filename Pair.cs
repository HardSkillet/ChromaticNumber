using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromaticNumber;

public struct Pair : IComparable<Pair>
{
    public int First { get; set; }
    public int Second { get; set; }
    public int Power { get; set; }
    public Pair(int first, int second) {
        First = first; 
        Second = second; 
        Power = 0;
    }
    public Pair(int first, int second, int power) : this(first, second)
    {
        Power = power;
    }

    public int CompareTo(Pair other)
    {
        if (other.Power < Power) return -1;
        if (other.Power > Power) return 1;

        if (other.First < First) return 1;
        if (other.First > First) return -1;

        if (other.Second < Second) return 1;
        if (other.Second > Second) return -1;

        else return 0;
    }
}
