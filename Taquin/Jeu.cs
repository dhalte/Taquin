using System;
using System.Collections.Generic;
using System.Text;

namespace TaquinLib
{
  public class Jeu
  {
    public int Largeur { get; private set; }
    public int Hauteur { get; private set; }
    public int NbCases { get; private set; }
    public int CaseVide { get; private set; }
    private int[] PlateauInitial;
    public Jeu(int largeur, int hauteur)
    {
      if (largeur <= 1 || hauteur <= 1)
      {
        throw new ArgumentException();
      }
      Largeur = largeur;
      Hauteur = hauteur;
      NbCases = largeur * hauteur;
      GenereJeu();
      if (!IsResoluble())
      {
        RendResoluble();
      }
    }
    private void GenereJeu()
    {
      PlateauInitial = new int[NbCases];
      for (int i = 0; i < NbCases; i++)
      {
        PlateauInitial[i] = i;
      }
      Random rnd = new Random();
      for (int i = 0; i < NbCases; i++)
      {
        int p = rnd.Next(0, NbCases);
        int t = PlateauInitial[p];
        PlateauInitial[p] = PlateauInitial[i];
        PlateauInitial[i] = t;
      }
    }
    private bool IsResoluble()
    {
      int nbSwitch = 0;
      for (int i = 0; i < NbCases - 1; i++)
      {
        int A = PlateauInitial[i];
        if (A == i)
        {
          // pièce déjà placée
          continue;
        }
        int iB = Array.IndexOf(PlateauInitial, i);
        int B = PlateauInitial[iB];
        if (A != CaseVide && B != CaseVide)
        {
          nbSwitch++;
        }
        PlateauInitial[i] = B;
        PlateauInitial[iB] = A;
      }
      return PlateauInitial[CaseVide] == CaseVide;
    }
    private void RendResoluble()
    {
      int i = 0;
      if (PlateauInitial[i] == CaseVide)
      {
        ++i;
      }
      int j = i + 1;
      if (PlateauInitial[j] == CaseVide)
      {
        ++j;
      }
      int t = PlateauInitial[j];
      PlateauInitial[j] = PlateauInitial[i];
      PlateauInitial[i] = t;
    }
  }
}
