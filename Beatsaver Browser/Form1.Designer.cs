namespace Beatsaver_Browser
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Action = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Thumbnail = new System.Windows.Forms.DataGridViewImageColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Beat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Song = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Difficulties = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Downloads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Plays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpVotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uploader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Link = new System.Windows.Forms.DataGridViewLinkColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1214, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Action,
            this.Thumbnail,
            this.ID,
            this.Beat,
            this.Song,
            this.BPM,
            this.Difficulties,
            this.Downloads,
            this.Plays,
            this.UpVotes,
            this.Uploader,
            this.Notes,
            this.Link});
            this.dataGridView1.Location = new System.Drawing.Point(7, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(1205, 599);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Action
            // 
            this.Action.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Action.HeaderText = "";
            this.Action.Name = "Action";
            this.Action.Width = 5;
            // 
            // Thumbnail
            // 
            this.Thumbnail.HeaderText = "Thumbnail";
            this.Thumbnail.Name = "Thumbnail";
            this.Thumbnail.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 43;
            // 
            // Beat
            // 
            this.Beat.HeaderText = "Beat Name";
            this.Beat.Name = "Beat";
            this.Beat.ReadOnly = true;
            // 
            // Song
            // 
            this.Song.HeaderText = "Song Name";
            this.Song.Name = "Song";
            this.Song.ReadOnly = true;
            // 
            // BPM
            // 
            this.BPM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.BPM.HeaderText = "BPM";
            this.BPM.Name = "BPM";
            this.BPM.ReadOnly = true;
            this.BPM.Width = 55;
            // 
            // Difficulties
            // 
            this.Difficulties.HeaderText = "Difficulties";
            this.Difficulties.Name = "Difficulties";
            this.Difficulties.ReadOnly = true;
            // 
            // Downloads
            // 
            this.Downloads.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Downloads.HeaderText = "Downloads";
            this.Downloads.Name = "Downloads";
            this.Downloads.ReadOnly = true;
            this.Downloads.Width = 85;
            // 
            // Plays
            // 
            this.Plays.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Plays.HeaderText = "Plays";
            this.Plays.Name = "Plays";
            this.Plays.ReadOnly = true;
            this.Plays.Width = 57;
            // 
            // UpVotes
            // 
            this.UpVotes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.UpVotes.HeaderText = "UpVotes";
            this.UpVotes.Name = "UpVotes";
            this.UpVotes.ReadOnly = true;
            this.UpVotes.Width = 73;
            // 
            // Uploader
            // 
            this.Uploader.HeaderText = "Uploader";
            this.Uploader.Name = "Uploader";
            this.Uploader.ReadOnly = true;
            // 
            // Notes
            // 
            this.Notes.HeaderText = "Notes";
            this.Notes.Name = "Notes";
            this.Notes.ReadOnly = true;
            // 
            // Link
            // 
            this.Link.HeaderText = "Link";
            this.Link.Name = "Link";
            this.Link.ReadOnly = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 654);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 62);
            this.button1.TabIndex = 2;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(139, 654);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 62);
            this.button2.TabIndex = 3;
            this.button2.Text = "Download and Install";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(253, 654);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(82, 62);
            this.button3.TabIndex = 4;
            this.button3.Text = "Download";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(480, 654);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 62);
            this.button4.TabIndex = 5;
            this.button4.Text = "Uninstall";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(370, 654);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(82, 62);
            this.button5.TabIndex = 6;
            this.button5.Text = "Install";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(593, 654);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(82, 62);
            this.button6.TabIndex = 7;
            this.button6.Text = "Uninstall and Delete Files";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(703, 654);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(82, 62);
            this.button7.TabIndex = 8;
            this.button7.Text = "Delete Files";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1214, 747);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Beatsaver Browser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Action;
        private System.Windows.Forms.DataGridViewImageColumn Thumbnail;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Beat;
        private System.Windows.Forms.DataGridViewTextBoxColumn Song;
        private System.Windows.Forms.DataGridViewTextBoxColumn BPM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Difficulties;
        private System.Windows.Forms.DataGridViewTextBoxColumn Downloads;
        private System.Windows.Forms.DataGridViewTextBoxColumn Plays;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpVotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uploader;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewLinkColumn Link;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}

