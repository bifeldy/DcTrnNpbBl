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

namespace DcTrnNpbBl.Models {

    public class CMODEL_TABEL_DC_PICKBL_HDR_T {
        public string DC_KODE { set; get; }
        public string WHK_KODE { set; get; }
        public decimal SEQ_NO { set; get; }
        public DateTime SEQ_DATE { set; get; }
        public string DOC_NO { set; get; }
        public DateTime DOC_DATE { set; get; }
        public decimal NPBDC_NO { set; get; }
        public DateTime NPBDC_DATE { set; get; }
        public string UNPICK { set; get; }
        public DateTime UPDREC_DATE { set; get; }
        public string KETER { set; get; }
        public string API_PENGIRIM { set; get; }
        public string API_TERIMA { set; get; }
        public DateTime START_PICKING { set; get; }
        public DateTime STOP_PICKING { set; get; }
        public DateTime START_SCANNING { set; get; }
        public DateTime STOP_SCANNING { set; get; }
        public DateTime UPDREC_ID { set; get; }
        public string WHK_NPWP { set; get; }
        public DateTime TGL_SPLIT { set; get; }
        public DateTime LOG_TLGTERIMA_RPB { set; get; }
    }

}
