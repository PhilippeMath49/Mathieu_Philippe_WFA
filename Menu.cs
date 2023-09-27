using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA
{
    public partial class Menu : Form
    {
        private Form1 gameForm;
        private bool gameFormIsLoad; 
        public Menu()
        {
            InitializeComponent();
        }
        //lance le jeu quand on clique sur le bouton
        private void buttonOneOnClick(object sender, EventArgs e)
        {
            //initialise un nouveau niveau avant de le faire apparaitre et cacher le menu
            Form1 gameForm = new Form1();
            gameForm.Show();
            this.Hide();
        }
    }
}
