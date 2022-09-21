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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using DCTRNNPBBL.Helpers._db;
using DCTRNNPBBL.Helpers._models;
using DCTRNNPBBL.Helpers._utils;
using DCTRNNPBBL.Models;

namespace DCTRNNPBBL.Panels {

    public partial class MainApp : UserControl {

        private readonly ILogger _logger;
        private readonly IOracle _oracle;
        private readonly IConverter _converter;

        bool programIdle = true;

        private List<CMODEL_TABEL_DC_HH_T> lsAllHh = new List<CMODEL_TABEL_DC_HH_T>();

        /* Split */

        private CMODEL_TABEL_DC_HH_T selectedSplitHhScan = null;

        private List<CMODEL_TABEL_DC_HH_T> lsAvailableSplitHhPick = new List<CMODEL_TABEL_DC_HH_T>();
        private BindingList<CMODEL_TABEL_DC_HH_T> blAvailableSplitHhPick = null;

        private List<CMODEL_TABEL_DC_HH_T> lsSelectedSplitHhPick = new List<CMODEL_TABEL_DC_HH_T>();
        private BindingList<CMODEL_TABEL_DC_HH_T> blSelectedSplitHhPick = null;

        private List<CMODEL_GRID_SPLIT> lsSplit = new List<CMODEL_GRID_SPLIT>();
        private BindingList<CMODEL_GRID_SPLIT> blSplit = null;

        /* Edit Split */

        private List<CMODEL_TABEL_DC_HH_T> lsAvailableEditSplitHhPick = new List<CMODEL_TABEL_DC_HH_T>();
        private BindingList<CMODEL_TABEL_DC_HH_T> blAvailableEditSplitHhPick = null;

        private List<CMODEL_TABEL_DC_HH_T> lsAvailableEditSplitHhScan = new List<CMODEL_TABEL_DC_HH_T>();
        private BindingList<CMODEL_TABEL_DC_HH_T> blAvailableEditSplitHhScan = null;

        private List<CMODEL_GRID_EDIT_SPLIT> lsEditSplit = new List<CMODEL_GRID_EDIT_SPLIT>();
        private BindingList<CMODEL_GRID_EDIT_SPLIT> blEditSplit = null;

        /* ** */

        public MainApp(ILogger logger, IOracle oracle, IConverter converter) {
            _logger = logger;
            _oracle = oracle;
            _converter = converter;

            InitializeComponent();
            OnInit();
        }

        private void OnInit() {

            // Change Form View
            Dock = DockStyle.Fill;
            SetUserInfo();
        }

        private void MainApp_Load(object sender, EventArgs e) {
            // tabSplit.Enter += new System.EventHandler(tabSplit_Enter);

            SetIdleBusyStatus(true);
        }

        public void SetEnableDisable(bool isEnabled) {

            // Split
            txtSplitNoRpb.Enabled = isEnabled;
            btnSplitLoad.Enabled = isEnabled;
            cmbBxSplitHhPicking.Enabled = isEnabled;
            btnSplitAddHhPicking.Enabled = isEnabled;
            btnSplitHhPicking.Enabled = isEnabled;
            cmbBxSplitHhScanning.Enabled = isEnabled;
            btnSplitSetHhScanning.Enabled = isEnabled;
            btnSplitProses.Enabled = isEnabled;
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
            }
        }

        private void SetHandHeld() {
            lsAvailableSplitHhPick = lsAllHh.ToList();
            lsAvailableSplitHhPick.Sort((x, y) => x.HH.CompareTo(y.HH));

            blAvailableSplitHhPick = new BindingList<CMODEL_TABEL_DC_HH_T>(lsAvailableSplitHhPick);
            blSelectedSplitHhPick = new BindingList<CMODEL_TABEL_DC_HH_T>(lsSelectedSplitHhPick);

            cmbBxSplitHhPicking.DisplayMember = "HH";
            cmbBxSplitHhPicking.ValueMember = "HH";
            cmbBxSplitHhPicking.DataSource = blAvailableSplitHhPick;

            cmbBxSplitHhScanning.DisplayMember = "HH";
            cmbBxSplitHhScanning.ValueMember = "HH";
            cmbBxSplitHhScanning.DataSource = blAvailableSplitHhPick;

            dtGrdSplitHhPicking.DataSource = blSelectedSplitHhPick;
            dtGrdSplitHhPicking.Columns["NO_HH"].Visible = false;
            dtGrdSplitHhPicking.Columns["DC_ID"].Visible = false;
            dtGrdSplitHhPicking.Columns["BAGIAN"].Visible = false;
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

            lsSelectedSplitHhPick.ForEach(d => {
                lsAvailableSplitHhPick.Remove(lsAvailableSplitHhPick.Find(l => l.HH == d.HH));
            });
            if (selectedSplitHhScan != null) {
                lsAvailableSplitHhPick.Remove(lsAvailableSplitHhPick.Find(l => l.HH == selectedSplitHhScan.HH));
            }
        }

        private void AddSplitHhPick() {
            if (programIdle) {
                List<CMODEL_TABEL_DC_HH_T> selected = lsAvailableSplitHhPick.Where(d => d.HH == cmbBxSplitHhPicking.SelectedValue.ToString()).ToList();
                selected.ForEach(d => {
                    lsAvailableSplitHhPick.Remove(d);
                    lsSelectedSplitHhPick.Add(d);
                });
                lsSelectedSplitHhPick.Sort((x, y) => x.HH.CompareTo(y.HH));

                blAvailableSplitHhPick.ResetBindings();
                blSelectedSplitHhPick.ResetBindings();
            }
        }

        private void DeleteSplitHhPick(string HH) {
            if (programIdle) {
                List<CMODEL_TABEL_DC_HH_T> selected = lsSelectedSplitHhPick.Where(d => d.HH == HH).ToList();
                selected.ForEach(d => {
                    lsSelectedSplitHhPick.Remove(d);
                    lsAvailableSplitHhPick.Add(d);
                });
                lsAvailableSplitHhPick.Sort((x, y) => x.HH.CompareTo(y.HH));

                blAvailableSplitHhPick.ResetBindings();
                blSelectedSplitHhPick.ResetBindings();
            }
        }

        private void AddSplitHhScan() {
            if (programIdle) {
                CMODEL_TABEL_DC_HH_T z = lsAvailableSplitHhPick.Where(d => d.HH == cmbBxSplitHhPicking.SelectedValue.ToString()).ToList().First();
                lsAvailableSplitHhPick.Remove(lsAvailableSplitHhPick.Find(l => l.HH == z.HH));

                if (selectedSplitHhScan != null) {
                    lsAvailableSplitHhPick.Add(selectedSplitHhScan);
                    lsAvailableSplitHhPick.Sort((x, y) => x.HH.CompareTo(y.HH));
                }

                selectedSplitHhScan = z;
                foreach (CMODEL_GRID_SPLIT s in blSplit) {
                    s.HH_SCAN = z.HH;
                }

                blAvailableSplitHhPick.ResetBindings();
                blSplit.ResetBindings();
            }
        }

        private async void LoadSplit(bool showEmptyMessageDialog = true) {
            SetIdleBusyStatus(false);
            await LoadHandHeld();

            string ctx = "Pencarian Split RPB ...";
            DataTable dtSplit = new DataTable();

            lsAvailableSplitHhPick.Clear();
            lsSelectedSplitHhPick.Clear();
            selectedSplitHhScan = null;
            lsSplit.Clear();

            if (!string.IsNullOrEmpty(txtSplitNoRpb.Text)) {

                // Fetch DC_PICKBL_HDR_T
                await Task.Run(async () => {
                    dtSplit = await _oracle.GetDataTableAsync($@"
                        SELECT
                            a.SEQ_NO,
                            a.DOC_DATE,
                            b.PLU_ID,
                            c.MBR_SINGKATAN SINGKATAN,
                            (PLA_LINE || '.' || PLA_RAK || '.' || PLA_SHELF || '.' || PLA_CELL) LOKASI,
                            b.QTY_RPB,
                            b.IP_PICKING HH_PICK,
                            b.IP_SCANNING HH_SCAN
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
                    ", new List<CDbQueryParamBind> {
                        new CDbQueryParamBind { NAME = "doc_no", VALUE = txtSplitNoRpb.Text }
                    });
                });

                if (dtSplit.Rows.Count <= 0) {
                    if (showEmptyMessageDialog) {
                        MessageBox.Show("Tidak Ada Data Split", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                }
                else {
                    lsSplit = _converter.ConvertDataTableToList<CMODEL_GRID_SPLIT>(dtSplit);
                    lsSplit.Sort((x, y) => x.PLU_ID.CompareTo(y.PLU_ID));

                    foreach (CMODEL_GRID_SPLIT d in lsSplit) {
                        CMODEL_TABEL_DC_HH_T h = lsAllHh.Where(l => l.HH == d.HH_PICK).ToList().FirstOrDefault();
                        if (h != null && !lsSelectedSplitHhPick.Contains(h)) {
                            lsSelectedSplitHhPick.Add(h);
                        }
                    }

                    SetHandHeld();

                    blSplit = new BindingList<CMODEL_GRID_SPLIT>(lsSplit);

                    txtSplitNoSeq.Text = lsSplit.First().SEQ_NO.ToString();
                    dtPckrSplitTglRpb.Value = lsSplit.First().DOC_DATE;

                    dtGrdSplit.DataSource = blSplit;
                    dtGrdSplit.Columns["SEQ_NO"].Visible = false;
                    dtGrdSplit.Columns["DOC_DATE"].Visible = false;
                    dtGrdSplit.Columns["HH_PICK"].Visible = false;
                    dtGrdSplit.Columns["HH_PICK"].Visible = false;

                    dtGrdSplit.Columns["PLU_ID"].ReadOnly = true;
                    dtGrdSplit.Columns["SINGKATAN"].ReadOnly = true;
                    dtGrdSplit.Columns["LOKASI"].ReadOnly = true;
                    dtGrdSplit.Columns["QTY_RPB"].ReadOnly = true;
                    dtGrdSplit.Columns["HH_SCAN"].ReadOnly = true;

                    if (!dtGrdSplit.Columns.Contains("IP_HH_PICK")) {
                        dtGrdSplit.Columns.Add(new DataGridViewComboBoxColumn {
                            Name = "IP_HH_PICK",
                            HeaderText = "HH_PICK",
                            DataPropertyName = "HH_PICK",
                            DisplayMember = "HH",
                            ValueMember = "HH",
                            DataSource = blSelectedSplitHhPick
                        });
                    }
                }
            }
            else {
                MessageBox.Show("Harap Isi DOC_NO Rpb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (blSplit != null) {
                blSplit.ResetBindings();
            }
            if (blAvailableSplitHhPick != null) {
                blAvailableSplitHhPick.ResetBindings();
            }
            if (blSelectedSplitHhPick != null) {
                blSelectedSplitHhPick.ResetBindings();
            }

            SetIdleBusyStatus(true);
        }

        // private void tabSplit_Enter(object sender, EventArgs e) {
        //     LoadHandHeld();
        // }

        private void btnSplitLoad_Click(object sender, EventArgs e) {
            LoadSplit();
        }

        private void btnSplitAddHhPicking_Click(object sender, EventArgs e) {
            AddSplitHhPick();
        }

        private void dtGrdSplitHhPicking_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            try {
                string ctx = "Delete HH Picking ...";

                if (e.ColumnIndex == dtGrdSplitHhPicking.Columns["X"].Index) {
                    if (lsSelectedSplitHhPick.Count > 1) {
                        bool safeForDelete = true;
                        string HH_PICK = dtGrdSplitHhPicking.Rows[e.RowIndex].Cells[dtGrdSplitHhPicking.Columns["HH"].Index].Value.ToString();
                        foreach (CMODEL_GRID_SPLIT data in lsSplit) {
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
            catch (Exception ex) {
                _logger.WriteError(ex);
            }
        }

        private void btnSplitSetHhScanning_Click(object sender, EventArgs e) {
            AddSplitHhScan();
        }

        private void btnSplitHhPicking_Click(object sender, EventArgs e) {
            string ctx = "Atur HH Split ...";

            if (lsSelectedSplitHhPick.Count <= 0) {
                MessageBox.Show("Harap Input HH Picking", ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                for (int i = 0; i < lsSplit.Count; i++) {
                    lsSplit[i].HH_PICK = lsSelectedSplitHhPick[i % lsSelectedSplitHhPick.Count].HH;
                }
            }
            blSplit.ResetBindings();
        }

        private async void btnSplitProses_Click(object sender, EventArgs e) {
            SetIdleBusyStatus(false);
            string ctx = "Proses Split ...";
            bool safeForUpdate = true;

            foreach (CMODEL_GRID_SPLIT data in lsSplit) {
                if (string.IsNullOrEmpty(data.HH_PICK) || string.IsNullOrEmpty(data.HH_SCAN)) {
                    MessageBox.Show("Masih Ada Yang Belum Di Assign HH", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    safeForUpdate = false;
                    break;
                }
            }

            if (safeForUpdate) {
                try {
                    await _oracle.MarkBeforeExecQueryCommitAndRollback();

                    foreach (CMODEL_GRID_SPLIT data in lsSplit) {
                        bool update1 = false;
                        await Task.Run(async () => {
                            update1 = await _oracle.ExecQueryAsync($@"
                                UPDATE DC_PICKBL_DTL_T
                                    SET IP_PICKING = :ip_picking, IP_SCANNING = :ip_scanning
                                WHERE SEQ_FK_NO = :seq_fk_no AND PLU_ID = :plu_id
                            ", new List<CDbQueryParamBind> {
                                new CDbQueryParamBind { NAME = "ip_picking", VALUE = data.HH_PICK },
                                new CDbQueryParamBind { NAME = "ip_scanning", VALUE = data.HH_SCAN },
                                new CDbQueryParamBind { NAME = "seq_fk_no", VALUE = data.SEQ_NO },
                                new CDbQueryParamBind { NAME = "plu_id", VALUE = data.PLU_ID }
                            }, false);
                        });
                        if (!update1) {
                            throw new Exception($"Gagal Mengatur HH Untuk {data.SINGKATAN}");
                        }
                    }

                    bool update2 = false;
                    await Task.Run(async () => {
                        update2 = await _oracle.ExecQueryAsync($@"
                            UPDATE DC_PICKBL_HDR_T
                                SET TGL_SPLIT = SYSDATE
                            WHERE SEQ_NO = :seq_no
                        ", new List<CDbQueryParamBind> {
                            new CDbQueryParamBind { NAME = "seq_no", VALUE = decimal.Parse(txtSplitNoSeq.Text) }
                        }, false);
                    });
                    if (!update2) {
                        throw new Exception($"Gagal Set Tanggal Split");
                    }

                    _oracle.MarkSuccessExecQueryAndCommit();

                    MessageBox.Show("Selesai", ctx, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSplitNoRpb.Text = "0";
                    LoadSplit(false);
                }
                catch (Exception ex) {
                    _oracle.MarkFailExecQueryAndRollback();
                    MessageBox.Show(ex.Message, ctx, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            SetIdleBusyStatus(true);
        }

        /* Edit Split */

        private async void LoadEditSplit(bool showEmptyMessageDialog = true) {
            SetIdleBusyStatus(false);
            await LoadHandHeld();

            string ctx = "Pencarian Edit Split RPB ...";
            DataTable dtEditSplit = new DataTable();

            lsAvailableEditSplitHhPick.Clear();
            lsAvailableEditSplitHhScan.Clear();
            lsEditSplit.Clear();
            dtGrdEditSplit.Rows.Clear();
            dtGrdEditSplit.Columns.Clear();

            if (!string.IsNullOrEmpty(txtEditSplitNoRpb.Text)) {

                // Fetch DC_PICKBL_HDR_T
                await Task.Run(async () => {
                    dtEditSplit = await _oracle.GetDataTableAsync($@"
                        SELECT
                            a.SEQ_NO,
                            a.DOC_DATE,
                            b.PLU_ID,
                            c.MBR_SINGKATAN SINGKATAN,
                            (PLA_LINE || '.' || PLA_RAK || '.' || PLA_SHELF || '.' || PLA_CELL) LOKASI,
                            b.QTY_RPB,
                            b.IP_PICKING HH_PICK,
                            b.QTY_PICKING,
                            b.IP_SCANNING HH_SCAN,
                            b.QTY_SCANNING
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
                    ", new List<CDbQueryParamBind> {
                        new CDbQueryParamBind { NAME = "doc_no", VALUE = txtEditSplitNoRpb.Text }
                    });
                });

                if (dtEditSplit.Rows.Count <= 0) {
                    if (showEmptyMessageDialog) {
                        MessageBox.Show("Tidak Ada Data Edit Split", ctx, MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                }
                else {
                    lsEditSplit = _converter.ConvertDataTableToList<CMODEL_GRID_EDIT_SPLIT>(dtEditSplit);
                    lsEditSplit.Sort((x, y) => x.PLU_ID.CompareTo(y.PLU_ID));

                    lsAvailableEditSplitHhPick = lsAllHh.ToList();
                    lsAvailableEditSplitHhPick.Sort((x, y) => x.HH.CompareTo(y.HH));
                    blAvailableEditSplitHhPick = new BindingList<CMODEL_TABEL_DC_HH_T>(lsAvailableEditSplitHhPick);

                    lsAvailableEditSplitHhScan = lsAllHh.ToList();
                    lsAvailableEditSplitHhScan.Sort((x, y) => x.HH.CompareTo(y.HH));
                    blAvailableEditSplitHhScan = new BindingList<CMODEL_TABEL_DC_HH_T>(lsAvailableEditSplitHhScan);

                    blEditSplit = new BindingList<CMODEL_GRID_EDIT_SPLIT>(lsEditSplit);

                    txtEditSplitNoSeq.Text = lsEditSplit.First().SEQ_NO.ToString();
                    dtPckrEditSplitTglRpb.Value = lsEditSplit.First().DOC_DATE;

                    dtGrdEditSplit.DataSource = blEditSplit;
                    dtGrdEditSplit.Columns.Remove(dtGrdEditSplit.Columns["SEQ_NO"]);
                    dtGrdEditSplit.Columns.Remove(dtGrdEditSplit.Columns["DOC_DATE"]);
                    dtGrdEditSplit.Columns.Remove(dtGrdEditSplit.Columns["HH_PICK"]);
                    dtGrdEditSplit.Columns.Remove(dtGrdEditSplit.Columns["HH_SCAN"]);

                    dtGrdEditSplit.Columns["PLU_ID"].ReadOnly = true;
                    dtGrdEditSplit.Columns["SINGKATAN"].ReadOnly = true;
                    dtGrdEditSplit.Columns["LOKASI"].ReadOnly = true;

                    dtGrdEditSplit.Columns.Add(new DataGridViewComboBoxColumn {
                        Name = "HH_PICK",
                        HeaderText = "HH_PICK",
                        DataPropertyName = "HH_PICK",
                        DisplayMember = "HH",
                        ValueMember = "HH",
                        DataSource = lsAvailableEditSplitHhPick
                    });

                    dtGrdEditSplit.Columns.Add(new DataGridViewComboBoxColumn {
                        Name = "HH_SCAN",
                        HeaderText = "HH_SCAN",
                        DataPropertyName = "HH_SCAN",
                        DisplayMember = "HH",
                        ValueMember = "HH",
                        DataSource = lsAvailableEditSplitHhScan
                    });
                }
            }
            else {
                MessageBox.Show("Harap Isi DOC_NO Rpb", ctx, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
            if (blEditSplit != null) {
                blEditSplit.ResetBindings();
            }
            if (blAvailableEditSplitHhPick != null) {
                blAvailableEditSplitHhPick.ResetBindings();
            }
            if (blAvailableEditSplitHhScan != null) {
                blAvailableEditSplitHhScan.ResetBindings();
            }

            foreach (DataGridViewRow row in dtGrdEditSplit.Rows) {
                // row.Cells[dtGrdEditSplit.Columns["QTY_PICKING"].Index].Value = (decimal)40;
                // row.Cells[dtGrdEditSplit.Columns["QTY_SCANNING"].Index].Value = (decimal)20;
                if (decimal.Parse(row.Cells[dtGrdEditSplit.Columns["QTY_PICKING"].Index].Value.ToString()) > 0) {
                    row.Cells[dtGrdEditSplit.Columns["HH_PICK"].Index].ReadOnly = true;
                }
                if (decimal.Parse(row.Cells[dtGrdEditSplit.Columns["QTY_SCANNING"].Index].Value.ToString()) > 0) {
                    row.Cells[dtGrdEditSplit.Columns["HH_SCAN"].Index].ReadOnly = true;
                }
                if (row.Cells[dtGrdEditSplit.Columns["HH_PICK"].Index].ReadOnly || row.Cells[dtGrdEditSplit.Columns["HH_SCAN"].Index].ReadOnly) {
                    row.DefaultCellStyle.BackColor = Color.Empty;
                }
                else {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 0);
                }
            }

            SetIdleBusyStatus(true);
        }

        private void btnEditSplitLoad_Click(object sender, EventArgs e) {
            LoadEditSplit();
        }

        private void btnEditSplitCariRpb_Click(object sender, EventArgs e) {
            blAvailableEditSplitHhPick.ResetBindings();
            blAvailableEditSplitHhScan.ResetBindings();
        }

        /* ** */

    }

}
