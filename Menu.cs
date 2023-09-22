﻿using System;
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

        private void buttonOneOnClick(object sender, EventArgs e)
        {
            if (gameFormIsLoad != true) {
                Form1 gameForm = new Form1();
                gameForm.Show();
            }
            
            this.Hide();
        }
    }
}
