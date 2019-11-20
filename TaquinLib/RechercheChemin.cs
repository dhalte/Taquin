using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaquinLib
{
  internal class RechercheChemin
  {
    private Jeu jeu;
    private List<int> cibles;
    private PlateauRencontre solution;
    internal RechercheChemin(Jeu jeu, List<int> voisins)
    {
      this.jeu = jeu;
      this.cibles = voisins;
    }

    // Il faut donc rechercher un chemin qui mène CaseVide 
    // sur l'un des voisins prometteurs
    // tout en évitant les cases interdites (jeu.PiecesRangees)
    // RQ : on a pris la précaution de placer temporairement 
    // la pièce en cours de traitement dans les pièces rangées.
    internal void Recherche()
    {
      PlateauRencontre plateauInitial = new PlateauRencontre(jeu);
      if (IsSolution(plateauInitial))
      {
        this.solution = plateauInitial;
        return;
      }
      HashSet<PlateauRencontre> plateauxRencontres = new HashSet<PlateauRencontre>();
      PlateauxPrometteurs plateauxPrometteurs = new PlateauxPrometteurs();
      plateauxRencontres.Add(plateauInitial);
      CalculeDistance(plateauInitial);
      plateauxPrometteurs.Add(plateauInitial);
      PlateauRencontre solution = null;
      while (solution == null)
      {
        if (plateauxPrometteurs.Count == 0)
        {
          throw new ApplicationException("Chemin non trouvé");
        }
        PlateauRencontre plateauRencontrePrometteur = plateauxPrometteurs.Next();
        foreach (var nextPlateau in NextPlateaux(plateauRencontrePrometteur))
        {
          if (plateauxRencontres.Contains(nextPlateau))
          {
            continue;
          }
          if (IsSolution(nextPlateau))
          {
            solution = nextPlateau;
            break;
          }
          plateauxRencontres.Add(nextPlateau);
          CalculeDistance(nextPlateau);
          plateauxPrometteurs.Add(nextPlateau);
        }
      }
      this.solution = solution;
    }

    private IEnumerable<PlateauRencontre> NextPlateaux(PlateauRencontre plateau)
    {
      
      Point coordVide = jeu.Coordonnees(plateau.PosVide);
      foreach (Size direction in Jeu.PointsCardinaux)
      {
        Point nextCoord = Point.Add(coordVide, direction);
        if (!jeu.InPlateau(nextCoord))
        {
          continue;
        }
        int nextPos = jeu.Indice(nextCoord);
        if (jeu.PiecesRangees[nextPos])
        {
          continue;
        }
        yield return new PlateauRencontre(jeu, plateau, nextPos);
      }
    }

    private bool IsSolution(PlateauRencontre nextPlateau)
    {
      foreach (int cible in cibles)
      {
        if (nextPlateau.PosVide == cible)
        {
          return true;
        }
      }
      return false;
    }

    private void CalculeDistance(PlateauRencontre plateauRencontre)
    {
      int distance = int.MaxValue;
      foreach (int posCible in cibles)
      {
        int distance1 = jeu.Distance(jeu.Coordonnees(plateauRencontre.PosVide), jeu.Coordonnees(posCible));
        if (distance1 < distance)
        {
          distance = distance1;
        }
      }
      plateauRencontre.distanceCibles = distance;
    }

    internal IList<int> PositionsVide()
    {
      if (solution == null)
      {
        throw new ApplicationException();
      }
      List<int> positionsVide = new List<int>();
      PlateauRencontre plateau = solution;
     
      // On exclut l'origine qui donne la position de départ de la case vide
      while (plateau.parent != null)
      {
        positionsVide.Add(plateau.PosVide);
        plateau = plateau.parent;
      }
      positionsVide.Reverse();
      return positionsVide;
    }
  }
}
