using System;
using System.IO;
using OfficeOpenXml;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaquinCalculZone;
using System.Drawing;

namespace UnitTestTaquin
{
  /// <summary>
  /// Description résumée pour TestCalculZone
  /// </summary>
  [TestClass]
  public class TestCalculZone
  {
    [TestMethod]
    public void TestCalcul2x3()
    {
      Calcul calcul = new Calcul();
      Size sz = new Size(2, 3);
      List<IList<int>> positionsGagnantes = new List<IList<int>>();
      
      positionsGagnantes.Add(new int[] { 0, 1, 2 });
      positionsGagnantes.Add(new int[] { 0, 1, 3 });
      int handler = calcul.ComputeDock(sz, 3, positionsGagnantes);
      // Cette position est celle qui demande le plus grand nombre de mouvements pour sa résolution : 18
      IList<int> position = new int[] { 1, 0, 5 };
      IList<int> solution = calcul.GetSolution(handler, position);
      IList<int> solutionAttendue = new int[] { 3, 1, 0, 2, 3, 5, 4, 2, 0, 1, 3, 2, 4, 5, 3, 1, 0, 2 };
      Assert.AreEqual(solution.Count, solutionAttendue.Count);
      for (int i = 0; i < solution.Count; i++)
      {
        Assert.AreEqual(solution[i], solutionAttendue[i]);
      }
    }

    [TestMethod]
    public void TestDump2x3()
    {
      Calcul calcul = new Calcul();
      Size sz = new Size(2, 3);
      List<IList<int>> positionsGagnantes = new List<IList<int>>();
      positionsGagnantes.Add(new int[] { 0, 1, 2 });
      positionsGagnantes.Add(new int[] { 0, 1, 3 });
      int handler = calcul.ComputeDock(sz, 3, positionsGagnantes);

      List<List<string>> dump = calcul.DumpDock(handler);
      FileInfo fi = new FileInfo("Dump2x3.xlsx");
      using (ExcelPackage excel = new ExcelPackage())
      {
        ExcelWorksheet ws = excel.Workbook.Worksheets.Add("dump");
        int row, col;
        row = 1;
        foreach (List<string> itemRow in dump)
        {
          col = 1;
          foreach (string item in itemRow)
          {
            int x, y;
            for (int i = 0; i < item.Length; i++)
            {
              x = i % 2;
              y = i / 2;
              ExcelRange cell = ws.Cells[row + y, col + x];
              cell.Value = "" + item[i];
              cell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            }
            col += 3;
          }

          row += 4;
        }
        excel.SaveAs(fi);
      }
      System.Diagnostics.Process.Start(fi.FullName);
    }

    [TestMethod]
    public void TestCalcul2x2()
    {
      Calcul calcul = new Calcul();
      Size sz = new Size(2, 2);
      List<IList<int>> positionsGagnantes = new List<IList<int>>();

      positionsGagnantes.Add(new int[] { 2, 3 });
      int handler = calcul.ComputeDock(sz, 2, positionsGagnantes);
      // Cette position est celle qui demande le plus grand nombre de mouvements pour sa résolution : 6
      IList<int> position = new int[] { 1, 0 };
      IList<int> solution = calcul.GetSolution(handler, position);
      IList<int> solutionAttendue = new int[] { 1, 3, 2, 0, 1, 3 };
      Assert.AreEqual(solution.Count, solutionAttendue.Count);
      for (int i = 0; i < solution.Count; i++)
      {
        Assert.AreEqual(solution[i], solutionAttendue[i]);
      }
    }

    [TestMethod]
    public void TestDump2x2()
    {
      Calcul calcul = new Calcul();
      Size sz = new Size(2, 2);
      List<IList<int>> positionsGagnantes = new List<IList<int>>();
      positionsGagnantes.Add(new int[] { 2, 3 });
      int handler = calcul.ComputeDock(sz, 2, positionsGagnantes);

      List<List<string>> dump = calcul.DumpDock(handler);
      FileInfo fi = new FileInfo("Dump2x2.xlsx");
      using (ExcelPackage excel = new ExcelPackage())
      {
        ExcelWorksheet ws = excel.Workbook.Worksheets.Add("dump");
        int row, col;
        row = 1;
        foreach (List<string> itemRow in dump)
        {
          col = 1;
          foreach (string item in itemRow)
          {
            int x, y;
            for (int i = 0; i < item.Length; i++)
            {
              x = i % 2;
              y = i / 2;
              ExcelRange cell = ws.Cells[row + y, col + x];
              cell.Value = "" + item[i];
              cell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            }
            col += 3;
          }

          row += 4;
        }
        excel.SaveAs(fi);
      }
      System.Diagnostics.Process.Start(fi.FullName);
    }
  }
}
