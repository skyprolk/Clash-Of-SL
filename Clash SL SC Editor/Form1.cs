using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
//using System.Windows.Shapes;

namespace csssceditor
{
    public partial class Form1 : Form
    {
        private Decoder m_vStorageObject;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    LoadSC(dialog.FileName);  
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pictureBox1.Image = null;
            label1.Text = null;
            Render();
            RefreshMenu();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void viewPolygonsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Render();
        }

        public void RefreshMenu()
        {
            textureToolStripMenuItem.Visible = false;
            shapeToolStripMenuItem.Visible = false;
            objectToolStripMenuItem.Visible = false;
            chunkToolStripMenuItem.Visible = false;
            if(treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag != null)
                {
                    ScObject data = (ScObject)treeView1.SelectedNode.Tag;
                    switch(data.GetDataType())
                    {
                        case 99:
                            chunkToolStripMenuItem.Visible = true;
                            break;
                        case 0:
                            shapeToolStripMenuItem.Visible = true;
                            break;
                        case 2:
                            textureToolStripMenuItem.Visible = true;
                            break;
                        case 7:
                            objectToolStripMenuItem.Visible = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void LoadSC(string fileName)
        {
            m_vStorageObject = new Decoder(fileName);
            m_vStorageObject.Decode();

            treeView1.Nodes.Clear();
            pictureBox1.Image = null;
            label1.Text = null;
            RefreshMenu();
            treeView1.Populate(m_vStorageObject.GetExports());
            //treeView1.Populate(m_vStorageObject.GetShapes());
            treeView1.Populate(m_vStorageObject.GetTextures());
            //treeView1.Populate(m_vStorageObject.GetMovieClips());
        }

        private void Render()
        {
            RenderingOptions options = new RenderingOptions()
            {
                ViewPolygons = viewPolygonsToolStripMenuItem.Checked
            };

            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag != null)
                {
                    ScObject data = (ScObject)treeView1.SelectedNode.Tag;
                    pictureBox1.Image = data.Render(options);
                    pictureBox1.Refresh();
                    label1.Text = data.GetInfo();
                }
            }   
        }

        public void Export()
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Image File | *.png";
                string filename = "export";
                if (treeView1.SelectedNode.Text != null)
                    filename = treeView1.SelectedNode.Text;
                dlg.FileName = filename + ".png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(dlg.FileName))
                        File.Delete(dlg.FileName);
                    pictureBox1.Image.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
                    
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FileStream input = new FileStream(m_vStorageObject.GetFileName(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                m_vStorageObject.Save(input);
            }
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void exportToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void exportToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    Bitmap chunk = (Bitmap)Image.FromFile(dialog.FileName);
                    if (treeView1.SelectedNode != null)
                    {
                        if (treeView1.SelectedNode.Tag != null)
                        {
                            ShapeChunk data = (ShapeChunk)treeView1.SelectedNode.Tag;
                            data.Replace(chunk);
                            m_vStorageObject.AddChange(m_vStorageObject.GetTextures()[data.GetTextureId()]);
                            Render();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag != null)
                {
                    Texture data = new Texture((Texture)treeView1.SelectedNode.Tag);
                    m_vStorageObject.AddTexture(data);
                    m_vStorageObject.AddChange(data);
                    treeView1.Populate(new List<ScObject>() { data });
                }
            }
        }

        private void changeTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceTexture form = new ReplaceTexture();
            List<short> textureIds = new List<short>();
            foreach (Texture texture in m_vStorageObject.GetTextures())
                textureIds.Add(texture.GetTextureId());
            ((ComboBox)form.Controls["comboBox1"]).DataSource = textureIds;
            if(form.ShowDialog() == DialogResult.OK)
            {
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Tag != null)
                    {
                        ShapeChunk data = (ShapeChunk)treeView1.SelectedNode.Tag;
                        data.SetTextureId(Convert.ToByte(((ComboBox)form.Controls["comboBox1"]).SelectedItem));
                        m_vStorageObject.AddChange(data);
                        Render();
                    }
                }
            }
            form.Dispose();
        }

        private void duplicateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag != null)
                {
                    Export data = (Export)treeView1.SelectedNode.Tag;
                    CloneExport form = new CloneExport();
                    ((TextBox)form.Controls["textBox1"]).Text = data.GetName();
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        string result = ((TextBox)form.Controls["textBox1"]).Text;
                        if(result != "" && m_vStorageObject.GetExports().FindIndex(exp => exp.GetName() == result) == -1 )
                        {
                            MovieClip mv = new MovieClip((MovieClip)data.GetDataObject());
                            m_vStorageObject.AddMovieClip(mv);
                            m_vStorageObject.AddChange(mv);
                            Export ex = new Export(m_vStorageObject);
                            ex.SetId(mv.GetId());
                            ex.SetExportName(result);
                            ex.SetDataObject(mv);
                            m_vStorageObject.AddExport(ex);
                            m_vStorageObject.AddChange(ex);
                            treeView1.Populate(new List<ScObject>() { ex });
                        }
                        else
                        {
                            MessageBox.Show("Cloning failed. Invalid ExportName.");
                        }
                    }
                    form.Dispose();
                }
            }
        }
    }
}
