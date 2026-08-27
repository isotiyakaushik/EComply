namespace EComply
{
    partial class MDIParent1
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
            components = new System.ComponentModel.Container();
            toolTip = new ToolTip(components);
            pnlTop = new Panel();
            pnlBottom = new Panel();
            pnlLeft = new Panel();
            pnlRight = new Panel();
            menuStrip1 = new MenuStrip();
            mastersToolStripMenuItem = new ToolStripMenuItem();
            MenuDatabase = new ToolStripMenuItem();
            menuAddCompany = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.MidnightBlue;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 24);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(705, 22);
            pnlTop.TabIndex = 4;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.MidnightBlue;
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 308);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(705, 22);
            pnlBottom.TabIndex = 5;
            // 
            // pnlLeft
            // 
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 46);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(10, 262);
            pnlLeft.TabIndex = 6;
            pnlLeft.Visible = false;
            // 
            // pnlRight
            // 
            pnlRight.Dock = DockStyle.Right;
            pnlRight.Location = new Point(695, 46);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(10, 262);
            pnlRight.TabIndex = 7;
            pnlRight.Visible = false;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { mastersToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(705, 24);
            menuStrip1.TabIndex = 9;
            menuStrip1.Text = "menuStrip1";
            // 
            // mastersToolStripMenuItem
            // 
            mastersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { MenuDatabase, menuAddCompany });
            mastersToolStripMenuItem.Name = "mastersToolStripMenuItem";
            mastersToolStripMenuItem.Size = new Size(60, 20);
            mastersToolStripMenuItem.Text = "Masters";
            // 
            // MenuDatabase
            // 
            MenuDatabase.Name = "MenuDatabase";
            MenuDatabase.Size = new Size(180, 22);
            MenuDatabase.Text = "Database";
            // 
            // menuAddCompany
            // 
            menuAddCompany.Name = "menuAddCompany";
            menuAddCompany.Size = new Size(180, 22);
            menuAddCompany.Text = "Add Company";
            menuAddCompany.Click += menuAddCompany_Click;
            // 
            // MDIParent1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(705, 330);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 3, 4, 3);
            Name = "MDIParent1";
            ShowIcon = false;
            Text = "EComply";
            WindowState = FormWindowState.Maximized;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private Panel pnlTop;
        private Panel pnlBottom;
        private Panel pnlLeft;
        private Panel pnlRight;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mastersToolStripMenuItem;
        private ToolStripMenuItem MenuDatabase;
        private ToolStripMenuItem menuAddCompany;
    }
}



