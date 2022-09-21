/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Entry Point
 * 
 */

using System;
using System.Windows.Forms;

using Autofac;

using DCTRNNPBBL.Forms;
using DCTRNNPBBL.Helpers;

namespace DCTRNNPBBL {

    static class Program {

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IContainer Container = AutofacContainer.Instance.GetContainer();
            using(ILifetimeScope scope = Container.BeginLifetimeScope()) {
                Application.Run(Container.Resolve<CMain>());
            }
        }

    }

}
