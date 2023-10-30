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

namespace DcTrnNpbBl.Models {

    class CMODEL_GRID_TRANSFER_RESEND_NPB {
        public string DC_KODE { set; get; }
        public decimal SEQ_NO { set; get; }
        public DateTime SEQ_DATE { set; get; }
        public string DOC_NO { set; get; }
        public DateTime DOC_DATE { set; get; }
        public decimal PLU_ID { set; get; }
        public string SINGKATAN { set; get; }
        public string LOKASI { set; get; }
        public decimal QTY { set; get; }
        public decimal PICK { set; get; }
        public decimal SCAN { set; get; }
        public decimal HPP { set; get; }
        public decimal PRICE { set; get; }
        public decimal GROSS { set; get; }
        public decimal PPN { set; get; }
        public decimal PPN_RATE { set; get; }
        public decimal SJ_QTY { set; get; }
        public DateTime TGLEXP { set; get; }
        public decimal NPBDC_NO { set; get; }
        public DateTime NPBDC_DATE { set; get; }
        public string WHK_KODE { set; get; }
        public string TBL_NPWP_DC { set; get; }
        public DateTime START_PICKING { set; get; }
        public DateTime TIME_PICKING { set; get; }
        public DateTime STOP_PICKING { set; get; }
        public DateTime START_SCANNING { set; get; }
        public DateTime TIME_SCANNING { set; get; }
        public DateTime STOP_SCANNING { set; get; }
    }

}
