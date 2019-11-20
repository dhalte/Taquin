using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaquinCalculZone
{

  public class Calcul
  {
    private SituationDocks docks = new SituationDocks();
    public int ComputeDock(Size sz,int NbPiecesInPositionsGagnantes, IList<IList<int>> ListePositionsGagnantes)
    {
      SituationDock dock = new SituationDock(sz, NbPiecesInPositionsGagnantes);
      docks.Add(dock);
      int result = docks.Count - 1;
      SituationQueue queue = new SituationQueue();
      foreach (IList<int> situationGagnante in ListePositionsGagnantes)
      {
        if (situationGagnante.Count != NbPiecesInPositionsGagnantes)
        {
          throw new ArgumentException();
        }
        Situation situation = new Situation(dock, situationGagnante);
        dock.Add(situation);
        if (!queue.Add(situation))
        {
          throw new ApplicationException();
        }
      }
      while (queue.Count > 0)
      {
        Situation situation = queue.Next();
        foreach (Situation voisin in situation.Voisins())
        {
          if (!dock.Contains(voisin))
          {
            if (!dock.Add(voisin))
            {
              throw new ApplicationException();
            }
            if (!queue.Add(voisin))
            {
              throw new ApplicationException();
            }
          }
        }
      }
      return result;
    }

    public IList<int> GetSolution(int handler, IList<int> posPieces)
    {
      SituationDock dock = docks[handler];
      Size sz = dock.Size;
      if (posPieces.Count != dock.NbPieces)
      {
        throw new ApplicationException();
      }
      Situation situation = new Situation(dock, posPieces);
      situation = dock.GetSituation(situation);
      List<int> result = new List<int>();
      while (situation.Parent != null)
      {
        situation = situation.Parent;
        result.Add(situation.PositionVide);
      }
      return result;
    }

    public List<List<string>> DumpDock(int handler)
    {
      List<List<string>> result = new List<List<string>>();
      SituationDock dock = docks[handler];
      foreach (Situation situation in dock)
      {
        result.Add(situation.DumpSolution());
      }
      return result;
    }

  }
}
