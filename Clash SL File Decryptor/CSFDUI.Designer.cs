namespace CSFD
{
    partial class CSFDUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CSFDUI));
            this.BtnDecCsv = new MaterialSkin.Controls.MaterialRaisedButton();
            this.BtnDecSC = new MaterialSkin.Controls.MaterialRaisedButton();
            this.BtnEncCsv = new MaterialSkin.Controls.MaterialRaisedButton();
            this.BtnEncSC = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.tabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblScVersion = new MaterialSkin.Controls.MaterialLabel();
            this.RadioVersion7Lower = new MaterialSkin.Controls.MaterialRadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lblCopy = new MaterialSkin.Controls.MaterialLabel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnDecCsv
            // 
            this.BtnDecCsv.Depth = 0;
            this.BtnDecCsv.Location = new System.Drawing.Point(19, 45);
            this.BtnDecCsv.MouseState = MaterialSkin.MouseState.HOVER;
            this.BtnDecCsv.Name = "BtnDecCsv";
            this.BtnDecCsv.Primary = true;
            this.BtnDecCsv.Size = new System.Drawing.Size(146, 40);
            this.BtnDecCsv.TabIndex = 0;
            this.BtnDecCsv.Text = "Decompress CSV";
            this.BtnDecCsv.UseVisualStyleBackColor = true;
            this.BtnDecCsv.Click += new System.EventHandler(this.BtnDecCsv_Click);
            // 
            // BtnDecSC
            // 
            this.BtnDecSC.Depth = 0;
            this.BtnDecSC.Location = new System.Drawing.Point(19, 45);
            this.BtnDecSC.MouseState = MaterialSkin.MouseState.HOVER;
            this.BtnDecSC.Name = "BtnDecSC";
            this.BtnDecSC.Primary = true;
            this.BtnDecSC.Size = new System.Drawing.Size(123, 40);
            this.BtnDecSC.TabIndex = 1;
            this.BtnDecSC.Text = "Decompress SC";
            this.BtnDecSC.UseVisualStyleBackColor = true;
            this.BtnDecSC.Click += new System.EventHandler(this.BtnDecSC_Click);
            // 
            // BtnEncCsv
            // 
            this.BtnEncCsv.Depth = 0;
            this.BtnEncCsv.Location = new System.Drawing.Point(187, 45);
            this.BtnEncCsv.MouseState = MaterialSkin.MouseState.HOVER;
            this.BtnEncCsv.Name = "BtnEncCsv";
            this.BtnEncCsv.Primary = true;
            this.BtnEncCsv.Size = new System.Drawing.Size(149, 40);
            this.BtnEncCsv.TabIndex = 2;
            this.BtnEncCsv.Text = "Compress CSV";
            this.BtnEncCsv.UseVisualStyleBackColor = true;
            this.BtnEncCsv.Click += new System.EventHandler(this.BtnEncCsv_Click);
            // 
            // BtnEncSC
            // 
            this.BtnEncSC.Depth = 0;
            this.BtnEncSC.Location = new System.Drawing.Point(213, 45);
            this.BtnEncSC.MouseState = MaterialSkin.MouseState.HOVER;
            this.BtnEncSC.Name = "BtnEncSC";
            this.BtnEncSC.Primary = true;
            this.BtnEncSC.Size = new System.Drawing.Size(123, 40);
            this.BtnEncSC.TabIndex = 3;
            this.BtnEncSC.Text = "Compress SC";
            this.BtnEncSC.UseVisualStyleBackColor = true;
            this.BtnEncSC.Click += new System.EventHandler(this.BtnEncSC_Click);
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.BaseTabControl = this.tabControl1;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Location = new System.Drawing.Point(-1, 63);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(383, 48);
            this.materialTabSelector1.TabIndex = 17;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Depth = 0;
            this.tabControl1.Location = new System.Drawing.Point(12, 117);
            this.tabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(350, 119);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.BtnDecCsv);
            this.tabPage1.Controls.Add(this.BtnEncCsv);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(342, 93);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "CSV";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblScVersion);
            this.tabPage2.Controls.Add(this.BtnDecSC);
            this.tabPage2.Controls.Add(this.RadioVersion7Lower);
            this.tabPage2.Controls.Add(this.BtnEncSC);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(342, 93);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SC";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblScVersion
            // 
            this.lblScVersion.AutoSize = true;
            this.lblScVersion.Depth = 0;
            this.lblScVersion.Font = new System.Drawing.Font("Roboto", 11F);
            this.lblScVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblScVersion.Location = new System.Drawing.Point(3, 8);
            this.lblScVersion.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblScVersion.Name = "lblScVersion";
            this.lblScVersion.Size = new System.Drawing.Size(69, 19);
            this.lblScVersion.TabIndex = 6;
            this.lblScVersion.Text = "Version: ";
            // 
            // RadioVersion7Lower
            // 
            this.RadioVersion7Lower.AutoCheck = false;
            this.RadioVersion7Lower.AutoSize = true;
            this.RadioVersion7Lower.Checked = true;
            this.RadioVersion7Lower.Depth = 0;
            this.RadioVersion7Lower.Font = new System.Drawing.Font("Roboto", 10F);
            this.RadioVersion7Lower.Location = new System.Drawing.Point(284, 3);
            this.RadioVersion7Lower.Margin = new System.Windows.Forms.Padding(0);
            this.RadioVersion7Lower.MouseLocation = new System.Drawing.Point(-1, -1);
            this.RadioVersion7Lower.MouseState = MaterialSkin.MouseState.HOVER;
            this.RadioVersion7Lower.Name = "RadioVersion7Lower";
            this.RadioVersion7Lower.Ripple = true;
            this.RadioVersion7Lower.Size = new System.Drawing.Size(52, 30);
            this.RadioVersion7Lower.TabIndex = 4;
            this.RadioVersion7Lower.TabStop = true;
            this.RadioVersion7Lower.Text = "Any";
            this.RadioVersion7Lower.UseVisualStyleBackColor = true;
            this.RadioVersion7Lower.CheckedChanged += new System.EventHandler(this.RadioVersion7Lower_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lblCopy);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(342, 93);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Copyright";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lblCopy
            // 
            this.lblCopy.AutoSize = true;
            this.lblCopy.Depth = 0;
            this.lblCopy.Font = new System.Drawing.Font("Roboto", 11F);
            this.lblCopy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblCopy.Location = new System.Drawing.Point(77, 38);
            this.lblCopy.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblCopy.Name = "lblCopy";
            this.lblCopy.Size = new System.Drawing.Size(197, 38);
            this.lblCopy.TabIndex = 0;
            this.lblCopy.Text = "Copyright © 2021 Sky Production\r\n All rights reserved";
            this.lblCopy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CSFDUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 245);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.materialTabSelector1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CSFDUI";
            this.Sizable = false;
            this.Load += new System.EventHandler(this.CSFDUI_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialRaisedButton BtnDecCsv;
        private MaterialSkin.Controls.MaterialRaisedButton BtnDecSC;
        private MaterialSkin.Controls.MaterialRaisedButton BtnEncCsv;
        private MaterialSkin.Controls.MaterialRaisedButton BtnEncSC;
        private MaterialSkin.Controls.MaterialRadioButton RadioVersion7Lower;
        private MaterialSkin.Controls.MaterialTabControl tabControl1;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private MaterialSkin.Controls.MaterialLabel lblScVersion;
        private MaterialSkin.Controls.MaterialLabel lblCopy;
    }
}

