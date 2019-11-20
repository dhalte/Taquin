using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaquinLib
{
  internal class PlateauRencontre
  {
    private int NbCases;
    internal int PosVide;
    internal PlateauRencontre parent { get; private set; }
    internal int distanceCibles;

    internal PlateauRencontre(Jeu jeu)
    {
      this.NbCases = jeu.NbCases;
      this.parent = null;
      PosVide = jeu.PosVide;
    }
    internal PlateauRencontre(Jeu jeu, PlateauRencontre parent, int nextPosVide)
    {
      this.NbCases= jeu.NbCases;
      this.parent = parent;
      PosVide = nextPosVide;
    }
    public override bool Equals(object obj)
    {
      if (this == obj)
      {
        return true;
      }
      PlateauRencontre o = obj as PlateauRencontre;
      if (o == null)
      {
        return false;
      }      
      return PosVide == o.PosVide;
    }

    public override int GetHashCode()
    {
      return PosVide;
    }
  }
}
