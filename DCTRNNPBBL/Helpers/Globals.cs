/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Ini Gajelas Sih .. Wkwk
 *              :: Buat Nge-Hold Semua
 * 
 */

using DCTRNNPBBL.Forms;
using DCTRNNPBBL.Panels;

namespace DCTRNNPBBL.Helpers {

    public interface IGlobals {
        CMain Main { get; set; }
        CekProgram CekProgram { get; set; }
        Login Login { get; set; }
    }

    class CGlobals : IGlobals {
        public CMain Main { get; set; }
        public CekProgram CekProgram { get; set; }
        public Login Login { get; set; }
    }

}
