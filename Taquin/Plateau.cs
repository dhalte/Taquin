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
    private int PosDestination;
    private int DeltaAnimation;
    private int PosPieceAnimation;
    private PointF DeltaDrawAnimation = new PointF(0, 0);
    private int Etape;
    private int NbEtapes = 20;
    private System.Windows.Forms.Timer TimerAnimation;
    private int PeriodeAnimation = 20;
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
      bool bResolu = Jeu.IsResolu();
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
          int posPiece = Jeu.PositionPiece(iPiece);
          Point coordPiece = Jeu.Coordonnees(posPiece);
          RectangleF rcF = new RectangleF(new PointF(coordPiece.X * szCasePlateau.Width, coordPiece.Y * szCasePlateau.Height), szCasePlateau);
          if (posPiece == PosPieceAnimation && AnimationEnCours)
          {
            float k = (float)Etape / (float)NbEtapes;
            rcF.Offset(k * DeltaDrawAnimation.X, k * DeltaDrawAnimation.Y);
            // Debug.Print($"PosPieceAnimation={PosPieceAnimation}, rcF=({rcF.X};{rcF.Y})");
          }
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
          if (!bResolu)
          {
            e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(rcF));
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

    private void DoAnimation()
    {
      TimerAnimation.Stop();
      Etape++;
      if (Etape > NbEtapes)
      {
        int posVide = Jeu.PositionPiece(Jeu.CaseVide);
        posVide += DeltaAnimation;
        Jeu.SwitchVide(posVide);
        if (posVide == PosDestination)
        {
          AnimationEnCours = false;
          return;
        }
        Etape = 0;
        PosPieceAnimation = posVide + DeltaAnimation;
      }
      PbPlateau.Refresh();
      TimerAnimation.Start();
    }
  }
}
