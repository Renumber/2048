using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048
{
    public class View : Form
    {
        private bool move_flag;
        private Model model;
        private Label back_label, score_label;
        private Timer timer;

        public View()
        {
            model = new Model();
            move_flag = false;
            back_label = new Label();
            score_label = new Label();
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            back_label.Size = new Size(Two.block_size * Two.block_num + Two.margin * (Two.block_num - 1), Two.block_size * Two.block_num + Two.margin * (Two.block_num - 1));
            back_label.Location = new Point(Two.padding + Two.margin, Two.block_size + Two.margin * 2 + Two.padding);
            back_label.BackColor = Color.LightGray;

            score_label.Size = new Size(Two.block_size * Two.block_num + Two.margin * (Two.block_num - 1), 40);
            score_label.Location = new Point(Two.padding + Two.margin, Two.padding + Two.margin);
            score_label.BackColor = Color.SteelBlue;
            score_label.ForeColor = Color.White;
            score_label.Font = new Font("Serif", 10, FontStyle.Bold);
            score_label.TextAlign = ContentAlignment.MiddleCenter;
            score_label.Text = "점수 : " + model.score + "점";
            for (int j = 0; j < Two.block_num; j++)
            {
                for (int i = 0; i < Two.block_num; i++)
                {
                    this.Controls.Add(model.blocks[j, i]);
                }
            }
            this.Controls.Add(back_label);
            this.Controls.Add(score_label);

            this.Size = new Size(Two.form_length, Two.form_height);
            this.Opacity = 0.9;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyDown += new KeyEventHandler(this.keyDown);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = Two.interval_num;
            timer.Tick += new EventHandler(timer_diplay);
            timer.Start();
        }
        private void timer_diplay(object sender, EventArgs e)
        {
            int[] vel = { 0, 0 };
            int cnt = 0;
            if (move_flag)
            {
                for (int j = 0; j < Two.block_num; j++)
                {
                    for (int i = 0; i < Two.block_num; i++)
                    {
                        if (!model.blocks[j, i].is_stop())
                        {
                            cnt++;
                            vel[1] = model.blocks[j, i].vel[1];
                            vel[0] = model.blocks[j, i].vel[0];
                            model.blocks[j, i].Location = new Point(model.blocks[j, i].Location.X + vel[1], model.blocks[j, i].Location.Y + vel[0]);
                        }
                    }
                }
                if (cnt == 0)
                {
                    score_label.Text = " 점수 : " + model.score + "점";
                    move_flag = false;
                    if (model.make_flag)
                    {
                        model.make_rnd_block();
                        model.make_flag = false;
                    }
                    else
                    {
                        model.check_over();
                    }
                }
            }
        }
        private void keyDown(object sender, KeyEventArgs e)
        {
            if (!move_flag)
            {
                int[] a = new int[2];
                int[] b = new int[2];
                int[] start = new int[2];
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        a[0] = 1; a[1] = 0;
                        b[0] = 0; b[1] = 1;
                        start[0] = 0; start[1] = 0;
                        move_flag = true;
                        model.move_block(a, b, start);
                        break;
                    case Keys.Down:
                        a[0] = -1; a[1] = 0;
                        b[0] = 0; b[1] = -1;
                        start[0] = Two.block_num - 1; start[1] = Two.block_num - 1;
                        move_flag = true;
                        model.move_block(a, b, start);
                        break;
                    case Keys.Right:
                        a[0] = 0; a[1] = -1;
                        b[0] = -1; b[1] = 0;
                        start[0] = Two.block_num - 1; start[1] = Two.block_num - 1;
                        move_flag = true;
                        model.move_block(a, b, start);
                        break;
                    case Keys.Left:
                        a[0] = 0; a[1] = 1;
                        b[0] = 1; b[1] = 0;
                        start[0] = 0; start[1] = 0;
                        move_flag = true;
                        model.move_block(a, b, start);
                        break;
                    case Keys.Escape:
                        this.Close();
                        break;
                    case Keys.R:
                        restart();
                        break;
                    case Keys.F1:
                        RankView rankView = new RankView();
                        rankView.ShowDialog();
                        break;
                    default:
                        break;
                }
            }
        }
        public void restart()
        {
            for (int j = 0; j < Two.block_num; j++)
            {
                for (int i = 0; i < Two.block_num; i++)
                {
                    this.Controls.Remove(model.blocks[j, i]);
                }
            }
            model = new Model();
            score_label.Text = "점수 : " + model.score + "점";
            for (int j = 0; j < Two.block_num; j++)
            {
                for (int i = 0; i < Two.block_num; i++)
                {
                    this.Controls.Add(model.blocks[j, i]);
                    model.blocks[j, i].BringToFront();
                }
            }
        }
    }
}
