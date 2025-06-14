namespace ScrapeEdit
{
    partial class Form_XML_Editor
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
            rtxt_XML_Viewport = new RichTextBox();
            SuspendLayout();
            // 
            // rtxt_XML_Viewport
            // 
            rtxt_XML_Viewport.AcceptsTab = true;
            rtxt_XML_Viewport.Location = new Point(12, 12);
            rtxt_XML_Viewport.Name = "rtxt_XML_Viewport";
            rtxt_XML_Viewport.Size = new Size(664, 426);
            rtxt_XML_Viewport.TabIndex = 1;
            rtxt_XML_Viewport.Text = "";
            rtxt_XML_Viewport.WordWrap = false;
            // 
            // Form_XML_Editor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(688, 450);
            Controls.Add(rtxt_XML_Viewport);
            Name = "Form_XML_Editor";
            Text = "Form_XML_Editor";
            ResumeLayout(false);
        }

        #endregion
        private RichTextBox rtxt_XML_Viewport;
    }
}