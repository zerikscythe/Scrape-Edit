using System.Text.RegularExpressions;

namespace ScrapeEdit
{
    public partial class Form_XML_Editor : Form
    {
       
        public Form_XML_Editor(TreeNodeDetail node)
        {
            InitializeComponent();
            rtxt_XML_Viewport.Text = node.Game.CreateGameListXML_Entry();
            HighlightXmlTags(rtxt_XML_Viewport);
        }

        private void HighlightXmlTags(RichTextBox richTextBox)
        {
            // Save the current selection details.
            int originalSelectionStart = richTextBox.SelectionStart;
            int originalSelectionLength = richTextBox.SelectionLength;

            // Regular expression pattern that matches anything starting with '<' and ending with '>'.
            string pattern = @"<[^>]+>";

            // Retrieve all matches.
            MatchCollection matches = Regex.Matches(richTextBox.Text, pattern);

            // Optionally suspend the layout to avoid flicker.
            richTextBox.SuspendLayout();

            foreach (Match match in matches)
            {
                // Select the found text.
                richTextBox.Select(match.Index, match.Length);
                // Change the selection color to blue.
                richTextBox.SelectionColor = Color.Blue;
            }

            // Restore the original selection.
            richTextBox.Select(originalSelectionStart, originalSelectionLength);
            // Optionally reset the selection color if desired.
            richTextBox.SelectionColor = richTextBox.ForeColor;

            richTextBox.ResumeLayout();
        }

    }
}
