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
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EComply
{
    public partial class F_AddCompny : Form
    {
        F_GstWebView gstLoginForm;
        private string EditGstin;
        private DbConnectionFactory factory;
        private GenericRepository repo;
        string TempSqluteDbPath = "";
        string TempOracelDbPath = "";
        string TempSqlServerDbPath = "";
        string TempMySqlDbPath = "";
        bool IsDbConnectionValid = false;
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

                var dbData = repo.QuerySingleOrDefaultAsync<MasterDB.CompanyDBMaster>("SELECT * FROM CompanyDBMaster WHERE gstin = @gstin", new { gstin = EditGstin }).Result;
                if (dbData != null)
                {
                    if(dbData.db_type == "Sqlite")
                    {
                        rbSystemDefault.Checked = true;
                        txtDbConnection.Text = dbData.db_connection_string;
                        TempSqluteDbPath = dbData.db_connection_string;

                        txtDbConnection.Enabled = false;
                        btnCheckDbConnection.Visible = false;
                        txtDbConnection.Size = new Size(503, 23);
                        btnBrowse.Visible = true;
                        lblDatabaseLable.Text = "Database Location:";
                    }
                    else if (dbData.db_type == "Oracle")
                    {
                        

                        rbOracle.Checked = true;
                        txtDbConnection.Text = dbData.db_connection_string;

                        txtDbConnection.Enabled = true;
                        btnCheckDbConnection.Visible = true;
                        txtDbConnection.Size = new Size(622, 23);
                        btnBrowse.Visible = false;
                        lblDatabaseLable.Text = "Connection String:";
                        TempOracelDbPath = dbData.db_connection_string;
                    }
                    else if (dbData.db_type == "SqlServer")
                    {
                        rbSqlServer.Checked = true;
                        txtDbConnection.Text = dbData.db_connection_string;

                        txtDbConnection.Enabled = true;
                        btnCheckDbConnection.Visible = true;
                        txtDbConnection.Size = new Size(622, 23);
                        btnBrowse.Visible = false;
                        lblDatabaseLable.Text = "Connection String:";
                        TempOracelDbPath = dbData.db_connection_string;
                    }
                    else if (dbData.db_type == "MySql")
                    {
                        rbMySql.Checked = true;
                        txtDbConnection.Text = dbData.db_connection_string;

                        txtDbConnection.Enabled = true;
                        btnCheckDbConnection.Visible = true;
                        txtDbConnection.Size = new Size(622, 23);
                        btnBrowse.Visible = false;
                        lblDatabaseLable.Text = "Connection String:";
                        TempOracelDbPath = dbData.db_connection_string;
                    }
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
                if (!IsValid())
                    return;

                int countCM = await repo.ExecuteScalarAsync<int>("SELECT count(*) FROM CompanyMaster WHERE gstin = @gstin", new { gstin = txtGstin.Text });
                int countCDBM = await repo.ExecuteScalarAsync<int>("SELECT count(*) FROM CompanyDBMaster WHERE gstin = @gstin", new { gstin = txtGstin.Text });

                #region Company Master
                if (countCM == 0)
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
                #endregion

                #region Company DB Master
                if (countCM == 0)
                {
                    string db = "";
                    if (rbSystemDefault.Checked)
                        db = "Sqlite";
                    else if (rbOracle.Checked)
                        db = "Oracle";
                    else if (rbSqlServer.Checked)
                        db = "SqlServer";
                    else if (rbMySql.Checked)
                        db = "MySql";

                    int rows = await repo.ExecuteAsync(@"
                                    INSERT INTO CompanyDBMaster
                                        (gstin, db_type, db_connection_string)
                                    VALUES
                                        (@Gstin, @db_type, @db_connection_string)",
                                    new
                                    {
                                        Gstin = txtGstin.Text,
                                        db_type = db,
                                        db_connection_string = txtDbConnection.Text
                                    });

                    if (rows > 0)
                    {
                        //MessageBox.Show("Company Added!");
                    }
                }
                else
                {
                    string db = "";
                    if (rbSystemDefault.Checked)
                        db = "Sqlite";
                    else if (rbOracle.Checked)
                        db = "Oracle";
                    else if (rbSqlServer.Checked)
                        db = "SqlServer";
                    else if (rbMySql.Checked)
                        db = "MySql";

                    int rows = await repo.ExecuteAsync(@"
                                    UPDATE CompanyDBMaster
                                    SET db_type = @db_type,
                                        db_connection_string = @db_connection_string
                                    WHERE gstin = @Gstin",
                                    new
                                    {
                                        Gstin = txtGstin.Text,
                                        db_type = db,
                                        db_connection_string = txtDbConnection.Text
                                    });
                    if (rows > 0)
                    {
                        //MessageBox.Show("Company Updated!");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Error.HandleHide(ex);
            }
        }

        bool IsValid()
        {
            try
            {
                if (string.IsNullOrEmpty(txtGstin.Text))
                {
                    MessageBox.Show("Please enter GSTIN.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(txtTradeName.Text))
                {
                    MessageBox.Show("Please enter Trade Name.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(txtAddress.Text))
                {
                    MessageBox.Show("Please enter Address.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(txtMobileNo.Text))
                {
                    MessageBox.Show("Please enter Mobile No.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Please enter Email.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(txtGstUserName.Text))
                {
                    MessageBox.Show("Please enter GST Username.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(txtGstPassword.Text))
                {
                    MessageBox.Show("Please enter GST Password.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(txtEinvEwayUserName.Text))
                {
                    MessageBox.Show("Please enter E-Invoice/E-Waybill Username.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(txtEinvEwayPassword.Text))
                {
                    MessageBox.Show("Please enter E-Invoice/E-Waybill Password.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                btnCheckDbConnection_Click(null, null);
                if (!IsDbConnectionValid)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        void DatabaseSelect(DatabaseType db)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Error.HandleShow(ex);
            }
        }

        private void rbSystemDefault_CheckedChanged(object sender, EventArgs e)
        {
            bool flg = rbSystemDefault.Checked;
            if (flg)
            {
                txtDbConnection.Enabled = false;
                btnCheckDbConnection.Visible = false;
                txtDbConnection.Size = new Size(503, 23);
                btnBrowse.Visible = true;
                lblDatabaseLable.Text = "Database Location:";
                txtDbConnection.Text = TempSqluteDbPath;
                DatabaseSelect(DatabaseType.Sqlite);

            }
        }

        private void rbOracle_CheckedChanged(object sender, EventArgs e)
        {
            bool flg = rbOracle.Checked;
            if (flg)
            {
                txtDbConnection.Enabled = true;
                btnCheckDbConnection.Visible = true;
                txtDbConnection.Size = new Size(622, 23);
                btnBrowse.Visible = false;
                lblDatabaseLable.Text = "Connection String:";
                txtDbConnection.Text = TempOracelDbPath;
                DatabaseSelect(DatabaseType.Oracle);
            }
        }

        private void rbSqlServer_CheckedChanged(object sender, EventArgs e)
        {
            bool flg = rbSqlServer.Checked;
            if (flg)
            {
                txtDbConnection.Enabled = true;
                btnCheckDbConnection.Visible = true;
                txtDbConnection.Size = new Size(622, 23);
                btnBrowse.Visible = false;
                lblDatabaseLable.Text = "Connection String:";
                txtDbConnection.Text = TempSqlServerDbPath;
                DatabaseSelect(DatabaseType.SqlServer);
            }
        }

        private void rbMySql_CheckedChanged(object sender, EventArgs e)
        {
            bool flg = rbMySql.Checked;
            if (flg)
            {
                txtDbConnection.Enabled = true;
                btnCheckDbConnection.Visible = true;
                txtDbConnection.Size = new Size(622, 23);
                btnBrowse.Visible = false;
                lblDatabaseLable.Text = "Connection String:";
                txtDbConnection.Text = TempMySqlDbPath;
                DatabaseSelect(DatabaseType.MySql);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Select Folder";
                    folderDialog.UseDescriptionForTitle = true;
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = folderDialog.SelectedPath;

                        if (!HasFolderReadWritePermission(selectedPath))
                        {
                            DialogResult result = MessageBox.Show(
                                "You don't have Read/Write permission on this folder. Do you want to grant permission now?",
                                "Permission Required",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning
                            );

                            if (result == DialogResult.Yes)
                            {
                                string currentUser = Environment.UserDomainName + "\\" + Environment.UserName;

                                DirectoryInfo dirInfo = new DirectoryInfo(selectedPath);
                                DirectorySecurity dirSecurity = dirInfo.GetAccessControl();

                                FileSystemAccessRule rule = new FileSystemAccessRule(
                                    currentUser,
                                    FileSystemRights.Modify,
                                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                    PropagationFlags.None,
                                    AccessControlType.Allow
                                );

                                dirSecurity.AddAccessRule(rule);
                                dirInfo.SetAccessControl(dirSecurity);

                                MessageBox.Show("Permission granted successfully.",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                return; // user declined, don't set the path
                            }
                        }

                        txtDbConnection.Text = selectedPath;
                        TempSqluteDbPath = selectedPath;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Administrator rights are required to grant permissions. Please run the application as Administrator.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Error.HandleShow(ex);
            }
        }

        private bool HasFolderReadWritePermission(string folderPath)
        {
            try
            {
                Directory.GetFiles(folderPath);

                string testFile = Path.Combine(folderPath, $"~permtest_{Guid.NewGuid()}.tmp");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool TestConnectionUsingDapper(string connectionString, DatabaseType dbType, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Please enter proper connection string.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                DbConnectionFactory Testfactory;
                GenericRepository Testrepo;
                Testfactory = new DbConnectionFactory(dbType, connectionString);
                Testrepo = new GenericRepository(Testfactory);


                try
                {
                    using (var connection = Testrepo.OpenConnection())
                    {
                        //connection.Open();                    
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return false;
                }
            }
        }

        private void btnCheckDbConnection_Click(object sender, EventArgs e)
        {
            bool success = false;
            string errormsg = string.Empty;

            try
            {
                if (rbSystemDefault.Checked)
                {
                    if (string.IsNullOrEmpty(txtDbConnection.Text))
                        success = false;
                    else
                        success = true;
                }
                else if (rbOracle.Checked)
                    success = TestConnectionUsingDapper(txtDbConnection.Text, DatabaseType.Oracle, out errormsg);
                else if (rbSqlServer.Checked)
                    success = TestConnectionUsingDapper(txtDbConnection.Text, DatabaseType.SqlServer, out errormsg);
                else if (rbMySql.Checked)
                    success = TestConnectionUsingDapper(txtDbConnection.Text, DatabaseType.MySql, out errormsg);
                else
                {
                    MessageBox.Show("Please select proper connection.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    IsDbConnectionValid = false;
                    return;
                }

                if (success)
                {
                    MessageBox.Show("Connection Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    IsDbConnectionValid = true;
                }
                else
                {
                    MessageBox.Show($"Database Connection Failed:\n{errormsg}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    IsDbConnectionValid = false;
                }

            }
            catch (Exception ex)
            {
                Common.Error.HandleShow(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
