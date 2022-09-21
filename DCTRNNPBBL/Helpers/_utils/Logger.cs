/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Alat Logging
 *              :: Harap Didaftarkan Ke DI Container
 * 
 */

using System;
using System.Diagnostics;
using System.IO;

namespace DCTRNNPBBL.Helpers._utils {

    public interface ILogger {
        void WriteError(string errorMessage, int skipFrame = 1);
        void WriteError(Exception errorException);
        void WriteLog(dynamic anyData);
    }

    public class CLogger : ILogger {

        private readonly IApp _app;

        public readonly string LogFolder;

        public CLogger(IApp app) {
            _app = app;

            LogFolder = $"{_app.AppLocation}/Error_Logs";

            InitializeLog();
        }

        private void InitializeLog() {
            if (!Directory.Exists(LogFolder)) {
                Directory.CreateDirectory(LogFolder);
            }
        }

        public void WriteError(string errorMessage, int skipFrame = 1) {
            StackFrame fromsub = new StackFrame(skipFrame, false);
            StreamWriter sw = new StreamWriter($"{LogFolder}/ErrGen_{_app.AppName}_{DateTime.Now:dd-MM-yyyy}.txt", true);
            sw.WriteLine($"##");
            sw.WriteLine($"#  ErrDate : {DateTime.Now:dd-MM-yyyy HH:mm:ss}");
            sw.WriteLine($"#  ErrFunc : {fromsub.GetMethod().Name}");
            sw.WriteLine($"#  ErrInfo : {errorMessage}");
            sw.WriteLine($"##");
            sw.Flush();
            sw.Close();
            WriteLog($"ERROR :: {fromsub.GetMethod().Name} :: {errorMessage}");
        }

        public void WriteError(Exception errorException) {
            WriteError(errorException.Message, 2);
        }

        public void WriteLog(dynamic anyData) {
            Console.WriteLine(anyData);
        }

    }

}