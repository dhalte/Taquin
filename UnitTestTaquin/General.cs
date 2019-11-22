using System;
using TaquinLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace UnitTestTaquin
{
  [TestClass]
  public class General
  {
    [TestMethod]
    public void TestCreationjeu()
    {
      Jeu jeu = new Jeu(5, 4);
    }

    [TestMethod]
    public void TestJeu()
    {
      int largeur, hauteur;
      largeur = 6;
      hauteur = 6;
      string description =
        "23 18 29 10 13 20" + Environment.NewLine +
        "21  8 11 19 26 24" + Environment.NewLine +
        "27  0 12 25 22 17" + Environment.NewLine +
        "32 35  1 14  5  6" + Environment.NewLine +
        "16  3  7 31 15 28" + Environment.NewLine +
        "30  4  9  2 33 34";

      largeur = 6;
      hauteur = 4;
      description =
         "0  1  2  3  4  5" + Environment.NewLine +
         " 6  7  8  9 23 11" + Environment.NewLine +
         "12 13 14 15 10 17" + Environment.NewLine +
         "18 19 20 22 21 16";

      largeur = 6;
      hauteur = 4;
      description =
           "0  1  2  3  4  5" + Environment.NewLine +
           "6  7  8  9 10 11" + Environment.NewLine +
          "12 13 14 15 17 16" + Environment.NewLine +
          "18 19 20 21 22 23";
      largeur = 2;
      hauteur = 3;
      description =
          "3 1" + Environment.NewLine +
          "0 5" + Environment.NewLine +
          "4 2";
      Jeu jeu = new Jeu(largeur, hauteur);
      Jeu.InitJeu(jeu, description);
      jeu.Resoudre();
      string sol = Jeu.DumpSolution(jeu);
    }
    [TestMethod]
    public void TestJeuMasse()
    {
      // environ 1 minute
      int nbTests = 100000;
      int minLargeur = 3, maxLargeur = 6;
      int minHauteur = 3, maxHauteur = 6;
      Random rnd = new Random();
      for (int i = 0; i < nbTests; i++)
      {
        int largeur = rnd.Next(minLargeur, maxLargeur + 1);
        int hauteur = rnd.Next(minHauteur, maxHauteur + 1);
        Jeu jeu = new Jeu(largeur, hauteur);
        jeu.Resoudre();
        bool bOK = Jeu.IsRange(jeu) && Jeu.IsResoluble(jeu);
        Assert.IsTrue(bOK);
      }
    }
  }
}
