/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Query Binding Dengan Parameter Turunan `DbParameter`
 *              :: Tidak Untuk Didaftarkan Ke DI Container
 * 
 */

using System.Data;

using System.Data.SqlTypes;
using Oracle.ManagedDataAccess.Types;
using NpgsqlTypes;

namespace DCTRNNPBBL.Helpers._models {

    public class CDbQueryParamBind {
        public string NAME { get; set; }
        public object VALUE { get; set; }
        public int SIZE { get; set; }
        public ParameterDirection DIRECTION { get; set; }
    }

}