namespace EComply
{
    partial class F_GstWebView
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
            pnlMain = new Panel();
            pnlContent = new Panel();
            GstWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            pnlRight = new Panel();
            pnlLeft = new Panel();
            pnlBottom = new Panel();
            pnlTop = new Panel();
            label1 = new Label();
            pnlMain.SuspendLayout();
            pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GstWebView).BeginInit();
            pnlTop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(pnlContent);
            pnlMain.Controls.Add(pnlRight);
            pnlMain.Controls.Add(pnlLeft);
            pnlMain.Controls.Add(pnlBottom);
            pnlMain.Controls.Add(pnlTop);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(800, 450);
            pnlMain.TabIndex = 1;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(GstWebView);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(34, 33);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(734, 384);
            pnlContent.TabIndex = 4;
            // 
            // GstWebView
            // 
            GstWebView.AllowExternalDrop = true;
            GstWebView.CreationProperties = null;
            GstWebView.DefaultBackgroundColor = Color.White;
            GstWebView.Dock = DockStyle.Fill;
            GstWebView.Location = new Point(0, 0);
            GstWebView.Name = "GstWebView";
            GstWebView.Size = new Size(734, 384);
            GstWebView.TabIndex = 0;
            GstWebView.ZoomFactor = 1D;
            // 
            // pnlRight
            // 
            pnlRight.Dock = DockStyle.Right;
            pnlRight.Location = new Point(768, 33);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(32, 384);
            pnlRight.TabIndex = 3;
            pnlRight.Visible = false;
            // 
            // pnlLeft
            // 
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 33);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(34, 384);
            pnlLeft.TabIndex = 2;
            pnlLeft.Visible = false;
            // 
            // pnlBottom
            // 
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 417);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(800, 33);
            pnlBottom.TabIndex = 1;
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(label1);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(800, 33);
            pnlTop.TabIndex = 0;
            pnlTop.Visible = false;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(155, 33);
            label1.TabIndex = 1;
            label1.Text = "label1";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // F_GstWebView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlMain);
            Name = "F_GstWebView";
            Text = "F_GstWebView";
            FormClosed += F_GstWebView_FormClosed;
            pnlMain.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GstWebView).EndInit();
            pnlTop.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMain;
        private Panel pnlContent;
        private Microsoft.Web.WebView2.WinForms.WebView2 GstWebView;
        private Panel pnlRight;
        private Panel pnlLeft;
        private Panel pnlBottom;
        private Panel pnlTop;
        private Label label1;
    }
}