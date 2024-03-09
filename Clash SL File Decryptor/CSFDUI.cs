using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Reflection;
using System.Windows.Forms;
using CSFD.Core;
using CSFD.Core.Threading;
using static CSFD.Core.Threading.ActionThread.Types;

namespace CSFD
{
    public partial class CSFDUI : MaterialForm
    {
        public CSFDUI()
        {
            InitializeComponent();
            var Skin = MaterialSkinManager.Instance;
            Skin.AddFormToManage(this);
            Skin.Theme = MaterialSkinManager.Themes.DARK;
            Skin.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Grey500, Accent.Blue200, TextShade.WHITE);
        }

        private void CSFDUI_Load(object sender, System.EventArgs e)
        {
            CSFDUI UI = (CSFDUI)Application.OpenForms["CSFDUI"];
            UI.Text = "Clash SL File De/Compressor v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " - © 2021";
            CheckerThread.Start();
        }

        private void BtnDecCsv_Click(object sender, System.EventArgs e)
        {
            ActionThread.Start(CsvDecrypt);
        }

        private void BtnEncCsv_Click(object sender, System.EventArgs e)
        {
            ActionThread.Start(CsvEncrypt);
        }

        private void BtnDecSC_Click(object sender, System.EventArgs e)
        {
             ActionThread.Start(ScDecrypt);
        }

        private void BtnEncSC_Click(object sender, System.EventArgs e)
        {
            ActionThread.Start(ScEncrypt);
        }

        private void RadioVersion7Lower_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ScVersion = true;
        }

        private void RadioHigher_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ScVersion = false;
        }
    }
}
