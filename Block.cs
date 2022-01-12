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
    public class Block : Label
    {
        public int[] vel; //0 y, 1 x
        public int x, y, goal_x, goal_y, num;
        private bool stop_flag;

        public Block(int y, int x)
        {
            this.x = x;
            this.y = y;
            this.num = 0;
            stop_flag = true;
            vel = new int[2];
            this.vel[0] = 0;
            this.vel[1] = 0;
            this.Text = "" + this.num;
            this.Size = new Size(Two.block_size, Two.block_size);
            this.set_location();
            this.BackColor = Color.FromArgb(248, 229, 208);
            this.ForeColor = Color.DimGray;
            this.Font = new Font("Serif", 12, FontStyle.Bold);
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Visible = false;
        }
        public void set_num(int num)
        {
            this.num = num;
            this.Text = "" + this.num;
        }
        public void set_location()
        {
            this.Location = new Point(Two.padding + Two.margin * (x + 1) + Two.block_size * x, Two.padding + Two.margin * (y + 2) + Two.block_size * (y + 1));
            goal_x = Two.padding + Two.margin * (x + 1) + Two.block_size * x;
            goal_y = Two.padding + Two.margin * (y + 2) + Two.block_size * (y + 1);
        }
        public void set_goal(int y, int x)
        {
            stop_flag = false;
            this.goal_x = (Two.padding + Two.margin * (x + 1) + Two.block_size * x);
            this.goal_y = (Two.padding + Two.margin * (y + 2) + Two.block_size * (y + 1));
            vel[1] = (this.goal_x - this.Location.X) / Two.interval_num;
            vel[0] = (this.goal_y - this.Location.Y) / Two.interval_num;
        }
        public void level_up()
        {
            this.BringToFront();
            set_num(num * 2);
            switch (num)
            {
                case 2: this.BackColor = Color.FromArgb(248, 229, 208); break;
                case 4: this.BackColor = Color.FromArgb(221, 221, 255); break;
                case 8: this.BackColor = Color.FromArgb(211, 243, 237); break;
                case 16: this.BackColor = Color.FromArgb(158, 221, 208); break;
                case 32: this.BackColor = Color.FromArgb(255, 189, 189); break;
                case 64: this.BackColor = Color.FromArgb(255, 221, 221); break;
                case 128: this.BackColor = Color.FromArgb(187, 209, 231); break;
                case 256: this.BackColor = Color.FromArgb(194, 221, 186); break;
                case 512: this.BackColor = Color.FromArgb(255, 255, 189); break;
                case 1024: this.BackColor = Color.FromArgb(255, 169, 176); break;
                case 2048: this.BackColor = Color.FromArgb(171, 149, 212); break;
                default:
                    if (num > 10000) this.Font = new Font("Serif", 10, FontStyle.Bold);
                    Random rnd = new Random();
                    this.BackColor = Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));
                    break;
            }
        }
        public void added_to(int y, int x)
        {
            num = 0;
            set_goal(y, x);
            this.Visible = true;
            this.Font = new Font("Serif", 12, FontStyle.Bold);
        }
        public bool is_stop()
        {
            if ((goal_x == Location.X) && (goal_y == Location.Y))
            {
                vel[0] = 0;
                vel[1] = 0;
                stop_flag = true;
                if (num == 0)
                {
                    this.Text = "" + this.num;
                    this.Visible = false;
                    this.BackColor = Color.FromArgb(248, 229, 208);
                    set_location();
                }
            }
            return stop_flag;
        }
    }
}