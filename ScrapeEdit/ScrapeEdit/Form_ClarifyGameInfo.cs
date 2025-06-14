using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScrapeEdit
{
    public partial class Form_ClarifyGameInfo : Form
    {
        public string EnteredTitle { get; private set; }
        public string SelectedConsoleID { get; private set; }

        public Form_ClarifyGameInfo(string defaultTitle, Dictionary<string, string> consoleDict)
        {
            InitializeComponent();

            txtTitle.Text = defaultTitle;
            cboConsole.DataSource = new BindingSource(consoleDict, null);
            cboConsole.DisplayMember = "Key";
            cboConsole.ValueMember = "Value";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            EnteredTitle = txtTitle.Text.Trim();
            SelectedConsoleID = ((KeyValuePair<string, string>)cboConsole.SelectedItem).Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnteredTitle = null;
            SelectedConsoleID = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

}
