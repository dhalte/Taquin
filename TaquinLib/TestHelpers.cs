using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaquinLib
{

  public partial class Jeu
  {
    // Le constructeur Jeu génère un jeu aléatoire
    // Pour la reproductibilité des tests, on offre ici la possibilité de forcer les positions initiales
    public static void InitJeu(Jeu jeu, string description)
    {
      string[] champs = Regex.Split(description, @"\s+");
      if (champs.Length != jeu.NbCases)
      {
        throw new ApplicationException();
      }
      int i = 0;
      foreach (string champ in champs)
      {
        if (!int.TryParse(champ, out int v))
        {
          throw new ApplicationException();
        }
        jeu.PlateauInitial[i++] = v;
      }
      int[] copy = jeu.DupliquePlateau(jeu.PlateauInitial);
      Array.Sort(copy);
      for (int j = 0; j < jeu.NbCases; j++)
      {
        if (copy[j] != j)
        {
          throw new ApplicationException();
        }
      }
    }

    // format du dump (utile pour des comparaisons avec le résultat attendu)
    // lignes séparées par environment.newline
    // fields sur 2 caractères, séparés par un espace
    public static string DumpPlateau(Jeu jeu)
    {
      int[] plateau = jeu.Plateau != null ? jeu.Plateau : jeu.PlateauInitial;
      return DumpPlateau(jeu, plateau);
    }
    public static string DumpPlateau(Jeu jeu, int[] plateau)
    {
      List<string> rows = new List<string>();
      for (int y = 0; y < jeu.Hauteur; y++)
      {
        List<string> row = new List<string>();
        for (int x = 0; x < jeu.Largeur; x++)
        {
          int i = jeu.Indice(x, y);
          row.Add($"{plateau[i],2}");
        }
        rows.Add(string.Join(" ", row));
      }
      return string.Join(Environment.NewLine, rows);
    }

    public static bool IsRange(Jeu jeu)
    {
      for (int i = 0; i < jeu.NbCases; i++)
      {
        if (jeu.Plateau[i] != i)
        {
          return false;
        }
        if (!jeu.PiecesRangees[i])
        {
          return false;
        }
      }
      return true;
    }

    // Jeu rangé sauf les deux cases au dessus du coin inférieur droit
    // (pour le cas où on rangerait une situation non résoluble)
    public static bool IsPresqueRange(Jeu jeu)
    {
      for (int i = 0; i < jeu.NbCases; i++)
      {
        if (i == jeu.CaseVide - jeu.Largeur - 1 || i == jeu.CaseVide - jeu.Largeur)
        {
          // elles doivent être interverties
          if (jeu.Plateau[i] == i || jeu.Pieces[i] == i || jeu.PiecesRangees[i])
          {
            return false;
          }

        }
        else
        {
          if (jeu.Plateau[i] != i || jeu.Pieces[i] != i || !jeu.PiecesRangees[i])
          {
            return false;
          }
        }
      }
      return true;
    }
    public static bool IsResoluble(Jeu jeu)
    {
      return jeu.IsResoluble();
    }
  }
}
