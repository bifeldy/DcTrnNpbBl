/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Turunan `CDatabase`
 *              :: Harap Didaftarkan Ke DI Container
 *              :: Instance Semua Database Bridge
 * 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using bifeldy_sd3_lib_452.Databases;
using bifeldy_sd3_lib_452.Handlers;
using bifeldy_sd3_lib_452.Models;
using bifeldy_sd3_lib_452.Utilities;

using DcTrnNpbBl.Models;
using DcTrnNpbBl.Utilities;

namespace DcTrnNpbBl.Handlers {

    public interface IDb : IDbHandler {
        Task BatalPick(string doc_no, decimal seq_no);
        Task<DataTable> LoadDocNoSplit();
        Task<DataTable> LoadSplitByDocNo(string selectedNoRpb);
        Task<bool> UpdateKeterItem(string stokPlanoRakDisplay, decimal seqNo, decimal pluId);
        Task<bool> ProsesSplit1(decimal qty, string lokasi, decimal cellId, string hhPick, string hhScan, string keter, decimal seqNo, decimal pluId);
        Task<bool> ProsesSplit2(decimal seqNo);
        Task<DataTable> LoadDocNoEditSplit();
        Task<DataTable> LoadEditSplitByDocNo(string selectedNoRpb);
        Task<DataTable> LoadDocNoProsesNpb();
        Task<DataTable> LoadProsesNpbByDocNo(string selectedNoRpb);
        Task<DataTable> LoadNpbDcNoReSend();
        Task<DataTable> LoadResendByNpbDcNo(string selectedNoNpb);
        Task<bool> SaveLogApi(string apiDcKode, string apiOwner, string apiPath, string jsonBody, string resApiStr, string status);
        Task<DataTable> ViewLaporanByNpbDcNo(string selectedNoNpb);
        Task<DataTable> ViewLogApi();
    }

    public sealed class CDb : CDbHandler, IDb {

        private readonly IApp _app;
        private readonly IConverter _converter;

        public CDb(IApp app, IOracle oracle, IPostgres postgres, IMsSQL mssql, IConverter converter) : base(app, oracle, postgres, mssql) {
            _app = app;
            _converter = converter;
        }

        public async Task BatalPick(string doc_no, decimal seq_no) {
            try {
                await OraPg.MarkBeforeCommitRollback();
                DataTable dt_no_pesanan = await OraPg.GetDataTableAsync(
                    $@"
                        SELECT DISTINCT
                            NO_PESANAN
                        FROM
                            DC_PICKBL_DTLBL_T
                        WHERE
                            SEQ_FK_NO = :seq_fk_no
                    ",
                    new List<CDbQueryParamBind> {
                            new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = seq_no }
                    }
                );
                if (dt_no_pesanan.Rows.Count <= 0) {
                    throw new Exception($"Tidak Ada No Pesanan");
                }
                List<CMODEL_UNPICK> ls_no_pesanan = _converter.DataTableToList<CMODEL_UNPICK>(dt_no_pesanan);
                string[] arr_no_pesanan = ls_no_pesanan.Select(l => l.NO_PESANAN).ToArray();
                bool update1 = await OraPg.ExecQueryAsync(
                    $@"
                        UPDATE
                            DC_PICKBL_T
                        SET
                            UNPICK = 'Y'
                        WHERE
                            NO_PESANAN IN (:arr_no_pesanan)
                    ",
                    new List<CDbQueryParamBind> {
                            new CDbQueryParamBind { NAME = "arr_no_pesanan", VALUE = arr_no_pesanan }
                    }
                );
                if (!update1) {
                    throw new Exception($"Gagal Set Un-Pick Pesanan '{string.Join(", ", arr_no_pesanan)}'");
                }
                bool delete1 = await OraPg.ExecQueryAsync(
                    $@"
                        DELETE FROM
                            DC_PICKBL_T
                        WHERE
                            NO_PESANAN IN (:arr_no_pesanan) AND
                            UNPICK = 'Y'
                    ",
                    new List<CDbQueryParamBind> {
                            new CDbQueryParamBind { NAME = "arr_no_pesanan", VALUE = arr_no_pesanan }
                    }
                );
                if (!delete1) {
                    throw new Exception($"Gagal Menghapus Header Pesanan '{string.Join(", ", arr_no_pesanan)}'");
                }
                bool delete2 = await OraPg.ExecQueryAsync(
                    $@"
                        DELETE FROM
                            DC_PICKBL_DTLBL_T
                        WHERE
                            SEQ_FK_NO = :seq_fk_no
                    ",
                    new List<CDbQueryParamBind> {
                        new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = seq_no }
                    }
                );
                if (!delete2) {
                    throw new Exception($"Gagal Menghapus Detail Pesanan '{string.Join(", ", arr_no_pesanan)}'");
                }
                bool delete3 = await OraPg.ExecQueryAsync(
                    $@"
                        DELETE FROM
                            DC_PICKBL_DTL_T
                        WHERE
                            SEQ_FK_NO = :seq_fk_no
                    ",
                    new List<CDbQueryParamBind> {
                        new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = seq_no }
                    }
                );
                if (!delete3) {
                    throw new Exception($"Gagal Menghapus Detail RPB '{seq_no}'");
                }
                bool update2 = await OraPg.ExecQueryAsync(
                    $@"
                        UPDATE
                            DC_PICKBL_HDR_T
                        SET
                            UNPICK = 'Y'
                        WHERE
                            DOC_NO = :doc_no
                    ",
                    new List<CDbQueryParamBind> {
                        new CDbQueryParamBind { NAME = "doc_no", VALUE = doc_no }
                    }
                );
                if (!update2) {
                    throw new Exception($"Gagal Set Un-Pick Header RPB '{doc_no}'");
                }
                bool delete4 = await OraPg.ExecQueryAsync(
                    $@"
                        DELETE FROM
                            DC_PICKBL_HDR_T
                        WHERE
                            DOC_NO = :doc_no AND
                            UNPICK = 'Y'
                    ",
                    new List<CDbQueryParamBind> {
                        new CDbQueryParamBind { NAME = "doc_no", VALUE = doc_no }
                    }
                );
                if (!delete4) {
                    throw new Exception($"Gagal Menghapus Header RPB '{doc_no}'");
                }
                OraPg.MarkSuccessCommitAndClose();
            }
            catch (Exception ex) {
                OraPg.MarkFailedRollbackAndClose();
                throw ex;
            }
        }

        public async Task<DataTable> LoadDocNoSplit() {
            return await OraPg.GetDataTableAsync(
                $@"
                    SELECT
                        DOC_NO
                    FROM
                        DC_PICKBL_HDR_T
                    WHERE
                        (TGL_SPLIT IS NULL OR TGL_SPLIT = '') AND
                        (UNPICK IS NULL OR UNPICK <> 'Y')
                "
            );
        }

        public async Task<DataTable> LoadSplitByDocNo(string selectedNoRpb) {
            await OraPg.ExecQueryAsync(
                $@"
                    UPDATE
                        DC_PICKBL_DTL_T
                    SET
                        KETER = :keter
                    WHERE
                        (KETER IS NOT NULL OR KETER <> '')
                        AND SEQ_FK_NO IN (
                            SELECT SEQ_NO
                            FROM DC_PICKBL_HDR_T
                            WHERE DOC_NO = :seq_fk_no
                        )
                ",
                new List<CDbQueryParamBind> {
                    new CDbQueryParamBind { NAME = "keter", VALUE = null },
                    new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = selectedNoRpb }
                }
            );
            return await OraPg.GetDataTableAsync(
                $@"
                    SELECT
                        a.DC_KODE,
                        a.SEQ_NO,
                        a.SEQ_DATE,
                        a.DOC_NO,
                        a.DOC_DATE,
                        b.PLU_ID,
                        c.MBR_SINGKATAN AS SINGKATAN,
                        (d.PLA_LINE || '.' || d.PLA_RAK || '.' || d.PLA_SHELF || '.' || d.PLA_CELL) AS LOKASI,
                        d.PLA_CELLID AS CELLID_PLANO,
                        d.PLA_QTY_STOK,
                        b.QTY_RPB,
                        b.KETER,
                        d.PLA_DISPLAY,
                        b.IP_PICKING AS HH_PICK,
                        b.IP_SCANNING AS HH_SCAN
                    FROM
                        DC_PICKBL_HDR_T a,
                        DC_PICKBL_DTL_T b,
                        DC_BARANG_T c,
                        (
                            SELECT *
                            FROM DC_PLANOGRAM_T WHERE PLA_DISPLAY = 'Y'
                        ) d
                    WHERE
                        a.DOC_NO = :doc_no AND
                        (a.UNPICK IS NULL OR a.UNPICK <> 'Y') AND
                        a.SEQ_NO = b.SEQ_FK_NO AND
                        b.PLU_ID = c.MBR_PLUID AND
                        b.PLU_ID = d.PLA_FK_PLUID (+) AND
                        (a.TGL_SPLIT IS NULL OR a.TGL_SPLIT = '')
                ",
                new List<CDbQueryParamBind> {
                            new CDbQueryParamBind { NAME = "doc_no", VALUE = selectedNoRpb }
                }
            );
        }

        public async Task<bool> UpdateKeterItem(string keter, decimal seqNo, decimal pluId) {
            return await OraPg.ExecQueryAsync(
                $@"
                    UPDATE
                        DC_PICKBL_DTL_T
                    SET
                        KETER = :keter
                    WHERE
                        SEQ_FK_NO = :seq_fk_no AND
                        PLU_ID = :plu_id
                ",
                new List<CDbQueryParamBind> {
                    new CDbQueryParamBind { NAME = "keter", VALUE = keter },
                    new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = seqNo },
                    new CDbQueryParamBind { NAME = "plu_id", VALUE = pluId }
                }
            );
        }

        public async Task<bool> ProsesSplit1(decimal qty, string lokasi, decimal cellId, string hhPick, string hhScan, string keter, decimal seqNo, decimal pluId) {
            return await OraPg.ExecQueryAsync(
                $@"
                    UPDATE
                        DC_PICKBL_DTL_T
                    SET
                        QTY_PICK = :qty_rpb,
                        RAK_PLANO = :rak_plano,
                        CELLID_PLANO = :cellid_plano,
                        IP_PICKING = :ip_picking,
                        IP_SCANNING = :ip_scanning,
                        KETER = :keter
                    WHERE
                        SEQ_FK_NO = :seq_fk_no AND
                        PLU_ID = :plu_id
                ",
                new List<CDbQueryParamBind> {
                    new CDbQueryParamBind { NAME = "qty_rpb", VALUE = qty },
                    new CDbQueryParamBind { NAME = "rak_plano", VALUE = lokasi },
                    new CDbQueryParamBind { NAME = "cellid_plano", VALUE = cellId },
                    new CDbQueryParamBind { NAME = "ip_picking", VALUE = hhPick },
                    new CDbQueryParamBind { NAME = "ip_scanning", VALUE = hhScan },
                    new CDbQueryParamBind { NAME = "keter", VALUE = keter },
                    new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = seqNo },
                    new CDbQueryParamBind { NAME = "plu_id", VALUE = pluId }
                }
            );
        }

        public async Task<bool> ProsesSplit2(decimal seqNo) {
            return await OraPg.ExecQueryAsync(
                $@"
                    UPDATE
                        DC_PICKBL_HDR_T
                    SET
                        TGL_SPLIT = SYSDATE
                    WHERE
                        SEQ_NO = :seq_no
                ",
                new List<CDbQueryParamBind> {
                    new CDbQueryParamBind { NAME = "seq_no", VALUE = seqNo }
                }
            );
        }

        public async Task<DataTable> LoadDocNoEditSplit() {
            return await OraPg.GetDataTableAsync(
                $@"
                    SELECT
                        DOC_NO
                    FROM
                        DC_PICKBL_HDR_T
                    WHERE
                        (TGL_SPLIT IS NOT NULL OR TGL_SPLIT <> '') AND
                        (UNPICK IS NULL OR UNPICK <> 'Y')
                "
            );
        }

        public async Task<DataTable> LoadEditSplitByDocNo(string selectedNoRpb) {
            return await OraPg.GetDataTableAsync(
                $@"
                    SELECT
                        a.DC_KODE,
                        a.SEQ_NO,
                        a.SEQ_DATE,
                        a.DOC_NO,
                        a.DOC_DATE,
                        b.PLU_ID,
                        c.MBR_SINGKATAN AS SINGKATAN,
                        a.TGL_SPLIT,
                        (PLA_LINE || '.' || PLA_RAK || '.' || PLA_SHELF || '.' || PLA_CELL) AS LOKASI,
                        b.QTY_RPB,
                        b.TIME_PICKING,
                        b.IP_PICKING AS HH_PICK,
                        b.TIME_SCANNING,
                        b.IP_SCANNING AS HH_SCAN
                    FROM
                        DC_PICKBL_HDR_T a,
                        DC_PICKBL_DTL_T b,
                        DC_BARANG_T c,
                        (
                            SELECT *
                            FROM DC_PLANOGRAM_T WHERE PLA_DISPLAY = 'Y'
                        ) d
                    WHERE
                        a.DOC_NO = :doc_no AND
                        (a.UNPICK IS NULL OR a.UNPICK <> 'Y') AND
                        a.SEQ_NO = b.SEQ_FK_NO AND
                        b.PLU_ID = c.MBR_PLUID AND
                        b.PLU_ID = d.PLA_FK_PLUID (+) AND
                        (a.TGL_SPLIT IS NOT NULL OR a.TGL_SPLIT <> '')
                ",
                new List<CDbQueryParamBind> {
                    new CDbQueryParamBind { NAME = "doc_no", VALUE = selectedNoRpb }
                }
            );
        }

        public async Task<DataTable> LoadDocNoProsesNpb() {
            return await OraPg.GetDataTableAsync(
                $@"
                    SELECT
                        DOC_NO
                    FROM
                        DC_PICKBL_HDR_T
                    WHERE
                        (TGL_SPLIT IS NOT NULL OR TGL_SPLIT <> '') AND
                        (NPBDC_DATE IS NULL OR NPBDC_DATE = '') AND
                        (UNPICK IS NULL OR UNPICK <> 'Y')
                "
            );
        }

        public async Task<DataTable> LoadProsesNpbByDocNo(string selectedNoRpb) {
            return await OraPg.GetDataTableAsync(
                $@"
                    SELECT
                        a.DC_KODE,
                        a.SEQ_NO,
                        a.SEQ_DATE,
                        a.DOC_NO,
                        a.DOC_DATE,
                        b.PLU_ID,
                        c.MBR_SINGKATAN AS SINGKATAN,
                        (d.PLA_LINE || '.' || d.PLA_RAK || '.' || d.PLA_SHELF || '.' || d.PLA_CELL) AS LOKASI,
                        b.QTY_RPB AS QTY,
                        b.QTY_PICKING AS PICK,
                        b.QTY_SCANNING AS SCAN,
                        CAST(b.HPP as DECIMAL(30,3)) AS HPP,
                        CAST(NVL(c.mbr_acost, NVL(c.mbr_lcost, 0)) as DECIMAL(30,3)) AS PRICE,
                        CAST(b.QTY_SCANNING *
                            NVL (c.mbr_acost, NVL(c.mbr_lcost, 0))
                                as DECIMAL(30,3))
                                    AS GROSS,
                        CASE
                            WHEN
                                a.WHK_NPWP <> e.TBL_NPWP_DC AND
                                c.MBR_BKP = 'Y'
                            THEN
                                CAST((GETPPNNEW(b.PLU_ID)/100) *
                                    (b.QTY_RPB * NVL (c.mbr_acost, NVL(c.mbr_lcost, 0)))
                                        as DECIMAL(30,3))
                            ELSE
                                0
                        END AS PPN,
                        b.PPN_RATE,
                        b.SJ_QTY,
                        b.TGLEXP,
                        a.NPBDC_NO,
                        a.NPBDC_DATE,
                        a.WHK_KODE,
                        e.TBL_NPWP_DC,
                        a.START_PICKING,
                        b.TIME_PICKING,
                        a.STOP_PICKING,
                        a.START_SCANNING,
                        b.TIME_SCANNING,
                        a.STOP_SCANNING
                    FROM
                        DC_PICKBL_HDR_T a,
                        DC_PICKBL_DTL_T b,
                        DC_BARANG_DC_V c,
                        (
                            SELECT *
                            FROM DC_PLANOGRAM_T WHERE PLA_DISPLAY = 'Y'
                        ) d,
                        DC_TABEL_DC_T e
                    WHERE
                        a.DOC_NO = :doc_no AND
                        (a.UNPICK IS NULL OR a.UNPICK <> 'Y') AND
                        a.SEQ_NO = b.SEQ_FK_NO AND
                        b.PLU_ID = c.MBR_FK_PLUID AND
                        b.PLU_ID = d.PLA_FK_PLUID (+) AND
                        (a.TGL_SPLIT IS NOT NULL OR a.TGL_SPLIT <> '') AND
                        (a.NPBDC_DATE IS NULL OR a.NPBDC_DATE = '')
                ",
                new List<CDbQueryParamBind> {
                    new CDbQueryParamBind { NAME = "doc_no", VALUE = selectedNoRpb }
                }
            );
        }

        public async Task<DataTable> LoadNpbDcNoReSend() {
            return await OraPg.GetDataTableAsync(
                $@"
                    SELECT NPBDC_NO FROM DC_PICKBL_HDR_H
                    WHERE
                        (NPBDC_DATE IS NOT NULL OR NPBDC_DATE <> '') AND
                        (UNPICK IS NULL OR UNPICK <> 'Y')
                "
            );
        }

        public async Task<DataTable> LoadResendByNpbDcNo(string selectedNoNpb) {
            return await OraPg.GetDataTableAsync(
                $@"
                    SELECT
                        a.DC_KODE,
                        a.SEQ_NO,
                        a.SEQ_DATE,
                        a.DOC_NO,
                        a.DOC_DATE,
                        b.PLU_ID,
                        c.MBR_SINGKATAN AS SINGKATAN,
                        (d.PLA_LINE || '.' || d.PLA_RAK || '.' || d.PLA_SHELF || '.' || d.PLA_CELL) AS LOKASI,
                        b.QTY_RPB AS QTY,
                        b.QTY_PICKING AS PICK,
                        b.QTY_SCANNING AS SCAN,
                        CAST(b.HPP as DECIMAL(30,3)) AS HPP,
                        CAST(b.PRICE as DECIMAL(30,3)) AS PRICE,
                        CAST(b.GROSS as DECIMAL(30,3)) AS GROSS,
                        CAST(b.PPN as DECIMAL(30,3)) AS PPN,
                        b.PPN_RATE,
                        b.SJ_QTY,
                        b.TGLEXP,
                        a.NPBDC_NO,
                        a.NPBDC_DATE,
                        a.WHK_KODE,
                        e.TBL_NPWP_DC
                    FROM
                        DC_PICKBL_HDR_H a,
                        DC_PICKBL_DTL_H b,
                        DC_BARANG_DC_V c,
                        (
                            SELECT *
                            FROM DC_PLANOGRAM_T WHERE PLA_DISPLAY = 'Y'
                        ) d,
                        DC_TABEL_DC_T e
                    WHERE
                        a.NPBDC_NO = :npbdc_no AND
                        a.SEQ_NO = b.SEQ_FK_NO AND
                        b.PLU_ID = c.MBR_FK_PLUID AND
                        c.MBR_FK_PLUID = d.PLA_FK_PLUID (+) AND
		                d.PLA_DC_KODE = a.DC_KODE
                ",
                new List<CDbQueryParamBind> {
                    new CDbQueryParamBind { NAME = "npbdc_no", VALUE = selectedNoNpb }
                }
            );
        }

        public async Task<bool> SaveLogApi(string apiDcKode, string apiOwner, string apiPath, string jsonBody, string resApiStr, string status) {
            return await OraPg.ExecQueryAsync(
                $@"
                    INSERT INTO LOG_API_DC (KODEDC, PEMILIKAPI, NAMAMETHOD, DATAKIRIM, DATABALIK, TANGGAL, STATUS)
                    VALUES (:kode_dc, :pemilik_api, :nama_method, :data_kirim, :data_balik, :tanggal, :status)
                ",
                new List<CDbQueryParamBind> {
                    new CDbQueryParamBind { NAME = "kode_dc", VALUE = apiDcKode },
                    new CDbQueryParamBind { NAME = "pemilik_api", VALUE = apiOwner },
                    new CDbQueryParamBind { NAME = "nama_method", VALUE = apiPath },
                    new CDbQueryParamBind { NAME = "data_kirim", VALUE = jsonBody },
                    new CDbQueryParamBind { NAME = "data_balik", VALUE = resApiStr },
                    new CDbQueryParamBind { NAME = "tanggal", VALUE = DateTime.Now },
                    new CDbQueryParamBind { NAME = "status", VALUE = status }
                }
            );
        }

        public async Task<DataTable> ViewLaporanByNpbDcNo(string selectedNoNpb) {
            return await OraPg.GetDataTableAsync(
                $@"
                    SELECT
                        e.doc_no as doc_no,
                        a.hdr_no_doc AS no_npb,
                        TO_CHAR (a.hdr_tgl_doc, 'dd-mm-yyyy') AS tgl_npb, 
                        e.dc_kode AS pengirim,
                        e.whk_kode AS penerima,
                        TO_CHAR (a.hdr_no_inv) || '/' || e.dc_kode || '/' || TO_CHAR(a.hdr_tgl_inv,'dd-mm-yyyy') AS no_ref,
                        f.rak_plano AS alamat_planogram,
                        b.his_fk_pluid AS pluid,
                        c.mbr_singkatan AS deskripsi,
                        f.fraction AS fraction, 
                        TO_NUMBER(NVL(b.f_dummy_1, '0')) AS minta,
                        b.his_qty AS kirim,
                        ROUND(b.his_qty * d.mbr_panjang * d.mbr_lebar * d.mbr_tinggi / 1000000, 6) AS volume,
                        ROUND((b.his_price + b.his_ppn), 5) AS dppppn, 
                        b.his_ppn AS ppn,
                        ROUND(((b.his_price + b.his_ppn) * b.his_qty), 5) AS total
                    FROM
                        DC_HEADER_TRANSAKSI_T a,
                        DC_HISTORY_TRANSAKSI_T b,
                        dc_barang_dc_v c,
                        (
                            SELECT
                                *
                            FROM
                                DC_BRG_DIMENSI_T
                            WHERE
                                mbr_flag_pcs = 'Y'
                        ) d,
                        DC_PICKBL_HDR_H e,
                        DC_PICKBL_DTL_H f,
                        (
                            SELECT
                                z.hdr_hdr_id,
                                z.hdr_tbl_lokasiid
                            FROM 
                                DC_PICKBL_HDR_H y,
                                DC_HEADER_TRANSAKSI_T z
                            WHERE
                                y.NPBDC_NO = :npbdc_no
                                AND z.HDR_NO_INV = y.SEQ_NO
                                AND z.hdr_type_trans = 'NPB DC'
                        ) g
                    WHERE
                        a.hdr_hdr_id = b.his_hdr_fk_id
                        AND a.hdr_tbl_lokasiid = b.his_tbl_lokasiid
                        AND b.his_fk_pluid = c.mbr_fk_pluid
                        AND a.hdr_fk_dcid = c.mbr_fk_dcid
                        AND b.his_fk_pluid = d.mbr_fk_pluid(+)
                        AND a.hdr_no_inv = e.seq_no
                        AND b.his_fk_pluid = f.plu_id
                        AND e.seq_no = f.seq_fk_no
                        AND a.hdr_type_trans = 'NPB DC'
                        AND b.his_type_trans = 'NPB DC'
                        /* AND a.hdr_hdr_id = '13315752' --param */
                        AND a.hdr_hdr_id = g.hdr_hdr_id
                        /* AND a.hdr_tbl_lokasiid = '124' --param */
                        AND a.hdr_tbl_lokasiid = g.hdr_tbl_lokasiid
                    ORDER BY
                        b.his_fk_plukode ASC
                ",
                new List<CDbQueryParamBind> {
                    new CDbQueryParamBind { NAME = "npbdc_no", VALUE = selectedNoNpb }
                }
            );
        }

        public async Task<DataTable> ViewLogApi() {
            return await OraPg.GetDataTableAsync(
                $@"
                    SELECT *
                    FROM (
                        SELECT *
                        FROM LOG_API_DC
                        WHERE PEMILIKAPI = 'TAG_BL'
                        ORDER BY TANGGAL DESC
                    )
                    WHERE ROWNUM <= 50
                "
            );
        }

    }

}
