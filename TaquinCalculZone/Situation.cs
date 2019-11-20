using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaquinCalculZone
{
  internal class Situation
  {
    internal SituationDock Dock;
    internal Situation Parent;
    internal int Longueur { get; private set; }
    internal int PositionVide { get => PositionPieces[Dock.IndiceVide]; }

    private int[] PositionPieces;
    private int hashCode;
    //                                                            Nord             Est             Sud             Ouest
    private static readonly Size[] Cardinaux = new Size[] { new Size(0, -1), new Size(1, 0), new Size(0, 1), new Size(-1, 0) };
    internal Situation(SituationDock dock, IList<int> posPieces)
    {
      Dock = dock;
      Parent = null;
      Longueur = 0;
      PositionPieces = new int[posPieces.Count];
      for (int i = 0; i < posPieces.Count; i++)
      {
        PositionPieces[i] = posPieces[i];
      }
      hashCode = ComputeHashCode();
    }

    internal Situation(Situation parent, int newPositionV)
    {
      if (parent == null)
      {
        throw new ArgumentException();
      }
      Parent = parent;
      Dock = parent.Dock;
      Longueur = parent.Longueur + 1;
      
      if (!Dock.IsValide(newPositionV))
      {
        throw new ArgumentException();
      }
      if (!Dock.IsVoisins(parent.PositionVide, newPositionV))
      {
        throw new ArgumentException();
      }
      PositionPieces = new int[parent.PositionPieces.Length];
      for (int i = 0; i < parent.PositionPieces.Length - 1; i++)
      {
        if (parent.PositionPieces[i] == newPositionV)
        {
          PositionPieces[i] = parent.PositionVide;
        }
        else
        {
          PositionPieces[i] = parent.PositionPieces[i];
        }
      }
      PositionPieces[Dock.IndiceVide] = newPositionV;
      hashCode = ComputeHashCode();
    }

    internal IEnumerable<Situation> Voisins()
    {
      List<Situation> voisins = new List<Situation>();
      Point coordVide = Dock.Coordonnees(PositionVide);
      foreach (Size direction in Cardinaux)
      {
        Point newCoordVide = Point.Add(coordVide, direction);
        if (Dock.IsValide(newCoordVide))
        {
          int newPositionVide = Dock.Indice(newCoordVide);
          Situation voisin = new Situation(this, newPositionVide);
          voisins.Add(voisin);
        }
      }
      return voisins;
    }

    internal List<string> DumpSolution()
    {
      List<string> result = new List<string>();
      Situation situation = this;
      do
      {
        result.Add(situation.DumpSituation());
        situation = situation.Parent;
      } while (situation != null);
      return result;
    }

    internal string DumpSituation()
    {
      char[] result = new char[Dock.NbCases];
      for (int i = 0; i < Dock.NbCases; i++)
      {
        bool bFound = false;
        for (int j = 0; j < PositionPieces.Length - 1; j++)
        {
          if (i == PositionPieces[j])
          {
            result[i] = (char)('A' + j);
            bFound = true;
            break;
          }
        }
        if (!bFound && i == PositionVide)
        {
          result[i] = 'V';
          bFound = true;
        }
        if (!bFound)
        {
          result[i] = ' ';
        }
      }

      return new string(result);
    }

    private int ComputeHashCode()
    {
      int hashCode = 7;
      unchecked
      {
        for (int i = 0; i < PositionPieces.Length; i++)
        {
          hashCode = 31 * hashCode + PositionPieces[i];
        }
      }
      return hashCode;
    }
    public override int GetHashCode()
    {
      return hashCode;
    }
    public override bool Equals(object obj)
    {
      Situation s = obj as Situation;
      if (s == null)
      {
        return false;
      }
      return hashCode == s.hashCode;
    }
  }
}
