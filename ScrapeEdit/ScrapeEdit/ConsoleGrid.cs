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
        private bool isRoot = false;

        public ConsoleGrid(TreeNodeDetail node)
        {
            InitializeComponent();
            this.node = node;
            
            if(Convert.ToInt32(node.Tag_ConsoleID) < 0) //-1 = port
                isPort = true;
            if (node.Parent == null)
                isRoot = true;

            PopulateDisplayArea();
        }

        private void PopulateDisplayArea()
        {
            if (!isPort && node.Console != null)
            {
                pb_Console_MainImg.Image = node.Console.MainImg != null ? node.Console.MainImg : null;
                pb_Console_IllustrationImg.Image = node.Console.ConsoleImg != null ? node.Console.ConsoleImg: null;
                pb_Console_Controller.Image = node.Console.ControllerImg != null ? node.Console.ControllerImg : null;
                lbl_Console_Developer.Text += node.Console.Manufacturer;
                lbl_Console_ReleaseDate.Text += node.Console.YearStart.ToString();
                lbl_Console_RomCount.Text += node.Tag_RomCount.ToString();

                if (isRoot)
                { 
                    lbl_Console_EoLDate.Text = "";
                    lbl_Console_RomCount.Text = "https://github.com/zerikscythe/Scrape-Edit";
                    lbl_Console_RomCount.TextAlign = ContentAlignment.MiddleLeft;
                    lbl_Console_RomCount.Font = new Font(lbl_Console_RomCount.Font.FontFamily, 12, FontStyle.Underline);
                    lbl_Console_RomCount.Location = lbl_Console_RomCount.Location with { Y = lbl_Console_RomCount.Location.Y - 25,
                    X = lbl_Console_RomCount.Location.X - 100};
                    lbl_Console_RomCount.ForeColor = Color.Blue;
                }
                else
                    lbl_Console_EoLDate.Text += node.Console.YearEnd.ToString();
                
            }

        }
    }
}
