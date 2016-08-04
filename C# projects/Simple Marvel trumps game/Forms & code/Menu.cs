using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace S.Dassiemnt2
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

    

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {//hide current menu and show playform       

                Playform f = new Playform();
                this.Hide();
                f.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            {//hide current menu and show howToPlay form       

                howToPlay f = new howToPlay();
                this.Hide();
                f.ShowDialog();
            }

        }
    }
}
