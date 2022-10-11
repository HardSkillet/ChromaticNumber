using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChromaticNumber;

public class Set<T> 
    where T : IComparable<T>
{
    private const int n = 10;
    public int Count { get; private set; } = 0;
    public T[] Array { get; private set; } = new T[n];
    public int Capacity { get; private set; } = 10;

    public void Add(T number)
    {
        if (Count == Capacity)
            IncreaseCapacity();

        Array[Count++] = number;
    }
    public T this[int index]
    {
        get
        {
            if (Count == 0) return default(T);
            if (index >= 0 && index < Count)
                return Array[index];

            throw new Exception();
        }
        set {
            Array[index] = value;
        }
    }
    public void IncreaseCapacity()
    {
        Capacity *= 2;

        T[] temp = new T[Capacity];

        for (int i = 0; i < Count; i++)
        {
            temp[i] = Array[i];
        }

        Array = temp;
    }

    public int Find(T find, int start, int end)
    {
        if (Count == 0)
            return -1;

        int chunkSize = 1 + (end - start);

        if (chunkSize == 0)
            return -1;

        int midpoint = start + (chunkSize / 2);

        if (Array[midpoint].CompareTo(find) == 0)
            return midpoint;

        else if (Array[midpoint].CompareTo(find) > 0)
            return Find(find, start, midpoint - 1);

        else
            return Find(find, midpoint + 1, end);
    }
    public void Clear() {
        Count = 0;
        Array = new T[n];
    }
    public static Set<T> Intersection(Set<T> a, Set<T> b)
    {
        int i = 0;
        int j = 0;
        Set<T> result = new();
        while (i < a.Count && j < b.Count)
        {
            if (a[i].CompareTo(b[j]) == 0)
            {
                result.Add(a[i]);
                i++;
                j++;
                continue;
            }

            if (a[i].CompareTo(b[j]) < 0)
            {
                i++;
                continue;
            }
            
            j++;
        }

        return result;
    }
    public static Set<T> Union(Set<T> a, Set<T> b)
    {
        var i = 0;
        var j = 0;
        Set<T> result = new();

        while (i < a.Count && j < b.Count)
        {
            if (a[i].CompareTo(b[j]) == 0)
            {
                result.Add(a[i]);
                i++;
                j++;
                continue;
            }

            if (a[i].CompareTo(b[j]) < 0)
            {
                result.Add(a[i++]);
                continue;
            }

            result.Add(b[j++]);

        }

        while (i < a.Count)
            result.Add(a[i++]);

        while (j < b.Count)
            result.Add(b[j++]);

        return result;
    }
    public static Set<T> Union(Set<T> a, T b) 
    {
        if (a.Find(b, 0, a.Count) > -1) {
            return a;
        }

        int i = 0;
        var result = new Set<T>();
        
        while (i < a.Count && a[i].CompareTo(b) < 0) 
            result.Add(a[i++]);

        result.Add(b);

        while (i < a.Count)
            result.Add(a[i++]);

        return result;
    }
    public static Set<T> Difference(Set<T> a, Set<T> b) {
        var i = 0;
        var j = 0;
        Set<T> result = new();

        while (i < a.Count && j < b.Count)
        {
            if (a[i].CompareTo(b[j]) < 0)
            {
                result.Add(a[i++]);
                continue;
            }

            if (a[i].CompareTo(b[j]) == 0)
            {
                i++;
                j++;
                continue;
            }
            j++;
        }

        while (i < a.Count)
            result.Add(a[i++]);

        return result;
    }
    public static bool EqualSet(Set<T> a, Set<T> b) 
    {
        if (a.Count != b.Count)
            return false;

        for (int i = 0; i < a.Count; i++) {
            if (a[i].CompareTo(b[i]) != 0)
                return false;
        }

        return true;
    }
    public void Sort(int leftIndex, int rightIndex) {
        var i = leftIndex;
        var j = rightIndex;
        var pivot = Array[(leftIndex+rightIndex)/2];
        while (i <= j)
        {
            while (Array[i].CompareTo(pivot) < 0)
            {
                i++;
            }

            while (Array[j].CompareTo(pivot) > 0)
            {
                j--;
            }
            if (i <= j)
            {
                T temp = Array[i];
                Array[i] = Array[j];
                Array[j] = temp;
                i++;
                j--;
            }
        }

        if (leftIndex < j)
            Sort(leftIndex, j);

        if (i < rightIndex)
            Sort(i, rightIndex);
    }
}
