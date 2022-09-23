﻿/**
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

using System.Data.Common;

namespace DCTRNNPBBL.Helpers._models {

    public class CDbExecProcResult {
        public bool STATUS { get; set; }
        public string QUERY { get; set; }
        public DbParameterCollection PARAMETERS { get; set; }
    }

}