/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Model Grid
 *              :: Tidak Untuk Didaftarkan Ke DI Container
 * 
 */

using System;

namespace DCTRNNPBBL.Models {

    public class CMODEL_GRID_SPLIT {
        public string DC_KODE { set; get; }
        public decimal SEQ_NO { set; get; }
        public DateTime DOC_DATE { set; get; }
        public decimal PLU_ID { set; get; }
        public string SINGKATAN { set; get; }
        public string LOKASI { set; get; }
        public decimal QTY_RPB { set; get; }
        public string HH_PICK { set; get; }
        public string HH_SCAN { set; get; }
    }

}
