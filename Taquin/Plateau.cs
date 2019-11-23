using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaquinLib;

namespace Taquin
{
  public partial class Plateau : UserControl
  {
    private Jeu Jeu;
    private Image Image;
    private bool AnimationEnCours;
    private bool ResolutionAutomatiqueEnCours;
    // Utilisé pour l'animation "manuelle", sur clic souris
    private int PosDestination;
    // animation manuelle, avancement de la case vide
    private int DeltaAnimation;
    // animation manuelle, la pièce qui glisse à la place de la case vide
    private int PosPieceAnimation;
    private PointF DeltaDrawAnimation = new PointF(0, 0);
    // pour faire glisser une pièce pendant l'animation
    // Etape : compteur de 0 à NbEtapes provoque un décalage du rectangle 
    // où est dessinée la pièce, proportionnel à Etape
    private int Etape;
    private int NbEtapes = 20;
    private System.Windows.Forms.Timer TimerAnimation;
    private int PeriodeAnimation = 25;
    public Plateau()
    {
      InitializeComponent();
    }
    internal void Init(Jeu jeu, Image image)
    {
      if (jeu == null)
      {
        throw new ArgumentException();
      }
      Jeu = jeu;
      Image = image;
      PositionnePlateau();
    }

    private void PositionnePlateau()
    {
      if (Jeu == null)
      {
        return;
      }
      float k = 1;
      if (Image != null)
      {
        k = (float)Image.Height / Image.Width;
      }
      Size szPlateau = new Size(Width, (int)(k * Width));
      if (szPlateau.Height > Height)
      {
        szPlateau = new Size((int)(Width / k), Height);
      }
      Point locationPlateau = new Point((Width - szPlateau.Width) / 2, (Height - szPlateau.Height) / 2);
      PbPlateau.Bounds = new Rectangle(locationPlateau, szPlateau);
      PbPlateau.Show();
    }

    private void PbPlateau_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.FillRectangle(new SolidBrush(BackColor), PbPlateau.ClientRectangle);
      if (Jeu == null)
      {
        return;
      }
      bool bResolu;
      if (ResolutionAutomatiqueEnCours)
      {
        bResolu = Jeu.ResolutionAutomatiqueIsResolu();
      }
      else
      {
        bResolu = Jeu.IsResolu();
      }
      SizeF szCasePlateau = new SizeF((float)PbPlateau.Width / Jeu.Largeur, (float)PbPlateau.Height / Jeu.Hauteur);
      SizeF szCaseImg = new SizeF(0, 0);
      if (Image != null)
      {
        szCaseImg = new SizeF((float)Image.Width / Jeu.Largeur, (float)Image.Height / Jeu.Hauteur);
      }
      StringFormat sf = new StringFormat();
      sf.Alignment = StringAlignment.Center;
      sf.LineAlignment = StringAlignment.Center;
      for (int y = 0; y < Jeu.Hauteur; y++)
      {
        for (int x = 0; x < Jeu.Largeur; x++)
        {
          int iPiece = Jeu.Indice(x, y);
          int posPiece;
          if (ResolutionAutomatiqueEnCours)
          {
            posPiece = Jeu.ResolutionAutomatiquePositionPiece(iPiece);
          }
          else
          {
            posPiece = Jeu.PositionPiece(iPiece);
          }
          Point coordPiece = Jeu.Coordonnees(posPiece);

          #region calcul cadre pièce
          // Eviter que les rectangles tracés autour des pièces n'empiètent les uns sur les autres
          // ou disparaissent en bordure de cadre.
          // Les positions théoriques sont exprimées en réels.
          // Le dessin au pixel exige des entiers.
          // Le coin supérieur gauche doit être arrondi au pixel supérieur
          // sauf si on est en bordure gauche ou supérieure.
          // le coin inférieur droit doit être arrondi au pixel inférieur
          // sauf si on est en bordure droite ou inférieure
          // coordonnées des coins supérieur gauche (x0,y0) et inférieur droit (x1,y1)
          int x0, x1, y0, y1;
          if (coordPiece.X == 0)
          {
            x0 = 0;
          }
          else
          {
            // on ajoute 1 pixel pour ne pas mordre sur le bord droit de la pièce de gauche
            // conversion vers 0, donc < à v si v > 0
            x0 = (int)(1 + coordPiece.X * szCasePlateau.Width);
          }
          if (coordPiece.Y == 0)
          {
            y0 = 0;
          }
          else
          {
            y0 = (int)(1 + coordPiece.Y * szCasePlateau.Height);
          }
          if (coordPiece.X == Jeu.Largeur - 1)
          {
            x1 = PbPlateau.Width;
          }
          else
          {
            x1 = (int)((1 + coordPiece.X) * szCasePlateau.Width);
          }
          if (coordPiece.Y == Jeu.Hauteur - 1)
          {
            y1 = PbPlateau.Height;
          }
          else
          {
            y1 = (int)((1 + coordPiece.Y) * szCasePlateau.Height);
          }
          #endregion calcul cadre pièce

          RectangleF rcF = new RectangleF(new PointF(coordPiece.X * szCasePlateau.Width, coordPiece.Y * szCasePlateau.Height), szCasePlateau);
          bool bDoOffset = AnimationEnCours;
          if (bDoOffset)
          {
            bDoOffset = (!ResolutionAutomatiqueEnCours && posPiece == PosPieceAnimation) ||
                        (ResolutionAutomatiqueEnCours && posPiece == Jeu.ResolutionAutomatiquePosPieceEnMouvement());
          }
          if (bDoOffset)
          {
            float k = (float)Etape / (float)NbEtapes;
            rcF.Offset(k * DeltaDrawAnimation.X, k * DeltaDrawAnimation.Y);
            x0 += (int)(k * DeltaDrawAnimation.X);
            x1 += (int)(k * DeltaDrawAnimation.X);
            y0 += (int)(k * DeltaDrawAnimation.Y);
            y1 += (int)(k * DeltaDrawAnimation.Y);
          }
          Rectangle rcCadrePiece = new Rectangle(x0, y0, x1 - x0, y1 - y0);
          if (Image == null)
          {
            if (bResolu || iPiece != Jeu.CaseVide)
            {
              e.Graphics.DrawString((iPiece + 1).ToString(), Font, Brushes.Black, rcF, sf);
            }
          }
          else
          {
            if (bResolu || iPiece != Jeu.CaseVide)
            {
              RectangleF rcFimg = new RectangleF(new PointF(x * szCaseImg.Width, y * szCaseImg.Height), szCaseImg);
              e.Graphics.DrawImage(Image, rcF, rcFimg, GraphicsUnit.Pixel);
            }
          }
          // traçage des lignes
          if (!bResolu && iPiece != Jeu.CaseVide)
          {
            e.Graphics.DrawRectangle(Pens.Black, rcCadrePiece);
          }
        }
      }
      // traçage des lignes
      if (!bResolu)
      {
        e.Graphics.DrawRectangle(new Pen(Color.Black, 3), PbPlateau.ClientRectangle);
      }
    }

    private void Plateau_Resize(object sender, EventArgs e)
    {
      PositionnePlateau();
    }
    public void ChangeImage(Image image)
    {
      Image = image;
      PbPlateau.Refresh();
    }

    private void PbPlateau_Click(object sender, EventArgs e)
    {
      if (Jeu == null)
      {
        return;
      }
      if (AnimationEnCours)
      {
        return;
      }
      // eh oui, un argument peut en cacher un autre
      MouseEventArgs ee = e as MouseEventArgs;
      SizeF szCasePlateau = new SizeF((float)PbPlateau.Width / Jeu.Largeur, (float)PbPlateau.Height / Jeu.Hauteur);
      // x tel que x*szCasePlateau.Width <= ee.X < (x+1)*szCasePlateau.Width
      int x = (int)(ee.X / szCasePlateau.Width);
      int y = (int)(ee.Y / szCasePlateau.Height);
      int posVide = Jeu.PositionPiece(Jeu.CaseVide);
      Point coordVide = Jeu.Coordonnees(posVide);
      if (x == coordVide.X && y == coordVide.Y)
      {
        return;
      }
      if (x != coordVide.X && y != coordVide.Y)
      {
        return;
      }
      PosDestination = Jeu.Indice(x, y);
      DeltaDrawAnimation = new PointF(0, 0);
      if (x < coordVide.X)
      {
        DeltaAnimation = -1;
        DeltaDrawAnimation.X = szCasePlateau.Width;
      }
      else if (x > coordVide.X)
      {
        DeltaAnimation = 1;
        DeltaDrawAnimation.X = -szCasePlateau.Width;
      }
      else if (y < coordVide.Y)
      {
        DeltaAnimation = -Jeu.Largeur;
        DeltaDrawAnimation.Y = szCasePlateau.Height;
      }
      else
      {
        DeltaAnimation = Jeu.Largeur;
        DeltaDrawAnimation.Y = -szCasePlateau.Height;
      }
      PosPieceAnimation = posVide + DeltaAnimation;
      // Debug.Print($"PosPieceAnimation={PosPieceAnimation}, DeltaDrawAnimation=({DeltaDrawAnimation.X};{DeltaDrawAnimation.Y})");
      StartAnimation();
    }

    private void StartAnimation()
    {
      TimerAnimation = new Timer { Interval = PeriodeAnimation };
      TimerAnimation.Tick += TimerAnimation_Tick;
      Etape = 0;
      AnimationEnCours = true;
      DoAnimation();
    }

    private void TimerAnimation_Tick(object sender, EventArgs e)
    {
      DoAnimation();
    }

    public event EventHandler<EventArgs> ResolutionAutomatiqueOver;
    private void DoAnimation()
    {
      TimerAnimation.Stop();
      Etape++;
      if (Etape > NbEtapes)
      {
        if (ResolutionAutomatiqueEnCours)
        {
          Trace("ResolutionAutomatiqueAvancePosition");
          int posVide = Jeu.ResolutionAutomatiqueAvancePosition();
          if (Jeu.ResolutionAutomatiqueIsResolu())
          {
            AnimationEnCours = false;
            ResolutionAutomatiqueEnCours = false;
            ResolutionAutomatiqueOver?.Invoke(this, EventArgs.Empty);
            PbPlateau.Refresh();
            return;
          }
          DeltaDrawAnimation = ResoudreAutomatiquementCalculeDeltaDrawAnimation();
          Etape = 0;
        }
        else
        {
          int posVide = Jeu.PositionPiece(Jeu.CaseVide);
          posVide += DeltaAnimation;
          Jeu.SwitchVide(posVide);
          if (posVide == PosDestination)
          {
            AnimationEnCours = false;
            PbPlateau.Refresh();
            return;
          }
          Etape = 0;
          PosPieceAnimation = posVide + DeltaAnimation;
        }
      }
      PbPlateau.Refresh();
      TimerAnimation.Start();
    }
    internal void ResoudreAutomatiquement()
    {
      Jeu.ResolutionAutomatiqueResoudre();
      ResolutionAutomatiqueEnCours = true;
      DeltaDrawAnimation = ResoudreAutomatiquementCalculeDeltaDrawAnimation();
      StartAnimation();
    }

    private PointF ResoudreAutomatiquementCalculeDeltaDrawAnimation()
    {
      PointF result = new PointF(0, 0);
      int posVide = Jeu.ResolutionAutomatiquePositionPiece(Jeu.CaseVide);
      int posPieceEnMouvement = Jeu.ResolutionAutomatiquePosPieceEnMouvement();
      Point coordVide = Jeu.Coordonnees(posVide);
      Point coordPosPieceEnMouvement = Jeu.Coordonnees(posPieceEnMouvement);
      if (coordVide.X < coordPosPieceEnMouvement.X)
      {
        result.X = -(float)PbPlateau.Width / (float)Jeu.Largeur;
      }
      else if (coordVide.X > coordPosPieceEnMouvement.X)
      {
        result.X = (float)PbPlateau.Width / (float)Jeu.Largeur;
      }
      else if (coordVide.Y < coordPosPieceEnMouvement.Y)
      {
        result.Y = -(float)PbPlateau.Height / (float)Jeu.Hauteur;
      }
      else
      {
        result.Y = (float)PbPlateau.Height / (float)Jeu.Hauteur;
      }
      return result;
    }

    private void Trace(string msg)
    {
      Debug.Print($"{DateTime.Now:HH:mm:ss.fff} {System.Threading.Thread.CurrentThread.ManagedThreadId} {msg}");
    }
  }
}
