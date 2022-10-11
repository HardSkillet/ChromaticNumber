// See https://aka.ms/new-console-template for more information
using ChromaticNumber;

Console.WriteLine("Hello, World!");

Set<int> p = new ();
Set<int> r = new();
int f = 1;
for (int i = 0; i < 20; i++)
{
    p.Add(i);
    r.Add(f * 2);
    f *= 2;
}
static List<int> peres(List<int> a, List<int > b) {
    int i = 0;
    int j = 0;
    var answer = new List<int>();
    while (i < a.Count && j < b.Count) {
        if (a[i] == b[j]) 
        { 
            answer.Add(a[i]);
            i++;
            j++;
            continue; 
        }

        if (a[i] < b[j]) { i++; continue; }
        if (b[j] < a[i]) { j++; continue; }
    }
    return answer;
}
var t = Set<int>.Union(p, r);
t = Set<int>.Union(r, p);
t = Set<int>.Intersection(p, r);
t = Set<int>.Intersection(r, p);
t = Set<int>.Difference(p, r);
t = Set<int>.Difference(r, p);
int[,] a = {
    { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, },
    { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, },
    { 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, },
    { 1, 0, 1, 0, 0, 0, 1, 1, 0, 0, },
    { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, },
    { 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, },
    { 0, 1, 0, 0, 1, 1, 0, 0, 0, 1, },
    { 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, },
    { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, },
    { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, }
};
var v = new Algrorythm(a);
v.Start();
List<List<int>> h = new();
for (int i = 0; i < 10; i++) {
    h.Add(new List<int>());
    for (int j = 0; j < 10; j++) {
        if (a[i, j] == 0) h[i].Add(j);
    }
}

List<List<int>> u = new();

for (int j = 0; j < 10; j++)
{
    u.Add(new List<int>());
    for (int i = 0; i < 10; i++)
    {
        if (a[i, j] == 0) u[j].Add(i);
    }
}

foreach (var l in h)
{
    foreach (var e in l)
    {
        Console.Write(e + " ");
    }
    Console.WriteLine("\n");
}
Console.WriteLine("\n");
foreach (var l in u)
{
    foreach (var e in l)
    {
        Console.Write(e + " ");
    }
    Console.WriteLine("\n");
}


Dictionary<(int, int),List<int>> d = new();
for (int i = 0; i < 10; i++) {
    List<int> d1 = new(); 
    for (int j = i + 1; j < 10; j++) {
        d1 = peres(peres(h[i], u[j]), peres(h[j], u[i]));
        if (d1.Contains(i) && d1.Contains(j)) d.Add((i+1, j+1),d1);
    }
}

Dictionary<(int, int), int> g = new();
foreach (var kvp in d) {
    g.Add(kvp.Key, kvp.Value.Count);
}


foreach (var l in d) {
    Console.Write($"({l.Key.Item1}, {l.Key.Item2}) |{l.Value.Count}| -> ");
    foreach (var e in l.Value) {
        Console.Write(e + 1 + " ");
    }
    Console.WriteLine();
}
Console.ReadLine();