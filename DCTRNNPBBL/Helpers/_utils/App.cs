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
using System.Linq;
using System.Windows.Forms;

namespace DCTRNNPBBL.Helpers._utils {

    public interface IApp {
        string GetVariabelOraSql(string key);
        string GetVariabelPg(string key);
        string AppName { get; }
        string AppLocation { get; }
        string AppVersion { get; }
        string IpAddress { get; }
        void Exit();
        int ScreenWidth { get; }
        int ScreenHeight { get; }
    }

    public class CApp : IApp {

        private readonly SettingLib.Class1 _SettingLib;
        private readonly SettingLibRest.Class1 _SettingLibRest;
        private readonly SD3Fungsi.Cls_ProgramMonitor ClsProgramMonitor;

        private readonly string AppLocation;
        private readonly string AppName;
        private readonly string AppVersion;
        private readonly string IpAddress;

        private readonly int ScreenWidth = 0;
        private readonly int ScreenHeight = 0;

        public CApp() {
            _SettingLib = new SettingLib.Class1();
            _SettingLibRest = new SettingLibRest.Class1();
            ClsProgramMonitor = new SD3Fungsi.Cls_ProgramMonitor();
            AppLocation = AppDomain.CurrentDomain.BaseDirectory;
            AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".EXE";
            AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            IpAddress = SD3Fungsi.IpKomp.GetIPAddress().Split(',').First();
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

        string IApp.AppName => AppName;

        string IApp.AppLocation => AppLocation;

        string IApp.AppVersion => AppVersion;

        string IApp.IpAddress => IpAddress;

        /* Aplikasi EXE */

        public void Exit() => Application.Exit();

        int IApp.ScreenWidth => ScreenWidth;

        int IApp.ScreenHeight => ScreenHeight;

    }

}