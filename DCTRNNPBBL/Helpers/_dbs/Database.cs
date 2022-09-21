/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Base Class Database
 *              :: Tidak Untuk Didaftarkan Ke DI Container
 *              :: Hanya Untuk Inherit
 * 
 */

using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;

using DCTRNNPBBL.Helpers._models;
using DCTRNNPBBL.Helpers._utils;

namespace DCTRNNPBBL.Helpers._db {

    public interface IDatabase {
        Task MarkBeforeExecQueryCommitAndRollback();
        void MarkSuccessExecQueryAndCommit();
        void MarkFailExecQueryAndRollback();
        void CloseConnection();
    }

    public abstract class CDatabase : IDatabase {

        private readonly ILogger _logger;

        protected DbConnection DatabaseConnection { get; set; }
        protected DbTransaction DatabaseTransaction { get; set; }

        public string DbUsername { get; set; }
        public string DbPassword { get; set; }
        public string DbIpAddrss { get; set; }
        public string DbPort { get; set; }
        public string DbName { get; set; }
        public string DbTnsOdp { get; set; }
        public string DbConnectionString { get; set; }

        public CDatabase(ILogger logger) {
            _logger = logger;
        }

        public void CloseConnection() {
            if (DatabaseConnection.State == ConnectionState.Open) {
                DatabaseConnection.Close();
            }
            if (DatabaseTransaction != null) {
                DatabaseTransaction.Dispose();
                DatabaseTransaction = null;
            }
        }

        public async Task MarkBeforeExecQueryCommitAndRollback() {
            if (DatabaseConnection.State != ConnectionState.Open) {
                await DatabaseConnection.OpenAsync();
            }
            DatabaseTransaction = DatabaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void MarkSuccessExecQueryAndCommit() {
            if (DatabaseTransaction != null) {
                DatabaseTransaction.Commit();
            }
            CloseConnection();
        }

        public void MarkFailExecQueryAndRollback() {
            if (DatabaseTransaction != null) {
                DatabaseTransaction.Rollback();
            }
            CloseConnection();
        }

        protected virtual async Task<DataTable> GetDataTableAsync(DbDataAdapter dataAdapter, bool autoCloseConnection = true) {
            DataTable dataTable = new DataTable();
            try {
                if (DatabaseConnection.State == ConnectionState.Open) {
                    throw new Exception("Database Connection Already In Use!");
                }
                else {
                    await DatabaseConnection.OpenAsync();
                    dataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex) {
                _logger.WriteError(ex);
            }
            finally {
                if (autoCloseConnection) {
                    DatabaseConnection.Close();
                }
            }
            return dataTable;
        }

        protected virtual async Task<dynamic> ExecScalarAsync(DbCommand databaseCommand, EReturnDataType returnType, bool autoCloseConnection = true) {
            dynamic result = null;
            try {
                if (DatabaseConnection.State == ConnectionState.Open) {
                    throw new Exception("Database Connection Already In Use!");
                }
                else {
                    await DatabaseConnection.OpenAsync();
                    object _obj = await databaseCommand.ExecuteScalarAsync();
                    switch (returnType) {
                        case EReturnDataType.FLOAT:
                        case EReturnDataType.DOUBLE:
                            result = (_obj == null) ? 0 : Convert.ToDouble(_obj.ToString());
                            break;
                        case EReturnDataType.INT:
                        case EReturnDataType.INT32:
                        case EReturnDataType.INTEGER:
                            result = (_obj == null) ? 0 : Convert.ToInt32(_obj.ToString());
                            break;
                        case EReturnDataType.STR:
                        case EReturnDataType.STRING:
                        default:
                            result = _obj?.ToString();
                            break;
                    }
                }
            }
            catch (Exception ex) {
                switch (returnType) {
                    case EReturnDataType.STR:
                    case EReturnDataType.STRING:
                        result = null;
                        break;
                    default:
                        result = 0;
                        break;
                }
                _logger.WriteError(ex);
            }
            finally {
                if (autoCloseConnection) {
                    DatabaseConnection.Close();
                }
            }
            return result;
        }

        // Harap Jalankan `await MarkBeforeCommitAndRollback();` Telebih Dahulu Jika `autoCloseConnection = false`
        // Lalu Bisa Menjalankan `ExecQueryAsync();` Berkali - Kali (Dengan Koneksi Yang Sama)
        // Setelah Selesai Panggil `MarkSuccessExecQueryAndCommit();` atau `MarkFailExecQueryAndRollback();` Jika Gagal
        protected virtual async Task<bool> ExecQueryAsync(DbCommand databaseCommand, bool autoCloseConnection = true) {
            bool result = false;
            try {
                if (autoCloseConnection) {
                    if (DatabaseConnection.State == ConnectionState.Open) {
                        throw new Exception("Database Connection Already In Use!");
                    }
                    await DatabaseConnection.OpenAsync();
                }
                result = await databaseCommand.ExecuteNonQueryAsync() >= 0;
            }
            catch (Exception ex) {
                _logger.WriteError(ex);
            }
            finally {
                if (autoCloseConnection) {
                    DatabaseConnection.Close();
                }
            }
            return result;
        }

        protected virtual async Task<bool> ExecProcedureAsync(DbCommand databaseCommand, bool autoCloseConnection = true) {
            bool result = false;
            try {
                if (DatabaseConnection.State == ConnectionState.Open) {
                    throw new Exception("Database Connection Already In Use!");
                }
                else {
                    await DatabaseConnection.OpenAsync();
                    // databaseCommand.CommandType = CommandType.StoredProcedure;
                    result = await databaseCommand.ExecuteNonQueryAsync() == -1;
                }
            }
            catch (Exception ex) {
                _logger.WriteError(ex);
            }
            finally {
                if (autoCloseConnection) {
                    DatabaseConnection.Close();
                }
            }
            return result;
        }

        protected virtual async Task<int> UpdateTable(DbDataAdapter dataAdapter, DataSet dataSet, string dataSetTableName, bool autoCloseConnection = true) {
            int result = 0;
            try {
                if (DatabaseConnection.State == ConnectionState.Open) {
                    throw new Exception("Database Connection Already In Use!");
                }
                else {
                    await DatabaseConnection.OpenAsync();
                    result = dataAdapter.Update(dataSet, dataSetTableName);
                }
            }
            catch (Exception ex) {
                _logger.WriteError(ex);
            }
            finally {
                if (autoCloseConnection) {
                    DatabaseConnection.Close();
                }
            }
            return result;
        }

        protected virtual async Task<DbDataReader> ExecReaderAsync(DbCommand databaseCommand, bool autoCloseConnection = false) {
            DbDataReader result = null;
            try {
                if (DatabaseConnection.State == ConnectionState.Open) {
                    throw new Exception("Database Connection Already In Use!");
                }
                else {
                    await DatabaseConnection.OpenAsync();
                    result = await databaseCommand.ExecuteReaderAsync();
                }
            }
            catch (Exception ex) {
                _logger.WriteError(ex);
            }
            finally {
                if (autoCloseConnection) {
                    DatabaseConnection.Close();
                }
            }
            return result;
        }

        protected virtual async Task<string> RetrieveBlob(DbCommand databaseCommand, string stringPathDownload, string stringFileName, bool autoCloseConnection = false) {
            string result = "";
            try {
                if (DatabaseConnection.State == ConnectionState.Open) {
                    throw new Exception("Database Connection Already In Use!");
                }
                else {
                    await DatabaseConnection.OpenAsync();
                    DbDataReader rdrGetBlob = await databaseCommand.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
                    if (!rdrGetBlob.HasRows) {
                        throw new Exception("Error file not found");
                    }
                    while (await rdrGetBlob.ReadAsync()) {
                        FileStream fs = new FileStream($"{stringPathDownload}/{stringFileName}", FileMode.OpenOrCreate, FileAccess.Write);
                        BinaryWriter bw = new BinaryWriter(fs);
                        long startIndex = 0;
                        int bufferSize = 8192;
                        byte[] outbyte = new byte[bufferSize - 1];
                        int retval = (int)rdrGetBlob.GetBytes(0, startIndex, outbyte, 0, bufferSize);
                        while (retval != bufferSize) {
                            bw.Write(outbyte);
                            bw.Flush();
                            Array.Clear(outbyte, 0, bufferSize);
                            startIndex += bufferSize;
                            retval = (int)rdrGetBlob.GetBytes(0, startIndex, outbyte, 0, bufferSize);
                        }
                        bw.Write(outbyte, 0, (retval > 0 ? retval : 1) - 1);
                        bw.Flush();
                        bw.Close();
                    }
                    rdrGetBlob.Close();
                    rdrGetBlob = null;
                }
            }
            catch (Exception ex) {
                _logger.WriteError(ex);
                result = ex.Message;
            }
            finally {
                if (autoCloseConnection) {
                    DatabaseConnection.Close();
                }
            }
            return result;
        }

    }

}
