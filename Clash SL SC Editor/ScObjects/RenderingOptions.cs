using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace CSSCE
{
    class RenderingOptions
    {
        public bool ViewPolygons { get; set; }

        public RenderingOptions()
        {
            this.ViewPolygons = false;
        }
    }
}