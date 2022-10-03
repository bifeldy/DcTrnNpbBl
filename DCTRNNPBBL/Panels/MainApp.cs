/**
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
        private BindingList<CMODEL_GRID_TRANSFER_RESEND_NPB> bindResendNpb = null;

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
            bindResendNpb = new BindingList<CMODEL_GRID_TRANSFER_RESEND_NPB>(listResendNpb);

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
            btnEditSplitUpdate.Enabled = isEnabled;
            dtGrdEditSplit.Enabled = isEnabled;

            /* Transfer NPB */

            cmbBxProsesNpbAllNo.Enabled = isEnabled;
            btnProsesNpbLoad.Enabled = isEnabled;
            btnProsesNpbBuat.Enabled = isEnabled;
            dtGrdProsesNpb.Enabled = isEnabled;

            /* Resend NPB */

            cmbBxReSendNpbAllNo.Enabled = isEnabled;
            btnReSendNpbLoad.Enabled = isEnabled;
            btnReSendNpbKirim.Enabled = isEnabled;
            dtGrdReSendNpb.Enabled = isEnabled;
            cmbBxReSendNpbApiTargetDcKode.Enabled = isEnabled;

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
                dtAllHhPick = await _oracle.GetDataTableAsync($@"SELECT * FROM DC_HH_T");
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
                    dtSplit = await _oracle.GetDataTableAsync(
                        $@"
                            SELECT
                                a.DC_KODE,
                                a.SEQ_NO,
                                a.DOC_DATE,
                                b.PLU_ID,
                                c.MBR_SINGKATAN AS SINGKATAN,
                                (PLA_LINE || '.' || PLA_RAK || '.' || PLA_SHELF || '.' || PLA_CELL) AS LOKASI,
                                b.QTY_RPB,
                                b.IP_PICKING AS HH_PICK,
                                b.IP_SCANNING AS HH_SCAN
                            FROM
                                DC_PICKBL_HDR_T a,
                                DC_PICKBL_DTL_T b,
                                DC_BARANG_T c,
                                DC_PLANOGRAM_T d
                            WHERE
                                a.DOC_NO = :doc_no AND
                                a.SEQ_NO = b.SEQ_FK_NO AND
                                b.PLU_ID = c.MBR_PLUID AND
                                b.PLU_ID = d.PLA_FK_PLUID AND
                                d.PLA_DISPLAY = 'Y' AND
                                (a.TGL_SPLIT IS NULL OR a.TGL_SPLIT = '')
                        ",
                        new List<CDbQueryParamBind> {
                            new CDbQueryParamBind { NAME = "doc_no", VALUE = selectedNoRpb }
                        }
                    );
                });
                if (dtSplit.Rows.Count <= 0) {
                    lblSplitRecHh.Text = "* Rekomendasi : 0 HH";
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
                    lblSplitRecHh.Text = $"* Rekomendasi : {totalLineNumber} HH";
                    dtPckrSplitTglRpb.Value = lsSplit.FirstOrDefault().DOC_DATE;
                    SetSplitHandHeld();
                    foreach (CMODEL_GRID_SPLIT data in lsSplit) {
                        listSplit.Add(data);
                    }
                    dtGrdSplit.DataSource = bindSplit;
                    EnableCustomColumnOnly(dtGrdSplit, new List<string> { "PLU_ID", "SINGKATAN", "LOKASI", "QTY_RPB", "HH_SCAN", "IP_HH_PICK" });
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
            }
            else {
                MessageBox.Show("Harap Isi DOC_NO Rpb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            bindSelectedSplitHhPick.ResetBindings();
            bindSplit.ResetBindings();
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
            bindSplit.ResetBindings();
        }

        private async void btnSplitProses_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
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
                    foreach (CMODEL_GRID_SPLIT data in listSplit) {
                        bool update1 = false;
                        await Task.Run(async () => {
                            update1 = await _oracle.ExecQueryAsync(
                                $@"
                                    UPDATE
                                        DC_PICKBL_DTL_T
                                    SET
                                        IP_PICKING = :ip_picking,
                                        IP_SCANNING = :ip_scanning
                                    WHERE
                                        SEQ_FK_NO = :seq_fk_no AND
                                        PLU_ID = :plu_id
                                ",
                                new List<CDbQueryParamBind> {
                                    new CDbQueryParamBind { NAME = "ip_picking", VALUE = data.HH_PICK },
                                    new CDbQueryParamBind { NAME = "ip_scanning", VALUE = data.HH_SCAN },
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
                    dtAllRpb = await _oracle.GetDataTableAsync(
                        $@"
                        SELECT
                            DOC_NO
                        FROM
                            DC_PICKBL_HDR_T
                        WHERE
                            (TGL_SPLIT IS NOT NULL OR TGL_SPLIT <> '')
                    "
                    );
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
                                DC_PLANOGRAM_T d
                            WHERE
                                a.DOC_NO = :doc_no AND
                                a.SEQ_NO = b.SEQ_FK_NO AND
                                b.PLU_ID = c.MBR_PLUID AND
                                b.PLU_ID = d.PLA_FK_PLUID AND
                                d.PLA_DISPLAY = 'Y' AND
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
            }
            else {
                MessageBox.Show("Harap Isi DOC_NO Rpb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            bindAvailableEditSplitHhPick.ResetBindings();
            bindAvailableEditSplitHhScan.ResetBindings();
            bindEditSplit.ResetBindings();
            CheckEnableDisableIpHhPickScanEditSplit();
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
                    data.HH_SCAN = selectedNewIpHhScan;
                }
                bindEditSplit.ResetBindings();
                CheckEnableDisableIpHhPickScanEditSplit();
            }
        }

        private async void btnEditSplitUpdate_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
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
                                a.DOC_NO,
                                a.DC_KODE,
                                a.SEQ_NO,
                                a.DOC_DATE,
                                b.PLU_ID,
                                c.MBR_SINGKATAN AS SINGKATAN,
                                (PLA_LINE || '.' || PLA_RAK || '.' || PLA_SHELF || '.' || PLA_CELL) AS LOKASI,
                                b.QTY_RPB AS QTY,
                                b.QTY_PICKING AS PICK,
                                b.QTY_SCANNING AS SCAN,
                                NVL (c.mbr_acost, NVL(c.mbr_lcost, 0)) AS PRICE,
                                b.QTY_RPB *
                                    NVL (c.mbr_acost, NVL(c.mbr_lcost, 0))
                                        AS GROSS,
                                CASE
                                    WHEN
                                        a.WHK_NPWP <> e.TBL_NPWP_DC AND
                                        c.MBR_BKP = 'Y'
                                    THEN
                                        (GETPPNNEW(b.PLU_ID)/100) *
                                            (b.QTY_RPB * NVL (c.mbr_acost, NVL(c.mbr_lcost, 0)))
                                    ELSE
                                        0
                                END AS PPN,
                                b.SJ_QTY,
                                b.HPP,
                                b.TGLEXP,
                                a.NPBDC_NO,
                                a.NPBDC_DATE,
                                a.WHK_KODE,
                                e.TBL_NPWP_DC
                            FROM
                                DC_PICKBL_HDR_T a,
                                DC_PICKBL_DTL_T b,
                                DC_BARANG_DC_V c,
                                DC_PLANOGRAM_T d,
                                DC_TABEL_DC_T e
                            WHERE
                                a.DOC_NO = :doc_no AND
                                a.SEQ_NO = b.SEQ_FK_NO AND
                                b.PLU_ID = c.MBR_FK_PLUID AND
                                b.PLU_ID = d.PLA_FK_PLUID AND
                                d.PLA_DISPLAY = 'Y' AND
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
                    dtGrdProsesNpb.Columns["PRICE"].DefaultCellStyle.Format = "c2";
                    dtGrdProsesNpb.Columns["PRICE"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");
                    dtGrdProsesNpb.Columns["GROSS"].DefaultCellStyle.Format = "c2";
                    dtGrdProsesNpb.Columns["GROSS"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");
                    dtGrdProsesNpb.Columns["PPN"].DefaultCellStyle.Format = "c2";
                    dtGrdProsesNpb.Columns["PPN"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");
                }
            }
            else {
                MessageBox.Show("Harap Isi DOC_NO Rpb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            bindTransferNpb.ResetBindings();
            foreach (DataGridViewRow row in dtGrdProsesNpb.Rows) {
                decimal pickCount = decimal.Parse(row.Cells[dtGrdProsesNpb.Columns["PICK"].Index].Value.ToString());
                decimal scanCount = decimal.Parse(row.Cells[dtGrdProsesNpb.Columns["SCAN"].Index].Value.ToString());
                if (pickCount > 0 && scanCount > 0) {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(105, 240, 175);
                }
                else {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 64, 129);
                }
            }
            SetIdleBusyStatus(true);
        }

        private async void btnProsesBuatNpb_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            string ctx = "Proses Transfer NPB ...";
            bool safeForNpb = false;
            foreach (CMODEL_GRID_TRANSFER_RESEND_NPB data in listTransferNpb) {
                if (data.PICK > 0 && data.SCAN > 0) {
                    safeForNpb = true;
                }
                else {
                    MessageBox.Show("Masih Ada Yang Belum Selesai Picking &/ Scanning", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    safeForNpb = false;
                    break;
                }
            }
            if (safeForNpb) {
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
                                    new CDbQueryParamBind { NAME = "d_tgl_ref", VALUE = dtPckrProsesNpbTglRpb.Value },
                                    new CDbQueryParamBind { NAME = "n_dcid1", VALUE = dcid },
                                    new CDbQueryParamBind { NAME = "n_hdrid", VALUE = (decimal) 0, DIRECTION = ParameterDirection.Output },
                                    new CDbQueryParamBind { NAME = "p_msg", VALUE = "", DIRECTION = ParameterDirection.Output, SIZE = 2000 }
                            }
                        );
                    });
                    string resProc = $"status = {(runProc.STATUS ? "Berhasil" : "Gagal")}";
                    if (runProc.PARAMETERS != null) {
                        resProc += Environment.NewLine + Environment.NewLine;
                        for (int i = 0; i < runProc.PARAMETERS.Count; i++) {
                            resProc += $"{runProc.PARAMETERS[i].ParameterName} = {runProc.PARAMETERS[i].Value}";
                            if (i + 1 < runProc.PARAMETERS.Count) resProc += Environment.NewLine;
                        }
                    }
                    MessageBox.Show(resProc, runProc.QUERY, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (runProc.STATUS) {
                        decimal npb = 0;
                        await Task.Run(async () => {
                            npb = await _oracle.ExecScalarAsync(
                                EReturnDataType.DECIMAL,
                                "SELECT NPBDC_NO FROM DC_PICKBL_HDR_T WHERE SEQ_NO = :seq_no AND DOC_NO = :doc_no",
                                new List<CDbQueryParamBind> {
                                new CDbQueryParamBind { NAME = "seq_no", VALUE = listTransferNpb.FirstOrDefault().SEQ_NO },
                                new CDbQueryParamBind { NAME = "doc_no", VALUE = listTransferNpb.FirstOrDefault().DOC_NO }
                                }
                            );
                        });
                        string msg = "No. NPB :: " + npb + Environment.NewLine;
                        msg += "Silahkan Kirim NPB Menggunakan Menu 'ReSend NPB'";
                        MessageBox.Show(msg, "NPB Berhasil Dibuat Tapi Belum Terkirim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else {
                    MessageBox.Show("Gagal Mendapatkan DCID", ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else {
                MessageBox.Show("Tidak Ada Data Yang Di Proses", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            SetIdleBusyStatus(true);
        }

        /* Re-Send NPB */

        private async void tabReSendNpb_Enter(object sender, EventArgs e) {
            if (programIdle) {
                SetIdleBusyStatus(false);
                listResendNpb.Clear();
                bindResendNpb.ResetBindings();
                listResendNpbAllNo.Clear();
                DataTable dtAllNpb = new DataTable();
                await Task.Run(async () => {
                    dtAllNpb = await _oracle.GetDataTableAsync(
                        $@"
                        SELECT NPBDC_NO FROM DC_PICKBL_HDR_T
                        WHERE
                            (TGL_SPLIT IS NOT NULL OR TGL_SPLIT <> '') AND
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

        private async void btnReSendNpbLoad_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            string ctx = "Pencarian Resend NPB ...";
            listResendNpb.Clear();
            string selectedNoNpb = cmbBxReSendNpbAllNo.Text;
            if (!string.IsNullOrEmpty(selectedNoNpb)) {
                DataTable dtResendNpb = new DataTable();
                await Task.Run(async () => {
                    dtResendNpb = await _oracle.GetDataTableAsync(
                        $@"
                            SELECT
                                a.DOC_NO,
                                a.DC_KODE,
                                a.SEQ_NO,
                                a.DOC_DATE,
                                b.PLU_ID,
                                c.MBR_SINGKATAN AS SINGKATAN,
                                (PLA_LINE || '.' || PLA_RAK || '.' || PLA_SHELF || '.' || PLA_CELL) AS LOKASI,
                                b.QTY_RPB AS QTY,
                                b.QTY_PICKING AS PICK,
                                b.QTY_SCANNING AS SCAN,
                                NVL (c.mbr_acost, NVL(c.mbr_lcost, 0)) AS PRICE,
                                b.QTY_RPB *
                                    NVL (c.mbr_acost, NVL(c.mbr_lcost, 0))
                                        AS GROSS,
                                CASE
                                    WHEN
                                        a.WHK_NPWP <> e.TBL_NPWP_DC AND
                                        c.MBR_BKP = 'Y'
                                    THEN
                                        (GETPPNNEW(b.PLU_ID)/100) *
                                            (b.QTY_RPB * NVL (c.mbr_acost, NVL(c.mbr_lcost, 0)))
                                    ELSE
                                        0
                                END AS PPN,
                                b.SJ_QTY,
                                b.HPP,
                                b.TGLEXP,
                                a.NPBDC_NO,
                                a.NPBDC_DATE,
                                a.WHK_KODE,
                                e.TBL_NPWP_DC
                            FROM
                                DC_PICKBL_HDR_T a,
                                DC_PICKBL_DTL_T b,
                                DC_BARANG_DC_V c,
                                DC_PLANOGRAM_T d,
                                DC_TABEL_DC_T e
                            WHERE
                                a.NPBDC_NO = :npbdc_no AND
                                a.SEQ_NO = b.SEQ_FK_NO AND
                                b.PLU_ID = c.MBR_FK_PLUID AND
                                b.PLU_ID = d.PLA_FK_PLUID AND
                                d.PLA_DISPLAY = 'Y' AND
                                (a.TGL_SPLIT IS NOT NULL OR a.TGL_SPLIT <> '') AND
                                (a.NPBDC_DATE IS NOT NULL OR a.NPBDC_DATE <> '')
                        ",
                        new List<CDbQueryParamBind> {
                            new CDbQueryParamBind { NAME = "npbdc_no", VALUE = selectedNoNpb }
                        }
                    );
                });
                if (dtResendNpb.Rows.Count <= 0) {
                    MessageBox.Show("Tidak Ada Data Resend NPB", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_GRID_TRANSFER_RESEND_NPB> lsResendNpb = _converter.ConvertDataTableToList<CMODEL_GRID_TRANSFER_RESEND_NPB>(dtResendNpb);
                    lsResendNpb.Sort((x, y) => x.PLU_ID.CompareTo(y.PLU_ID));
                    lsResendNpb.Sort((x, y) => x.SCAN.CompareTo(y.SCAN));
                    lsResendNpb.Sort((x, y) => x.PICK.CompareTo(y.PICK));
                    dtPckrReSendNpbTglRpb.Value = lsResendNpb.FirstOrDefault().DOC_DATE;
                    foreach (CMODEL_GRID_TRANSFER_RESEND_NPB data in lsResendNpb) {
                        listResendNpb.Add(data);
                    }
                    dtGrdReSendNpb.DataSource = bindResendNpb;
                    EnableCustomColumnOnly(dtGrdReSendNpb, new List<string> { "PLU_ID", "SINGKATAN", "LOKASI", "QTY", "PICK", "SCAN", "PRICE", "GROSS", "PPN" });
                    dtGrdReSendNpb.Columns["PRICE"].DefaultCellStyle.Format = "c2";
                    dtGrdReSendNpb.Columns["PRICE"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");
                    dtGrdReSendNpb.Columns["GROSS"].DefaultCellStyle.Format = "c2";
                    dtGrdReSendNpb.Columns["GROSS"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");
                    dtGrdReSendNpb.Columns["PPN"].DefaultCellStyle.Format = "c2";
                    dtGrdReSendNpb.Columns["PPN"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");
                }
            }
            else {
                MessageBox.Show("Harap Isi DOC_NO Rpb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            bindResendNpb.ResetBindings();
            SetIdleBusyStatus(true);
        }

        private async void btnReSendGetApi_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            string apiDcho = _app.GetConfig("api_dcho");
            string apiTargetUrl = _app.GetConfig("api_dev");
            string apiTargetKodeDc = cmbBxReSendNpbApiTargetDcKode.Text.ToUpper();
            if (!string.IsNullOrEmpty(apiTargetKodeDc) && apiTargetKodeDc != "G000" && !apiTargetKodeDc.Contains("DEV")) {
                await Task.Run(async () => {
                    string jsonBody = _api.ObjectToJson(new CMODEL_JSON_KIRIM_DCHO {
                        TipeData = "TAG_BL",
                        JenisIP = "IIS",
                        KodeDC = apiTargetKodeDc
                    });
                    var resApi = await _api.PostData(apiDcho, jsonBody);
                    string resApiStr = await resApi.Content.ReadAsStringAsync();
                    CMODEL_JSON_TERIMA_DCHO resApiObj = _api.JsonToObj<CMODEL_JSON_TERIMA_DCHO>(resApiStr);
                    apiTargetUrl = resApiObj.Url;
                });
            }
            txtReSendNpbApiTargetUrl.Text = apiTargetUrl;
            SetIdleBusyStatus(true);
        }

        private async void btnReSendNpbKirim_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            string ctx = "Proses Transfer NPB ...";
            string apiOwner = "TAG_BL-SHANTI";
            string apiDcKode = cmbBxReSendNpbApiTargetDcKode.Text.ToUpper();
            string apiTarget = txtReSendNpbApiTargetUrl.Text;
            if (string.IsNullOrEmpty(apiDcKode) || string.IsNullOrEmpty(apiTarget)) {
                MessageBox.Show("API Tujuan Tidak Lengkap", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                if (listResendNpb.Count > 0) {
                    await Task.Run(async () => {
                        List<CMODEL_JSON_KIRIM_NPB_BL_DETAIL> blDetail = new List<CMODEL_JSON_KIRIM_NPB_BL_DETAIL>();
                        foreach (dynamic data in listResendNpb) {
                            CMODEL_JSON_KIRIM_NPB_BL_DETAIL detail = new CMODEL_JSON_KIRIM_NPB_BL_DETAIL {
                                plu_id = data.PLU_ID,
                                sj_qty = data.SJ_QTY,
                                hpp = data.HPP,
                                price = data.PRICE,
                                gross = data.GROSS,
                                ppn_rate = data.PPN,
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
                            bool hasilInsert = await _oracle.ExecQueryAsync($@"
                                INSERT INTO LOG_API_DC (KODEDC, PEMILIKAPI, NAMAMETHOD, DATAKIRIM, DATABALIK, TANGGAL, STATUS)
                                VALUES (:kode_dc, :pemilik_api, :nama_method, :data_kirim, :data_balik, :tanggal, :status)
                            ", new List<CDbQueryParamBind> {
                                new CDbQueryParamBind { NAME = "kode_dc", VALUE = apiDcKode },
                                new CDbQueryParamBind { NAME = "pemilik_api", VALUE = apiOwner },
                                new CDbQueryParamBind { NAME = "nama_method", VALUE = urlPaths[urlPaths.Length-1] },
                                new CDbQueryParamBind { NAME = "data_kirim", VALUE = jsonBody },
                                new CDbQueryParamBind { NAME = "data_balik", VALUE = resApiStr },
                                new CDbQueryParamBind { NAME = "tanggal", VALUE = DateTime.Now },
                                new CDbQueryParamBind { NAME = "status", VALUE = $"{(int) resApi.StatusCode} {resApi.StatusCode}" }
                            });
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
        }

        /* ** */

        private async void tabLogs_Enter(object sender, EventArgs e) {
            if (programIdle) {
                SetIdleBusyStatus(false);
                DataTable dtLogs = new DataTable();
                await Task.Run(async () => {
                    dtLogs = await _oracle.GetDataTableAsync($@"SELECT * FROM LOG_API_DC ORDER BY TANGGAL DESC");
                });
                dtGrdLogs.DataSource = _converter.ConvertDataTableToList<CMODEL_TABEL_LOG_API_DC>(dtLogs);
                dtGrdLogs.Columns["TANGGAL"].DisplayIndex = 0;
                dtGrdLogs.Columns["STATUS"].DisplayIndex = 1;
                SetIdleBusyStatus(true);
            }
        }

    }

}
