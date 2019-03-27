﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class List : Form
    {
        public List()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void AddCard_Click(object sender, EventArgs e)
        {
            Card card = new Card(this.Location.X + this.Width, this.Location.Y);
            flowLayoutPanel1.Controls.Add(card);
            flowLayoutPanel1.Height += card.Height;
            this.Height += card.Height;
            AddCard.Location = new Point(AddCard.Location.X, AddCard.Location.Y + card.Height);
        }

        private void List_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
