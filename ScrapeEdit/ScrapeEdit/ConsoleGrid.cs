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
    public partial class ConsoleGrid : UserControl
    {
        private TreeNodeDetail node;
        private bool isPort = false;

        public ConsoleGrid(TreeNodeDetail node)
        {
            InitializeComponent();
            this.node = node;
            
            if(Convert.ToInt32(node.Tag_ConsoleID) < 0) //-1 = port
                isPort = true;

            PopulateDisplayArea();
        }

        private void PopulateDisplayArea()
        {
            if (!isPort)
            {
                pb_Console_MainImg.Image = node.Console.MainImg != null ? node.Console.MainImg : null;
                pb_Console_IllustrationImg.Image = node.Console.ConsoleImg != null ? node.Console.ConsoleImg: null;
                pb_Console_Controller.Image = node.Console.ControllerImg != null ? node.Console.ControllerImg : null;
                lbl_Console_Developer.Text += node.Console.Manufacturer;
                lbl_Console_ReleaseDate.Text += node.Console.YearStart.ToString();
                lbl_Console_EoLDate.Text += node.Console.YearEnd.ToString();
                lbl_Console_RomCount.Text += node.Tag_RomCount.ToString();
            }
            //tb_Console_Description.Text = node.Console.des
        }


    }
}
