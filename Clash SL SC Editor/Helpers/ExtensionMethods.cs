using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace CSSCE
{
    static class ExtensionMethods
    {
        public static void Populate(this TreeView tv, List<ScObject> scd)
        {
            foreach (ScObject data in scd)
            {
                var dataTypeKey = data.GetDataType().ToString();
                var dataTypeName = data.GetDataTypeName();
                var id = data.GetId().ToString();
                if (!tv.Nodes.ContainsKey(dataTypeKey))
                {
                    tv.Nodes.Add(dataTypeKey, dataTypeName);
                }
                tv.Nodes[dataTypeKey].Nodes.Add(id, data.GetName());
                tv.Nodes[dataTypeKey].Nodes[id].Tag = data;
                tv.Nodes[dataTypeKey].Nodes[id].PopulateChildren(data);
            }
        }

        public static void PopulateChildren(this TreeNode tn, ScObject sco)
        {
            foreach (var child in sco.GetChildren())
            {
                tn.Nodes.Add(child.GetId().ToString(), child.GetName());
                tn.Nodes[child.GetId().ToString()].Tag = child;
                PopulateChildren(tn.Nodes[child.GetId().ToString()], child);
            }
        }
    }
}
