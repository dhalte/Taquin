using OfficeOpenXml;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace CarreMagique
{
  // Recherche des carrés magiques "normaux" de taille 4
  // Placer les 16 valeurs de 1 à 6 en un carré 4x4 
  // de manière que les sommes des lignes, colonnes, diagonales principales soient identiques.
  //
  // 1+..+16 = 16*17/2
  // donc chaque ligne a pour somme 16*17/2/4=34
  //
  public class Recherche
  {
    public List<Arrangement> arrangements;
    public List<Carre> solutions;
    // Pour limiter les solutions équivalentes par simple rotation ou symétrie,
    // on impose au nombre 1 d'être en (0,0), (1,0) ou (1,1).
    // Cela réduit aussi grandement le temps de recherche.
    Point[] positionsNombre1 = new Point[] { new Point(0, 0), new Point(1, 0), new Point(1, 1) };
    Point positionNombre1;
    public void RechercheArrangements()
    {
      Stopwatch sw = Stopwatch.StartNew();
      arrangements = new List<Arrangement>();
      Arrangement argt = new Arrangement(new int[] { 0, 0, 0, 0 });
      // 2064 arrangements de 4 sur 16 de somme 34, en 4 ms
      RechercheArrangements(0, argt);
      Debug.Print($"{arrangements.Count} arrangements de 4 sur 16 de somme 34, en {sw.ElapsedMilliseconds} ms");
      sw.Restart();
      solutions = new List<Carre>();
      Carre carre = new Carre();
      foreach (Point positionNombre1 in positionsNombre1)
      {
        this.positionNombre1 = positionNombre1;
        RechecheSolutions(0, carre);
      }
      Debug.Print($"{solutions.Count} solutions de carré magique 4x4 avec les nombres 1 à 16, en {sw.ElapsedMilliseconds} ms");
      // 1296 solutions de carré magique 4x4 normaux, en 2h15mn
      SauveSolutions();
    }

    // Recherche des arrangements de 4 sur 16 dont la somme est 34
    private void RechercheArrangements(int lvl, Arrangement argt)
    {
      for (int i = 1; i <= 16; i++)
      {
        if (!argt.Contient(lvl, i))
        {
          argt[lvl] = i;
          if (lvl == 3)
          {
            if (argt.Somme == 34)
            {
              arrangements.Add(new Arrangement(argt));
            }
          }
          else
          {
            RechercheArrangements(lvl + 1, argt);
          }
        }
      }
    }

    private void RechecheSolutions(int lvl, Carre carre)
    {
      foreach (Arrangement argt in arrangements)
      {
        if (!ArrangementCompatibleAvecPositionNombre1(argt, lvl))
        {
          continue;
        }
        if (carre.Compatible(lvl, argt))
        {
          carre[lvl] = argt;
          if (lvl == 3)
          {
            if (carre.Solution)
            {
              solutions.Add(new Carre(carre));
            }
          }
          else
          {
            RechecheSolutions(lvl + 1, carre);
          }
        }
      }
    }

    // lvl == 0 : 
    //   soit positionNombre1 == (0,0) et argt[0] == 1
    //   soit positionNombre1 == (1,0) et argt[1] == 1
    //   soit positionNombre1 == (1,1) et argt[0] != 1 et argt[1] != 1
    // lvl == 1 : 
    //   soit positionNombre1 == (0,0) et argt[0] != 1 et argt[1] != 1
    //   soit positionNombre1 == (1,0) et argt[0] != 1 et argt[1] != 1
    //   soit positionNombre1 == (1,1) et argt[1] == 1
    // lvl >  1 :
    //   argt[0] != 1 et argt[1] != 1
    private bool ArrangementCompatibleAvecPositionNombre1(Arrangement argt, int lvl)
    {
      switch (lvl)
      {
        case 0:
          if (positionNombre1 == positionsNombre1[0])
          {
            return argt[0] == 1;
          }
          if (positionNombre1 == positionsNombre1[1])
          {
            return argt[1] == 1;
          }
          return argt[0] != 1 && argt[1] != 1;
        case 1:
          if (positionNombre1 == positionsNombre1[0])
          {
            return argt[0] != 1 && argt[1] != 1;
          }
          if (positionNombre1 == positionsNombre1[1])
          {
            return argt[0] != 1 && argt[1] != 1;
          }
          return argt[1] == 1;
        default:
          return argt[0] != 1 && argt[1] != 1;
      }
    }

    private void SauveSolutions()
    {
      string fileName = $"CarreMagique {DateTime.Now:yyyy-MM-dd HH-mm-ss}.xlsx";
      fileName = Path.Combine(Environment.CurrentDirectory, fileName);
      using (ExcelPackage excel = new ExcelPackage(new FileInfo(fileName)))
      {
        ExcelWorkbook wb = excel.Workbook;
        ExcelWorksheet sheet = wb.Worksheets.Add("Solutions");
        int idxSolution = 0;
        int idxRow = 1, idxCol = 1;
        foreach (Carre carre in solutions)
        {
          SauveSolution(carre, sheet, idxRow, idxCol);
          idxCol += 5;
          ++idxSolution;
          if ((idxSolution % 10) == 0)
          {
            idxRow += 5;
            idxCol = 1;
          }
        }
        excel.Save();
      }
      Process.Start(fileName);
    }

    private void SauveSolution(Carre carre, ExcelWorksheet sheet, int idxRow, int idxCol)
    {
      for (int y = 0; y < 4; y++)
      {
        for (int x = 0; x < 4; x++)
        {
          ExcelRange range = sheet.Cells[idxRow + y, idxCol + x];
          range.Value = carre[y][x];
          range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
          range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
          range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        }
      }
    }
  }
  public class Arrangement : List<int>
  {
    public Arrangement(IEnumerable<int> collection) : base(collection)
    {
    }
    public bool Contient(int lvl, int value)
    {
      for (int i = 0; i < lvl; i++)
      {
        if (this[i] == value)
        {
          return true;
        }
      }
      return false;
    }
    public int Somme
    {
      get
      {
        int somme = 0;
        for (int i = 0; i < 4; i++)
        {
          somme += this[i];
        }
        return somme;
      }
    }
  }
  public class Carre : List<Arrangement>
  {
    public Carre()
    {
      for (int i = 0; i < 4; i++)
      {
        this.Add(null);
      }
    }

    public Carre(IEnumerable<Arrangement> collection) : base(collection)
    {
    }

    public bool Solution
    {
      get
      {
        for (int x = 0; x < 4; x++)
        {
          if (!ColonneOk(x))
          {
            return false;
          }
        }

        return DiagsOK;
      }
    }

    public bool DiagsOK
    {
      get
      {
        int diag0 = 0, diag1 = 0;
        for (int y = 0; y < 4; y++)
        {
          diag0 += this[y][y];
          diag1 += this[y][3 - y];
        }
        return diag0 == 34 && diag1 == 34;
      }
    }

    private bool ColonneOk(int x)
    {
      int somme = 0;
      for (int y = 0; y < 4; y++)
      {
        somme += this[y][x];
      }
      return somme == 34;
    }

    // les lignes précédentes ne contiennent pas de nombre identique à ceux de la ligne courante.
    internal bool Compatible(int lvl, Arrangement argt)
    {
      for (int y = 0; y < lvl; y++)
      {
        for (int x = 0; x < 4; x++)
        {
          int v = this[y][x];
          if (argt.Contient(4, v))
          {
            return false;
          }
        }
      }
      return true;
    }
  }

}
