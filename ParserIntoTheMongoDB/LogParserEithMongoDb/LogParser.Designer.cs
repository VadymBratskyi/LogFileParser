namespace LogParserEithMongoDb
{
    partial class LogParser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogParser));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openFolderWithJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLOGSFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDatasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridFsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2PathFolder = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxConnectionString = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label5CountInserted = new System.Windows.Forms.Label();
            this.label4WithoutPair = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelCountFiles = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelFileName = new System.Windows.Forms.Label();
            this.progressBarToFinish = new System.Windows.Forms.ProgressBar();
            this.progressBarFile = new System.Windows.Forms.ProgressBar();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderWithJSONToolStripMenuItem,
            this.openLOGSFilesToolStripMenuItem,
            this.showDatasToolStripMenuItem,
            this.gridFsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1105, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openFolderWithJSONToolStripMenuItem
            // 
            this.openFolderWithJSONToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openFolderWithJSONToolStripMenuItem.Image")));
            this.openFolderWithJSONToolStripMenuItem.Name = "openFolderWithJSONToolStripMenuItem";
            this.openFolderWithJSONToolStripMenuItem.Size = new System.Drawing.Size(155, 20);
            this.openFolderWithJSONToolStripMenuItem.Text = "Open folder with JSON";
            this.openFolderWithJSONToolStripMenuItem.Click += new System.EventHandler(this.openFolderWithJSONToolStripMenuItem_Click);
            // 
            // openLOGSFilesToolStripMenuItem
            // 
            this.openLOGSFilesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openLOGSFilesToolStripMenuItem.Image")));
            this.openLOGSFilesToolStripMenuItem.Name = "openLOGSFilesToolStripMenuItem";
            this.openLOGSFilesToolStripMenuItem.Size = new System.Drawing.Size(120, 20);
            this.openLOGSFilesToolStripMenuItem.Text = "Open LOGS files";
            this.openLOGSFilesToolStripMenuItem.Click += new System.EventHandler(this.openLOGSFilesToolStripMenuItem_Click);
            // 
            // showDatasToolStripMenuItem
            // 
            this.showDatasToolStripMenuItem.Image = global::LogParserEithMongoDb.Properties.Resources.analysis;
            this.showDatasToolStripMenuItem.Name = "showDatasToolStripMenuItem";
            this.showDatasToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.showDatasToolStripMenuItem.Text = "Show Datas";
            this.showDatasToolStripMenuItem.Click += new System.EventHandler(this.showDatasToolStripMenuItem_Click);
            // 
            // gridFsToolStripMenuItem
            // 
            this.gridFsToolStripMenuItem.Name = "gridFsToolStripMenuItem";
            this.gridFsToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.gridFsToolStripMenuItem.Text = "GridFs";
            this.gridFsToolStripMenuItem.Click += new System.EventHandler(this.gridFsToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.textBox2PathFolder);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textBoxConnectionString);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel1.Size = new System.Drawing.Size(1105, 709);
            this.panel1.TabIndex = 10;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(0, 133);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(1101, 403);
            this.textBox1.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(0, 108);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 10, 0, 2);
            this.label5.Size = new System.Drawing.Size(52, 25);
            this.label5.TabIndex = 21;
            this.label5.Text = "Process";
            // 
            // textBox2PathFolder
            // 
            this.textBox2PathFolder.BackColor = System.Drawing.Color.White;
            this.textBox2PathFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox2PathFolder.Location = new System.Drawing.Point(0, 71);
            this.textBox2PathFolder.Multiline = true;
            this.textBox2PathFolder.Name = "textBox2PathFolder";
            this.textBox2PathFolder.ReadOnly = true;
            this.textBox2PathFolder.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2PathFolder.Size = new System.Drawing.Size(1101, 37);
            this.textBox2PathFolder.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(0, 56);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.label6.Size = new System.Drawing.Size(87, 15);
            this.label6.TabIndex = 16;
            this.label6.Text = "Path of Folder";
            // 
            // textBoxConnectionString
            // 
            this.textBoxConnectionString.BackColor = System.Drawing.Color.White;
            this.textBoxConnectionString.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxConnectionString.Location = new System.Drawing.Point(0, 15);
            this.textBoxConnectionString.Multiline = true;
            this.textBoxConnectionString.Name = "textBoxConnectionString";
            this.textBoxConnectionString.ReadOnly = true;
            this.textBoxConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxConnectionString.Size = new System.Drawing.Size(1101, 41);
            this.textBoxConnectionString.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.label4.Size = new System.Drawing.Size(163, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "Connection string to log DB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Count files without pair";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(168, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Count objects inserted into the DB";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label5CountInserted, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label4WithoutPair, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 688);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1105, 45);
            this.tableLayoutPanel2.TabIndex = 22;
            // 
            // label5CountInserted
            // 
            this.label5CountInserted.AutoSize = true;
            this.label5CountInserted.Location = new System.Drawing.Point(177, 22);
            this.label5CountInserted.Name = "label5CountInserted";
            this.label5CountInserted.Size = new System.Drawing.Size(13, 13);
            this.label5CountInserted.TabIndex = 6;
            this.label5CountInserted.Text = "0";
            // 
            // label4WithoutPair
            // 
            this.label4WithoutPair.AutoSize = true;
            this.label4WithoutPair.Location = new System.Drawing.Point(177, 0);
            this.label4WithoutPair.Name = "label4WithoutPair";
            this.label4WithoutPair.Size = new System.Drawing.Size(13, 13);
            this.label4WithoutPair.TabIndex = 5;
            this.label4WithoutPair.Text = "0";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.labelCountFiles);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.labelTime);
            this.panel2.Controls.Add(this.labelFileName);
            this.panel2.Controls.Add(this.progressBarToFinish);
            this.panel2.Controls.Add(this.progressBarFile);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 570);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1105, 93);
            this.panel2.TabIndex = 23;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(96, 62);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 10);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // labelCountFiles
            // 
            this.labelCountFiles.AutoSize = true;
            this.labelCountFiles.Location = new System.Drawing.Point(124, 38);
            this.labelCountFiles.Name = "labelCountFiles";
            this.labelCountFiles.Size = new System.Drawing.Size(13, 13);
            this.labelCountFiles.TabIndex = 5;
            this.labelCountFiles.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(3, 38);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(120, 13);
            this.label15.TabIndex = 4;
            this.label15.Text = "Count files to finish:";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(95, 59);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(49, 13);
            this.labelTime.TabIndex = 3;
            this.labelTime.Text = "00:00:00";
            this.labelTime.Visible = false;
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(63, 3);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(0, 13);
            this.labelFileName.TabIndex = 2;
            // 
            // progressBarToFinish
            // 
            this.progressBarToFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarToFinish.Location = new System.Drawing.Point(3, 72);
            this.progressBarToFinish.Name = "progressBarToFinish";
            this.progressBarToFinish.Size = new System.Drawing.Size(1097, 13);
            this.progressBarToFinish.TabIndex = 1;
            // 
            // progressBarFile
            // 
            this.progressBarFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarFile.Cursor = System.Windows.Forms.Cursors.Default;
            this.progressBarFile.Location = new System.Drawing.Point(3, 19);
            this.progressBarFile.Name = "progressBarFile";
            this.progressBarFile.RightToLeftLayout = true;
            this.progressBarFile.Size = new System.Drawing.Size(1097, 13);
            this.progressBarFile.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(3, 59);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Time to finish:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(4, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "File name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 663);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 2);
            this.label1.Size = new System.Drawing.Size(59, 25);
            this.label1.TabIndex = 21;
            this.label1.Text = "Statistics";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LogParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1105, 733);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "LogParser";
            this.Text = "LogParser";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openFolderWithJSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLOGSFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDatasToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TextBox textBoxConnectionString;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox textBox2PathFolder;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Label label5CountInserted;
        public System.Windows.Forms.Label label4WithoutPair;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.ProgressBar progressBarToFinish;
        public System.Windows.Forms.ProgressBar progressBarFile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label labelCountFiles;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.Label labelTime;
        public System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem gridFsToolStripMenuItem;
    }
}

