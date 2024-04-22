/**
* 
* Author       :: Basilius Bias Astho Christyono
* Phone        :: (+62) 889 236 6466
* 
* Department   :: IT SD 03
* Mail         :: bias@indomaret.co.id
* 
* Catatan      :: Model Dataset
*              :: Tidak Untuk Didaftarkan Ke DI Container
* 
*/

using System;

namespace DcTrnNpbBl.Models {

    public class CMODEL_DS_NPBTAGBL {
        public decimal SJ_QTY { set; get; }
        public DateTime DOC_DATE { set; get; }
        public string TBL_NPWP_DC { set; get; }
        public decimal NPBDC_NO { set; get; }
        public DateTime NPBDC_DATE { set; get; }
        public decimal HPP { set; get; }
        public decimal PRICE { set; get; }
        public decimal GROSS { set; get; }
        public decimal PPN_RATE { set; get; }
        public DateTime TGLEXP { set; get; }
        public string DOC_NO { set; get; }
        public string NO_NPB { set; get; }
        public string TGL_NPB { set; get; }
        public string PENGIRIM { set; get; }
        public string PENERIMA { set; get; }
        public string NO_REF { set; get; }
        public string ALAMAT_PLANOGRAM { set; get; }
        public decimal PLUID { set; get; }
        public string DESKRIPSI { set; get; }
        public decimal FRACTION { set; get; }
        public decimal MINTA { set; get; }
        public decimal KIRIM { set; get; }
        public decimal VOLUME { set; get; }
        public decimal DPPPPN { set; get; }
        public decimal PPN { set; get; }
        public decimal TOTAL { set; get; }
    }

}
