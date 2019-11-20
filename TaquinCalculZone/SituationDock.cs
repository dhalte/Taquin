using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaquinCalculZone
{
  internal class SituationDock : HashSet<Situation>
  {
    internal Size Size;
    internal int Largeur;
    internal int Hauteur;
    internal int NbCases;
    internal int NbPieces;
    internal int IndiceVide;

    internal SituationDock(Size sz, int nbPieces)
    {
      Size = sz;
      Largeur = sz.Width;
      Hauteur = sz.Height;
      NbCases = Largeur * Hauteur;
      NbPieces = nbPieces;
      IndiceVide = nbPieces - 1;
    }

    internal bool IsValide(int indice)
    {
      return 0 <= indice && indice < NbCases;
    }
    internal bool IsValide(Point coord)
    {
      return 0 <= coord.X && coord.X < Largeur && 0 <= coord.Y && coord.Y < Hauteur;
    }
    internal Point Coordonnees(int indice)
    {
      if (!IsValide(indice))
      {
        throw new ArgumentException();
      }
      return new Point(indice % Largeur, indice / Largeur);
    }
    internal int Indice(int x, int y)
    {
      return Indice(new Point(x, y));
    }
    internal int Indice(Point coord)
    {
      if (!IsValide(coord))
      {
        throw new ArgumentException();
      }
      return coord.X + Largeur * coord.Y;
    }
    internal bool IsVoisins(int indice1, int indice2)
    {
      return Math.Abs(indice1 - indice2) == 1 || Math.Abs(indice1 - indice2) == Largeur;
    }

    internal Situation GetSituation(Situation situation)
    {
      // Framework Core 2.0 et .NET 4.7.2
      bool bOK = base.TryGetValue(situation, out Situation situationRef);
      if (!bOK)
      {
        throw new ApplicationException();
      }
      return situationRef;
    }
  }
}
