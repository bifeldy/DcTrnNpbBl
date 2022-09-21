using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autofac;

using DCTRNNPBBL.Helpers;
using DCTRNNPBBL.Helpers._db;
using DCTRNNPBBL.Helpers._utils;
using DCTRNNPBBL.Panels;

namespace DCTRNNPBBL.Forms {

    public partial class CMain : Form {

        private readonly IGlobals _globals;
        private readonly IApp _app;

        public CMain(IGlobals globals, IApp app) {
            _globals = globals;
            _app = app;

            InitializeComponent();
            OnInit();
        }
        public Panel PanelContainer {
            get {
                return panelContainer;
            }
        }

        public StatusStrip StatusStripContainer {
            get {
                return statusStripContainer;
            }
        }

        private void OnInit() {
            _globals.Main = this;

            // Reclaim Space From Right StatusBar Grip
            statusStripContainer.Padding = new Padding(
                statusStripContainer.Padding.Left,
                statusStripContainer.Padding.Top,
                statusStripContainer.Padding.Left,
                statusStripContainer.Padding.Bottom
            );

            Text = _app.AppName;
        }

        private void CMain_Load(object sender, EventArgs e) {
            ShowCheckProgramPanel();
        }

        private void ShowCheckProgramPanel() {

            // Create And Show `CekProgram` Panel
            if (!panelContainer.Controls.ContainsKey("CekProgram")) {
                panelContainer.Controls.Add(AutofacContainer.Instance.GetContainer().Resolve<CekProgram>());
            }
            panelContainer.Controls["CekProgram"].BringToFront();
        }

        public void HideLogo() {
            imgLogo.Visible = false;
        }

    }

}
