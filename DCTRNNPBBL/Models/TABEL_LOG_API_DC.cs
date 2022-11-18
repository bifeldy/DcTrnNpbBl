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

    class CMODEL_TABEL_LOG_API_DC {
        public string KODEDC { set; get; }
        public string PEMILIKAPI { set; get; }
        public string NAMAMETHOD { set; get; }
        public string DATAKIRIM { set; get; }
        public string DATABALIK { set; get; }
        public DateTime TANGGAL { set; get; }
        public string STATUS { set; get; }
        public string KETERANGAN { set; get; }
    }

}
