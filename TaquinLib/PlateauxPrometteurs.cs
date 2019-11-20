using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaquinLib
{
  class PlateauxPrometteurs
  {
    private readonly SortedList<int, List<PlateauRencontre>> List = new SortedList<int, List<PlateauRencontre>>();
    internal int Count { get; private set; } = 0;
    internal void Add(PlateauRencontre plateau)
    {
      if (!List.ContainsKey(plateau.distanceCibles))
      {
        List.Add(plateau.distanceCibles, new List<PlateauRencontre>());
      }
      List[plateau.distanceCibles].Add(plateau);
      Count++;
    }
    internal PlateauRencontre Next()
    {
      PlateauRencontre result = null;
      foreach (List<PlateauRencontre> listePlateaux in List.Values)
      {
        if (listePlateaux.Count>0)
        {
          result = listePlateaux[listePlateaux.Count - 1];
          listePlateaux.RemoveAt(listePlateaux.Count - 1);
          Count--;
          break;
        }
      }
      return result;
    }
  }
}
