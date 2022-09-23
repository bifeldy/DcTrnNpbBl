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
 *              :: Instance Postgre
 * 
 */

using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System.Data;

using Npgsql;

using DCTRNNPBBL.Helpers._utils;
using DCTRNNPBBL.Helpers._models;

namespace DCTRNNPBBL.Helpers._db {

    public interface IPostgres : IDatabase {
        Task<DataTable> GetDataTableAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<dynamic> ExecScalarAsync(EReturnDataType returnType, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<bool> ExecQueryAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<CDbExecProcResult> ExecProcedureAsync(string procedureName, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<int> UpdateTable(DataSet dataSet, string dataSetTableName, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<DbDataReader> ExecReaderAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = false);
        Task<string> RetrieveBlob(string stringPathDownload, string stringFileName, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = false);
        Task<string> GetDcKode();
    }

    public class CPostgres : CDatabase, IPostgres {

        private readonly IApp _app;

        private NpgsqlCommand DatabaseCommand { get; set; }
        private NpgsqlDataAdapter DatabaseAdapter { get; set; }

        private string DC_KODE;

        public CPostgres(IApp app, ILogger logger) : base(logger) {
            _app = app;

            InitializeOracleDatabase();
        }

        private void InitializeOracleDatabase() {
            DbIpAddrss = _app.GetVariabelPg("IPPostgres");
            DbPort = _app.GetVariabelPg("PortPostgres");
            DbUsername = _app.GetVariabelPg("UserPostgres");
            DbPassword = _app.GetVariabelPg("PasswordPostgres");
            DbName = _app.GetVariabelPg("DatabasePostgres");
            DbConnectionString = $"Host={DbIpAddrss};Port={DbPort};Username={DbUsername};Password={DbPassword};Database={DbName};";
            DatabaseConnection = new NpgsqlConnection(DbConnectionString);
            DatabaseCommand = new NpgsqlCommand("", (NpgsqlConnection) DatabaseConnection);
            DatabaseAdapter = new NpgsqlDataAdapter(DatabaseCommand);
        }

        /** Bagian Ini Mirip :: Oracle - Ms. Sql Server - PostgreSQL */

        private void BindQueryParameter(List<CDbQueryParamBind> parameters) {
            DatabaseCommand.Parameters.Clear();
            if (parameters != null) {
                for (int i = 0; i < parameters.Count; i++) {
                    NpgsqlParameter param = new NpgsqlParameter {
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
            string sqlTextQueryParameters = "(";
            for (int i = 0; i < bindParam.Count; i++) {
                sqlTextQueryParameters += $"'{bindParam[i].VALUE}'";
                if (i + 1 < bindParam.Count) sqlTextQueryParameters += ",";
            }
            sqlTextQueryParameters += ")";
            DatabaseCommand.CommandText = $"CALL {procedureName} {sqlTextQueryParameters}";
            DatabaseCommand.CommandType = CommandType.Text;
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

        /** */

        public async Task<string> GetDcKode() {
            if (string.IsNullOrEmpty(DC_KODE)) {
                DC_KODE = await ExecScalarAsync(EReturnDataType.STRING, "SELECT TBL_DC_KODE FROM DC_TABEL_DC_T");
            }
            return DC_KODE;
        }

    }

}
