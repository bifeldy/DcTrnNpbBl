/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Halaman Awal
 *              :: Harap Didaftarkan Ke DI Container
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

using bifeldy_sd3_lib_452.Models;
using bifeldy_sd3_lib_452.Utilities;

using DcTrnNpbBl.Forms;
using DcTrnNpbBl.Handlers;
using DcTrnNpbBl.Utilities;
using DcTrnNpbBl.Models;

namespace DcTrnNpbBl.Panels {

    public sealed partial class CMainPanel : UserControl {

        private readonly IApp _app;
        private readonly ILogger _logger;
        private readonly IDb _db;
        private readonly IConfig _config;
        private readonly IWinReg _winreg;
        private readonly IConverter _converter;
        private readonly IApi _api;

        private CMainForm mainForm;

        private bool isInitialized = false;

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

        public CMainPanel(IApp app, ILogger logger, IDb db, IConfig config, IWinReg winreg, IConverter converter, IApi api) {
            _app = app;
            _logger = logger;
            _db = db;
            _config = config;
            _winreg = winreg;
            _converter = converter;
            _api = api;

            InitializeComponent();
            OnInit();
        }

        public Label LabelStatus => lblStatus;

        public ProgressBar ProgressBarStatus => prgrssBrStatus;

        private void OnInit() {
            Dock = DockStyle.Fill;
        }

        private void ImgDomar_Click(object sender, EventArgs e) {
            mainForm.Width = 800;
            mainForm.Height = 600;
        }

        private async void CMainPanel_Load(object sender, EventArgs e) {
            if (!isInitialized) {

                mainForm = (CMainForm) Parent.Parent;
                mainForm.FormBorderStyle = FormBorderStyle.Sizable;
                mainForm.MaximizeBox = true;
                mainForm.MinimizeBox = true;

                appInfo.Text = _app.AppName;
                string dcKode = null;
                string namaDc = null;
                await Task.Run(async () => {
                    dcKode = await _db.GetKodeDc();
                    namaDc = await _db.GetNamaDc();
                });
                userInfo.Text = $".: {dcKode} - {namaDc} :: {_db.LoggedInUsername} :.";

                bool windowsStartup = _config.Get<bool>("WindowsStartup", bool.Parse(_app.GetConfig("windows_startup")));
                chkWindowsStartup.Checked = windowsStartup;

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

                SetIdleBusyStatus(true);

                isInitialized = true;
            }

            SetIdleBusyStatus(_app.IsIdle);
        }

        public void SetIdleBusyStatus(bool isIdle) {
            _app.IsIdle = isIdle;
            LabelStatus.Text = $"Program {(isIdle ? "Idle" : "Sibuk")} ...";
            ProgressBarStatus.Style = isIdle ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
            EnableDisableControl(Controls, isIdle);
        }

        private void EnableDisableControl(ControlCollection controls, bool isIdle) {
            foreach (Control control in controls) {
                if (
                    control is Button ||
                    control is CheckBox ||
                    control is DataGridView ||
                    control is ComboBox
                ) {
                    control.Enabled = isIdle;
                }
                else {
                    EnableDisableControl(control.Controls, isIdle);
                }
            }
        }

        private void ChkWindowsStartup_CheckedChanged(object sender, EventArgs e) {
            CheckBox cb = (CheckBox) sender;
            _config.Set("WindowsStartup", cb.Checked);
            _winreg.SetWindowsStartup(cb.Checked);
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

        public void DtClearSelection() {
            dtGrdSplit.ClearSelection();
            dtGrdEditSplit.ClearSelection();
            dtGrdProsesNpb.ClearSelection();
            dtGrdLogs.ClearSelection();
        }

        private void dtGrd_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            MessageBox.Show(
                $"Data Tembakan / Editan DB Langsung (?)\r\n\r\nPastikan IP HH Pada DC_PICKBL_DTL_T Terdaftar Di Tabel DC_HH_T\r\n\r\nIP HH Pick Harus Berbeda Dengan IP HH Scan",
                "Bind UI Tampilan IP HH Bermasalah",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        /* Split */

        private async void tabSplit_Enter(object sender, EventArgs e) {
            if (_app.IsIdle) {
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
                    dtAllRpb = await _db.LoadDocNoSplit();
                });
                List<CMODEL_TABEL_DC_PICKBL_HDR_T> lsDtAllRpb = _converter.DataTableToList<CMODEL_TABEL_DC_PICKBL_HDR_T>(dtAllRpb);
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
                dtAllHhPick = await _db.OraPg_GetDataTable("SELECT DISTINCT HH FROM DC_HH_T");
            });

            if (dtAllHhPick.Rows.Count <= 0) {
                MessageBox.Show("Tidak Ada Data Hand-Held", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else {
                lsAllHh = _converter.DataTableToList<CMODEL_TABEL_DC_HH_T>(dtAllHhPick);
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
            if (_app.IsIdle) {
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
            if (_app.IsIdle) {
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
            if (_app.IsIdle) {
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
                    dtSplit = await _db.LoadSplitByDocNo(selectedNoRpb);
                });
                if (dtSplit.Rows.Count <= 0) {
                    lblSplitRecHh.Text = "* Saran : 0 HH";
                    MessageBox.Show("Tidak Ada Data Split", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_GRID_SPLIT> lsSplit = _converter.DataTableToList<CMODEL_GRID_SPLIT>(dtSplit);
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
                btnTidakPick.Enabled = dtSplit.Rows.Count > 0;
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
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 64, 129);
                }
                else if (qtyrpb > plaqtystok) {
                    row.Cells[dtGrdSplit.Columns["KETER"].Index].Value = $"Stok Plano Tidak Cukup, Tersisa {plaqtystok}";
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 0);
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

        private async void btnTidakPick_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            DtClearSelection();
            string ctx = "Tidak Pick ...";
            if (listSplit.Count > 0) {
                decimal totalStok = 0;
                try {
                    await _db.MarkBeforeCommitRollback();
                    foreach (CMODEL_GRID_SPLIT data in listSplit) {
                        string stokPlanoRakDisplay = "";
                        if (data.CELLID_PLANO <= 0) {
                            stokPlanoRakDisplay = "Belum Ada Rak Display / Tablok";
                        }
                        else if (data.QTY_RPB > data.PLA_QTY_STOK) {
                            stokPlanoRakDisplay = $"Stok Plano Tidak Cukup, Tersisa {data.PLA_QTY_STOK}";
                        }
                        totalStok += (data.QTY_RPB > data.PLA_QTY_STOK) ? data.PLA_QTY_STOK : data.QTY_RPB;
                        bool update = false;
                        await Task.Run(async () => {
                            update = await _db.UpdateKeterItem(stokPlanoRakDisplay, data.SEQ_NO, data.PLU_ID);
                        });
                        if (!update) {
                            throw new Exception($"Gagal Mengubah Keterangan Untuk {data.SINGKATAN}");
                        }
                    }
                    _db.MarkSuccessCommitAndClose();
                }
                catch (Exception ex) {
                    _db.MarkFailedRollbackAndClose();
                    MessageBox.Show(ex.Message, ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (totalStok > 0) {
                    MessageBox.Show(
                        "Tidak Bisa Menolak Permintaan" + Environment.NewLine + "Ada Stok Yang Mencukupi",
                        ctx,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation
                    );
                }
                else {
                    string doc_no = listSplit.FirstOrDefault().DOC_NO;
                    decimal seq_no = listSplit.FirstOrDefault().SEQ_NO;
                    DialogResult dialogResult = MessageBox.Show(
                        $"Yakin Akan Tidak Pick RPB '{doc_no}' (?)",
                        ctx,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );
                    if (dialogResult == DialogResult.Yes) {
                        try {
                            await Task.Run(async () => {
                                await _db.BatalPick(doc_no, seq_no);
                            });
                            MessageBox.Show("Selesai Tidak Pick", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex) {
                            MessageBox.Show(ex.Message, ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else {
                MessageBox.Show("Tidak Ada Data Yang Di Proses", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            SetIdleBusyStatus(true);
        }

        private async void btnSplitProses_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            DtClearSelection();
            string ctx = "Proses Split ...";
            bool safeForUpdate = false;
            bool bolehSplit = true;
            decimal totalStok = 0;
            foreach (CMODEL_GRID_SPLIT data in listSplit) {
                totalStok += (data.QTY_RPB > data.PLA_QTY_STOK) ? data.PLA_QTY_STOK : data.QTY_RPB;
                if (string.IsNullOrEmpty(data.HH_PICK) || string.IsNullOrEmpty(data.HH_SCAN)) {
                    MessageBox.Show("Masih Ada Yang Belum Di Assign HH", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    safeForUpdate = false;
                    break;
                }
                safeForUpdate = true;
            }
            if (safeForUpdate && totalStok <= 0) {
                MessageBox.Show(
                    "Semua Item Tidak Memiliki Stok Tersedia" + Environment.NewLine + "Silahkan Gunakan Tombol Tidak Pick Untuk Membatalkan",
                    ctx,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
                safeForUpdate = false;
            }
            if (safeForUpdate && listSplit.Count > 0) {
                try {
                    await _db.MarkBeforeCommitRollback();
                    foreach (CMODEL_GRID_SPLIT data in listSplit) {
                        string stokPlanoRakDisplay = "";
                        if (data.CELLID_PLANO <= 0) {
                            bolehSplit = false;
                            stokPlanoRakDisplay = "Belum Ada Rak Display / Tablok";
                        }
                        else if (data.QTY_RPB > data.PLA_QTY_STOK) {
                            MessageBox.Show(
                                data.SINGKATAN + Environment.NewLine + $"Menggunakan Stok Yang Tersisa = {data.PLA_QTY_STOK}",
                                "Stok Plano Tidak Cukup",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation
                            );
                        }
                        bool update = false;
                        await Task.Run(async () => {
                            update = await _db.UpdateKeterItem(stokPlanoRakDisplay, data.SEQ_NO, data.PLU_ID);
                        });
                        if (!update) {
                            throw new Exception($"Gagal Mengubah Keterangan Untuk {data.SINGKATAN}");
                        }
                    }
                    if (!bolehSplit) {
                        _db.MarkSuccessCommitAndClose();
                        MessageBox.Show("Silahkan Cek Kolom Keterangan", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else {
                        foreach (CMODEL_GRID_SPLIT data in listSplit) {
                            bool update1 = false;
                            await Task.Run(async () => {
                                update1 = await _db.ProsesSplit1(
                                    (data.QTY_RPB > data.PLA_QTY_STOK) ? data.PLA_QTY_STOK : data.QTY_RPB,
                                    data.LOKASI,
                                    data.CELLID_PLANO,
                                    data.HH_PICK,
                                    data.HH_SCAN,
                                    "",
                                    data.SEQ_NO,
                                    data.PLU_ID
                                );
                            });
                            if (!update1) {
                                throw new Exception($"Gagal Mengatur HH Untuk {data.SINGKATAN}");
                            }
                        }
                        bool update2 = false;
                        await Task.Run(async () => {
                            update2 = await _db.ProsesSplit2(listSplit.FirstOrDefault().SEQ_NO);
                        });
                        if (!update2) {
                            throw new Exception($"Gagal Set Tanggal Split");
                        }
                        _db.MarkSuccessCommitAndClose();
                        MessageBox.Show("Selesai Proses", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex) {
                    _db.MarkFailedRollbackAndClose();
                    MessageBox.Show(ex.Message, ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else {
                MessageBox.Show("Tidak Ada Data Yang Di Proses", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            SetIdleBusyStatus(true);
            if (!bolehSplit || totalStok <= 0) {
                btnSplitLoad_Click(null, EventArgs.Empty);
            }
        }

        /* Edit Split */

        private async void tabEditSplit_Enter(object sender, EventArgs e) {
            if (_app.IsIdle) {
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
                    dtAllRpb = await _db.LoadDocNoEditSplit();
                });
                List<CMODEL_TABEL_DC_PICKBL_HDR_T> lsDtAllRpb = _converter.DataTableToList<CMODEL_TABEL_DC_PICKBL_HDR_T>(dtAllRpb);
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
                    dtEditSplit = await _db.LoadEditSplitByDocNo(selectedNoRpb);
                });
                if (dtEditSplit.Rows.Count <= 0) {
                    MessageBox.Show("Tidak Ada Data Edit Split", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_GRID_EDIT_SPLIT> lsEditSplit = _converter.DataTableToList<CMODEL_GRID_EDIT_SPLIT>(dtEditSplit);
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
                List<CMODEL_TABEL_DC_HH_T> oldIpHhPick = lsAllHh.Where(d => d.HH == editSplit.HH_PICK).ToList();
                oldIpHhPick.ForEach(d => {
                    if (!listAvailableEditSplitHhScan.Contains(d)) {
                        listAvailableEditSplitHhScan.Add(d);
                    }
                });
                listAvailableEditSplitHhScan.Sort((x, y) => x.HH.CompareTo(y.HH));
                editSplit.HH_PICK = selectedNewIpHhPick;
                List<CMODEL_TABEL_DC_HH_T> newIpHhPick = lsAllHh.Where(d => d.HH == editSplit.HH_PICK).ToList();
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
                    await _db.MarkBeforeCommitRollback();
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
                            update1 = await _db.OraPg_ExecQuery(
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
                                paramBind
                            );
                        });
                        if (!update1) {
                            throw new Exception($"Gagal Mengatur HH Untuk {data.SINGKATAN}");
                        }
                    }
                    _db.MarkSuccessCommitAndClose();
                    MessageBox.Show("Selesai Update", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) {
                    _db.MarkFailedRollbackAndClose();
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
            if (_app.IsIdle) {
                SetIdleBusyStatus(false);
                listTransferNpb.Clear();
                bindTransferNpb.ResetBindings();
                listTransferNpbAllNo.Clear();
                DataTable dtAllRpb = new DataTable();
                await Task.Run(async () => {
                    dtAllRpb = await _db.LoadDocNoProsesNpb();
                });
                List<CMODEL_TABEL_DC_PICKBL_HDR_T> lsDtAllRpb = _converter.DataTableToList<CMODEL_TABEL_DC_PICKBL_HDR_T>(dtAllRpb);
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
                    dtTransferNpb = await _db.LoadProsesNpbByDocNo(selectedNoRpb);
                });
                if (dtTransferNpb.Rows.Count <= 0) {
                    MessageBox.Show("Tidak Ada Data Transfer NPB", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_GRID_TRANSFER_RESEND_NPB> lsTransferNpb = _converter.DataTableToList<CMODEL_GRID_TRANSFER_RESEND_NPB>(dtTransferNpb);
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
                if (totalScan <= 0) {
                    MessageBox.Show($"Tidak Ada Pemenuhan.{Environment.NewLine}Gagal Buat NPB DC.{Environment.NewLine}Silahkan Load Ulang Untuk Refresh.{Environment.NewLine}Gunakan Tombol Un-Pick Untuk Membatalkan RPB.", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else {
                    decimal dcid = 0;
                    await Task.Run(async () => {
                        dcid = await _db.OraPg_ExecScalar<decimal>(
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
                            runProc = await _db.OraPg_CALL_(
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
                                npb = await _db.OraPg_ExecScalar<decimal>(
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
                if (totalScan > 0) {
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
                            await Task.Run(async () => {
                                await _db.BatalPick(doc_no, seq_no);
                            });
                            MessageBox.Show("Selesai Un-Pick", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex) {
                            MessageBox.Show(ex.Message, ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            SetIdleBusyStatus(true);
        }

        /* Re-Send NPB */

        private async void tabReSendNpb_Enter(object sender, EventArgs e) {
            if (_app.IsIdle) {
                SetIdleBusyStatus(false);
                listResendNpb.Clear();
                listResendNpbAllNo.Clear();
                DataTable dtAllNpb = new DataTable();
                await Task.Run(async () => {
                    dtAllNpb = await _db.LoadNpbDcNoReSend();
                });
                List<CMODEL_TABEL_DC_PICKBL_HDR_T> lsDtAllNpb = _converter.DataTableToList<CMODEL_TABEL_DC_PICKBL_HDR_T>(dtAllNpb);
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
                    dtResendNpb = await _db.LoadResendByNpbDcNo(selectedNoNpb);
                });
                if (dtResendNpb.Rows.Count <= 0) {
                    MessageBox.Show("Tidak Ada Data Re/Send NPB", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_GRID_TRANSFER_RESEND_NPB> lsResendNpb = _converter.DataTableToList<CMODEL_GRID_TRANSFER_RESEND_NPB>(dtResendNpb);
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
            string apiDcho = _config.Get<string>("ApiDchoUrl", _app.GetConfig("api_dcho_url"));
            string apiTargetUrl = _config.Get<string>("ApiDevUrl", _app.GetConfig("api_dev_url"));
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
                    string jsonBody = _converter.ObjectToJson(new CMODEL_JSON_KIRIM_DCHO {
                        TipeData = _config.Get<string>("ApiTipeData", _app.GetConfig("api_tipe_data")),
                        JenisIP = _config.Get<string>("ApiJenisIp", _app.GetConfig("api_jenis_ip")),
                        KodeDC = kode_dc
                    });
                    var resApi = await _api.PostData(apiDcho, jsonBody);
                    string resApiStr = await resApi.Content.ReadAsStringAsync();
                    CMODEL_JSON_TERIMA_DCHO resApiObj = _converter.JsonToObject<CMODEL_JSON_TERIMA_DCHO>(resApiStr);
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
                            string jsonBody = _converter.ObjectToJson(bl);
                            var resApi = await _api.PostData(apiTarget, jsonBody);
                            string apiStatusText = $"{(int)resApi.StatusCode} {resApi.StatusCode} - {ctx}";
                            string resApiStr = await resApi.Content.ReadAsStringAsync();
                            CMODEL_JSON_TERIMA_NPB_BL resApiObj = _converter.JsonToObject<CMODEL_JSON_TERIMA_NPB_BL>(resApiStr);
                            MessageBoxIcon msgBxIco;
                            if (resApi.StatusCode >= System.Net.HttpStatusCode.OK && resApi.StatusCode < System.Net.HttpStatusCode.MultipleChoices) {
                                msgBxIco = MessageBoxIcon.Information;
                            }
                            else {
                                msgBxIco = MessageBoxIcon.Error;
                            }
                            MessageBox.Show(resApiObj.Info, apiStatusText, MessageBoxButtons.OK, msgBxIco);
                            bool hasilInsert = await _db.SaveLogApi(
                                apiDcKode,
                                apiOwner,
                                urlPaths[urlPaths.Length - 1],
                                jsonBody,
                                resApiStr,
                                $"{(int) resApi.StatusCode} {resApi.StatusCode}"
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
                    dtReport = await _db.ViewLaporanByNpbDcNo(selectedNoNpb);
                });
                rptViewer.Reset();
                rptViewer.Clear();
                if (dtReport.Rows.Count <= 0) {
                    MessageBox.Show("Tidak Ada Data Untuk Laporan", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else {
                    List<CMODEL_DS_NPBTAGBL> lsReport = _converter.DataTableToList<CMODEL_DS_NPBTAGBL>(dtReport);
                    List<ReportParameter> paramList = new List<ReportParameter> {
                        new ReportParameter("user", $"{_db.LoggedInUsername}"),
                        new ReportParameter("dc_kode_nama_pengirim", $"{lsReport.FirstOrDefault().PENGIRIM}"),
                        new ReportParameter("tgl_npb", $"Tanggal NPB : {lsReport.FirstOrDefault().TGL_NPB}"),
                        new ReportParameter("no_npb", $"No. NPB : {lsReport.FirstOrDefault().NO_NPB}"),
                        new ReportParameter("dc_kode_nama_penerima", $"{lsReport.FirstOrDefault().PENERIMA}"),
                        new ReportParameter("rpb_no_tgl", $"{lsReport.FirstOrDefault().DOC_NO}/{lsReport.FirstOrDefault().NO_REF}")
                    };
                    rptViewer.LocalReport.ReportPath = _app.AppLocation + "/Rdlcs/NpbTagBl.rdlc";
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
            if (_app.IsIdle) {
                SetIdleBusyStatus(false);
                DataTable dtLogs = new DataTable();
                await Task.Run(async () => {
                    dtLogs = await _db.ViewLogApi();
                });
                List<CMODEL_TABEL_LOG_API_DC> logs = _converter.DataTableToList<CMODEL_TABEL_LOG_API_DC>(dtLogs);
                foreach (CMODEL_TABEL_LOG_API_DC l in logs) {
                    try {
                        CMODEL_JSON_TERIMA_NPB_BL resApiObj = _converter.JsonToObject<CMODEL_JSON_TERIMA_NPB_BL>(l.DATABALIK);
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
