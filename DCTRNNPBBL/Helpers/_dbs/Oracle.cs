/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Turunan `CDatabase`
 *              :: Harap Didaftarkan Ke DI Container
 *              :: Instance Oracle
 * 
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

using Oracle.ManagedDataAccess.Client;

using DCTRNNPBBL.Helpers._models;
using DCTRNNPBBL.Helpers._utils;
using System.Windows.Forms;

namespace DCTRNNPBBL.Helpers._db {

    public interface IOracle : IDatabase {
        Task<DataTable> GetDataTableAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<dynamic> ExecScalarAsync(EReturnDataType returnType, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<bool> ExecQueryAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<CDbExecProcResult> ExecProcedureAsync(string procedureName, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<int> UpdateTable(DataSet dataSet, string dataSetTableName, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<DbDataReader> ExecReaderAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = false);
        Task<string> RetrieveBlob(string stringPathDownload, string stringFileName, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = false);
        string LoggedInUsername { get; }
        Task<string> GetJenisDc();
        Task<string> CekVersi();
        Task<string> GetKodeDc();
        Task<string> GetNamaDc();
        Task<bool> LoginUser(string usernameNik, string password);
    }

    public class COracle : CDatabase, IOracle {

        private readonly IApp _app;

        private OracleCommand DatabaseCommand { get; set; }
        private OracleDataAdapter DatabaseAdapter { get; set; }

        private string DcCode;
        private string DcName;
        private string DcJenis;
        private string LoggedInUsername;

        public COracle(IApp app, ILogger logger) : base(logger) {
            _app = app;

            InitializeOracleDatabase();
        }

        private void InitializeOracleDatabase() {
            DbUsername = _app.GetVariabelOraSql("UserOrcl");
            DbPassword = _app.GetVariabelOraSql("PasswordOrcl");
            DbTnsOdp = _app.GetVariabelOraSql("ODPOrcl");
            DbName = DbTnsOdp.Split(
                new string[] { "SERVICE_NAME=" },
                StringSplitOptions.None
            )[1].Split(
                new string[] { ")" },
                StringSplitOptions.None
            )[0];
            DbConnectionString = $"Data Source={DbTnsOdp};User ID={DbUsername};Password={DbPassword};";
            DatabaseConnection = new OracleConnection(DbConnectionString);
            DatabaseCommand = new OracleCommand("", (OracleConnection) DatabaseConnection);
            DatabaseAdapter = new OracleDataAdapter(DatabaseCommand);
        }

        private void BindQueryParameter(List<CDbQueryParamBind> parameters) {
            DatabaseCommand.BindByName = true;
            DatabaseCommand.Parameters.Clear();
            if (parameters != null) {
                // string sqlTextQueryParameters = "(";
                // for (int i = 0; i < parameters.Count; i++) {
                //     sqlTextQueryParameters += $"'{parameters[i].VALUE}'";
                //     if (i + 1 < parameters.Count) sqlTextQueryParameters += ",";
                // }
                // sqlTextQueryParameters += ")";
                for (int i = 0; i < parameters.Count; i++) {
                    OracleParameter param = new OracleParameter {
                        ParameterName = parameters[i].NAME,
                        Value = parameters[i].VALUE
                    };
                    if (parameters[i].SIZE > 0) {
                        param.Size = parameters[i].SIZE;
                    }
                    if (parameters[i].DIRECTION > 0) {
                        param.Direction = parameters[i].DIRECTION;
                    }
                    DatabaseCommand.Parameters.Add(param);
                }
            }
        }

        /** Bagian Ini Mirip :: Oracle - Ms. Sql Server - PostgreSQL */

        public async Task<DataTable> GetDataTableAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true) {
            DatabaseCommand.CommandText = queryString;
            DatabaseCommand.CommandType = CommandType.Text;
            BindQueryParameter(bindParam);
            return await base.GetDataTableAsync(DatabaseAdapter, closeConnection);
        }

        public async Task<dynamic> ExecScalarAsync(EReturnDataType returnType, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true) {
            DatabaseCommand.CommandText = queryString;
            DatabaseCommand.CommandType = CommandType.Text;
            BindQueryParameter(bindParam);
            return await base.ExecScalarAsync(DatabaseCommand, returnType, closeConnection);
        }

        public async Task<bool> ExecQueryAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true) {
            DatabaseCommand.CommandText = queryString;
            DatabaseCommand.CommandType = CommandType.Text;
            BindQueryParameter(bindParam);
            return await base.ExecQueryAsync(DatabaseCommand, closeConnection);
        }

        public async Task<CDbExecProcResult> ExecProcedureAsync(string procedureName, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true) {
            DatabaseCommand.CommandText = procedureName;
            DatabaseCommand.CommandType = CommandType.StoredProcedure;
            BindQueryParameter(bindParam);
            return await base.ExecProcedureAsync(DatabaseCommand, closeConnection);
        }

        public async Task<int> UpdateTable(DataSet dataSet, string dataSetTableName, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true) {
            DatabaseCommand.CommandText = queryString;
            DatabaseCommand.CommandType = CommandType.Text;
            BindQueryParameter(bindParam);
            return await base.UpdateTable(DatabaseAdapter, dataSet, dataSetTableName, closeConnection);
        }

        /// <summary> Jangan Lupa Di Close !!</summary>
        public async Task<DbDataReader> ExecReaderAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = false) {
            DatabaseCommand.CommandText = queryString;
            DatabaseCommand.CommandType = CommandType.Text;
            BindQueryParameter(bindParam);
            return await base.ExecReaderAsync(DatabaseCommand, closeConnection);
        }

        /// <summary> Jangan Lupa Di Close !!</summary>
        public async Task<string> RetrieveBlob(string stringPathDownload, string stringFileName, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = false) {
            DatabaseCommand.CommandText = queryString;
            DatabaseCommand.CommandType = CommandType.Text;
            BindQueryParameter(bindParam);
            return await base.RetrieveBlob(DatabaseCommand, stringPathDownload, stringFileName, closeConnection);
        }

        /** Custom Queries */

        string IOracle.LoggedInUsername => LoggedInUsername;

        public async Task<string> GetJenisDc() {
            if (string.IsNullOrEmpty(DcJenis)) {
                DcJenis = await ExecScalarAsync(EReturnDataType.STRING, "SELECT TBL_JENIS_DC FROM DC_TABEL_DC_T");
            }
            return DcJenis;
        }

        public async Task<string> GetKodeDc() {
            if (string.IsNullOrEmpty(DcCode)) {
                DcCode = await ExecScalarAsync(EReturnDataType.STRING, "SELECT TBL_DC_KODE FROM DC_TABEL_DC_T");
            }
            return DcCode;
        }

        public async Task<string> GetNamaDc() {
            if (string.IsNullOrEmpty(DcName)) {
                DcName = await ExecScalarAsync(EReturnDataType.STRING, "SELECT TBL_DC_NAMA FROM DC_TABEL_DC_T");
            }
            return DcName;
        }

        public async Task<string> CekVersi() {
            string resCekVersi = null;
            await Task.Run(async () => {
                resCekVersi = _app.CekVersiOracle(DbConnectionString, await GetKodeDc());
            });
            return resCekVersi;
        }

        public async Task<bool> LoginUser(string userNameNik, string password) {
            if (string.IsNullOrEmpty(LoggedInUsername)) {
                #if DEBUG
                    LoggedInUsername = await ExecScalarAsync(EReturnDataType.STRING, $@"
                        SELECT
                            user_name
                        FROM
                            dc_user_t
                        WHERE
                            (user_name = :user_name OR user_nik = :user_nik)
                            AND user_password = :password
                    ", new List<CDbQueryParamBind> {
                        new CDbQueryParamBind { NAME = "user_name", VALUE = userNameNik },
                        new CDbQueryParamBind { NAME = "user_nik", VALUE = userNameNik },
                        new CDbQueryParamBind { NAME = "password", VALUE = password }
                    });
                #else
                    string res = _app.CekUser(DbConnectionString, userNameNik, password);
                    if (res == "Y") {
                        LoggedInUsername = userNameNik;
                    }
                    else {
                        MessageBox.Show(res, "Cek User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                #endif
            }
            return !string.IsNullOrEmpty(LoggedInUsername);
        }

    }

}
