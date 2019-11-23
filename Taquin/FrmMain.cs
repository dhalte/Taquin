using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaquinLib;

namespace Taquin
{
  public partial class FrmMain : Form
  {
    private bool bJeuEnCours;
    private int Largeur;
    private int Hauteur;
    private Image Image;
    public FrmMain()
    {
      InitializeComponent();
    }

    private void TabMain_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (TabMain.SelectedTab == TabJeu)
      {
        InitJeu();
      }
    }

    private void InitJeu()
    {
      if (!bJeuEnCours)
      {
        bJeuEnCours = TryInitJeu();
        if (bJeuEnCours)
        {
          Jeu jeu = new Jeu(Largeur, Hauteur);
          PlateauJeu.Init(jeu, Image);
          PlateauJeu.Show();
          PnlBoutons.Enabled = true;
        }
        else
        {
          PlateauJeu.Hide();
          PnlBoutons.Enabled = false;
        }
      }
    }

    private bool TryInitJeu()
    {
      if (!int.TryParse(TbLargeur.Text, out Largeur))
      {
        return false;
      }
      if (Largeur <= 1)
      {
        return false;
      }
      if (!int.TryParse(TbHauteur.Text, out Hauteur))
      {
        return false;
      }
      if (Hauteur <= 1)
      {
        return false;
      }
      string imgFileName = TbImage.Text;
      RbImage.CheckedChanged -= RbImage_CheckedChanged;
      RbTexte.Checked = true;
      PnlChoixImageTexte.Enabled = false;
      Image = null;
      if (!string.IsNullOrEmpty(imgFileName))
      {
        try
        {
          Image = Image.FromFile(imgFileName);
          RbImage.Checked = true;
          PnlChoixImageTexte.Enabled = true;
          RbImage.CheckedChanged += RbImage_CheckedChanged;
        }
        catch (Exception)
        {
          return false;
        }
      }
      return true;
    }

    private void TbLargeur_TextChanged(object sender, EventArgs e)
    {
      bJeuEnCours = false;
    }

    private void TbHauteur_TextChanged(object sender, EventArgs e)
    {
      bJeuEnCours = false;
    }

    private void TbImage_TextChanged(object sender, EventArgs e)
    {
      bJeuEnCours = false;
    }

    private void RbImage_CheckedChanged(object sender, EventArgs e)
    {
      if (RbImage.Checked)
      {
        PlateauJeu.ChangeImage(Image);
      }
      else
      {
        PlateauJeu.ChangeImage(null);
      }
    }
    
    private void BtNouveauJeu_Click(object sender, EventArgs e)
    {
      bJeuEnCours = false;
      InitJeu();
      PlateauJeu.Refresh();
    }

    // Lancement de l'animation de résolution automatique
    private void BtResoudre_Click(object sender, EventArgs e)
    {
      PnlBoutons.Enabled = false;
      // Asynchrone
      ResoudreAutomatiquement();
    }
    private void ResoudreAutomatiquement()
    {
      PlateauJeu.ResoudreAutomatiquement();      
    }

    private void PlateauJeu_ResolutionAutomatiqueOver(object sender, EventArgs e)
    {
      PnlBoutons.Enabled = true;
    }
  }
}
