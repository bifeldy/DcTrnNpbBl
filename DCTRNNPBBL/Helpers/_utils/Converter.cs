/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Alat Konversi
 *              :: Harap Didaftarkan Ke DI Container
 * 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DCTRNNPBBL.Helpers._utils {

    public interface IConverter {
        List<T> ConvertDataTableToList<T>(DataTable dt);
    }

    public class CConverter : IConverter {

        private readonly ILogger _logger;

        public CConverter(ILogger logger) {
            _logger = logger;
        }

        public List<T> ConvertDataTableToList<T>(DataTable dt) {
            List<string> columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            PropertyInfo[] properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row => {
                T objT = Activator.CreateInstance<T>();
                foreach (PropertyInfo pro in properties) {
                    if (columnNames.Contains(pro.Name.ToLower())) {
                        try {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) {
                            _logger.WriteError(ex);
                        }
                    }
                }
                return objT;
            }).ToList();
        }

    }

}
