using ChromaticNumberl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromaticNumber;

public class AlgorithmNovikova
{
    public Graph G { get; set; }
    public int Best { get; set; } 
    private int Ocenka { get; set; }
    public AlgorithmNovikova(Graph g, int cBest, int ocenka)
    { 
        G = g;
        Best = cBest;
        Ocenka= ocenka;
    }
    public void Start(SetInt nodes, List<SetInt> c)
    {
        if (nodes.Count <= 0)
        {
            if (c.Count < Best)
                Best = c.Count;
            if (Best == Ocenka)
                return;

            return;
        }

        var bf = new BronFunction(G);
        var indSets = bf.Start(nodes);
        foreach (var temp in indSets)
        {
            var newC = c;
            newC.Add(temp);
            var newNodes = SetInt.Difference(nodes, temp);
            Start(newNodes, newC);
            newC.Remove(temp);
        }

        return;
    }
}

