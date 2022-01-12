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
    public class Model
    {
        public Block[,] blocks;
        public bool moveable, make_flag;
        public int score;
        private int block_cnt;
        private Random rnd;
        private int[] a, b, start;

        public Model()
        {
            moveable = true;
            make_flag = false;
            score = 0;
            block_cnt = 0;
            rnd = new Random();
            blocks = new Block[Two.block_num, Two.block_num];
            for (int j = 0; j < Two.block_num; j++)
            {
                for (int i = 0; i < Two.block_num; i++)
                {
                    blocks[j, i] = new Block(j, i);
                }
            }
            make_rnd_block();
            make_rnd_block();
        }
        public void make_rnd_block()
        {
            int rnd_num = rnd.Next(0, Two.block_num * Two.block_num - block_cnt);
            int cnt = 0;
            for (int j = 0; j < Two.block_num; j++)
            {
                for (int i = 0; i < Two.block_num; i++)
                {
                    if (blocks[j, i].num == 0)
                    {
                        if (rnd_num == (cnt++))
                        {
                            blocks[j, i].Visible = true;
                            blocks[j, i].set_num(2);
                            block_cnt++;
                        }
                    }
                }
            }
        }
        public void move_block(int[] a, int[] b, int[] start)
        {
            this.a = a;
            this.b = b;
            this.start = start;
            for (int j = 0; j < Two.block_num; j++)
            {
                // shift
                for (int i = Two.block_num - 2; i >= 0; i--)
                {
                    if (blocks[get_index(i, j)[0], get_index(i, j)[1]].num == 0)
                    {
                        shift_block(get_index(i, j)[0], get_index(i, j)[1]);
                    }
                }
                //add
                for (int i = 0; i < Two.block_num - 1; i++)
                {
                    int num = blocks[get_index(i, j)[0], get_index(i, j)[1]].num;
                    if (num != 0 && num == blocks[get_index(i + 1, j)[0], get_index(i + 1, j)[1]].num)
                    {
                        add_block(i, j);
                        score += num;
                    }
                }
            }
        }
        public int[] get_index(int i, int j)
        {
            int[] index = new int[2];
            index[0] = start[0] + a[0] * i + b[0] * j;
            index[1] = start[1] + a[1] * i + b[1] * j;
            return index;
        }
        public void shift_block(int y, int x)
        {
            Block temp = new Block(Two.block_num, Two.block_num);
            int fun_num, shift_a;
            shift_a = a[0] + a[1];
            if (shift_a > 0)
            {
                fun_num = Two.block_num - (y * a[0]) - (x * a[1]) - 1;
            }
            else
            {
                fun_num = ((y * a[0]) + (x * a[1])) * (-1);
            }
            for (int i = 0; i < fun_num; i++)
            {
                int[] index_1 = { y + (a[0] * i), x + (a[1] * i) };
                int[] index_2 = { y + (a[0] * (i + 1)), x + (a[1] * (i + 1)) };
                if ((blocks[index_2[0], index_2[1]].num) > 0)
                {
                    make_flag = true;
                    temp = blocks[index_1[0], index_1[1]];
                    blocks[index_1[0], index_1[1]] = blocks[index_2[0], index_2[1]];
                    blocks[index_2[0], index_2[1]] = temp;

                    blocks[index_1[0], index_1[1]].x = index_1[1];
                    blocks[index_1[0], index_1[1]].y = index_1[0];
                    blocks[index_2[0], index_2[1]].x = index_2[1];
                    blocks[index_2[0], index_2[1]].y = index_2[0];

                    blocks[index_1[0], index_1[1]].set_goal(index_1[0], index_1[1]);
                    blocks[index_2[0], index_2[1]].set_location();
                }
            }
        }
        public void add_block(int i, int j)
        {
            make_flag = true;
            blocks[get_index(i, j)[0], get_index(i, j)[1]].level_up();
            blocks[get_index(i + 1, j)[0],
            get_index(i + 1, j)[1]].added_to(get_index(i, j)[0], get_index(i, j)[1]);
            for (int k = Two.block_num - 2; k >= 0; k--)
            {
                if (blocks[get_index(k, j)[0], get_index(k, j)[1]].num == 0)
                {
                    shift_block(get_index(k, j)[0], get_index(k, j)[1]);
                }
            }
            block_cnt--;
        }
        public void check_over()
        {
            moveable = false;
            for (int j = 0; j < Two.block_num; j++)
            {
                for (int i = 0; i < Two.block_num; i++)
                {
                    int num = blocks[j, i].num;
                    if (i < Two.block_num - 1 && j < Two.block_num - 1)
                    {
                        if (num == blocks[j, i + 1].num || (num * blocks[j, i + 1].num) == 0) moveable = true;
                        else if (num == blocks[j + 1, i].num || (num * blocks[j + 1, i].num) == 0) moveable = true;
                    }
                    else if (j < Two.block_num - 1)
                    {
                        if (num == blocks[j + 1, i].num || (num * blocks[j + 1, i].num) == 0)
                            moveable = true;
                    }
                    else if (i < Two.block_num - 1)
                    {
                        if (num == blocks[j, i + 1].num || (num * blocks[j, i + 1].num) == 0)
                            moveable = true;
                    }
                }
            }
            if (!moveable)
            {
                MessageBox.Show("Game Over\n" + score + " 점\n" + "R 키로 재시작");
                Two.rank.add_score(score);
                Two.save();
                RankView rankView = new RankView();
                rankView.ShowDialog();
            }
        }
    }
}