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

namespace DCTRNNPBBL.Forms {

    public partial class Form1 : Form {

        IApp _app;
        IOracle _oracle;

        public Form1(IApp app, IOracle oracle) {
            _app = app;
            _oracle = oracle;

            InitializeComponent();

            CMain form = AutofacContainer.Instance.GetContainer().Resolve<CMain>();
        }

    }

}
