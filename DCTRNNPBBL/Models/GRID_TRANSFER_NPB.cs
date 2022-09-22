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

    class CMODEL_GRID_TRANSFER_NPB {
        public decimal PLU_ID { set; get; }
        public string SINGKATAN { set; get; }
        public string LOKASI { set; get; }
        public decimal QTY_RPB { set; get; }
        public decimal QTY_PICKING { set; get; }
        public decimal QTY_SCANNING { set; get; }
        public decimal PRICE { set; get; }
        public decimal GROSS { set; get; }
        public decimal PPN { set; get; }
    }

}
