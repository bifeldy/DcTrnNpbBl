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
 *              :: Instance Microsoft SQL Server
 * 
 */

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

using DCTRNNPBBL.Helpers._models;
using DCTRNNPBBL.Helpers._utils;

namespace DCTRNNPBBL.Helpers._db {

    public interface IMsSQL : IDatabase {
        Task<DataTable> GetDataTableAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<dynamic> ExecScalarAsync(EReturnDataType returnType, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<bool> ExecQueryAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<CDbExecProcResult> ExecProcedureAsync(string procedureName, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<int> UpdateTable(DataSet dataSet, string dataSetTableName, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = true);
        Task<DbDataReader> ExecReaderAsync(string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = false);
        Task<string> RetrieveBlob(string stringPathDownload, string stringFileName, string queryString, List<CDbQueryParamBind> bindParam = null, bool closeConnection = false);
    }

    public class CMsSQL : CDatabase, IMsSQL {

        private readonly IApp _app;

        private SqlCommand DatabaseCommand { get; set; }
        private SqlDataAdapter DatabaseAdapter { get; set; }

        public CMsSQL(IApp app, ILogger logger) : base(logger) {
            _app = app;

            InitializeMsSqlDatabase();
        }

        private void InitializeMsSqlDatabase() {
            DbIpAddrss = _app.GetVariabelOraSql("IPSql");
            DbUsername = _app.GetVariabelOraSql("UserSql");
            DbPassword = _app.GetVariabelOraSql("PasswordSql");
            DbName = _app.GetVariabelOraSql("DatabaseSql");
            DbConnectionString = $"Data Source={DbIpAddrss};Initial Catalog={DbName};User ID={DbUsername};Password={DbPassword};";
            DatabaseConnection = new SqlConnection(DbConnectionString);
            DatabaseCommand = new SqlCommand("", (SqlConnection) DatabaseConnection);
            DatabaseAdapter = new SqlDataAdapter(DatabaseCommand);
        }

        private void BindQueryParameter(List<CDbQueryParamBind> parameters) {
            DatabaseCommand.Parameters.Clear();
            if (parameters != null) {
                for (int i = 0; i < parameters.Count; i++) {
                    SqlParameter param = new SqlParameter {
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

    }

}
