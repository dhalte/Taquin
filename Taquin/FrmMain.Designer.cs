namespace Taquin
{
  partial class FrmMain
  {
    /// <summary>
    /// Variable nécessaire au concepteur.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Nettoyage des ressources utilisées.
    /// </summary>
    /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Code généré par le Concepteur Windows Form

    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.TabMain = new System.Windows.Forms.TabControl();
      this.TabParam = new System.Windows.Forms.TabPage();
      this.TbImage = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.TbHauteur = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.TbLargeur = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.TabJeu = new System.Windows.Forms.TabPage();
      this.TableJeu = new System.Windows.Forms.TableLayoutPanel();
      this.PnlControles = new System.Windows.Forms.Panel();
      this.BtResoudre = new System.Windows.Forms.Button();
      this.BtNouveauJeu = new System.Windows.Forms.Button();
      this.PnlChoixImageTexte = new System.Windows.Forms.Panel();
      this.RbTexte = new System.Windows.Forms.RadioButton();
      this.RbImage = new System.Windows.Forms.RadioButton();
      this.PlateauJeu = new Taquin.Plateau();
      this.PnlBoutons = new System.Windows.Forms.Panel();
      this.TabMain.SuspendLayout();
      this.TabParam.SuspendLayout();
      this.TabJeu.SuspendLayout();
      this.TableJeu.SuspendLayout();
      this.PnlControles.SuspendLayout();
      this.PnlChoixImageTexte.SuspendLayout();
      this.PnlBoutons.SuspendLayout();
      this.SuspendLayout();
      // 
      // TabMain
      // 
      this.TabMain.Controls.Add(this.TabParam);
      this.TabMain.Controls.Add(this.TabJeu);
      this.TabMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TabMain.Location = new System.Drawing.Point(0, 0);
      this.TabMain.Name = "TabMain";
      this.TabMain.SelectedIndex = 0;
      this.TabMain.Size = new System.Drawing.Size(800, 450);
      this.TabMain.TabIndex = 0;
      this.TabMain.SelectedIndexChanged += new System.EventHandler(this.TabMain_SelectedIndexChanged);
      // 
      // TabParam
      // 
      this.TabParam.Controls.Add(this.TbImage);
      this.TabParam.Controls.Add(this.label3);
      this.TabParam.Controls.Add(this.TbHauteur);
      this.TabParam.Controls.Add(this.label2);
      this.TabParam.Controls.Add(this.TbLargeur);
      this.TabParam.Controls.Add(this.label1);
      this.TabParam.Location = new System.Drawing.Point(4, 22);
      this.TabParam.Name = "TabParam";
      this.TabParam.Padding = new System.Windows.Forms.Padding(3);
      this.TabParam.Size = new System.Drawing.Size(792, 424);
      this.TabParam.TabIndex = 0;
      this.TabParam.Text = "Réglages";
      this.TabParam.UseVisualStyleBackColor = true;
      // 
      // TbImage
      // 
      this.TbImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TbImage.Location = new System.Drawing.Point(91, 117);
      this.TbImage.Name = "TbImage";
      this.TbImage.Size = new System.Drawing.Size(693, 20);
      this.TbImage.TabIndex = 5;
      this.TbImage.Text = "C:\\Users\\halte\\Pictures\\1120x490-Montagne.jpg";
      this.TbImage.TextChanged += new System.EventHandler(this.TbImage_TextChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(35, 120);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(36, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Image";
      // 
      // TbHauteur
      // 
      this.TbHauteur.Location = new System.Drawing.Point(91, 76);
      this.TbHauteur.Name = "TbHauteur";
      this.TbHauteur.Size = new System.Drawing.Size(100, 20);
      this.TbHauteur.TabIndex = 3;
      this.TbHauteur.Text = "4";
      this.TbHauteur.TextChanged += new System.EventHandler(this.TbHauteur_TextChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(32, 79);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(45, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Hauteur";
      // 
      // TbLargeur
      // 
      this.TbLargeur.Location = new System.Drawing.Point(91, 36);
      this.TbLargeur.Name = "TbLargeur";
      this.TbLargeur.Size = new System.Drawing.Size(100, 20);
      this.TbLargeur.TabIndex = 1;
      this.TbLargeur.Text = "4";
      this.TbLargeur.TextChanged += new System.EventHandler(this.TbLargeur_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(32, 39);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(43, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Largeur";
      // 
      // TabJeu
      // 
      this.TabJeu.Controls.Add(this.TableJeu);
      this.TabJeu.Location = new System.Drawing.Point(4, 22);
      this.TabJeu.Name = "TabJeu";
      this.TabJeu.Padding = new System.Windows.Forms.Padding(3);
      this.TabJeu.Size = new System.Drawing.Size(792, 424);
      this.TabJeu.TabIndex = 1;
      this.TabJeu.Text = "Jeu";
      this.TabJeu.UseVisualStyleBackColor = true;
      // 
      // TableJeu
      // 
      this.TableJeu.ColumnCount = 2;
      this.TableJeu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.TableJeu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
      this.TableJeu.Controls.Add(this.PlateauJeu, 0, 0);
      this.TableJeu.Controls.Add(this.PnlControles, 1, 0);
      this.TableJeu.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TableJeu.Location = new System.Drawing.Point(3, 3);
      this.TableJeu.Name = "TableJeu";
      this.TableJeu.RowCount = 1;
      this.TableJeu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.TableJeu.Size = new System.Drawing.Size(786, 418);
      this.TableJeu.TabIndex = 0;
      // 
      // PnlControles
      // 
      this.PnlControles.Controls.Add(this.PnlBoutons);
      this.PnlControles.Controls.Add(this.PnlChoixImageTexte);
      this.PnlControles.Dock = System.Windows.Forms.DockStyle.Fill;
      this.PnlControles.Location = new System.Drawing.Point(639, 3);
      this.PnlControles.Name = "PnlControles";
      this.PnlControles.Size = new System.Drawing.Size(144, 412);
      this.PnlControles.TabIndex = 1;
      // 
      // BtResoudre
      // 
      this.BtResoudre.Location = new System.Drawing.Point(29, 32);
      this.BtResoudre.Name = "BtResoudre";
      this.BtResoudre.Size = new System.Drawing.Size(75, 23);
      this.BtResoudre.TabIndex = 8;
      this.BtResoudre.Text = "Résoudre";
      this.BtResoudre.UseVisualStyleBackColor = true;
      this.BtResoudre.Click += new System.EventHandler(this.BtResoudre_Click);
      // 
      // BtNouveauJeu
      // 
      this.BtNouveauJeu.Location = new System.Drawing.Point(29, 3);
      this.BtNouveauJeu.Name = "BtNouveauJeu";
      this.BtNouveauJeu.Size = new System.Drawing.Size(75, 23);
      this.BtNouveauJeu.TabIndex = 7;
      this.BtNouveauJeu.Text = "Nouveau";
      this.BtNouveauJeu.UseVisualStyleBackColor = true;
      this.BtNouveauJeu.Click += new System.EventHandler(this.BtNouveauJeu_Click);
      // 
      // PnlChoixImageTexte
      // 
      this.PnlChoixImageTexte.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PnlChoixImageTexte.Controls.Add(this.RbTexte);
      this.PnlChoixImageTexte.Controls.Add(this.RbImage);
      this.PnlChoixImageTexte.Location = new System.Drawing.Point(4, 4);
      this.PnlChoixImageTexte.Name = "PnlChoixImageTexte";
      this.PnlChoixImageTexte.Size = new System.Drawing.Size(137, 57);
      this.PnlChoixImageTexte.TabIndex = 0;
      // 
      // RbTexte
      // 
      this.RbTexte.AutoSize = true;
      this.RbTexte.Location = new System.Drawing.Point(4, 28);
      this.RbTexte.Name = "RbTexte";
      this.RbTexte.Size = new System.Drawing.Size(52, 17);
      this.RbTexte.TabIndex = 1;
      this.RbTexte.TabStop = true;
      this.RbTexte.Text = "Texte";
      this.RbTexte.UseVisualStyleBackColor = true;
      // 
      // RbImage
      // 
      this.RbImage.AutoSize = true;
      this.RbImage.Location = new System.Drawing.Point(4, 4);
      this.RbImage.Name = "RbImage";
      this.RbImage.Size = new System.Drawing.Size(54, 17);
      this.RbImage.TabIndex = 0;
      this.RbImage.TabStop = true;
      this.RbImage.Text = "Image";
      this.RbImage.UseVisualStyleBackColor = true;
      this.RbImage.CheckedChanged += new System.EventHandler(this.RbImage_CheckedChanged);
      // 
      // PlateauJeu
      // 
      this.PlateauJeu.Dock = System.Windows.Forms.DockStyle.Fill;
      this.PlateauJeu.Location = new System.Drawing.Point(3, 3);
      this.PlateauJeu.Name = "PlateauJeu";
      this.PlateauJeu.Size = new System.Drawing.Size(630, 412);
      this.PlateauJeu.TabIndex = 0;
      this.PlateauJeu.ResolutionAutomatiqueOver += new System.EventHandler<System.EventArgs>(this.PlateauJeu_ResolutionAutomatiqueOver);
      // 
      // PnlBoutons
      // 
      this.PnlBoutons.Controls.Add(this.BtResoudre);
      this.PnlBoutons.Controls.Add(this.BtNouveauJeu);
      this.PnlBoutons.Location = new System.Drawing.Point(4, 67);
      this.PnlBoutons.Name = "PnlBoutons";
      this.PnlBoutons.Size = new System.Drawing.Size(137, 58);
      this.PnlBoutons.TabIndex = 1;
      // 
      // FrmMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.TabMain);
      this.Name = "FrmMain";
      this.Text = "Form1";
      this.TabMain.ResumeLayout(false);
      this.TabParam.ResumeLayout(false);
      this.TabParam.PerformLayout();
      this.TabJeu.ResumeLayout(false);
      this.TableJeu.ResumeLayout(false);
      this.PnlControles.ResumeLayout(false);
      this.PnlChoixImageTexte.ResumeLayout(false);
      this.PnlChoixImageTexte.PerformLayout();
      this.PnlBoutons.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl TabMain;
    private System.Windows.Forms.TabPage TabParam;
    private System.Windows.Forms.TextBox TbImage;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox TbHauteur;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox TbLargeur;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TabPage TabJeu;
    private System.Windows.Forms.TableLayoutPanel TableJeu;
    private Plateau PlateauJeu;
    private System.Windows.Forms.Panel PnlControles;
    private System.Windows.Forms.Panel PnlChoixImageTexte;
    private System.Windows.Forms.RadioButton RbTexte;
    private System.Windows.Forms.RadioButton RbImage;
    private System.Windows.Forms.Button BtNouveauJeu;
    private System.Windows.Forms.Button BtResoudre;
    private System.Windows.Forms.Panel PnlBoutons;
  }
}

