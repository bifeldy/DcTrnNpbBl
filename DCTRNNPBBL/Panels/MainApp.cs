﻿/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Aplikasi Start Point
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

using Autofac;

using DCTRNNPBBL.Helpers._db;
using DCTRNNPBBL.Helpers._models;
using DCTRNNPBBL.Helpers._utils;
using DCTRNNPBBL.Models;

namespace DCTRNNPBBL.Panels {

    public partial class MainApp : UserControl {

        private readonly IApp _app;
        private readonly ILogger _logger;
        private readonly IOracle _oracle;
        private readonly IConverter _converter;
        private readonly IApi _api;

        bool programIdle = true;

        private List<CMODEL_TABEL_DC_HH_T> lsAllHh = null;

        /* Split */

        private CMODEL_TABEL_DC_HH_T selectedSplitHhScan = null;

        private List<CMODEL_TABEL_DC_PICKBL_HDR_T> listSplitAllNo = null;
        private List<CMODEL_TABEL_DC_HH_T> listAvailableSplitHh = null;
        private List<CMODEL_TABEL_DC_HH_T> listSelectedSplitHhPick = null;
        private List<CMODEL_GRID_SPLIT> listSplit = null;

        private BindingList<CMODEL_TABEL_DC_PICKBL_HDR_T> bindSplitAllNo = null;
        private BindingList<CMODEL_TABEL_DC_HH_T> bindAvailableSplitHh = null;
        private BindingList<CMODEL_TABEL_DC_HH_T> bindSelectedSplitHhPick = null;
        private BindingList<CMODEL_GRID_SPLIT> bindSplit = null;

        /* Edit Split */

        private List<CMODEL_TABEL_DC_PICKBL_HDR_T> listEditSplitAllNo = null;
        private List<CMODEL_TABEL_DC_HH_T> listAvailableEditSplitHhPick = null;
        private List<CMODEL_TABEL_DC_HH_T> listAvailableEditSplitHhScan = null;
        private List<CMODEL_GRID_EDIT_SPLIT> listEditSplit = null;

        private BindingList<CMODEL_TABEL_DC_PICKBL_HDR_T> bindEditSplitAllNo = null;
        private BindingList<CMODEL_TABEL_DC_HH_T> bindAvailableEditSplitHhPick = null;
        private BindingList<CMODEL_TABEL_DC_HH_T> bindAvailableEditSplitHhScan = null;
        private BindingList<CMODEL_GRID_EDIT_SPLIT> bindEditSplit = null;

        /* Transfer NPB */

        private List<CMODEL_TABEL_DC_PICKBL_HDR_T> listTransferNpbAllNo = null;
        private List<CMODEL_GRID_TRANSFER_RESEND_NPB> listTransferNpb = null;

        private BindingList<CMODEL_TABEL_DC_PICKBL_HDR_T> bindTransferNpbAllNo = null;
        private BindingList<CMODEL_GRID_TRANSFER_RESEND_NPB> bindTransferNpb = null;

        /* Resend NPB */

        private List<CMODEL_TABEL_DC_PICKBL_HDR_T> listResendNpbAllNo = null;
        private List<CMODEL_GRID_TRANSFER_RESEND_NPB> listResendNpb = null;

        private BindingList<CMODEL_TABEL_DC_PICKBL_HDR_T> bindResendNpbAllNo = null;

        /* ** */

        public MainApp(IApp app, ILogger logger, IOracle oracle, IConverter converter, IApi api) {
            _app = app;
            _logger = logger;
            _oracle = oracle;
            _converter = converter;
            _api = api;

            InitializeComponent();
            OnInit();
        }

        private void OnInit() {

            // Change Form View
            Dock = DockStyle.Fill;
            SetUserInfo();
        }

        private void MainApp_Load(object sender, EventArgs e) {
            tabSplit.Enter += new EventHandler(tabSplit_Enter);
            tabEditSplit.Enter += new EventHandler(tabEditSplit_Enter);
            tabProsesNpb.Enter += new EventHandler(tabProsesNpb_Enter);
            tabReSendNpb.Enter += new EventHandler(tabReSendNpb_Enter);
            tabLogs.Enter += new EventHandler(tabLogs_Enter);

            lsAllHh = new List<CMODEL_TABEL_DC_HH_T>();

            /* Split */

            listSplitAllNo = new List<CMODEL_TABEL_DC_PICKBL_HDR_T>();
            listAvailableSplitHh = new List<CMODEL_TABEL_DC_HH_T>();
            listSelectedSplitHhPick = new List<CMODEL_TABEL_DC_HH_T>();
            listSplit = new List<CMODEL_GRID_SPLIT>(); ;

            bindSplitAllNo = new BindingList<CMODEL_TABEL_DC_PICKBL_HDR_T>(listSplitAllNo);
            bindAvailableSplitHh = new BindingList<CMODEL_TABEL_DC_HH_T>(listAvailableSplitHh);
            bindSelectedSplitHhPick = new BindingList<CMODEL_TABEL_DC_HH_T>(listSelectedSplitHhPick);
            bindSplit = new BindingList<CMODEL_GRID_SPLIT>(listSplit);

            /* Edit Split */

            listEditSplitAllNo = new List<CMODEL_TABEL_DC_PICKBL_HDR_T>();
            listAvailableEditSplitHhPick = new List<CMODEL_TABEL_DC_HH_T>();
            listAvailableEditSplitHhScan = new List<CMODEL_TABEL_DC_HH_T>();
            listEditSplit = new List<CMODEL_GRID_EDIT_SPLIT>();

            bindEditSplitAllNo = new BindingList<CMODEL_TABEL_DC_PICKBL_HDR_T>(listEditSplitAllNo);
            bindAvailableEditSplitHhPick = new BindingList<CMODEL_TABEL_DC_HH_T>(listAvailableEditSplitHhPick);
            bindAvailableEditSplitHhScan = new BindingList<CMODEL_TABEL_DC_HH_T>(listAvailableEditSplitHhScan);
            bindEditSplit = new BindingList<CMODEL_GRID_EDIT_SPLIT>(listEditSplit);

            /* Transfer NPB */

            listTransferNpbAllNo = new List<CMODEL_TABEL_DC_PICKBL_HDR_T>();
            listTransferNpb = new List<CMODEL_GRID_TRANSFER_RESEND_NPB>();

            bindTransferNpbAllNo = new BindingList<CMODEL_TABEL_DC_PICKBL_HDR_T>(listTransferNpbAllNo);
            bindTransferNpb = new BindingList<CMODEL_GRID_TRANSFER_RESEND_NPB>(listTransferNpb);

            /* Resend NPB */

            listResendNpbAllNo = new List<CMODEL_TABEL_DC_PICKBL_HDR_T>();
            listResendNpb = new List<CMODEL_GRID_TRANSFER_RESEND_NPB>();

            bindResendNpbAllNo = new BindingList<CMODEL_TABEL_DC_PICKBL_HDR_T>(listResendNpbAllNo);

            /* ** */

            SetIdleBusyStatus(true);
        }

        public void SetEnableDisable(bool isEnabled) {
            chkSemuaKolom.Enabled = isEnabled;
            foreach (TabPage tabPage in tabContent.TabPages) {
                tabPage.Enabled = isEnabled;
            }

            /* Split */

            cmbBxSplitAllNo.Enabled = isEnabled;
            btnSplitLoad.Enabled = isEnabled;
            cmbBxSplitHhPicking.Enabled = isEnabled;
            cmbBxSplitHhScanning.Enabled = isEnabled;
            btnSplitAddHhPicking.Enabled = isEnabled;
            btnSplitHhPicking.Enabled = isEnabled;
            btnSplitSetHhScanning.Enabled = isEnabled;
            btnSplitProses.Enabled = isEnabled;
            dtGrdSplit.Enabled = isEnabled;
            dtGrdSplitHhPicking.Enabled = isEnabled;

            /* Edit Split  */

            cmbBxEditSplitAllNo.Enabled = isEnabled;
            btnEditSplitLoad.Enabled = isEnabled;
            btnEditSplitUpdate.Enabled = btnEditSplitUpdate.Enabled && isEnabled;
            dtGrdEditSplit.Enabled = isEnabled;

            /* Transfer NPB */

            cmbBxProsesNpbAllNo.Enabled = isEnabled;
            btnProsesNpbLoad.Enabled = isEnabled;
            btnProsesNpbBuat.Enabled = btnProsesNpbBuat.Enabled && isEnabled;
            btnUnPickRpb.Enabled = btnUnPickRpb.Enabled && isEnabled;
            dtGrdProsesNpb.Enabled = isEnabled;

            /* Resend NPB */

            cmbBxReSendNpbAllNo.Enabled = isEnabled;
            btnReSendNpbLaporan.Enabled = isEnabled;

            /* ** */
        }

        public void EnableCustomColumnOnly(DataGridView dtGrdVw, List<string> visibleColumn) {
            foreach (DataGridViewColumn dtGrdCol in dtGrdVw.Columns) {
                if (!visibleColumn.Contains(dtGrdCol.Name)) {
                    dtGrdCol.Visible = chkSemuaKolom.Checked;
                }
                if (dtGrdCol.GetType() != typeof(DataGridViewButtonColumn) && dtGrdCol.GetType() != typeof(DataGridViewComboBoxColumn)) {
                    dtGrdCol.ReadOnly = true;
                }
                else {
                    dtGrdCol.ReadOnly = false;
                }
            }
        }

        public void SetIdleBusyStatus(bool isIdle) {
            programIdle = isIdle;
            lblStatus.Text = $"Program {(isIdle ? "Idle" : "Sibuk")} ...";
            prgrssBrStatus.Style = isIdle ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;

            SetEnableDisable(isIdle);
        }

        public async void SetUserInfo() {
            userInfo.Text = $".: {await _oracle.GetKodeDc()} - {await _oracle.GetNamaDc()} :: {_oracle.LoggedInUsername} :.";
        }

        public void DtClearSelection() {
            dtGrdSplit.ClearSelection();
            dtGrdEditSplit.ClearSelection();
            dtGrdProsesNpb.ClearSelection();
            dtGrdLogs.ClearSelection();
        }

        private void dtGrd_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            MessageBox.Show(
                $"Data Tembakan / Editan DB Langsung (?)\r\n1. Pastikan IP HH Terdaftar Di Tabel\r\n2. IP HH Pick Harus Berbeda Dengan IP HH Scan",
                "Bind UI Tampilan IP HH Bermasalah",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        /* Split */

        private async void tabSplit_Enter(object sender, EventArgs e) {
            if (programIdle) {
                SetIdleBusyStatus(false);
                listSplit.Clear();
                bindSplit.ResetBindings();
                listAvailableSplitHh.Clear();
                bindAvailableSplitHh.ResetBindings();
                listSelectedSplitHhPick.Clear();
                bindSelectedSplitHhPick.ResetBindings();
                selectedSplitHhScan = null;
                listSplitAllNo.Clear();
                DataTable dtAllRpb = new DataTable();
                await Task.Run(async () => {
                    dtAllRpb = await _oracle.GetDataTableAsync(
                        $@"
                        SELECT
                            DOC_NO
                        FROM
                            DC_PICKBL_HDR_T
                        WHERE
                            (TGL_SPLIT IS NULL OR TGL_SPLIT = '')
                    "
                    );
                });
                List<CMODEL_TABEL_DC_PICKBL_HDR_T> lsDtAllRpb = _converter.ConvertDataTableToList<CMODEL_TABEL_DC_PICKBL_HDR_T>(dtAllRpb);
                lsDtAllRpb.Sort((x, y) => x.DOC_NO.CompareTo(y.DOC_NO));
                foreach (CMODEL_TABEL_DC_PICKBL_HDR_T rpb in lsDtAllRpb) {
                    listSplitAllNo.Add(rpb);
                }
                cmbBxSplitAllNo.DataSource = bindSplitAllNo;
                cmbBxSplitAllNo.DisplayMember = "DOC_NO";
                cmbBxSplitAllNo.ValueMember = "DOC_NO";
                bindSplitAllNo.ResetBindings();
                SetIdleBusyStatus(true);
            }
        }

        private async Task LoadHandHeld() {
            string ctx = "Pencarian HH ...";
            DataTable dtAllHhPick = new DataTable();

            await Task.Run(async () => {
                dtAllHhPick = await _oracle.GetDataTableAsync($@"SELECT DISTINCT HH FROM DC_HH_T");
            });

            if (dtAllHhPick.Rows.Count <= 0) {
                MessageBox.Show("Tidak Ada Data Hand-Held", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else {
                lsAllHh = _converter.ConvertDataTableToList<CMODEL_TABEL_DC_HH_T>(dtAllHhPick);
                lsAllHh.Sort((x, y) => x.HH.CompareTo(y.HH));
            }
        }

        private void SetSplitHandHeld() {
            listAvailableSplitHh.Clear();
            foreach (CMODEL_TABEL_DC_HH_T hh in lsAllHh) {
                listAvailableSplitHh.Add(hh);
            }
            cmbBxSplitHhPicking.DisplayMember = "HH";
            cmbBxSplitHhPicking.ValueMember = "HH";
            cmbBxSplitHhPicking.DataSource = bindAvailableSplitHh;
            cmbBxSplitHhScanning.DisplayMember = "HH";
            cmbBxSplitHhScanning.ValueMember = "HH";
            cmbBxSplitHhScanning.DataSource = bindAvailableSplitHh;
            dtGrdSplitHhPicking.DataSource = bindSelectedSplitHhPick;
            EnableCustomColumnOnly(dtGrdSplitHhPicking, new List<string> { "HH", "X" });
            if (!dtGrdSplitHhPicking.Columns.Contains("X")) {
                dtGrdSplitHhPicking.Columns.Add(new DataGridViewButtonColumn {
                    Name = "X",
                    HeaderText = " ",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    Text = "X",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat
                });
            }
            foreach (CMODEL_TABEL_DC_HH_T sshp in listSelectedSplitHhPick) {
                listAvailableSplitHh.Remove(listAvailableSplitHh.SingleOrDefault(l => l.HH == sshp.HH));
            }
            if (selectedSplitHhScan != null) {
                listAvailableSplitHh.Remove(listAvailableSplitHh.SingleOrDefault(l => l.HH == selectedSplitHhScan.HH));
            }
            bindAvailableSplitHh.ResetBindings();
            bindSelectedSplitHhPick.ResetBindings();
        }

        private void AddSplitHhPick() {
            if (programIdle) {
                SetIdleBusyStatus(false);
                List<CMODEL_TABEL_DC_HH_T> selected = listAvailableSplitHh.Where(d => d.HH == cmbBxSplitHhPicking.Text).ToList();
                selected.ForEach(d => {
                    listAvailableSplitHh.Remove(d);
                    listSelectedSplitHhPick.Add(d);
                });
                listSelectedSplitHhPick.Sort((x, y) => x.HH.CompareTo(y.HH));
                bindAvailableSplitHh.ResetBindings();
                bindSelectedSplitHhPick.ResetBindings();
                btnSplitProses.Enabled = listSplit.Count > 0;
                SetIdleBusyStatus(true);
            }
        }

        private void DeleteSplitHhPick(string HH) {
            if (programIdle) {
                SetIdleBusyStatus(false);
                List<CMODEL_TABEL_DC_HH_T> selected = listSelectedSplitHhPick.Where(d => d.HH == HH).ToList();
                selected.ForEach(d => {
                    listSelectedSplitHhPick.Remove(d);
                    listAvailableSplitHh.Add(d);
                });
                listAvailableSplitHh.Sort((x, y) => x.HH.CompareTo(y.HH));
                bindAvailableSplitHh.ResetBindings();
                bindSelectedSplitHhPick.ResetBindings();
                SetIdleBusyStatus(true);
            }
        }

        private void AddSplitHhScan() {
            if (programIdle) {
                SetIdleBusyStatus(false);
                CMODEL_TABEL_DC_HH_T z = listAvailableSplitHh.Where(d => d.HH == cmbBxSplitHhPicking.Text).ToList().FirstOrDefault();
                listAvailableSplitHh.Remove(listAvailableSplitHh.SingleOrDefault(l => l.HH == z.HH));
                if (selectedSplitHhScan != null) {
                    listAvailableSplitHh.Add(selectedSplitHhScan);
                    listAvailableSplitHh.Sort((x, y) => x.HH.CompareTo(y.HH));
                }
                selectedSplitHhScan = z;
                foreach (CMODEL_GRID_SPLIT s in listSplit) {
                    s.HH_SCAN = z.HH;
                }
                bindAvailableSplitHh.ResetBindings();
                btnSplitProses.Enabled = listSplit.Count > 0;
                SetIdleBusyStatus(true);
            }
        }

        private async void btnSplitLoad_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            string ctx = "Pencarian Split RPB ...";
            await LoadHandHeld();
            selectedSplitHhScan = null;
            listSelectedSplitHhPick.Clear();
            listSplit.Clear();
            string selectedNoRpb = cmbBxSplitAllNo.Text;
            if (!string.IsNullOrEmpty(selectedNoRpb)) {
                DataTable dtSplit = new DataTable();
                await Task.Run(async () => {
                    await _oracle.ExecQueryAsync(
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
                    dtSplit = await _oracle.GetDataTableAsync(
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
                                a.SEQ_NO = b.SEQ_FK_NO AND
                                b.PLU_ID = c.MBR_PLUID AND
                                b.PLU_ID = d.PLA_FK_PLUID (+) AND
                                (a.TGL_SPLIT IS NULL OR a.TGL_SPLIT = '')
                        ",
                        new List<CDbQueryParamBind> {
                            new CDbQueryParamBind { NAME = "doc_no", VALUE = selectedNoRpb }
                        }
                    );
                });
                if (dtSplit.Rows.Count <= 0) {
                    lblSplitRecHh.Text = "* Saran : 0 HH";
                    MessageBox.Show("Tidak Ada Data Split", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_GRID_SPLIT> lsSplit = _converter.ConvertDataTableToList<CMODEL_GRID_SPLIT>(dtSplit);
                    lsSplit.Sort((x, y) => x.PLU_ID.CompareTo(y.PLU_ID));
                    List<string> line = new List<string>();
                    foreach (CMODEL_GRID_SPLIT d in lsSplit) {
                        line.Add(d.LOKASI.Split('.')[0]);
                        CMODEL_TABEL_DC_HH_T h = lsAllHh.Where(l => l.HH == d.HH_PICK).ToList().FirstOrDefault();
                        if (h != null && !listSelectedSplitHhPick.Contains(h)) {
                            listSelectedSplitHhPick.Add(h);
                        }
                    }
                    int totalLineNumber = line.Distinct().Count();
                    lblSplitRecHh.Text = $"* Saran : {totalLineNumber} HH";
                    dtPckrSplitTglRpb.Value = lsSplit.FirstOrDefault().DOC_DATE;
                    SetSplitHandHeld();
                    foreach (CMODEL_GRID_SPLIT data in lsSplit) {
                        listSplit.Add(data);
                    }
                    dtGrdSplit.DataSource = bindSplit;
                    EnableCustomColumnOnly(dtGrdSplit, new List<string> { "PLU_ID", "SINGKATAN", "LOKASI", "QTY_RPB", "KETER", "HH_SCAN", "IP_HH_PICK" });
                    if (!dtGrdSplit.Columns.Contains("IP_HH_PICK")) {
                        dtGrdSplit.Columns.Add(new DataGridViewComboBoxColumn {
                            Name = "IP_HH_PICK",
                            HeaderText = "HH_PICK",
                            DataPropertyName = "HH_PICK",
                            DisplayMember = "HH",
                            ValueMember = "HH",
                            DataSource = bindSelectedSplitHhPick
                        });
                    }
                }
                btnSplitProses.Enabled = dtSplit.Rows.Count > 0;
            }
            else {
                MessageBox.Show("Harap Isi DOC_NO Rpb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            bindSelectedSplitHhPick.ResetBindings();
            bindSplit.ResetBindings();
            foreach (DataGridViewRow row in dtGrdSplit.Rows) {
                decimal cellidPlano = (decimal) row.Cells[dtGrdSplit.Columns["CELLID_PLANO"].Index].Value;
                decimal plaqtystok = (decimal) row.Cells[dtGrdSplit.Columns["PLA_QTY_STOK"].Index].Value;
                decimal qtyrpb = (decimal) row.Cells[dtGrdSplit.Columns["QTY_RPB"].Index].Value;
                if (cellidPlano <= 0) {
                    row.Cells[dtGrdSplit.Columns["KETER"].Index].Value = "Belum Ada Rak Display / Tablok";
                }
                if (qtyrpb > plaqtystok) {
                    row.Cells[dtGrdSplit.Columns["KETER"].Index].Value = "Stok Plano Tidak Cukup";
                }
                if (cellidPlano <= 0 || qtyrpb > plaqtystok) {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 64, 129);
                }
            }
            DtClearSelection();
            SetIdleBusyStatus(true);
        }

        private void btnSplitAddHhPicking_Click(object sender, EventArgs e) {
            AddSplitHhPick();
        }

        private void btnSplitSetHhScanning_Click(object sender, EventArgs e) {
            AddSplitHhScan();
        }

        private void dtGrdSplitHhPicking_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            string ctx = "Delete HH Picking ...";
            if (e.ColumnIndex == dtGrdSplitHhPicking.Columns["X"].Index) {
                if (bindSelectedSplitHhPick.Count > 1) {
                    bool safeForDelete = true;
                    string HH_PICK = dtGrdSplitHhPicking.Rows[e.RowIndex].Cells[dtGrdSplitHhPicking.Columns["HH"].Index].Value.ToString();
                    foreach (CMODEL_GRID_SPLIT data in listSplit) {
                        if (data.HH_PICK == HH_PICK) {
                            safeForDelete = false;
                            break;
                        }
                    }
                    if (safeForDelete) {
                        DeleteSplitHhPick(HH_PICK);
                    }
                    else {
                        MessageBox.Show("HH Ini Masih Dikaitkan Dengan Data", ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else {
                    MessageBox.Show("Minimal 1 HH Digunakan", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnSplitHhPicking_Click(object sender, EventArgs e) {
            string ctx = "Atur HH Split ...";
            if (bindSelectedSplitHhPick.Count <= 0) {
                MessageBox.Show("Harap Input HH Picking", ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                for (int i = 0; i < listSplit.Count; i++) {
                    listSplit[i].HH_PICK = listSelectedSplitHhPick[i % listSelectedSplitHhPick.Count].HH;
                }
            }
            btnSplitProses.Enabled = listSplit.Count > 0;
            bindSplit.ResetBindings();
        }

        private async void btnSplitProses_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            DtClearSelection();
            string ctx = "Proses Split ...";
            bool safeForUpdate = false;
            foreach (CMODEL_GRID_SPLIT data in listSplit) {
                if (string.IsNullOrEmpty(data.HH_PICK) || string.IsNullOrEmpty(data.HH_SCAN)) {
                    MessageBox.Show("Masih Ada Yang Belum Di Assign HH", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    safeForUpdate = false;
                    break;
                }
                safeForUpdate = true;
            }
            if (safeForUpdate && listSplit.Count > 0) {
                try {
                    await _oracle.MarkBeforeExecQueryCommitAndRollback();
                    bool bolehSplit = true;
                    foreach (CMODEL_GRID_SPLIT data in listSplit) {
                        string stokPlanoRakDisplay = "";
                        if (data.CELLID_PLANO <= 0) {
                            stokPlanoRakDisplay = "Belum Ada Rak Display / Tablok";
                        }
                        if (data.QTY_RPB > data.PLA_QTY_STOK) {
                            stokPlanoRakDisplay = "Stok Plano Tidak Cukup";
                        }
                        bool update = false;
                        await Task.Run(async () => {
                            update = await _oracle.ExecQueryAsync(
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
                                    new CDbQueryParamBind { NAME = "keter", VALUE = stokPlanoRakDisplay },
                                    new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = data.SEQ_NO },
                                    new CDbQueryParamBind { NAME = "plu_id", VALUE = data.PLU_ID }
                                },
                                false
                            );
                        });
                        if (!update) {
                            throw new Exception($"Gagal Mengubah Keterangan Untuk {data.SINGKATAN}");
                        }
                        if (!string.IsNullOrEmpty(stokPlanoRakDisplay)) {
                            bolehSplit = false;
                        }
                    }
                    if (!bolehSplit) {
                        _oracle.MarkSuccessExecQueryAndCommit();
                        MessageBox.Show("Silahkan Cek Kolom Keterangan", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        btnSplitLoad_Click(null, EventArgs.Empty);
                    }
                    else {
                        foreach (CMODEL_GRID_SPLIT data in listSplit) {
                            bool update1 = false;
                            await Task.Run(async () => {
                                update1 = await _oracle.ExecQueryAsync(
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
                                        new CDbQueryParamBind { NAME = "qty_rpb", VALUE = data.QTY_RPB },
                                        new CDbQueryParamBind { NAME = "rak_plano", VALUE = data.LOKASI },
                                        new CDbQueryParamBind { NAME = "cellid_plano", VALUE = data.CELLID_PLANO },
                                        new CDbQueryParamBind { NAME = "ip_picking", VALUE = data.HH_PICK },
                                        new CDbQueryParamBind { NAME = "ip_scanning", VALUE = data.HH_SCAN },
                                        new CDbQueryParamBind { NAME = "keter", VALUE = "" },
                                        new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = data.SEQ_NO },
                                        new CDbQueryParamBind { NAME = "plu_id", VALUE = data.PLU_ID }
                                    },
                                    false
                                );
                            });
                            if (!update1) {
                                throw new Exception($"Gagal Mengatur HH Untuk {data.SINGKATAN}");
                            }
                        }
                        bool update2 = false;
                        await Task.Run(async () => {
                            update2 = await _oracle.ExecQueryAsync(
                                $@"
                                    UPDATE
                                        DC_PICKBL_HDR_T
                                    SET
                                        TGL_SPLIT = SYSDATE
                                    WHERE
                                        SEQ_NO = :seq_no
                                ",
                                new List<CDbQueryParamBind> {
                                    new CDbQueryParamBind { NAME = "seq_no", VALUE = listSplit.FirstOrDefault().SEQ_NO }
                                },
                                false
                            );
                        });
                        if (!update2) {
                            throw new Exception($"Gagal Set Tanggal Split");
                        }
                        _oracle.MarkSuccessExecQueryAndCommit();
                        MessageBox.Show("Selesai Proses", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex) {
                    _oracle.MarkFailExecQueryAndRollback();
                    MessageBox.Show(ex.Message, ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else {
                MessageBox.Show("Tidak Ada Data Yang Di Proses", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            SetIdleBusyStatus(true);
        }

        /* Edit Split */

        private async void tabEditSplit_Enter(object sender, EventArgs e) {
            if (programIdle) {
                SetIdleBusyStatus(false);
                listEditSplit.Clear();
                bindEditSplit.ResetBindings();
                listAvailableEditSplitHhPick.Clear();
                bindAvailableEditSplitHhPick.ResetBindings();
                listAvailableEditSplitHhScan.Clear();
                bindAvailableEditSplitHhScan.ResetBindings();
                listEditSplitAllNo.Clear();
                DataTable dtAllRpb = new DataTable();
                await Task.Run(async () => {
                    dtAllRpb = await _oracle.GetDataTableAsync($@"
                        SELECT
                            DOC_NO
                        FROM
                            DC_PICKBL_HDR_T
                        WHERE
                            (TGL_SPLIT IS NOT NULL OR TGL_SPLIT <> '')
                    ");
                });
                List<CMODEL_TABEL_DC_PICKBL_HDR_T> lsDtAllRpb = _converter.ConvertDataTableToList<CMODEL_TABEL_DC_PICKBL_HDR_T>(dtAllRpb);
                lsDtAllRpb.Sort((x, y) => x.DOC_NO.CompareTo(y.DOC_NO));
                foreach (CMODEL_TABEL_DC_PICKBL_HDR_T rpb in lsDtAllRpb) {
                    listEditSplitAllNo.Add(rpb);
                }
                cmbBxEditSplitAllNo.DataSource = bindEditSplitAllNo;
                cmbBxEditSplitAllNo.DisplayMember = "DOC_NO";
                cmbBxEditSplitAllNo.ValueMember = "DOC_NO";
                bindEditSplitAllNo.ResetBindings();
                SetIdleBusyStatus(true);
            }
        }

        private async void btnEditSplitLoad_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            string ctx = "Pencarian Edit Split RPB ...";
            await LoadHandHeld();
            listAvailableEditSplitHhPick.Clear();
            listAvailableEditSplitHhScan.Clear();
            listEditSplit.Clear();
            string selectedNoRpb = cmbBxEditSplitAllNo.Text;
            if (!string.IsNullOrEmpty(selectedNoRpb)) {
                DataTable dtEditSplit = new DataTable();
                await Task.Run(async () => {
                    dtEditSplit = await _oracle.GetDataTableAsync(
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
                                a.SEQ_NO = b.SEQ_FK_NO AND
                                b.PLU_ID = c.MBR_PLUID AND
                                b.PLU_ID = d.PLA_FK_PLUID (+) AND
                                (a.TGL_SPLIT IS NOT NULL OR a.TGL_SPLIT <> '')
                        ",
                        new List<CDbQueryParamBind> {
                            new CDbQueryParamBind { NAME = "doc_no", VALUE = selectedNoRpb }
                        }
                    );
                });
                if (dtEditSplit.Rows.Count <= 0) {
                    MessageBox.Show("Tidak Ada Data Edit Split", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_GRID_EDIT_SPLIT> lsEditSplit = _converter.ConvertDataTableToList<CMODEL_GRID_EDIT_SPLIT>(dtEditSplit);
                    lsEditSplit.Sort((x, y) => x.PLU_ID.CompareTo(y.PLU_ID));
                    lsEditSplit.Sort((x, y) => x.TIME_SCANNING.CompareTo(y.TIME_SCANNING));
                    lsEditSplit.Sort((x, y) => x.TIME_PICKING.CompareTo(y.TIME_PICKING));
                    foreach (CMODEL_TABEL_DC_HH_T hh in lsAllHh) {
                        listAvailableEditSplitHhPick.Add(hh);
                        listAvailableEditSplitHhScan.Add(hh);
                    }
                    dtPckrEditSplitTglRpb.Value = lsEditSplit.FirstOrDefault().DOC_DATE;
                    foreach (CMODEL_GRID_EDIT_SPLIT data in lsEditSplit) {
                        listEditSplit.Add(data);
                        List<CMODEL_TABEL_DC_HH_T> selectedPick = listAvailableEditSplitHhPick.Where(d => d.HH == data.HH_SCAN).ToList();
                        selectedPick.ForEach(d => {
                            listAvailableEditSplitHhPick.Remove(d);
                        });
                        List<CMODEL_TABEL_DC_HH_T> selectedScan = listAvailableEditSplitHhScan.Where(d => d.HH == data.HH_PICK).ToList();
                        selectedScan.ForEach(d => {
                            listAvailableEditSplitHhScan.Remove(d);
                        });
                    }
                    dtGrdEditSplit.DataSource = bindEditSplit;
                    EnableCustomColumnOnly(dtGrdEditSplit, new List<string> { "PLU_ID", "SINGKATAN", "LOKASI", "QTY_RPB", "TIME_PICKING", "IP_HH_PICK", "TIME_SCANNING", "IP_HH_SCAN" });
                    if (!dtGrdEditSplit.Columns.Contains("IP_HH_PICK")) {
                        dtGrdEditSplit.Columns.Add(new DataGridViewComboBoxColumn {
                            Name = "IP_HH_PICK",
                            HeaderText = "HH_PICK",
                            DataPropertyName = "HH_PICK",
                            DisplayMember = "HH",
                            ValueMember = "HH",
                            DataSource = bindAvailableEditSplitHhPick,
                            DisplayIndex = dtGrdEditSplit.Columns.Count - 2
                        });
                    }
                    if (!dtGrdEditSplit.Columns.Contains("IP_HH_SCAN")) {
                        dtGrdEditSplit.Columns.Add(new DataGridViewComboBoxColumn {
                            Name = "IP_HH_SCAN",
                            HeaderText = "HH_SCAN",
                            DataPropertyName = "HH_SCAN",
                            DisplayMember = "HH",
                            ValueMember = "HH",
                            DataSource = bindAvailableEditSplitHhScan
                        });
                    }
                }
                btnEditSplitUpdate.Enabled = dtEditSplit.Rows.Count > 0;
            }
            else {
                MessageBox.Show("Harap Isi DOC_NO Rpb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            bindAvailableEditSplitHhPick.ResetBindings();
            bindAvailableEditSplitHhScan.ResetBindings();
            bindEditSplit.ResetBindings();
            CheckEnableDisableIpHhPickScanEditSplit();
            DtClearSelection();
            SetIdleBusyStatus(true);
        }

        private void CheckEnableDisableIpHhPickScanEditSplit() {
            bool canChangeIpHhScan = true;
            foreach (DataGridViewRow row in dtGrdEditSplit.Rows) {
                DateTime pickTime = DateTime.Parse(row.Cells[dtGrdEditSplit.Columns["TIME_PICKING"].Index].Value.ToString());
                DateTime scanTime = DateTime.Parse(row.Cells[dtGrdEditSplit.Columns["TIME_SCANNING"].Index].Value.ToString());
                if (pickTime == DateTime.MinValue || scanTime == DateTime.MinValue) {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 0);
                    if (pickTime != DateTime.MinValue) {
                        row.Cells[dtGrdEditSplit.Columns["IP_HH_PICK"].Index].ReadOnly = true;
                    }
                    if (scanTime != DateTime.MinValue) {
                        row.Cells[dtGrdEditSplit.Columns["IP_HH_SCAN"].Index].ReadOnly = true;
                        canChangeIpHhScan = false;
                    }
                }
                else {
                    row.Cells[dtGrdEditSplit.Columns["IP_HH_PICK"].Index].ReadOnly = true;
                    row.Cells[dtGrdEditSplit.Columns["IP_HH_SCAN"].Index].ReadOnly = true;
                    canChangeIpHhScan = false;
                }
            }
            if (!canChangeIpHhScan) {
                foreach (DataGridViewRow row in dtGrdEditSplit.Rows) {
                    row.Cells[dtGrdEditSplit.Columns["IP_HH_SCAN"].Index].ReadOnly = true;
                }
            }
        }

        private void dtGrdEditSplit_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            if (e.Control is ComboBox cmbBx) {
                cmbBx.SelectionChangeCommitted -= cmbBx_SelectionChangeCommitted;
                cmbBx.SelectionChangeCommitted += cmbBx_SelectionChangeCommitted;
            }
        }

        private void cmbBx_SelectionChangeCommitted(object sender, EventArgs e) {
            Point currentcell = dtGrdEditSplit.CurrentCellAddress;
            if (currentcell.X == dtGrdEditSplit.Columns["IP_HH_SCAN"].Index) {
                DataGridViewComboBoxEditingControl cmbBxIpHhScan = sender as DataGridViewComboBoxEditingControl;
                string selectedNewIpHhScan = cmbBxIpHhScan.Text;
                foreach (CMODEL_GRID_EDIT_SPLIT data in listEditSplit) {
                    List<CMODEL_TABEL_DC_HH_T> oldIpHhScan = lsAllHh.Where(d => d.HH == data.HH_SCAN).ToList();
                    oldIpHhScan.ForEach(d => {
                        if (!listAvailableEditSplitHhPick.Contains(d)) {
                            listAvailableEditSplitHhPick.Add(d);
                        }
                    });
                    listAvailableEditSplitHhPick.Sort((x, y) => x.HH.CompareTo(y.HH));
                    data.HH_SCAN = selectedNewIpHhScan;
                    List<CMODEL_TABEL_DC_HH_T> newIpHhScan = lsAllHh.Where(d => d.HH == data.HH_SCAN).ToList();
                    newIpHhScan.ForEach(d => {
                        listAvailableEditSplitHhPick.Remove(d);
                    });
                }
                bindEditSplit.ResetBindings();
                CheckEnableDisableIpHhPickScanEditSplit();
            }
            else if (currentcell.X == dtGrdEditSplit.Columns["IP_HH_PICK"].Index) {
                DataGridViewComboBoxEditingControl cmbBxIpHhPick = sender as DataGridViewComboBoxEditingControl;
                string selectedNewIpHhPick = cmbBxIpHhPick.Text;
                CMODEL_GRID_EDIT_SPLIT editSplit = listEditSplit[currentcell.Y];
                List<CMODEL_TABEL_DC_HH_T> oldIpHhPick= lsAllHh.Where(d => d.HH == editSplit.HH_PICK).ToList();
                oldIpHhPick.ForEach(d => {
                    if (!listAvailableEditSplitHhScan.Contains(d)) {
                        listAvailableEditSplitHhScan.Add(d);
                    }
                });
                listAvailableEditSplitHhScan.Sort((x, y) => x.HH.CompareTo(y.HH));
                editSplit.HH_PICK= selectedNewIpHhPick;
                List<CMODEL_TABEL_DC_HH_T> newIpHhPick= lsAllHh.Where(d => d.HH == editSplit.HH_PICK).ToList();
                newIpHhPick.ForEach(d => {
                    listAvailableEditSplitHhScan.Remove(d);
                });
                bindEditSplit.ResetBindings();
                CheckEnableDisableIpHhPickScanEditSplit();
            }
        }

        private async void btnEditSplitUpdate_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            DtClearSelection();
            string ctx = "Update Edit Split ...";
            bool safeForUpdate = false;
            List<CMODEL_GRID_EDIT_SPLIT> updateAbleSplit = new List<CMODEL_GRID_EDIT_SPLIT>();
            foreach (CMODEL_GRID_EDIT_SPLIT data in listEditSplit) {
                if (string.IsNullOrEmpty(data.HH_PICK) || string.IsNullOrEmpty(data.HH_SCAN)) {
                    MessageBox.Show("Masih Ada Yang Belum Di Assign HH", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    safeForUpdate = false;
                    break;
                }
                if (data.TIME_PICKING == DateTime.MinValue || data.TIME_SCANNING == DateTime.MinValue) {
                    updateAbleSplit.Add(data);
                    safeForUpdate = true;
                }
            }
            if (safeForUpdate && updateAbleSplit.Count > 0) {
                try {
                    await _oracle.MarkBeforeExecQueryCommitAndRollback();
                    foreach (CMODEL_GRID_EDIT_SPLIT data in updateAbleSplit) {
                        bool update1 = false;
                        await Task.Run(async () => {
                            List<CDbQueryParamBind> paramBind = new List<CDbQueryParamBind>();
                            if (data.TIME_PICKING == DateTime.MinValue) {
                                paramBind.Add(new CDbQueryParamBind { NAME = "ip_picking", VALUE = data.HH_PICK });
                            }
                            if (data.TIME_SCANNING == DateTime.MinValue) {
                                paramBind.Add(new CDbQueryParamBind { NAME = "ip_scanning", VALUE = data.HH_SCAN });
                            }
                            paramBind.Add(new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = data.SEQ_NO });
                            paramBind.Add(new CDbQueryParamBind { NAME = "plu_id", VALUE = data.PLU_ID });
                            update1 = await _oracle.ExecQueryAsync(
                                $@"
                                    UPDATE
                                        DC_PICKBL_DTL_T
                                    SET
                                        {(data.TIME_PICKING == DateTime.MinValue ? "IP_PICKING = :ip_picking" : "")}
                                        {(data.TIME_PICKING == DateTime.MinValue && data.TIME_SCANNING == DateTime.MinValue ? "," : "")}
                                        {(data.TIME_SCANNING == DateTime.MinValue ? "IP_SCANNING = :ip_scanning" : "")}
                                    WHERE
                                        SEQ_FK_NO = :seq_fk_no AND PLU_ID = :plu_id AND
                                        {(data.TIME_PICKING == DateTime.MinValue ? "(TIME_PICKING IS NULL OR TIME_PICKING = '')" : "")}
                                        {(data.TIME_PICKING == DateTime.MinValue && data.TIME_SCANNING == DateTime.MinValue ? "AND" : "")}
                                        {(data.TIME_SCANNING == DateTime.MinValue ? "(TIME_SCANNING IS NULL OR TIME_SCANNING = '')" : "")}
                                ",
                                paramBind,
                                false
                            );
                        });
                        if (!update1) {
                            throw new Exception($"Gagal Mengatur HH Untuk {data.SINGKATAN}");
                        }
                    }
                    _oracle.MarkSuccessExecQueryAndCommit();
                    MessageBox.Show("Selesai Update", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) {
                    _oracle.MarkFailExecQueryAndRollback();
                    MessageBox.Show(ex.Message, ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else {
                MessageBox.Show("Tidak Ada Data Yang Di Perbaharui", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            SetIdleBusyStatus(true);
        }

        /* Transfer NPB */

        private async void tabProsesNpb_Enter(object sender, EventArgs e) {
            if (programIdle) {
                SetIdleBusyStatus(false);
                listTransferNpb.Clear();
                bindTransferNpb.ResetBindings();
                listTransferNpbAllNo.Clear();
                DataTable dtAllRpb = new DataTable();
                await Task.Run(async () => {
                    dtAllRpb = await _oracle.GetDataTableAsync(
                        $@"
                        SELECT
                            DOC_NO
                        FROM
                            DC_PICKBL_HDR_T
                        WHERE
                            (TGL_SPLIT IS NOT NULL OR TGL_SPLIT <> '') AND
                            (NPBDC_DATE IS NULL OR NPBDC_DATE = '')
                    "
                    );
                });
                List<CMODEL_TABEL_DC_PICKBL_HDR_T> lsDtAllRpb = _converter.ConvertDataTableToList<CMODEL_TABEL_DC_PICKBL_HDR_T>(dtAllRpb);
                lsDtAllRpb.Sort((x, y) => x.DOC_NO.CompareTo(y.DOC_NO));
                foreach (CMODEL_TABEL_DC_PICKBL_HDR_T rpb in lsDtAllRpb) {
                    listTransferNpbAllNo.Add(rpb);
                }
                cmbBxProsesNpbAllNo.DataSource = bindTransferNpbAllNo;
                cmbBxProsesNpbAllNo.DisplayMember = "DOC_NO";
                cmbBxProsesNpbAllNo.ValueMember = "DOC_NO";
                bindTransferNpbAllNo.ResetBindings();
                SetIdleBusyStatus(true);
            }
        }

        private async void btnProsesNpbLoad_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            string ctx = "Pencarian Transfer NPB ...";
            listTransferNpb.Clear();
            string selectedNoRpb = cmbBxProsesNpbAllNo.Text;
            if (!string.IsNullOrEmpty(selectedNoRpb)) {
                DataTable dtTransferNpb = new DataTable();
                await Task.Run(async () => {
                    dtTransferNpb = await _oracle.GetDataTableAsync(
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
                                CAST(b.QTY_RPB *
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
                });
                if (dtTransferNpb.Rows.Count <= 0) {
                    MessageBox.Show("Tidak Ada Data Transfer NPB", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_GRID_TRANSFER_RESEND_NPB> lsTransferNpb = _converter.ConvertDataTableToList<CMODEL_GRID_TRANSFER_RESEND_NPB>(dtTransferNpb);
                    lsTransferNpb.Sort((x, y) => x.PLU_ID.CompareTo(y.PLU_ID));
                    lsTransferNpb.Sort((x, y) => x.SCAN.CompareTo(y.SCAN));
                    lsTransferNpb.Sort((x, y) => x.PICK.CompareTo(y.PICK));
                    dtPckrProsesNpbTglRpb.Value = lsTransferNpb.FirstOrDefault().DOC_DATE;
                    foreach (CMODEL_GRID_TRANSFER_RESEND_NPB data in lsTransferNpb) {
                        listTransferNpb.Add(data);
                    }
                    dtGrdProsesNpb.DataSource = bindTransferNpb;
                    EnableCustomColumnOnly(dtGrdProsesNpb, new List<string> { "PLU_ID", "SINGKATAN", "LOKASI", "QTY", "PICK", "SCAN", "PRICE", "GROSS", "PPN" });
                    dtGrdProsesNpb.Columns["HPP"].DefaultCellStyle.Format = "c2";
                    dtGrdProsesNpb.Columns["HPP"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");
                    dtGrdProsesNpb.Columns["PRICE"].DefaultCellStyle.Format = "c2";
                    dtGrdProsesNpb.Columns["PRICE"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");
                    dtGrdProsesNpb.Columns["GROSS"].DefaultCellStyle.Format = "c2";
                    dtGrdProsesNpb.Columns["GROSS"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");
                    dtGrdProsesNpb.Columns["PPN"].DefaultCellStyle.Format = "c2";
                    dtGrdProsesNpb.Columns["PPN"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");
                }
                btnProsesNpbBuat.Enabled = dtTransferNpb.Rows.Count > 0;
                btnUnPickRpb.Enabled = dtTransferNpb.Rows.Count > 0;
            }
            else {
                MessageBox.Show("Harap Isi DOC_NO Rpb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            bindTransferNpb.ResetBindings();
            foreach (DataGridViewRow row in dtGrdProsesNpb.Rows) {
                DateTime pickDate = DateTime.Parse(row.Cells[dtGrdProsesNpb.Columns["STOP_PICKING"].Index].Value.ToString());
                DateTime scanDate = DateTime.Parse(row.Cells[dtGrdProsesNpb.Columns["STOP_SCANNING"].Index].Value.ToString());
                decimal pickCount = decimal.Parse(row.Cells[dtGrdProsesNpb.Columns["PICK"].Index].Value.ToString());
                decimal scanCount = decimal.Parse(row.Cells[dtGrdProsesNpb.Columns["SCAN"].Index].Value.ToString());
                if (pickDate <= DateTime.MinValue || scanDate <= DateTime.MinValue) {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 64, 129);
                }
                else if (pickCount <= 0 || scanCount <= 0) {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 0);
                }
                else {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(105, 240, 175);
                }
            }
            DtClearSelection();
            SetIdleBusyStatus(true);
        }

        private async void btnProsesBuatNpb_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            DtClearSelection();
            string ctx = "Proses & Transfer NPB ...";
            bool safeForNpb = true;
            decimal totalPick = 0;
            decimal totalScan = 0;
            foreach (CMODEL_GRID_TRANSFER_RESEND_NPB data in listTransferNpb) {
                totalPick += data.PICK;
                totalScan += data.SCAN;
                if (data.STOP_PICKING <= DateTime.MinValue || data.STOP_SCANNING <= DateTime.MinValue) {
                    MessageBox.Show("Masih Ada Item Yang Belum Di Picking &/ Scanning", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    safeForNpb = false;
                    break;
                }
            }
            if (safeForNpb) {
                if (totalPick <= 0 || totalScan <= 0) {
                    MessageBox.Show($"Tidak Ada Pemenuhan.{Environment.NewLine}Gagal Buat NPB DC.{Environment.NewLine}Silahkan Load Ulang Untuk Refresh.{Environment.NewLine}Gunakan Tombol Un-Pick Untuk Membatalkan RPB.", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else {
                    decimal dcid = 0;
                    await Task.Run(async () => {
                        dcid = await _oracle.ExecScalarAsync(
                            EReturnDataType.INT,
                            "SELECT get_dcid(:dc_kode) FROM DUAL",
                            new List<CDbQueryParamBind> {
                                new CDbQueryParamBind { NAME = "dc_kode", VALUE = listTransferNpb.FirstOrDefault().DC_KODE }
                            }
                        );
                    });
                    if (dcid > 0) {
                        string procName = "DC_ANTARGUDANG_WEB.PROSES_NPBDCBL";
                        CDbExecProcResult runProc = null;
                        await Task.Run(async () => {
                            runProc = await _oracle.ExecProcedureAsync(
                                procName,
                                new List<CDbQueryParamBind> {
                                    new CDbQueryParamBind { NAME = "n_noref", VALUE = listTransferNpb.FirstOrDefault().SEQ_NO },
                                    new CDbQueryParamBind { NAME = "d_tgl_ref", VALUE = listTransferNpb.FirstOrDefault().SEQ_DATE },
                                    new CDbQueryParamBind { NAME = "n_dcid1", VALUE = dcid },
                                    new CDbQueryParamBind { NAME = "n_hdrid", VALUE = (decimal) 0, DIRECTION = ParameterDirection.Output },
                                    new CDbQueryParamBind { NAME = "p_msg", VALUE = "", DIRECTION = ParameterDirection.Output, SIZE = 2000 }
                                }
                            );
                        });
                        string resProc = $"status = {(runProc.STATUS ? "Berhasil" : "Gagal")} Menjalankan Procedure";
                        bool npbOk = runProc.STATUS;
                        if (runProc.PARAMETERS != null) {
                            resProc += Environment.NewLine + Environment.NewLine;
                            for (int i = 0; i < runProc.PARAMETERS.Count; i++) {
                                resProc += $"{runProc.PARAMETERS[i].ParameterName} = {runProc.PARAMETERS[i].Value}";
                                if (i + 1 < runProc.PARAMETERS.Count) resProc += Environment.NewLine;
                                if (runProc.PARAMETERS[i].ParameterName == "p_msg") {
                                    npbOk = npbOk && string.IsNullOrEmpty(runProc.PARAMETERS[i].Value.ToString());
                                }
                            }
                        }
                        MessageBox.Show(resProc, runProc.QUERY, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (npbOk) {
                            decimal npb = 0;
                            await Task.Run(async () => {
                                npb = await _oracle.ExecScalarAsync(
                                    EReturnDataType.DECIMAL,
                                    "SELECT NPBDC_NO FROM DC_PICKBL_HDR_H WHERE SEQ_NO = :seq_no AND DOC_NO = :doc_no",
                                    new List<CDbQueryParamBind> {
                                        new CDbQueryParamBind { NAME = "seq_no", VALUE = listTransferNpb.FirstOrDefault().SEQ_NO },
                                        new CDbQueryParamBind { NAME = "doc_no", VALUE = listTransferNpb.FirstOrDefault().DOC_NO }
                                    }
                                );
                            });
                            cmbBxReSendNpbAllNo.Text = npb.ToString();
                            MessageBox.Show($"No. NPB :: {npb}", "NPB Berhasil Dibuat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tabContent.SelectTab(tabReSendNpb);
                            LoadReSendNpb();
                        }
                    }
                    else {
                        MessageBox.Show("Gagal Mendapatkan DCID", ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            SetIdleBusyStatus(true);
        }

        private async void btnUnPickRpb_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            DtClearSelection();
            string ctx = "Un-Pick RPB ...";
            bool safeForUnPick = true;
            decimal totalPick = 0;
            decimal totalScan = 0;
            foreach (CMODEL_GRID_TRANSFER_RESEND_NPB data in listTransferNpb) {
                totalPick += data.PICK;
                totalScan += data.SCAN;
                if (data.STOP_PICKING <= DateTime.MinValue || data.STOP_SCANNING <= DateTime.MinValue) {
                    MessageBox.Show("Masih Ada Item Yang Belum Di Picking &/ Scanning", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    safeForUnPick = false;
                    break;
                }
            }
            if (safeForUnPick) {
                if (totalPick > 0 || totalScan > 0) {
                    MessageBox.Show($"Gagal Un-Pick RPB.{Environment.NewLine}Dapat Memenuhi Kebutuhan.{Environment.NewLine}Silahkan Load Ulang Untuk Refresh.{Environment.NewLine}Kemudian Lanjut Membuat NPB DC.", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else {
                    string doc_no = listTransferNpb.FirstOrDefault().DOC_NO;
                    decimal seq_no = listTransferNpb.FirstOrDefault().SEQ_NO;
                    DialogResult dialogResult = MessageBox.Show(
                        $"Yakin Akan Un-Pick Batal RPB '{doc_no}' (?)",
                        ctx,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );
                    if (dialogResult == DialogResult.Yes) {
                        try {
                            await _oracle.MarkBeforeExecQueryCommitAndRollback();
                            string no_pesanan = string.Empty;
                            await Task.Run(async () => {
                                no_pesanan = await _oracle.ExecScalarAsync(
                                    EReturnDataType.STRING,
                                    $@"
                                        SELECT
                                            NO_PESANAN
                                        FROM
                                            DC_PICKBL_DTLBL_T
                                        WHERE
                                            SEQ_FK_NO = :seq_fk_no
                                    ",
                                    new List<CDbQueryParamBind> {
                                        new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = seq_no }
                                    },
                                    false
                                );
                            });
                            if (string.IsNullOrEmpty(no_pesanan)) {
                                throw new Exception($"No Pesanan Tidak Ditemukan");
                            }
                            bool update1 = false;
                            await Task.Run(async () => {
                                update1 = await _oracle.ExecQueryAsync(
                                    $@"
                                        UPDATE
                                            DC_PICKBL_T
                                        SET
                                            UNPICK = 'Y'
                                        WHERE
                                            NO_PESANAN = :no_pesanan
                                    ",
                                    new List<CDbQueryParamBind> {
                                        new CDbQueryParamBind { NAME = "no_pesanan", VALUE = no_pesanan }
                                    },
                                    false
                                );
                            });
                            if (!update1) {
                                throw new Exception($"Gagal Set Un-Pick Pesanan '{no_pesanan}'");
                            }
                            bool delete1 = false;
                            await Task.Run(async () => {
                                delete1 = await _oracle.ExecQueryAsync(
                                    $@"
                                        DELETE FROM
                                            DC_PICKBL_T
                                        WHERE
                                            NO_PESANAN = :no_pesanan AND
                                            UNPICK = 'Y'
                                    ",
                                    new List<CDbQueryParamBind> {
                                        new CDbQueryParamBind { NAME = "no_pesanan", VALUE = no_pesanan }
                                    },
                                    false
                                );
                            });
                            if (!delete1) {
                                throw new Exception($"Gagal Menghapus Header Pesanan '{no_pesanan}'");
                            }
                            bool delete2 = false;
                            await Task.Run(async () => {
                                delete2 = await _oracle.ExecQueryAsync(
                                    $@"
                                        DELETE FROM
                                            DC_PICKBL_DTLBL_T
                                        WHERE
                                            SEQ_FK_NO = :seq_fk_no
                                    ",
                                    new List<CDbQueryParamBind> {
                                        new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = seq_no }
                                    },
                                    false
                                );
                            });
                            if (!delete2) {
                                throw new Exception($"Gagal Menghapus Detail Pesanan '{no_pesanan}'");
                            }
                            bool delete3 = false;
                            await Task.Run(async () => {
                                delete3 = await _oracle.ExecQueryAsync(
                                    $@"
                                        DELETE FROM
                                            DC_PICKBL_DTL_T
                                        WHERE
                                            SEQ_FK_NO = :seq_fk_no
                                    ",
                                    new List<CDbQueryParamBind> {
                                        new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = seq_no }
                                    },
                                    false
                                );
                            });
                            if (!delete3) {
                                throw new Exception($"Gagal Menghapus Detail RPB '{seq_no}'");
                            }
                            bool update2 = false;
                            await Task.Run(async () => {
                                update2 = await _oracle.ExecQueryAsync(
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
                                    },
                                    false
                                );
                            });
                            if (!update2) {
                                throw new Exception($"Gagal Set Un-Pick Header RPB '{doc_no}'");
                            }
                            bool delete4 = false;
                            await Task.Run(async () => {
                                delete4 = await _oracle.ExecQueryAsync(
                                    $@"
                                        DELETE FROM
                                            DC_PICKBL_HDR_T
                                        WHERE
                                            DOC_NO = :doc_no AND
                                            UNPICK = 'Y'
                                    ",
                                    new List<CDbQueryParamBind> {
                                        new CDbQueryParamBind { NAME = "doc_no", VALUE = doc_no }
                                    },
                                    false
                                );
                            });
                            if (!delete4) {
                                throw new Exception($"Gagal Menghapus Header RPB '{doc_no}'");
                            }
                            _oracle.MarkSuccessExecQueryAndCommit();
                            MessageBox.Show("Selesai Un-Pick", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex) {
                            _oracle.MarkFailExecQueryAndRollback();
                            MessageBox.Show(ex.Message, ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            SetIdleBusyStatus(true);
        }

        /* Re-Send NPB */

        private async void tabReSendNpb_Enter(object sender, EventArgs e) {
            if (programIdle) {
                SetIdleBusyStatus(false);
                listResendNpb.Clear();
                listResendNpbAllNo.Clear();
                DataTable dtAllNpb = new DataTable();
                await Task.Run(async () => {
                    dtAllNpb = await _oracle.GetDataTableAsync(
                        $@"
                            SELECT NPBDC_NO FROM DC_PICKBL_HDR_H
                            WHERE
                                (NPBDC_DATE IS NOT NULL OR NPBDC_DATE <> '')
                        "
                        );
                });
                List<CMODEL_TABEL_DC_PICKBL_HDR_T> lsDtAllNpb = _converter.ConvertDataTableToList<CMODEL_TABEL_DC_PICKBL_HDR_T>(dtAllNpb);
                foreach (CMODEL_TABEL_DC_PICKBL_HDR_T npb in lsDtAllNpb) {
                    listResendNpbAllNo.Add(npb);
                }
                cmbBxReSendNpbAllNo.DataSource = bindResendNpbAllNo;
                cmbBxReSendNpbAllNo.DisplayMember = "NPBDC_NO";
                cmbBxReSendNpbAllNo.ValueMember = "NPBDC_NO";
                bindResendNpbAllNo.ResetBindings();
                SetIdleBusyStatus(true);
            }
        }

        private async void LoadReSendNpb() {
            SetIdleBusyStatus(false);
            string ctx = "Pencarian Re/Send NPB ...";
            listResendNpb.Clear();
            string selectedNoNpb = cmbBxReSendNpbAllNo.Text;
            if (!string.IsNullOrEmpty(selectedNoNpb)) {
                DataTable dtResendNpb = new DataTable();
                await Task.Run(async () => {
                    dtResendNpb = await _oracle.GetDataTableAsync(
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
                });
                if (dtResendNpb.Rows.Count <= 0) {
                    MessageBox.Show("Tidak Ada Data Re/Send NPB", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_GRID_TRANSFER_RESEND_NPB> lsResendNpb = _converter.ConvertDataTableToList<CMODEL_GRID_TRANSFER_RESEND_NPB>(dtResendNpb);
                    lsResendNpb.Sort((x, y) => x.PLU_ID.CompareTo(y.PLU_ID));
                    lsResendNpb.Sort((x, y) => x.SCAN.CompareTo(y.SCAN));
                    lsResendNpb.Sort((x, y) => x.PICK.CompareTo(y.PICK));
                    txtReSendNpbApiTargetDcKode.Text = lsResendNpb.FirstOrDefault().WHK_KODE;
                    foreach (CMODEL_GRID_TRANSFER_RESEND_NPB data in lsResendNpb) {
                        listResendNpb.Add(data);
                    }
                }
            }
            else {
                MessageBox.Show("Harap Isi NPBDC_NO Npb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            SetIdleBusyStatus(true);
            GetApiNpb();
        }

        private async void GetApiNpb() {
            SetIdleBusyStatus(false);
            string apiDcho = _app.GetConfig("api_dcho_url");
            string apiTargetUrl = _app.GetConfig("api_dev_url");
            #if DEBUG
                txtReSendNpbApiTargetDcKode.Text = "G000";
            #endif
            string apiTargetKodeDc = txtReSendNpbApiTargetDcKode.Text.ToUpper();
            if (!string.IsNullOrEmpty(apiTargetKodeDc) && apiTargetKodeDc != "G000") {
                await Task.Run(async () => {
                    List<CMODEL_JSON_KIRIM_DCHO_DC> kode_dc = new List<CMODEL_JSON_KIRIM_DCHO_DC> {
                        new CMODEL_JSON_KIRIM_DCHO_DC {
                            KodeDC = apiTargetKodeDc
                        }
                    };
                    string jsonBody = _api.ObjectToJson(new CMODEL_JSON_KIRIM_DCHO {
                        TipeData = _app.GetConfig("api_tipe_data"),
                        JenisIP = _app.GetConfig("api_jenis_ip"),
                        KodeDC = kode_dc
                    });
                    var resApi = await _api.PostData(apiDcho, jsonBody);
                    string resApiStr = await resApi.Content.ReadAsStringAsync();
                    CMODEL_JSON_TERIMA_DCHO resApiObj = _api.JsonToObj<CMODEL_JSON_TERIMA_DCHO>(resApiStr);
                    apiTargetUrl = resApiObj.Url.FirstOrDefault().Url;
                });
            }
            txtReSendNpbApiTargetUrl.Text = apiTargetUrl;
            SetIdleBusyStatus(true);
            TransferNpb();
        }

        private async void TransferNpb() {
            SetIdleBusyStatus(false);
            string ctx = "Proses Transfer NPB ...";
            string apiOwner = "TAG_BL";
            string apiDcKode = txtReSendNpbApiTargetDcKode.Text.ToUpper();
            string apiTarget = txtReSendNpbApiTargetUrl.Text;
            if (string.IsNullOrEmpty(apiDcKode) || string.IsNullOrEmpty(apiTarget)) {
                MessageBox.Show("API Tujuan Tidak Lengkap", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                if (listResendNpb.Count > 0) {
                    await Task.Run(async () => {
                        List<CMODEL_JSON_KIRIM_NPB_BL_DETAIL> blDetail = new List<CMODEL_JSON_KIRIM_NPB_BL_DETAIL>();
                        foreach (CMODEL_GRID_TRANSFER_RESEND_NPB data in listResendNpb) {
                            CMODEL_JSON_KIRIM_NPB_BL_DETAIL detail = new CMODEL_JSON_KIRIM_NPB_BL_DETAIL {
                                plu_id = Convert.ToInt32(data.PLU_ID),
                                sj_qty = Convert.ToInt32(data.SJ_QTY),
                                hpp = data.HPP,
                                price = data.PRICE,
                                gross = data.GROSS,
                                ppn_rate = data.PPN_RATE,
                                tglexp = data.TGLEXP
                            };
                            blDetail.Add(detail);
                        }
                        CMODEL_JSON_KIRIM_NPB_BL bl = new CMODEL_JSON_KIRIM_NPB_BL {
                            dc_kode = listResendNpb.FirstOrDefault().DC_KODE,
                            doc_date = listResendNpb.FirstOrDefault().DOC_DATE,
                            dc_npwp = listResendNpb.FirstOrDefault().TBL_NPWP_DC,
                            doc_no = listResendNpb.FirstOrDefault().DOC_NO,
                            npbdc_no = listResendNpb.FirstOrDefault().NPBDC_NO,
                            npbdc_date = listResendNpb.FirstOrDefault().NPBDC_DATE,
                            whk_kode = listResendNpb.FirstOrDefault().WHK_KODE,
                            detail = blDetail
                        };
                        try {
                            string[] urlPaths = apiTarget.Split('/');
                            string jsonBody = _api.ObjectToJson(bl);
                            var resApi = await _api.PostData(apiTarget, jsonBody);
                            string apiStatusText = $"{(int)resApi.StatusCode} {resApi.StatusCode} - {ctx}";
                            string resApiStr = await resApi.Content.ReadAsStringAsync();
                            CMODEL_JSON_TERIMA_NPB_BL resApiObj = _api.JsonToObj<CMODEL_JSON_TERIMA_NPB_BL>(resApiStr);
                            MessageBoxIcon msgBxIco;
                            if (resApi.StatusCode >= System.Net.HttpStatusCode.OK && resApi.StatusCode < System.Net.HttpStatusCode.MultipleChoices) {
                                msgBxIco = MessageBoxIcon.Information;
                            }
                            else {
                                msgBxIco = MessageBoxIcon.Error;
                            }
                            MessageBox.Show(resApiObj.Info, apiStatusText, MessageBoxButtons.OK, msgBxIco);
                            bool hasilInsert = await _oracle.ExecQueryAsync(
                                $@"
                                    INSERT INTO LOG_API_DC (KODEDC, PEMILIKAPI, NAMAMETHOD, DATAKIRIM, DATABALIK, TANGGAL, STATUS)
                                    VALUES (:kode_dc, :pemilik_api, :nama_method, :data_kirim, :data_balik, :tanggal, :status)
                                ",
                                new List<CDbQueryParamBind> {
                                    new CDbQueryParamBind { NAME = "kode_dc", VALUE = apiDcKode },
                                    new CDbQueryParamBind { NAME = "pemilik_api", VALUE = apiOwner },
                                    new CDbQueryParamBind { NAME = "nama_method", VALUE = urlPaths[urlPaths.Length-1] },
                                    new CDbQueryParamBind { NAME = "data_kirim", VALUE = jsonBody },
                                    new CDbQueryParamBind { NAME = "data_balik", VALUE = resApiStr },
                                    new CDbQueryParamBind { NAME = "tanggal", VALUE = DateTime.Now },
                                    new CDbQueryParamBind { NAME = "status", VALUE = $"{(int) resApi.StatusCode} {resApi.StatusCode}" }
                                }
                            );
                            if (!hasilInsert) {
                                throw new Exception("Gagal Insert Ke LOG_API_DC");
                            }
                        }
                        catch (Exception ex) {
                            MessageBox.Show(ex.Message, ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
                else {
                    MessageBox.Show("Tidak Ada Data Yang Di Kirim", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            SetIdleBusyStatus(true);
            ViewLaporan();
        }

        private async void ViewLaporan() {
            SetIdleBusyStatus(false);
            string ctx = "Cetak Laporan ...";
            string selectedNoNpb = cmbBxReSendNpbAllNo.Text;
            if (!string.IsNullOrEmpty(selectedNoNpb)) {
                DataTable dtReport = new DataTable();
                await Task.Run(async () => {
                    dtReport = await _oracle.GetDataTableAsync(
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
                });
                rptViewer.Reset();
                rptViewer.Clear();
                if (dtReport.Rows.Count <= 0) {
                    MessageBox.Show("Tidak Ada Data Untuk Laporan", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_DS_NPBTAGBL> lsReport = _converter.ConvertDataTableToList<CMODEL_DS_NPBTAGBL>(dtReport);
                    List<ReportParameter> paramList = new List<ReportParameter> {
                        new ReportParameter("user", $"{_oracle.LoggedInUsername}"),
                        new ReportParameter("dc_kode_nama_pengirim", $"{lsReport.FirstOrDefault().PENGIRIM}"),
                        new ReportParameter("tgl_npb", $"Tanggal NPB : {lsReport.FirstOrDefault().TGL_NPB}"),
                        new ReportParameter("no_npb", $"No. NPB : {lsReport.FirstOrDefault().NO_NPB}"),
                        new ReportParameter("dc_kode_nama_penerima", $"{lsReport.FirstOrDefault().PENERIMA}"),
                        new ReportParameter("rpb_no_tgl", $"{lsReport.FirstOrDefault().DOC_NO}/{lsReport.FirstOrDefault().NO_REF}")
                    };
                    rptViewer.LocalReport.ReportPath = _app.AppLocation + "/Reports/NpbTagBl.rdlc";
                    rptViewer.LocalReport.DataSources.Add(new ReportDataSource("NpbTagBl", dtReport));
                    rptViewer.LocalReport.SetParameters(paramList);
                    rptViewer.RefreshReport();
                }
            }
            else {
                MessageBox.Show("Harap Isi NPBDC_NO Npb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            SetIdleBusyStatus(true);
        }

        private void btnReSendNpb_Click(object sender, EventArgs e) {
            LoadReSendNpb();
        }

        private void btnReSendNpbLaporan_Click(object sender, EventArgs e) {
            ViewLaporan();
        }

        /* ** */

        private async void tabLogs_Enter(object sender, EventArgs e) {
            if (programIdle) {
                SetIdleBusyStatus(false);
                DataTable dtLogs = new DataTable();
                await Task.Run(async () => {
                    dtLogs = await _oracle.GetDataTableAsync($@"SELECT * FROM ( SELECT * FROM LOG_API_DC WHERE PEMILIKAPI = 'TAG_BL' ORDER BY TANGGAL DESC ) WHERE ROWNUM <= 50");
                });
                List<CMODEL_TABEL_LOG_API_DC> logs = _converter.ConvertDataTableToList<CMODEL_TABEL_LOG_API_DC>(dtLogs);
                foreach (CMODEL_TABEL_LOG_API_DC l in logs) {
                    try {
                        CMODEL_JSON_TERIMA_NPB_BL resApiObj = _api.JsonToObj<CMODEL_JSON_TERIMA_NPB_BL>(l.DATABALIK);
                        l.KETERANGAN = resApiObj.Info;
                    }
                    catch (Exception ex) {
                        _logger.WriteError(ex);
                    }
                }
                dtGrdLogs.DataSource = logs;
                EnableCustomColumnOnly(dtGrdLogs, new List<string> { "KODEDC", "TANGGAL", "STATUS", "KETERANGAN" });
                dtGrdLogs.Columns["TANGGAL"].DisplayIndex = 0;
                DtClearSelection();
                SetIdleBusyStatus(true);
            }
        }

    }

}
