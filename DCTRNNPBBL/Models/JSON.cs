/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Model Komunikasi JSON
 *              :: Tidak Untuk Didaftarkan Ke DI Container
 * 
 */

using System;
using System.Collections.Generic;

namespace DCTRNNPBBL.Models {

    public class CMODEL_JSON_KIRIM_DCHO {
        public string TipeData { set; get; }
        public string JenisIP { set; get; }
        public string KodeDC { set; get; }
    }

    public class CMODEL_JSON_TERIMA_DCHO {
        public bool IsSuccess { set; get; }
        public string ResultCode { set; get; }
        public string ResultDetail { set; get; }
        public string Url { set; get; }
    }

    public class CMODEL_JSON_KIRIM_NPB_BL_DETAIL {
        public decimal plu_id { set; get; }
        public decimal sj_qty { set; get; }
        public decimal hpp { set; get; }
        public decimal price { set; get; }
        public decimal gross { set; get; }
        public decimal ppn_rate { set; get; }
        public DateTime tglexp { set; get; }
    }

    public class CMODEL_JSON_KIRIM_NPB_BL {
        public string doc_no { set; get; }
        public DateTime doc_date { set; get; }
        public string dc_kode { set; get; }
        public string whk_kode { set; get; }
        public string dc_npwp { set; get; }
        public decimal npbdc_no { set; get; }
        public DateTime npbdc_date { set; get; }
        public List<CMODEL_JSON_KIRIM_NPB_BL_DETAIL> detail { set; get; }
    }

    public class CMODEL_JSON_TERIMA_NPB_BL {
        public bool IsSuccess { set; get; }
        public string Info { set; get; }
    }

}