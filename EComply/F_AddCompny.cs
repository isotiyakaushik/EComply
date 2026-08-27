using DBDapper;
using EComply.Common;
using Model;
using Models;
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
    public partial class F_AddCompny : Form
    {
        F_GstWebView gstLoginForm;
        private string EditGstin;
        private DbConnectionFactory factory;
        private GenericRepository repo;
        public F_AddCompny(string type, string gstin = "")
        {
            InitializeComponent();
            factory = new DbConnectionFactory(DatabaseType.Sqlite, Statics.MainConnectionString);
            repo = new GenericRepository(factory);
            if (type == "Add")
            {
                this.Text = "Add Company";
                lblFormTitel.Text = "Add Company";
                txtGstUserName.Text = "AAACG7984Q.1008";
                txtGstPassword.Text = "Muk@20262";
            }
            else if (type == "Edit")
            {
                this.Text = "Edit Company";
                lblFormTitel.Text = "Edit Company";
                EditGstin = gstin;
                FillData();
            }
        }

        void FillData()
        {
            try
            {
                var company = repo.QuerySingleOrDefaultAsync<MasterDB.CompanyMaster>("SELECT * FROM CompanyMaster WHERE gstin = @gstin", new { gstin = EditGstin }).Result;
                if (company != null)
                {
                    txtGstin.Text = company.gstin;
                    txtTradeName.Text = company.trade_name;
                    txtAddress.Text = company.address;
                    txtMobileNo.Text = company.mobile_no;
                    txtEmail.Text = company.email;
                    txtGstUserName.Text = company.gst_user_name;
                    txtGstPassword.Text = company.gst_password;
                    txtEinvEwayUserName.Text = company.e_user_name;
                    txtEinvEwayPassword.Text = company.e_password;
                }
            }
            catch (Exception ex)
            {
                Error.HandleHide(ex);
            }
        }

        private async void btnGstImport_Click(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(txtGstUserName.Text) && !string.IsNullOrEmpty(txtGstPassword.Text))
                {
                    if (Statics.GSTcookieContainer != null && Statics.GSTcookieContainer.Count > 0)
                    {
                        string data = await gstLoginForm.GstProfileService();

                        Gst.GstProfile gstProfile = Newtonsoft.Json.JsonConvert.DeserializeObject<Gst.GstProfile>(data);

                        txtGstin.Text = gstProfile.gstin.Trim();
                        txtTradeName.Text = gstProfile.tradeNam.Trim();
                        txtMobileNo.Text = gstProfile.contacted.mobNum.ToString().Trim();
                        txtEmail.Text = gstProfile.contacted.email.Trim();
                        txtAddress.Text = gstProfile.pradr.adr.Trim();

                    }
                    else
                    {
                        Statics.GstUserName = txtGstUserName.Text.Trim();
                        Statics.GstPassword = txtGstPassword.Text;
                        gstLoginForm = new F_GstWebView();
                        gstLoginForm.Show();
                        bool flg = await gstLoginForm.Start();
                        if (flg)
                        {
                            btnGstImport_Click(null, null);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter GST Username and Password.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Error.HandleHide(ex);
            }
        }

        private void cbGstShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtGstPassword.PasswordChar = cbGstShowPassword.Checked ? '\0' : '*';
        }

        private void cbIShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtEinvEwayPassword.PasswordChar = cbIShowPassword.Checked ? '\0' : '*';
        }

        private async void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {

                int count = await repo.ExecuteScalarAsync<int>("SELECT count(*) FROM CompanyMaster WHERE gstin = @gstin", new { gstin = txtGstin.Text });

                if (count == 0)
                {
                    int rows = await repo.ExecuteAsync(@"
                                    INSERT INTO CompanyMaster
                                        (gstin, trade_name, address, mobile_no, email, gst_user_name, gst_password, e_user_name, e_password)
                                    VALUES
                                        (@Gstin, @TradeName, @Address, @MobileNo, @Email, @GstUserName, @GstPassword, @EUserName, @EPassword)",
                                    new
                                    {
                                        Gstin = txtGstin.Text,
                                        TradeName = txtTradeName.Text,
                                        Address = txtAddress.Text,
                                        MobileNo = txtMobileNo.Text,
                                        Email = txtEmail.Text,
                                        GstUserName = txtGstUserName.Text,
                                        GstPassword = txtGstPassword.Text,
                                        EUserName = txtEinvEwayUserName.Text,
                                        EPassword = txtEinvEwayPassword.Text
                                    });

                    if (rows > 0)
                    {
                        MessageBox.Show("Company Added!");
                    }
                }
                else
                {
                    int rows = await repo.ExecuteAsync(@"
                                    UPDATE CompanyMaster
                                    SET trade_name = @TradeName,
                                        address = @Address,
                                        mobile_no = @MobileNo,
                                        email = @Email,
                                        gst_user_name = @GstUserName,
                                        gst_password = @GstPassword,
                                        e_user_name = @EUserName,
                                        e_password = @EPassword
                                    WHERE gstin = @Gstin",
                                    new
                                    {
                                        Gstin = txtGstin.Text,
                                        TradeName = txtTradeName.Text,
                                        Address = txtAddress.Text,
                                        MobileNo = txtMobileNo.Text,
                                        Email = txtEmail.Text,
                                        GstUserName = txtGstUserName.Text,
                                        GstPassword = txtGstPassword.Text,
                                        EUserName = txtEinvEwayUserName.Text,
                                        EPassword = txtEinvEwayPassword.Text
                                    });
                    if (rows > 0)
                    {
                        MessageBox.Show("Company Updated!");
                    }
                }

                //if ()
            }
            catch (Exception ex)
            {
                Error.HandleHide(ex);
            }
        }
    }
}
