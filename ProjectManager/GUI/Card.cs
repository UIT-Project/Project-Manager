﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GUI
{
    public partial class Card : UserControl
    {
        //====================================================
        int oX;
        int oY;

        //====================================================

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        public static System.Drawing.Region GetRoundedRegion(int controlWidth, int controlHeight)
        {
            return System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, controlWidth - 2, controlHeight - 2, 10, 10));
        }

        public Card(int X, int Y)
        {
            InitializeComponent();
            oX = X;
            oY = Y;
            this.Region = GetRoundedRegion(this.Width, this.Height);

            this.Location = new Point(oX, oY);
            
        }

        //============================================================

        private void Card_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.DarkGray;
            editButton.Visible = true;
        }

        private void Card_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.White;
            //editButton.Visible = false;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            CardEdit cardEdit = new CardEdit(this.oX, this.oY + this.Location.Y);
            cardEdit.Show();
        }

        private void editButton_MouseEnter(object sender, EventArgs e)
        {
            editButton.BackColor = Color.DarkGray;
        }

        private void editButton_MouseLeave(object sender, EventArgs e)
        {
            editButton.BackColor = Color.White;
        }

        //private void Card_MouseClick(object sender, MouseEventArgs e)
        //{
        //    CardDetail cardDetail = new CardDetail();
        //    cardDetail.Show();
        //}

        //private void Card_MouseClick(object sender, EventArgs e)
        //{
        //    CardDetail cardDetail = new CardDetail();
        //    cardDetail.Show();
        //}

        private void Card_Load(object sender, EventArgs e)
        {
            CardDetail cardDetail = new CardDetail();
            cardDetail.Show();
        }
    }
}
