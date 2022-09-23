/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Login User
 * 
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autofac;

using DCTRNNPBBL.Helpers._utils;
using DCTRNNPBBL.Helpers._db;
using DCTRNNPBBL.Helpers;
using System.Linq;

namespace DCTRNNPBBL.Panels {

    public partial class Login : UserControl {

        private readonly IGlobals _globals;
        private readonly IApp _app;
        private readonly IOracle _oracle;

        public Login(IGlobals globals, IApp app, IOracle oracle) {
            _globals = globals;
            _app = app;
            _oracle = oracle;

            InitializeComponent();
            OnInit();
        }

        private void OnInit() {
            _globals.Login = this;

            // Change Form View
            _globals.CekProgram.LoadingInformation.Text = "Harap Menunggu ...";

            Dock = DockStyle.Fill;
        }

        private void CLogin_Load(object sender, EventArgs e) {
            //
        }

        private void ShowLoading(bool isShow) {

            // Set State To Loading
            ToggleEnableDisableInput(!isShow);
            if (isShow) {
                _globals.Main.PanelContainer.Controls.Find("CekProgram", false).FirstOrDefault().BringToFront();
            }
            else {
                _globals.Main.PanelContainer.Controls.Find("Login", false).FirstOrDefault().BringToFront();
            }
        }

        private void ToggleEnableDisableInput(bool isEnable) {

            // Enable / Disable View
            btnLogin.Enabled = isEnable;
            txtUserNameNik.Enabled = isEnable;
            txtPassword.Enabled = isEnable;
        }

        private void ShowMainAppPanel() {

            // Change Window Size & Position To Middle Screen
            _globals.Main.Width = 800;
            _globals.Main.Height = 600;
            _globals.Main.SetDesktopLocation((_app.ScreenWidth - _globals.Main.Width) / 2, (_app.ScreenHeight - _globals.Main.Height) / 2);

            // Change Panel To Fully Windowed Mode And Go To `MainApp`
            _globals.Main.HideLogo();
            _globals.Main.PanelContainer.Dock = DockStyle.Fill;
            IEnumerable<Panel> panels = _globals.Main.PanelContainer.Controls.OfType<Panel>();
            foreach (Panel panel in panels) {
                _globals.Main.PanelContainer.Controls.Remove(panel);
            }

            // Show `MainApp`, And Remove `Login` Panel
            if (!_globals.Main.PanelContainer.Controls.ContainsKey("MainApp")) {
                _globals.Main.PanelContainer.Controls.Add(AutofacContainer.Instance.GetContainer().Resolve<MainApp>());
            }
            _globals.Main.PanelContainer.Controls["MainApp"].BringToFront();
        }

        private async void ProcessLogin() {

            // Disable View While Loading
            ShowLoading(true);

            // Check User Input
            if (string.IsNullOrEmpty(txtUserNameNik.Text) || string.IsNullOrEmpty(txtPassword.Text)) {
                ShowLoading(false);
                MessageBox.Show("Username / NIK Dan Kata Sandi Wajib Diisi!", "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            // Login Check Credential
            bool resultLogin = false;
            await Task.Run(async () => {
                resultLogin = await _oracle.LoginUser(txtUserNameNik.Text, txtPassword.Text);
            });
            if (!resultLogin) {
                ShowLoading(false);
                MessageBox.Show("Login Gagal, Silahkan Coba Lagi!", "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                ShowMainAppPanel();
            }
        }

        private void checkKeyboard(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Enter:
                    ProcessLogin();
                    break;
                default:
                    break;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            ProcessLogin();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e) {
            checkKeyboard(sender, e);
        }

        private void txtUserNameNik_KeyDown(object sender, KeyEventArgs e) {
            checkKeyboard(sender, e);
        }

    }

}
