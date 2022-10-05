/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Cek Versi, IP, etc.
 * 
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autofac;

using DCTRNNPBBL.Helpers;
using DCTRNNPBBL.Helpers._db;
using DCTRNNPBBL.Helpers._utils;

namespace DCTRNNPBBL.Panels {

    public partial class CekProgram : UserControl {

        private readonly IGlobals _globals;
        private readonly IApp _app;
        private readonly IOracle _oracle;

        public CekProgram(IGlobals globals, IApp app, IOracle oracle) {
            _globals = globals;
            _app = app;
            _oracle = oracle;

            InitializeComponent();
            OnInit();
        }

        public Label LoadingInformation {
            get {
                return loadingInformation;
            }
        }

        private void OnInit() {
            _globals.CekProgram = this;
            loadingInformation.Text = "Sedang Mengecek Program ...";

            Dock = DockStyle.Fill;
        }

        private void CCekProgram_Load(object sender, EventArgs e) {
            CheckProgram();
        }

        private async void CheckProgram() {
            List<string> dc = new List<string> { "INDUK", "DEPO" };

            // Check Jenis DC
            bool allowed = false;
            await Task.Run(async () => {
                allowed = dc.Contains(await _oracle.GetJenisDc());
            });
            if (allowed) {

                // Check Version
                string responseCekProgram = null;
                await Task.Run(async () => {

                    // Change Form View
                    _globals.Main.StatusStripContainer.Items["statusStripIpAddress"].Text = _app.GetIpAddress();
                    _globals.Main.StatusStripContainer.Items["statusStripAppVersion"].Text = $"v{_app.AppVersion}";
                    _globals.Main.StatusStripContainer.Items["statusStripKodeDc"].Text = _oracle.DbName;

                    responseCekProgram = await _oracle.CekVersi();
                });
                if (responseCekProgram.Contains("OKE")) {
                    ShowLoginPanel();
                    return;
                }
                MessageBox.Show(responseCekProgram, "Program Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            MessageBox.Show(
                $"Program Hanya Dapat Di Jalankan Di DC {Environment.NewLine}{string.Join(", ", dc.ToArray())}",
                "Program Checker",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            _app.Exit();
        }

        private void ShowLoginPanel() {

            // Create & Show `Login` Panel
            if (!_globals.Main.PanelContainer.Controls.ContainsKey("Login")) {
                _globals.Main.PanelContainer.Controls.Add(AutofacContainer.Instance.GetContainer().Resolve<Login>());
            }
            _globals.Main.PanelContainer.Controls["Login"].BringToFront();
        }

    }

}
