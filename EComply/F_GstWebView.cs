using EComply.Common;
using Microsoft.VisualBasic;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EComply
{
    public partial class F_GstWebView : Form
    {
        private const string LoginUrl = "https://services.gst.gov.in/services/login";
        private const string WelcomeUrl = "https://services.gst.gov.in/services/auth/fowelcome";
        private System.Windows.Forms.Timer _refreshTimer;
        private string GST_UserName { get; set; }
        private string GST_Password { get; set; }
        private TaskCompletionSource<bool> _navigationTcs;
        private string CurrentUrl { get; set; }
        public F_GstWebView()
        {
            InitializeComponent();
            this.BringToFront();
        }

        void GetCred(string username = "", string password = "")
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                GST_UserName = Statics.GstUserName;
                GST_Password = Statics.GstPassword;
            }
            else
            {
                GST_UserName = username;
                GST_Password = password;
            }
        }

        public async Task<bool> Start_old()
        {
            bool flg = false;
            try
            {
                GetCred();

                var userDataFolder = Path.Combine(Application.StartupPath, "PowerGST_WebView2_Profile");

                if (GstWebView.CoreWebView2 == null)
                {
                    var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
                    await GstWebView.EnsureCoreWebView2Async(env);

                    var options = env.CreateCoreWebView2ControllerOptions();
                    options.ProfileName = Statics.CompanyGSTN;
                    options.IsInPrivateModeEnabled = false;

                    await GstWebView.EnsureCoreWebView2Async(env, options);

                    GstWebView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
                    GstWebView.CoreWebView2.Settings.IsScriptEnabled = true;
                    GstWebView.CoreWebView2.NavigationCompleted += GstWebView_NavigationCompleted;
                }

                _navigationTcs = new TaskCompletionSource<bool>();
                GstWebView.CoreWebView2.Navigate(LoginUrl);
                bool success1 = await _navigationTcs.Task;

                if (success1 && await FillLoginForm(GST_UserName, GST_Password))
                {
                    await GetCookiesFromWebView();
                    flg = true;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Error.HandleHide(ex);
            }

            return flg;
        }

        private void DeleteProfileFolderIfExists(string profileRoot, string gstin)
        {
            try
            {
                if (!Directory.Exists(profileRoot))
                    return;

                foreach (var dir in Directory.GetDirectories(profileRoot, "WV2Profile_*" + gstin.ToLower() + "*"))
                {
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch (Exception ex)
                    {
                        Error.HandleHide(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.HandleHide(ex);
            }
        }

        public async Task<bool> Start()
        {
            bool flg = false;
            try
            {
                string Profile = "";
                if (string.IsNullOrEmpty(Statics.CompanyGSTN))
                    Profile = "Temp_Profile";
                else
                    Profile = Statics.CompanyGSTN;
                GetCred();

                //var userDataFolder = Path.Combine(Application.StartupPath, "PowerGST_WebView2_Profile");
                var userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GST_WebView2_Profile");
                DeleteProfileFolderIfExists(Path.Combine(userDataFolder, "EBWebView"), Profile);

                if (GstWebView.CoreWebView2 == null)
                {
                    var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);

                    var options = env.CreateCoreWebView2ControllerOptions();
                    options.ProfileName = Profile;
                    options.IsInPrivateModeEnabled = false;

                    await GstWebView.EnsureCoreWebView2Async(env, options);

                    GstWebView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
                    GstWebView.CoreWebView2.Settings.IsScriptEnabled = true;
                    GstWebView.CoreWebView2.NavigationCompleted += GstWebView_NavigationCompleted;
                }

                _navigationTcs = new TaskCompletionSource<bool>();
                GstWebView.CoreWebView2.Navigate(LoginUrl);
                bool success1 = await _navigationTcs.Task;

                if (success1 && await FillLoginForm(GST_UserName, GST_Password))
                {
                    await GetCookiesFromWebView();
                    flg = true;
                    StartAutoRefreshTimer();
                }
            }
            catch (Exception ex)
            {
                Error.HandleHide(ex);
            }
            return flg;
        }

        private void StartAutoRefreshTimer()
        {
            if (_refreshTimer == null)
            {
                _refreshTimer = new System.Windows.Forms.Timer();
                _refreshTimer.Interval = 5 * 60 * 1000; // 5 minute
                _refreshTimer.Tick += RefreshTimer_Tick;
            }
            _refreshTimer.Start();
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (GstWebView.CoreWebView2 != null)
                {
                    GstWebView.CoreWebView2.Reload();

                    if (GstWebView.CoreWebView2.Source.Equals("https://services.gst.gov.in/services/error/accessdenied", StringComparison.OrdinalIgnoreCase))
                    {
                        Statics.GSTcookieContainer = null;
                        
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Error.HandleHide(ex);
            }
        }

        private void F_GstWebView_FormClosed(object sender, FormClosedEventArgs e)
        {
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
        }

        private void GstWebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                CurrentUrl = GstWebView.CoreWebView2.Source;
                _navigationTcs?.TrySetResult(e.IsSuccess);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Error.HandleHide(ex);
            }
        }

        private async Task<bool> FillLoginForm(string username, string password)
        {
            try
            {
                _navigationTcs = new TaskCompletionSource<bool>();
                GstWebView.CoreWebView2.Reload();
                bool reloaded = await _navigationTcs.Task;

                if (!reloaded) return false;

                while (true)
                {
                    if (this.IsDisposed || !this.Visible)
                        return false;

                    if (!GstWebView.CoreWebView2.Source.StartsWith(LoginUrl, StringComparison.OrdinalIgnoreCase))
                        return false;

                    bool fieldReady = await WaitForElementAsync("username", 15);
                    if (!fieldReady)
                    {
                        Error.HandleHide(new Exception("Login form field not found, timeout"));
                        return false;
                    }

                    await FillCredentialsAsync(username, password);

                    bool captchaEntered = await WaitForCaptchaAsync();
                    if (!captchaEntered)
                        return false;

                    bool loginSuccess = await ClickLoginButtonAsync();
                    if (loginSuccess)
                        return true;

                    await Task.Delay(500);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Error.HandleHide(ex);
                return false;
            }
        }

        private async Task<bool> WaitForElementAsync(string elementId, int maxWaitSeconds = 15)
        {
            string script = $@"
        (function() {{
            var el = document.getElementById('{elementId}');
            return el ? 'true' : 'false';
        }})();
    ";

            int elapsed = 0;
            int pollInterval = 300;

            while (elapsed < maxWaitSeconds * 1000)
            {
                string result = await GstWebView.CoreWebView2.ExecuteScriptAsync(script);
                bool exists = result?.Trim('"') == "true";

                if (exists)
                    return true;

                await Task.Delay(pollInterval);
                elapsed += pollInterval;
            }

            return false; // timeout - field kadi na aavyu
        }
        private async Task FillCredentialsAsync(string username, string password)
        {
            try
            {

                string safeUsername = username.Replace("\\", "\\\\").Replace("'", "\\'").Trim();
                string safePassword = password.Replace("\\", "\\\\").Replace("'", "\\'");

                const int MAX_LENGTH = 15;

                if (safeUsername.Length > MAX_LENGTH || safePassword.Length > MAX_LENGTH)
                {
                    Error.HandleHide(
                        new Exception($"Username/Password exceeds GST portal limit of {MAX_LENGTH} chars. " +
                                      $"Username length: {safeUsername.Length}, Password length: {safePassword.Length}"));

                    MessageBox.Show(
                                    $"Username and Password must not exceed {MAX_LENGTH} characters.\n" +
                                    $"Your Username: {safeUsername.Length} characters\n" +
                                    $"Your Password: {safePassword.Length} characters",
                                    "Invalid Credentials Length",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                    return;
                }

                await Task.Delay(1000);
                string fillScript = $@"
        (function() {{
            var nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
            var userField = document.getElementById('username');
            if (userField) {{
                nativeInputValueSetter.call(userField, '{safeUsername}');
                userField.dispatchEvent(new Event('input', {{ bubbles: true }}));
                userField.dispatchEvent(new Event('change', {{ bubbles: true }}));
            }}
            var passField = document.getElementById('user_pass');
            if (passField) {{
                nativeInputValueSetter.call(passField, '{safePassword}');
                passField.dispatchEvent(new Event('input', {{ bubbles: true }}));
                passField.dispatchEvent(new Event('change', {{ bubbles: true }}));
            }}
            var captchaField = document.getElementById('captcha');
            if (captchaField) {{
                captchaField.focus(); 
            }}
        }})();
    ";

                await GstWebView.CoreWebView2.ExecuteScriptAsync(fillScript);


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Error.HandleHide(ex);
            }
        }

        private async Task ClearCaptchaFieldAsync()
        {
            try
            {
                string clearScript = @"
        (function() {
            var field = document.getElementById('captcha');
            if (field) {
                var nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
                nativeInputValueSetter.call(field, '');
                field.dispatchEvent(new Event('input', { bubbles: true }));
                field.dispatchEvent(new Event('change', { bubbles: true }));
            }
        })();
    ";

                await GstWebView.CoreWebView2.ExecuteScriptAsync(clearScript);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Error.HandleHide(ex);
            }
        }

        private async Task<bool> WaitForCaptchaAsync()
        {
            try
            {
                string script = @"
                (function() {
                    var field = document.getElementById('captcha');
                    return field ? field.value : '';
                })();
            ";

                while (true)
                {
                    if (this.IsDisposed || !this.Visible)
                        return false;

                    if (!GstWebView.CoreWebView2.Source.StartsWith(LoginUrl, StringComparison.OrdinalIgnoreCase))
                        return false;

                    string result = await GstWebView.CoreWebView2.ExecuteScriptAsync(script);
                    string captchaValue = result?.Trim('"');

                    if (!string.IsNullOrEmpty(captchaValue) && captchaValue.Length == 6 && captchaValue.All(char.IsDigit))
                        return true;

                    await Task.Delay(500);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Error.HandleHide(ex);
                return false;
            }
        }

        private async Task<bool> ClickLoginButtonAsync()
        {
            try
            {
                string script = @"
                (function() {
                    var btn = document.querySelector('button.btn.btn-primary[data-ng-bind=""trans.HEAD_LOGIN""]');
                    if (btn) { btn.click(); return true; }
                    return false;
                })();
            ";

                string result = await GstWebView.CoreWebView2.ExecuteScriptAsync(script);
                bool clicked = result?.Trim('"') == "true";

                if (!clicked) return false;

                int elapsed = 0;
                int pollInterval = 300;
                int maxWaitSeconds = 3;

                while (elapsed < maxWaitSeconds * 1000)
                {
                    if (GstWebView.CoreWebView2.Source.Equals(WelcomeUrl, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (GstWebView.CoreWebView2.Source.Equals("https://services.gst.gov.in/services/auth/dashboard", StringComparison.OrdinalIgnoreCase))
                        return true;

                    await Task.Delay(pollInterval);
                    elapsed += pollInterval;
                }

                return false;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Error.HandleHide(ex);
                return false;
            }
        }

        public async Task GetCookiesFromWebView()
        {
            try
            {
                Statics.GSTcookieContainer = new CookieContainer();
                var cookieManager = GstWebView.CoreWebView2.CookieManager;
                var cookieList = await cookieManager.GetCookiesAsync(null);

                foreach (var c in cookieList)
                {
                    try
                    {
                        var netCookie = new Cookie(c.Name, c.Value, c.Path, c.Domain);
                        Statics.GSTcookieContainer.Add(netCookie);
                    }
                    catch { /* skip invalid format */ }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Error.HandleHide(ex);
            }
        }


        private HttpClient _httpClient;
        public async Task<string> GstProfileService()
        {
            var handler = new HttpClientHandler
            {
                CookieContainer = Statics.GSTcookieContainer,
                UseCookies = true,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip
                    | System.Net.DecompressionMethods.Deflate
                    | System.Net.DecompressionMethods.Brotli
            };

            var _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://services.gst.gov.in")
            };

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            _httpClient.DefaultRequestHeaders.Add("Origin", "https://services.gst.gov.in");
            _httpClient.DefaultRequestHeaders.Add("Referer", "https://services.gst.gov.in/services/auth/myprofile");
            _httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/151.0.0.0 Safari/537.36 Edg/151.0.0.0");
            _httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            _httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            _httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");

            var request = new HttpRequestMessage(HttpMethod.Post, "/services/auth/profile/detail");
            request.Content = new StringContent(string.Empty);
            request.Content.Headers.ContentLength = 0;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
