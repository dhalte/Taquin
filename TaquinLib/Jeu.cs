using TaquinCalculZone;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TaquinLib
{
  public partial class Jeu
  {
    public int Largeur { get; private set; }
    public int Hauteur { get; private set; }
    public int NbCases { get; private set; }
    internal int Indice(int x, int y)
    {
      if (x < 0 || Largeur <= x || y < 0 || Hauteur <= y)
      {
        throw new ArgumentException();
      }
      return x + y * Largeur;
    }
    internal int Indice(Point coord)
    {
      return Indice(coord.X, coord.Y);
    }
    internal Point Coordonnees(int indice)
    {
      if (indice < 0 || NbCases <= indice)
      {
        throw new ArgumentException();
      }
      return new Point(indice % Largeur, indice / Largeur);
    }
    internal int Distance(Point A, Point B)
    {
      return Math.Abs(A.X - B.X) + Math.Abs(A.Y - B.Y);
    }
    // n° d'ordre de la pièce vide (==NbCases-1)
    public int CaseVide { get; private set; }
    // indice des pièces placées dans les cases du plateau initial
    private int[] PlateauInitial;
    // Permet le placement en fin de ligne, colonne, dans des cas difficile
    // permet le placement en carré final
    private Calcul calculZone;

    // indice des pièces placées dans les cases de ce plateau de travail
    internal int[] Plateau;
    // emplacement des pièces
    internal int[] Pieces;

    internal int PosVide { get => Pieces[CaseVide]; }
    internal bool[] PiecesRangees;
    // directions                                             Nord,            Est,            Sud,            Ouest
    internal static readonly Size[] PointsCardinaux = { new Size(0, -1), new Size(1, 0), new Size(0, 1), new Size(-1, 0) };

    public Jeu(int largeur, int hauteur)
    {
      if (largeur <= 1 || hauteur <= 1)
      {
        throw new ArgumentException();
      }
      Largeur = largeur;
      Hauteur = hauteur;
      NbCases = largeur * hauteur;
      CaseVide = NbCases - 1;
      GenereJeu();
      if (!IsResoluble())
      {
        RendResoluble();
      }
      // préparation de données
      InitiCalculZones();
    }

    private void InitiCalculZones()
    {
      calculZone = new Calcul();
      Size sz;
      List<IList<int>> positionPiecesGagnantes;
      // Les solutions pour les fins de ligne
      sz = new Size(2, 3);
      positionPiecesGagnantes = new List<IList<int>>();
      positionPiecesGagnantes.Add(new int[] { 0, 1, 2 });
      positionPiecesGagnantes.Add(new int[] { 0, 1, 3 });
      // renverra toujours 0, l'indice de cette configuration de zone calculée : fin de ligne, rectangle 2x3
      calculZone.ComputeDock(sz, 3, positionPiecesGagnantes);
      // Les solutions pour les fins de colonne
      sz = new Size(3, 2);
      positionPiecesGagnantes.Clear();
      positionPiecesGagnantes.Add(new int[] { 0, 3, 1 });
      positionPiecesGagnantes.Add(new int[] { 0, 3, 4 });
      // renverra toujours 1, l'indice de cette configuration de zone calculée : fin de colonne, rectangle 3x2
      calculZone.ComputeDock(sz, 3, positionPiecesGagnantes);
      // Les solutions pour le carré final
      positionPiecesGagnantes.Clear();
      sz = new Size(2, 2);
      positionPiecesGagnantes.Add(new int[] { 2, 3 });
      // renverra toujours 2, l'indice de cette configuration de zone calculée : carré final, rectangle 2x2
      calculZone.ComputeDock(sz, 2, positionPiecesGagnantes);
    }

    private void GenereJeu()
    {
      PlateauInitial = new int[NbCases];
      PiecesRangees = new bool[NbCases]; // cases initialisées à false
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

    internal int[] DupliquePlateau(int[] plateauInitial)
    {
      int[] plateau = new int[NbCases];
      Array.Copy(plateauInitial, 0, plateau, 0, NbCases);
      return plateau;
    }

    private bool IsResoluble()
    {
      int S = 0;
      for (int indiceCase = 0; indiceCase < NbCases; indiceCase++)
      {
        int indicePiece = PlateauInitial[indiceCase];
        S += Distance(Coordonnees(indicePiece), Coordonnees(indiceCase));
      }
      return S % 2 == 0;
    }

    private void RendResoluble()
    {
      // TODO : comment rendre résoluble un plateau non résoluble ?
      //int i = 0;
      //if (PlateauInitial[i] == CaseVide)
      //{
      //  ++i;
      //}
      //int j = i + 1;
      //if (PlateauInitial[j] == CaseVide)
      //{
      //  ++j;
      //}
      //int t = PlateauInitial[j];
      //PlateauInitial[j] = PlateauInitial[i];
      //PlateauInitial[i] = t;
    }

    public void Resoudre()
    {
      Plateau = DupliquePlateau(PlateauInitial);
      InitPieces();

      for (int y = 0; y < Hauteur - 2; y++)
      {
        for (int x = 0; x < Largeur - 2; x++)
        {
          int indiceA = Indice(x, y);
          PlacePiece(indiceA, indiceA);
          if (y == Hauteur - 3)
          {
            RangeColonne(x);
          }
        }
        RangeLigne(y);
      }
      RangeDernierCarre();
    }

    private void InitPieces()
    {
      Pieces = new int[NbCases];
      for (int i = 0; i < NbCases; i++)
      {
        Pieces[Plateau[i]] = i;
      }
    }

    // indiceA est le n° de la pièce A auprès de laquelle il faut placer Vide
    // posDestA est la position finale qu'on veut donner à A
    // RQ : dans le rectangle (0,0)-(L-3,H-3), on aura posDestA == indiceA
    // Mais dans d'autres cas, on voudra placer A temporairement à une autre position que sa position définitive 
    private void PlacePiece(int indiceA, int posDestA)
    {
      while (Pieces[indiceA] != posDestA)
      {
        ApprocheVide(indiceA, posDestA);
        SwitchVide(Pieces[indiceA]);
      }
      if (indiceA == posDestA)
      {
        PiecesRangees[indiceA] = true;
      }
    }

    private void SwitchVide(int posVideDest)
    {
      int posVideActuelle = PosVide;
      if (!Voisins(posVideActuelle, posVideDest))
      {
        throw new ArgumentException();
      }
      // la pièce qui va être transposée avec la case vide
      int indiceA = Plateau[posVideDest];
      Plateau[posVideActuelle] = indiceA;
      Plateau[posVideDest] = CaseVide;
      Pieces[CaseVide] = posVideDest;
      Pieces[indiceA] = posVideActuelle;
    }

    private bool Voisins(int indiceVide, int indiceA)
    {
      Point coordVide = Coordonnees(PosVide), coordA = Coordonnees(indiceA);
      return Voisins(coordVide, coordA);
    }

    private bool Voisins(Point coordVide, Point coordA)
    {
      if (Math.Abs(coordVide.X - coordA.X) == 1 && coordVide.Y == coordA.Y)
      {
        return true;
      }
      if (Math.Abs(coordVide.Y - coordA.Y) == 1 && coordVide.X == coordA.X)
      {
        return true;
      }
      return false;
    }

    // indiceA est le n° de la pièce A auprès de laquelle il faut placer Vide
    // posDestA est la position finale qu'on veut donner à A
    // RQ : dans le rectangle (0,0)-(L-3,H-3), on aura posDestA == indiceA
    // Mais dans d'autres cas, on voudra placer A temporairement à une autre position que sa position définitive 
    private void ApprocheVide(int indiceA, int posDestA)
    {
      List<int> voisinsPrometteurs = RechercheVoisinsPrometteurs(indiceA, posDestA);
      RechercheChemin rechercheChemin = new RechercheChemin(this, voisinsPrometteurs);
      // On utilise cette astuce pour empêcher la recherche de déplacer A
      int posA = Position(indiceA);
      PiecesRangees[posA] = true;
      rechercheChemin.Recherche();
      PiecesRangees[posA] = false;
      IList<int> positionsVide = rechercheChemin.PositionsVide();
      foreach (int posVide in positionsVide)
      {
        SwitchVide(posVide);
      }
    }

    private List<int> RechercheVoisinsPrometteurs(int indiceA, int posDestA)
    {
      Point coordAFinale = Coordonnees(posDestA);
      int posA = Position(indiceA);
      Point coordA = Coordonnees(posA);
      int distance = int.MaxValue;
      List<int> resultats = new List<int>();
      foreach (Size pointCardinal in PointsCardinaux)
      {
        Point coordVoisin = Point.Add(coordA, pointCardinal);
        if (!InPlateau(coordVoisin))
        {
          continue;
        }
        int posVoisin = Indice(coordVoisin);
        if (PiecesRangees[posVoisin])
        {
          continue;
        }
        int distance2 = Distance(coordAFinale, coordVoisin);
        if (distance2 < distance)
        {
          resultats.Clear();
          distance = distance2;
        }
        if (distance2 == distance)
        {
          resultats.Add(posVoisin);
        }
      }
      return resultats;
    }

    internal bool InPlateau(int i)
    {
      return 0 <= i && i < NbCases;
    }
    internal bool InPlateau(Point p)
    {
      return 0 <= p.X && p.X < Largeur && 0 <= p.Y && p.Y < Hauteur;
    }

    private int Position(int indiceA)
    {
      if (indiceA < 0 || NbCases <= indiceA)
      {
        throw new ArgumentException();
      }
      return Pieces[indiceA];
    }

    // Il s'agit d'amener V dans un rectangle de fin de ligne ou colonne
    // sans s'encombrer de pièces à ne pas déplacer (hors celles déjà rangées)
    private void PlaceVideDansRectangle(Rectangle rc)
    {
      Point coordPieceVide = Coordonnees(PosVide);
      int indiceDest = 0;
      // D'abord choisir la case dans le rectangle la plus proche de V
      // Les rectangles ayant 6 cases, on n'a pas optimisé ce calcul
      int dist = int.MaxValue;
      for (int y = rc.Y; y < rc.Bottom; y++)
      {
        for (int x = rc.Left; x < rc.Right; x++)
        {
          Point p = new Point(x, y);
          int dist2 = Distance(coordPieceVide, p);
          if (dist2 < dist)
          {
            dist = dist2;
            indiceDest = Indice(p);
          }
        }
      }
      // Ensuite déplacer Vide jusqu'à cette position
      while (PosVide != indiceDest)
      {
        List<int> voisinsPrometteurs = RechercheVoisinsPrometteurs(CaseVide, indiceDest);
        if (voisinsPrometteurs.Count == 0)
        {
          throw new ApplicationException();
        }
        SwitchVide(voisinsPrometteurs[0]);
      }
    }

    private void RangeLigne(int y)
    {
      RangeLigneColonne(y, true);
    }
    private void RangeColonne(int x)
    {
      RangeLigneColonne(x, false);
    }
    private void RangeLigneColonne(int xy, bool bLigne)
    {
      int delta;
      Point coordFinalesC2;
      Point coordFinalesC1;
      if (bLigne)
      {
        coordFinalesC2 = new Point(Largeur - 2, xy);
        coordFinalesC1 = new Point(Largeur - 1, xy);
      }
      else
      {
        coordFinalesC2 = new Point(xy, Hauteur - 2);
        coordFinalesC1 = new Point(xy, Hauteur - 1);
      }
      int indiceC2 = Indice(coordFinalesC2);
      int indiceC1 = Indice(coordFinalesC1);
      int posC2 = Pieces[indiceC2];
      int posC1 = Pieces[indiceC1];
      Point coordC2 = Coordonnees(posC2);
      Point coordC1 = Coordonnees(posC1);
      if (posC2 == indiceC2 && posC1 == indiceC1)
      {
        // quelle chance, les deux pièces sont déjà à leur position définitive
        PiecesRangees[indiceC2] = true;
        PiecesRangees[indiceC1] = true;
        return;
      }
      Rectangle rc;
      if (bLigne)
      {
        rc = new Rectangle(coordFinalesC2, new Size(2, 3));
      }
      else
      {
        rc = new Rectangle(coordFinalesC2, new Size(3, 2));
      }

      bool bC1etC2inRectangle = rc.Contains(coordC1) && rc.Contains(coordC2);
      Point coordCaseVide = Coordonnees(PosVide);
      if (bC1etC2inRectangle)
      {
        bool bVideInRectangle = rc.Contains(coordCaseVide);
        if (!bVideInRectangle)
        {
          PlaceVideDansRectangle(rc);
          // ce rangement a pu éjecter C1 ou C2 du rectangle
          posC2 = Pieces[indiceC2];
          posC1 = Pieces[indiceC1];
          coordC2 = Coordonnees(posC2);
          coordC1 = Coordonnees(posC1);
          coordCaseVide = Coordonnees(PosVide);
          bC1etC2inRectangle = rc.Contains(coordC1) && rc.Contains(coordC2);
        }
      }
      if (bC1etC2inRectangle)
      {
        int[] posPieces = new int[3];
        Point tmpPoint;
        tmpPoint = new Point(coordC2.X - rc.X, coordC2.Y - rc.Y);
        posPieces[0] = tmpPoint.X + rc.Width * tmpPoint.Y;
        tmpPoint = new Point(coordC1.X - rc.X, coordC1.Y - rc.Y);
        posPieces[1] = tmpPoint.X + rc.Width * tmpPoint.Y;
        tmpPoint = new Point(coordCaseVide.X - rc.X, coordCaseVide.Y - rc.Y);
        posPieces[2] = tmpPoint.X + rc.Width * tmpPoint.Y;
        int handler;
        if (bLigne)
        {
          handler = 0;
        }
        else
        {
          handler = 1;
        }
        IList<int> positionsVideSuccessives = calculZone.GetSolution(handler, posPieces);

        foreach (int nextPosVideInSol in positionsVideSuccessives)
        {
          Size nextCoordVideInSol = new Size(nextPosVideInSol % rc.Width, nextPosVideInSol / rc.Width);
          tmpPoint = Point.Add(rc.Location, nextCoordVideInSol);
          int nextPosVideInGrille = Indice(tmpPoint);
          SwitchVide(nextPosVideInGrille);
        }
        if (indiceC2 != Pieces[indiceC2] || indiceC1 != Pieces[indiceC1])
        {
          throw new ApplicationException();
        }
        PiecesRangees[indiceC2] = true;
        PiecesRangees[indiceC1] = true;
        return;
      }
      int dist, dist1;
      bool bPlaceDabordC1 = true;
      dist = Distance(coordC1, coordFinalesC1);
      dist1 = Distance(coordC2, coordFinalesC2);
      if (dist1 < dist)
      {
        dist = dist1;
        bPlaceDabordC1 = false;
      }
      if (bLigne)
      {
        delta = Largeur;
      }
      else
      {
        delta = 1;
      }
      if (bPlaceDabordC1)
      {
        PlacePiece(indiceC1, indiceC2);
        PiecesRangees[indiceC2] = true;
        PlacePiece(indiceC2, indiceC2 + delta);
        PiecesRangees[indiceC2 + delta] = true;
        ApprocheVide(indiceC1, indiceC1);
        PiecesRangees[indiceC2 + delta] = false;
        PiecesRangees[indiceC2] = false;
        SwitchVide(indiceC2);
        SwitchVide(indiceC2 + delta);
      }
      else
      {
        PlacePiece(indiceC2, indiceC1);
        PiecesRangees[indiceC1] = true;
        PlacePiece(indiceC1, indiceC1 + delta);
        PiecesRangees[indiceC1 + delta] = true;
        ApprocheVide(indiceC2, indiceC2);
        PiecesRangees[indiceC1 + delta] = false;
        PiecesRangees[indiceC1] = false;
        SwitchVide(indiceC1);
        SwitchVide(indiceC1 + delta);
      }
      if (indiceC2 != Pieces[indiceC2] || indiceC1 != Pieces[indiceC1])
      {
        throw new ApplicationException();
      }
      PiecesRangees[indiceC2] = true;
      PiecesRangees[indiceC1] = true;
    }

    private void RangeDernierCarre()
    {
      int indiceC = CaseVide - 1;
      int posC = Pieces[indiceC];
      int indiceTopLeftCarre = CaseVide - Largeur - 1;
      Point coordOrigineCarre = Coordonnees(indiceTopLeftCarre);
      if (posC == indiceC && PosVide == CaseVide)
      {
        // déjà placé
        PiecesRangees[indiceC] = true;
        PiecesRangees[CaseVide] = true;
        PiecesRangees[indiceTopLeftCarre] = Plateau[indiceTopLeftCarre] == indiceTopLeftCarre;
        PiecesRangees[indiceTopLeftCarre + 1] = Plateau[indiceTopLeftCarre + 1] == indiceTopLeftCarre + 1;
        return;
      }
      Point coordC = Coordonnees(posC);
      Point coordCaseVide = Coordonnees(PosVide);
      Rectangle rc = new Rectangle(coordOrigineCarre, new Size(2, 2));
      int[] posPieces = new int[2];
      Point tmp;
      tmp = new Point(coordC.X - rc.X, coordC.Y - rc.Y);
      posPieces[0] = tmp.X + rc.Width * tmp.Y;
      tmp = new Point(coordCaseVide.X - rc.X, coordCaseVide.Y - rc.Y);
      posPieces[1] = tmp.X + rc.Width * tmp.Y;
      int handler = 2;
      IList<int> positionsVideSuccessives = calculZone.GetSolution(handler, posPieces);
      foreach (int nextPosVideInSol in positionsVideSuccessives)
      {
        Size nextCoordVideInSol = new Size(nextPosVideInSol % rc.Width, nextPosVideInSol / rc.Width);
        Point tmpPoint = Point.Add(rc.Location, nextCoordVideInSol);
        int nextPosVideInGrille = Indice(tmpPoint);
        SwitchVide(nextPosVideInGrille);
      }
      if (indiceC != Pieces[indiceC] || CaseVide != Pieces[CaseVide])
      {
        throw new ApplicationException();
      }
      PiecesRangees[indiceC] = true;
      PiecesRangees[CaseVide] = true;
      PiecesRangees[indiceTopLeftCarre] = Plateau[indiceTopLeftCarre] == indiceTopLeftCarre;
      PiecesRangees[indiceTopLeftCarre + 1] = Plateau[indiceTopLeftCarre + 1] == indiceTopLeftCarre + 1;

    }

  }
}
