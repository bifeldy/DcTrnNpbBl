/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Model Tabel
 *              :: Tidak Untuk Didaftarkan Ke DI Container
 * 
 */

using System;

namespace DCTRNNPBBL.Models {

    public class CMODEL_TABEL_DC_PICKBL_DTL_T {
        public decimal SEQ_FK_NO { set; get; }
        public decimal PLU_ID { set; get; }
        public string JENIS_ITEM { set; get; }
        public decimal FRACTION { set; get; }
        public decimal QTY_RPB { set; get; }
        public decimal QTY_PICK { set; get; }
        public decimal SJ_QTY { set; get; }
        public decimal HPP { set; get; }
        public decimal PRICE { set; get; }
        public decimal GROSS { set; get; }
        public decimal PPN { set; get; }
        public string RAK_PLANO { set; get; }
        public decimal CELLID_PLANO { set; get; }
        public decimal QTY_PICKING { set; get; }
        public DateTime TIME_PICKING { set; get; }
        public string USER_PICKING { set; get; }
        public string IP_PICKING { set; get; }
        public decimal QTY_SCANNING { set; get; }
        public DateTime TIME_SCANNING { set; get; }
        public string USER_SCANNING { set; get; }
        public string IP_SCANNING { set; get; }
        public string UPDREC_ID { set; get; }
        public DateTime UPDREC_DATE { set; get; }
        public string KETER { set; get; }
        public DateTime TGLEXP { set; get; }
        public decimal PPN_RATE { set; get; }
        public DateTime LOG_TGL_SEND { set; get; }
        public string LOG_STAT_SEND { set; get; }
        public string LOG_API_TUJUAN { set; get; }
    }

}