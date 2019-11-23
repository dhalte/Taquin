namespace Taquin
{
  partial class Plateau
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

    #region Code généré par le Concepteur de composants

    /// <summary> 
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.PbPlateau = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.PbPlateau)).BeginInit();
      this.SuspendLayout();
      // 
      // PbPlateau
      // 
      this.PbPlateau.Location = new System.Drawing.Point(34, 50);
      this.PbPlateau.Name = "PbPlateau";
      this.PbPlateau.Size = new System.Drawing.Size(100, 50);
      this.PbPlateau.TabIndex = 0;
      this.PbPlateau.TabStop = false;
      this.PbPlateau.Visible = false;
      this.PbPlateau.Click += new System.EventHandler(this.PbPlateau_Click);
      this.PbPlateau.Paint += new System.Windows.Forms.PaintEventHandler(this.PbPlateau_Paint);
      // 
      // Plateau
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.PbPlateau);
      this.Name = "Plateau";
      this.Resize += new System.EventHandler(this.Plateau_Resize);
      ((System.ComponentModel.ISupportInitialize)(this.PbPlateau)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox PbPlateau;
  }
}
