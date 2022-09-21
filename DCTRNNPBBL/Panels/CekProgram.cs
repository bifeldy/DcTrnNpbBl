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

            // Change Form View
            _globals.Main.StatusStripContainer.Items["statusStripIpAddress"].Text = _app.IpAddress;
            _globals.Main.StatusStripContainer.Items["statusStripAppVersion"].Text = $"v{_app.AppVersion}";

            // Check Version Running Async
            CheckVersiProgram();
        }

        private async void CheckVersiProgram() {

            // Check Version
            string responseCekProgram = null;
            await Task.Run(async () => {
                _globals.Main.StatusStripContainer.Items["statusStripKodeDc"].Text = $"{await _oracle.GetKodeDc()} - {await _oracle.GetNamaDc()}";
                responseCekProgram = await _oracle.CekVersi();
            });

            // Read Response
            if (responseCekProgram.Contains("OKE")) {
                ShowLoginPanel();
            }
            else {
                MessageBox.Show(responseCekProgram, "Program Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _app.Exit();
            }
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
