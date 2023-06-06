using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChromaticNumber;

public class Set<T> : IEnumerable<T>
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

            throw new InvalidProgramException();
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

    //TODO: Переделать на бинарный поиск
    public bool Contains(T element)
    {
        if (this.Find(element, 0, this.Count-1) == -1)
            return false;
        return true;
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
    public static SetInt Intersection(SetInt a, Pair b)
    {
        var temp = new SetInt();
        temp.Add(b.First);
        temp.Add(b.Second);

        return SetInt.Intersection(a, temp);
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
        var temp = new Set<T>();
        temp.Add(b);
        return Union(a, temp);
    }
    public static SetInt Union(SetInt a, Pair b)
    {
        var temp = new SetInt();
        temp.Add(b.First);
        temp.Add(b.Second);

        return SetInt.Union(a, temp);
    }


    public static Set<Pair> Difference(Set<Pair> a, Set<Pair> b, bool isCheckPower) 
    {
        var i = 0;
        var j = 0;
        Set<Pair> result = new();

        while (i < a.Count && j < b.Count)
        {
            if (a[i].CompareTo(b[j], isCheckPower) < 0)
            {
                result.Add(a[i++]);
                continue;
            }

            if (a[i].CompareTo(b[j], isCheckPower) == 0)
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
    public static Set<T> Difference(Set<T> a, T b)
    {
        var temp = new Set<T>();
        temp.Add(b);
        return Difference(a, temp);
    }

    public static SetInt Difference(SetInt a, Pair b)
    {
        var temp = new SetInt();
        temp.Add(b.First);
        temp.Add(b.Second);

        return SetInt.Difference(a, temp);
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

    public T First()
    {
        return Array[0];
    }

    public SetEnum<T> GetEnumerator()
    {
        return new SetEnum<T>(Count, Array, Capacity);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return (IEnumerator<T>)GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();   
    }
}

public class SetEnum<T> : IEnumerator<T>
{
    public int Count { get; private set; }
    public T[] Array { get; private set; }
    public int Capacity { get; private set; }
    private int Position { get; set; } = -1;
    public SetEnum(int count, T[] array, int capacity)
    {
        Count = count;
        Array = array;
        Capacity = capacity;
    }

    public T Current
    {
        get
        {
            try
            {
                return Array[Position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public bool MoveNext()
    {
        Position++;
        return Position < Count;
    }

    public void Reset()
    {
        Position = -1;
    }
}

public class SetInt : IEnumerable<int>
{
    private const int MaxDegree = 64;

    private bool CountIsChanged = false;
    private int count = 0;
    public int Count
    {
        get
        {
            if (Array == 0)
            {
                count = 0;
                return count;
            }
            if (!CountIsChanged)
                return count;

            var temp = Array;
            int i = 0;
            while (temp != 0)
            {
                var q = temp % 2;
                temp/= 2;
                if (q == 1)
                    i++;
            }

            count = i;
            CountIsChanged= false;
            return count;
        }
        set
        {
            count = value;
        }
    }
    private ulong Array { get; set; } = 0;

    public SetInt()
    { }

    public SetInt(ulong array, bool countIsChanged)
    {
        Array = array;
        CountIsChanged= countIsChanged;
    }

    public void Add(int number)
    {
        var t = (ulong)Math.Pow(2, number);

        Array = Array | t;

        count++;
    }

    public int this[int index]
    {
        get
        {
            if (Count == 0) return -1;
            if (index >= 0 && index < Count)
            {
                var temp = Array;
                int i = -1;
                while (index >= 0)
                {
                    ulong res = 0;
                    while (res == 0)
                    {
                        res = temp % 2;
                        temp /=2;
                        i++;
                    }
                    index--;
                }
                return i;
            }

            throw new InvalidProgramException();
        }
    }
    public bool Contains(int element)
    {
        var t = (ulong)Math.Pow(2, element);
        var temp = Array & t;
        if (temp > 0)
            return true;
        return false;
    }

    public void Clear()
    {
        Count = 0;
        Array = 0;
    }

    public static SetInt Intersection(SetInt a, SetInt b)
    {
        var f = a.Array & b.Array;
        return new(f, true);
    }

    public static SetInt Union(SetInt a, SetInt b)
    {
        var f = a.Array | b.Array;
        return new(f, true);
    }

    public static SetInt Union(SetInt a, int b)
    {
        var t = (ulong)Math.Pow(2, b);
        var f = a.Array | t;
        return new(f, true);
    }

    public static SetInt Union(SetInt a, Pair b)
    {
        var t = (ulong)Math.Pow(2, b.First);
        var t2 = (ulong)Math.Pow(2, b.Second);
        t = t | t2;
        var f = a.Array | t;
        return new(f, true);
    }

    public static SetInt Difference(SetInt a, SetInt b)
    {
        var f = a.Array & b.Array;
        f = a.Array ^ f;
        return new(f, true);
    }
    public static SetInt Difference(SetInt a, int b)
    {
        var t = (ulong)Math.Pow(2, b);
        var f = a.Array & t;
        f = a.Array ^ f;
        return new(f, true);
    }

    public static SetInt Difference(SetInt a, Pair b)
    {
        var t = (ulong)Math.Pow(2, b.First);
        var f = a.Array & t;
        f = a.Array ^ f;
        t = (ulong)Math.Pow(2, b.Second);
        var q = f & t;
        f = f ^ q;
        return new(f, true);
    }

    public SetIntEnum GetEnumerator()
    {
        return new SetIntEnum(Count, this);
    }

    IEnumerator<int> IEnumerable<int>.GetEnumerator()
    {
        return (IEnumerator<int>)GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

public class SetIntEnum : IEnumerator<int>
{
    public int Count { get; private set; }
    public SetInt Array { get; private set; }
    private int Position { get; set; } = -1;
    public SetIntEnum(int count, SetInt array)
    {
        Count = count;
        Array = array;
    }

    public int Current
    {
        get
        {
            try
            {
                return Array[Position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public bool MoveNext()
    {
        Position++;
        return Position < Count;
    }

    public void Reset()
    {
        Position = -1;
    }
}

