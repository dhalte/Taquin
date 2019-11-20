using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaquinCalculZone
{
  internal class SituationQueue
  {
    private SortedList<int, HashSet<Situation>> Liste = new SortedList<int, HashSet<Situation>>();
    internal bool Add(Situation situation)
    {
      if (!Liste.ContainsKey(situation.Longueur))
      {
        Liste.Add(situation.Longueur, new HashSet<Situation>());
      }
      bool bAdded = Liste[situation.Longueur].Add(situation);
      if (bAdded)
      {
        Count++;
      }
      return bAdded;
    }
    internal int Count { get; private set; }
    internal Situation Next()
    {
      if (Count <= 0)
      {
        throw new ApplicationException();
      }
      Situation situation = null;
      foreach (HashSet<Situation> dock in Liste.Values)
      {
        if (dock.Count > 0)
        {
          situation = dock.First();
          dock.Remove(situation);
          Count--;
          break;
        }
      }
      return situation;
    }
  }
}
