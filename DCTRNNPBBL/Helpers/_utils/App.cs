/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Pengaturan Aplikasi
 *              :: Harap Didaftarkan Ke DI Container
 * 
 */

using System;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCTRNNPBBL.Helpers._utils {

    public interface IApp {

        string GetConfig(string key);
        string CekVersiOracle(string connectionString, string kodeDc);
        string GetVariabelOraSql(string key);
        string GetVariabelPg(string key);
        string AppName { get; }
        string AppLocation { get; }
        string AppVersion { get; }
        string GetIpAddress();
        string CekUser(string p_connStr, string p_user, string p_password);
        void Exit();
        int ScreenWidth { get; }
        int ScreenHeight { get; }
    }

    public class CApp : IApp {

        private readonly LIBLOGIN.Class1 _LIBLOGIN;
        private readonly SettingLib.Class1 _SettingLib;
        private readonly SettingLibRest.Class1 _SettingLibRest;
        private readonly SD3Fungsi.Cls_ProgramMonitor ClsProgramMonitor;

        private readonly string AppLocation;
        private readonly string AppVersion;

        private readonly string AppName;
        private readonly int ScreenWidth = 0;
        private readonly int ScreenHeight = 0;

        private string IpAddress;
        private string MacAddress;

        public CApp() {
            _LIBLOGIN = new LIBLOGIN.Class1();
            _SettingLib = new SettingLib.Class1();
            _SettingLibRest = new SettingLibRest.Class1();
            ClsProgramMonitor = new SD3Fungsi.Cls_ProgramMonitor();
            AppLocation = AppDomain.CurrentDomain.BaseDirectory;
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //
            AppName = Assembly.GetExecutingAssembly().GetName().Name + ".EXE";
            ScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;
        }

        public SD3Fungsi.Cls_ProgramMonitor GetProgramMonitor() {
            return ClsProgramMonitor;
        }

        public string GetVariabelOraSql(string key) {
            return _SettingLib.GetVariabel(key);
        }

        public string GetVariabelPg(string key) {
            return _SettingLibRest.GetVariabel(key);
        }

        public string CekVersiOracle(string connectionString, string kodeDc) {
            #if !DEBUG
                return _SettingLib.GetVersiODP(connectionString, kodeDc, AppName, AppVersion, IpAddress);
            #else
                return "OKE";
            #endif
        }

        string IApp.AppLocation => AppLocation;

        string IApp.AppVersion => AppVersion;

        /* Aplikasi EXE */

        string IApp.AppName => AppName;

        public void Exit() => Application.Exit();

        public string GetConfig(string key) {
            return ConfigurationManager.AppSettings[key];
        }

        int IApp.ScreenWidth => ScreenWidth;

        int IApp.ScreenHeight => ScreenHeight;

        public string GetIpAddress() {
            if (string.IsNullOrEmpty(IpAddress)) {
                IpAddress = SD3Fungsi.IpKomp.GetIPAddress().Split(',').FirstOrDefault();
            }
            return IpAddress;
        }

        public string CekUser(string p_connStr, string p_user, string p_password) {
            return _LIBLOGIN.cekUser(p_connStr, p_user, p_password, AppName, GetIpAddress());
        }

    }

}