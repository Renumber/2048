using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2048
{
    public class RankView : Form
    {
        private const int formHeight = 200;
        private const int formLength = 150;
        private ListBox rankBox;

        public RankView()
        {
            rankBox = new ListBox();
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            rankBox.Size = new Size(formLength, formHeight);
            rankBox.Location = new Point(0, 0);
            rankBox.Font = new Font("Serif", 10);
            rankBox.KeyDown += new KeyEventHandler(this.keyDown);

            for (int i = 0; i < Two.rank.length; i++)
            {
                rankBox.Items.Add((i + 1) + "등 " + Two.rank.scores[i] + " ");
            }

            this.Controls.Add(rankBox);
            this.Text = "당신의 점수는???";
            this.Size = new Size(formLength, formHeight);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}