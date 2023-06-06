using ChromaticNumber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromaticNumberl;

public class BronFunction
{
    private Graph G { get; set; }
    private SetInt Nodes { get; set; }

    public BronFunction(Graph g)
    {
        G=g;
    }

    public List<SetInt> Start(SetInt nodes)
    {
        var result = new List<SetInt>();
        Nodes= nodes;
        //var nodes = new SetInt();

        Extend(new(), nodes, new(), in result);

        return result;
    }

    private void Extend(SetInt currentIndependentSet, SetInt candidates, SetInt used, in List<SetInt> result)
    {
        var wrong = used;
        while (candidates.Count > 0 && Check(candidates, used))
        {
            var c = candidates.First();
            currentIndependentSet = SetInt.Union(currentIndependentSet, c);
            var temp = SetInt.Intersection(G.AdjList[c], Nodes);
            temp = SetInt.Union(temp, c);
            var newWrong = SetInt.Difference(wrong, temp);
            var newCandidates = SetInt.Difference(candidates, temp);

            if (newCandidates.Count <= 0 && newWrong.Count <= 0)
                result.Add(currentIndependentSet);
            else
                Extend(currentIndependentSet, newCandidates, newWrong, in result);

            candidates = SetInt.Difference(candidates, c);
            currentIndependentSet = SetInt.Difference(currentIndependentSet, c);
            wrong = SetInt.Union(wrong, c);
        }
    }

    private bool Check(SetInt candidates, SetInt used)
    {
        foreach (var u in used)
        {
            var temp = SetInt.Intersection(candidates, G.AdjList[u]);

            if (temp.Count <= 0)
                return false;
        }

        return true;
    }

}
