/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Tempat Pendaftaran Untuk DI
 * 
 */

using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Autofac;
// using nama_project_lain_yang_mau_di_import;

using DCTRNNPBBL.Helpers._db;
using DCTRNNPBBL.Helpers._utils;

namespace DCTRNNPBBL.Helpers {

    class AutofacContainer {

        private readonly IContainer Container;

        private static AutofacContainer _instance = null;

        AutofacContainer() {
            Container = ConfigureDI();
        }

        public static AutofacContainer Instance {
            get {
                if (_instance == null) {
                    _instance = new AutofacContainer();
                }
                return _instance;
            }
        }

        static IContainer ConfigureDI() {
            ContainerBuilder builder = new ContainerBuilder();

            /* Other Solution Project */
            // builder.RegisterAssemblyTypes(Assembly.Load(nameof(nama_project_lain_yang_mau_di_import)))
            //     .Where(vsSln => vsSln.Namespace.Contains("nama_namespace_yang_mau_di_import"))
            //     .As(c => c.GetInterfaces().FirstOrDefault(i => i.Name == "I" + c.Name);

            /* Contoh .. Window, Services, Repos, Helpers, And More Other Classes */
            // builder.RegisterType<CObject>().As<IObject>();

            /* Global */
            builder.RegisterType<CGlobals>().As<IGlobals>().SingleInstance();

            /* Helpers */
            builder.RegisterType<CApp>().As<IApp>().SingleInstance();
            builder.RegisterType<CLogger>().As<ILogger>().SingleInstance();
            builder.RegisterType<CConverter>().As<IConverter>().SingleInstance();
            builder.RegisterType<CApi>().As<IApi>().SingleInstance();

            /* Database Repossitories */
            builder.RegisterType<COracle>().As<IOracle>().SingleInstance();
            builder.RegisterType<CMsSQL>().As<IMsSQL>().SingleInstance();
            builder.RegisterType<CPostgres>().As<IPostgres>().SingleInstance();

            /* Panel */
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(type => type.IsSubclassOf(typeof(UserControl)))
                .SingleInstance();

            /* Window */
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(type => type.IsSubclassOf(typeof(Form)))
                .SingleInstance();

            return builder.Build();
        }

        public IContainer GetContainer() {
            return Container;
        }

    }

}
