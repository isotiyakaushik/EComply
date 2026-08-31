using EComply.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EComply
{
    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 0;

        public MDIParent1()
        {
            InitializeComponent();
            this.LayoutMdi(MdiLayout.Cascade);
            this.LayoutMdi(MdiLayout.TileHorizontal);
            this.LayoutMdi(MdiLayout.TileVertical);
            this.LayoutMdi(MdiLayout.ArrangeIcons);
        }

        #region Open New Form Event Handlers

        private void OpenNewChildForm(Form newChildForm)
        {
            // પહેલા બધા existing child forms ને close કરો
            foreach (Form childForm in this.MdiChildren.ToArray())  // ToArray() જરૂરી - loop દરમિયાન collection modify થાય છે
            {
                childForm.Close();
                childForm.Dispose();
            }

            // હવે નવું child form open કરો
            newChildForm.MdiParent = this;
            newChildForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            newChildForm.ControlBox = false;
            newChildForm.WindowState = FormWindowState.Maximized;
            newChildForm.Show();
        }

        private void menuAddCompany_Click(object sender, EventArgs e)
        {
            try
            {
                //OpenNewChildForm(new F_AddCompny("Add"));
                OpenNewChildForm(new F_AddCompny("Edit", "24AAACG7984Q1ZU"));
            }
            catch (Exception ex)
            {
                Error.HandleHide(ex);
            }
        }

        #endregion
    }
}
