namespace ScrapeEdit
{
    partial class Form_M3U
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
            lb_M3U_gamefiles = new ListBox();
            chk_M3U_CreateSubDir = new CheckBox();
            btn_M3U_Create = new Button();
            btn_M3U_EntryUP = new Button();
            btn_M3U_EntryDOWN = new Button();
            chk_M3U_HideFiles = new CheckBox();
            tb_M3U_FileName = new TextBox();
            lbl_M3U_FileName = new Label();
            chk_M3U_CopyMetaData = new CheckBox();
            tb_M3U_ManulEntry = new TextBox();
            btn_M3U_Add = new Button();
            btn_M3U_Delete = new Button();
            SuspendLayout();
            // 
            // lb_M3U_gamefiles
            // 
            lb_M3U_gamefiles.FormattingEnabled = true;
            lb_M3U_gamefiles.ItemHeight = 15;
            lb_M3U_gamefiles.Location = new Point(8, 7);
            lb_M3U_gamefiles.Margin = new Padding(2);
            lb_M3U_gamefiles.Name = "lb_M3U_gamefiles";
            lb_M3U_gamefiles.Size = new Size(574, 124);
            lb_M3U_gamefiles.TabIndex = 0;
            // 
            // chk_M3U_CreateSubDir
            // 
            chk_M3U_CreateSubDir.AutoSize = true;
            chk_M3U_CreateSubDir.Location = new Point(638, 105);
            chk_M3U_CreateSubDir.Margin = new Padding(2);
            chk_M3U_CreateSubDir.Name = "chk_M3U_CreateSubDir";
            chk_M3U_CreateSubDir.Size = new Size(93, 19);
            chk_M3U_CreateSubDir.TabIndex = 1;
            chk_M3U_CreateSubDir.Text = "New SubDir?";
            chk_M3U_CreateSubDir.UseVisualStyleBackColor = true;
            // 
            // btn_M3U_Create
            // 
            btn_M3U_Create.Location = new Point(727, 136);
            btn_M3U_Create.Margin = new Padding(2);
            btn_M3U_Create.Name = "btn_M3U_Create";
            btn_M3U_Create.Size = new Size(78, 20);
            btn_M3U_Create.TabIndex = 2;
            btn_M3U_Create.Text = "Create";
            btn_M3U_Create.UseVisualStyleBackColor = true;
            btn_M3U_Create.Click += btn_M3U_Create_Click;
            // 
            // btn_M3U_EntryUP
            // 
            btn_M3U_EntryUP.Location = new Point(586, 9);
            btn_M3U_EntryUP.Margin = new Padding(2);
            btn_M3U_EntryUP.Name = "btn_M3U_EntryUP";
            btn_M3U_EntryUP.Size = new Size(30, 20);
            btn_M3U_EntryUP.TabIndex = 4;
            btn_M3U_EntryUP.Text = "^";
            btn_M3U_EntryUP.UseVisualStyleBackColor = true;
            btn_M3U_EntryUP.Click += btn_M3U_EntryUP_Click;
            // 
            // btn_M3U_EntryDOWN
            // 
            btn_M3U_EntryDOWN.Location = new Point(586, 33);
            btn_M3U_EntryDOWN.Margin = new Padding(2);
            btn_M3U_EntryDOWN.Name = "btn_M3U_EntryDOWN";
            btn_M3U_EntryDOWN.Size = new Size(30, 20);
            btn_M3U_EntryDOWN.TabIndex = 5;
            btn_M3U_EntryDOWN.Text = "v";
            btn_M3U_EntryDOWN.UseVisualStyleBackColor = true;
            btn_M3U_EntryDOWN.Click += btn_M3U_EntryDOWN_Click;
            // 
            // chk_M3U_HideFiles
            // 
            chk_M3U_HideFiles.AutoSize = true;
            chk_M3U_HideFiles.Location = new Point(638, 84);
            chk_M3U_HideFiles.Margin = new Padding(2);
            chk_M3U_HideFiles.Name = "chk_M3U_HideFiles";
            chk_M3U_HideFiles.Size = new Size(77, 19);
            chk_M3U_HideFiles.TabIndex = 6;
            chk_M3U_HideFiles.Text = "Hide Files";
            chk_M3U_HideFiles.UseVisualStyleBackColor = true;
            // 
            // tb_M3U_FileName
            // 
            tb_M3U_FileName.Location = new Point(638, 34);
            tb_M3U_FileName.Margin = new Padding(2);
            tb_M3U_FileName.Name = "tb_M3U_FileName";
            tb_M3U_FileName.Size = new Size(167, 23);
            tb_M3U_FileName.TabIndex = 7;
            // 
            // lbl_M3U_FileName
            // 
            lbl_M3U_FileName.AutoSize = true;
            lbl_M3U_FileName.Location = new Point(638, 12);
            lbl_M3U_FileName.Margin = new Padding(2, 0, 2, 0);
            lbl_M3U_FileName.Name = "lbl_M3U_FileName";
            lbl_M3U_FileName.Size = new Size(91, 15);
            lbl_M3U_FileName.TabIndex = 8;
            lbl_M3U_FileName.Text = ".M3U File Name";
            // 
            // chk_M3U_CopyMetaData
            // 
            chk_M3U_CopyMetaData.AutoSize = true;
            chk_M3U_CopyMetaData.Location = new Point(638, 61);
            chk_M3U_CopyMetaData.Margin = new Padding(2);
            chk_M3U_CopyMetaData.Name = "chk_M3U_CopyMetaData";
            chk_M3U_CopyMetaData.Size = new Size(112, 19);
            chk_M3U_CopyMetaData.TabIndex = 9;
            chk_M3U_CopyMetaData.Text = "Copy Metadata?";
            chk_M3U_CopyMetaData.UseVisualStyleBackColor = true;
            // 
            // tb_M3U_ManulEntry
            // 
            tb_M3U_ManulEntry.Location = new Point(172, 136);
            tb_M3U_ManulEntry.Name = "tb_M3U_ManulEntry";
            tb_M3U_ManulEntry.Size = new Size(329, 23);
            tb_M3U_ManulEntry.TabIndex = 10;
            // 
            // btn_M3U_Add
            // 
            btn_M3U_Add.Location = new Point(507, 135);
            btn_M3U_Add.Name = "btn_M3U_Add";
            btn_M3U_Add.Size = new Size(75, 23);
            btn_M3U_Add.TabIndex = 11;
            btn_M3U_Add.Text = "Add";
            btn_M3U_Add.UseVisualStyleBackColor = true;
            btn_M3U_Add.Click += btn_M3U_Add_Click;
            // 
            // btn_M3U_Delete
            // 
            btn_M3U_Delete.Location = new Point(8, 134);
            btn_M3U_Delete.Name = "btn_M3U_Delete";
            btn_M3U_Delete.Size = new Size(75, 23);
            btn_M3U_Delete.TabIndex = 12;
            btn_M3U_Delete.Text = "Delete";
            btn_M3U_Delete.UseVisualStyleBackColor = true;
            btn_M3U_Delete.Click += btn_M3U_Delete_Click;
            // 
            // Form_M3U
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(818, 171);
            Controls.Add(btn_M3U_Delete);
            Controls.Add(btn_M3U_Add);
            Controls.Add(tb_M3U_ManulEntry);
            Controls.Add(chk_M3U_CopyMetaData);
            Controls.Add(lbl_M3U_FileName);
            Controls.Add(tb_M3U_FileName);
            Controls.Add(chk_M3U_HideFiles);
            Controls.Add(btn_M3U_EntryDOWN);
            Controls.Add(btn_M3U_EntryUP);
            Controls.Add(btn_M3U_Create);
            Controls.Add(chk_M3U_CreateSubDir);
            Controls.Add(lb_M3U_gamefiles);
            Margin = new Padding(2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form_M3U";
            ShowIcon = false;
            Text = "M3U Creator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lb_M3U_gamefiles;
        private CheckBox chk_M3U_CreateSubDir;
        private Button btn_M3U_Create;
        private Button btn_M3U_EntryUP;
        private Button btn_M3U_EntryDOWN;
        private CheckBox chk_M3U_HideFiles;
        private TextBox tb_M3U_FileName;
        private Label lbl_M3U_FileName;
        private CheckBox chk_M3U_CopyMetaData;
        private TextBox tb_M3U_ManulEntry;
        private Button btn_M3U_Add;
        private Button btn_M3U_Delete;
    }
}