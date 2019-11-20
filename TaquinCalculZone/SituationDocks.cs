using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaquinCalculZone
{
  internal class SituationDocks : List<SituationDock>
  {
    internal SituationDock Get(int handler)
    {
      return this[handler];
    }
  }
}
