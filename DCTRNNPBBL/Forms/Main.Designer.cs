
namespace DCTRNNPBBL.Forms {
    partial class CMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CMain));
            this.statusStripContainer = new System.Windows.Forms.StatusStrip();
            this.statusStripKodeDc = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripIpAddress = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripAppVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.statusStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStripContainer
            // 
            this.statusStripContainer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripKodeDc,
            this.statusStripIpAddress,
            this.statusStripAppVersion});
            this.statusStripContainer.Location = new System.Drawing.Point(0, 339);
            this.statusStripContainer.Name = "statusStripContainer";
            this.statusStripContainer.Size = new System.Drawing.Size(584, 22);
            this.statusStripContainer.SizingGrip = false;
            this.statusStripContainer.TabIndex = 0;
            this.statusStripContainer.Text = "statusStrip1";
            // 
            // statusStripKodeDc
            // 
            this.statusStripKodeDc.Name = "statusStripKodeDc";
            this.statusStripKodeDc.Size = new System.Drawing.Size(79, 17);
            this.statusStripKodeDc.Text = "Disconnected";
            this.statusStripKodeDc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStripIpAddress
            // 
            this.statusStripIpAddress.Name = "statusStripIpAddress";
            this.statusStripIpAddress.Size = new System.Drawing.Size(444, 17);
            this.statusStripIpAddress.Spring = true;
            this.statusStripIpAddress.Text = "0.0.0.0";
            // 
            // statusStripAppVersion
            // 
            this.statusStripAppVersion.Name = "statusStripAppVersion";
            this.statusStripAppVersion.Size = new System.Drawing.Size(46, 17);
            this.statusStripAppVersion.Text = "v0.0.0.0";
            this.statusStripAppVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelContainer
            // 
            this.panelContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelContainer.Location = new System.Drawing.Point(0, 209);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(584, 130);
            this.panelContainer.TabIndex = 1;
            // 
            // imgLogo
            // 
            this.imgLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imgLogo.Image = ((System.Drawing.Image)(resources.GetObject("imgLogo.Image")));
            this.imgLogo.Location = new System.Drawing.Point(150, 40);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(300, 125);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgLogo.TabIndex = 11;
            this.imgLogo.TabStop = false;
            // 
            // CMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.imgLogo);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.statusStripContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Window";
            this.Load += new System.EventHandler(this.CMain_Load);
            this.statusStripContainer.ResumeLayout(false);
            this.statusStripContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStripContainer;
        private System.Windows.Forms.ToolStripStatusLabel statusStripKodeDc;
        private System.Windows.Forms.ToolStripStatusLabel statusStripIpAddress;
        private System.Windows.Forms.ToolStripStatusLabel statusStripAppVersion;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.PictureBox imgLogo;
    }
}