using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Json;

class Two{
	public static int block_num = 4;
	public static int block_size = 45;
	public static int margin = 5;
	public static int padding = 15;
	public static int form_height = margin * (block_num + 1) + block_size * block_num + margin * 2 + block_size + padding * 3;
	public static int form_length = margin * (block_num + 1) + block_size * block_num + padding * 2 + 4;
	public static int interval_num = 10;
	private static string file_name = block_num + @"database.txt";

	public static Rank rank;

	public static int Main() {
		if (File.Exists(file_name)) {
			load();
		}else{
			rank = new Rank();
			save();
		}
		Console.WriteLine(form_height + "," + form_length);
		Application.Run(new View());
		return 0;
	}
	public static void load() {
		DataContractJsonSerializer dc = new DataContractJsonSerializer(typeof(Rank));
		using (FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)) {
			rank = dc.ReadObject(fs) as Rank;
		}
		Console.WriteLine("load");
	}
	public static void save () {
		DataContractJsonSerializer dc = new DataContractJsonSerializer(typeof(Rank));
		using (FileStream fs = new FileStream(file_name, FileMode.Create, FileAccess.ReadWrite, FileShare.None)) {
			dc.WriteObject(fs, rank) ;
		}
		Console.WriteLine("save");
	}
}

[Serializable]
public class Rank{
	public int length = 9;
	public int[] scores;
	public Rank () {
		scores = new int[length];
	}
	public void add_score(int score) {
		for(int i = 0; i < length; i++) {
			if(scores[i] < score) {
				change_rank (i);
				scores[i] = score;
				break;
			}
		}
	}
	public void change_rank (int index) {
		for(int i = length-2 ; i >= index; i--) {
			scores [i+1] = scores[i];
		}
	}
}
public class RankView : Form {
	private const int formHeight = 200;
	private const int formLength = 150;
	private ListBox rankBox;

	public RankView () {
		rankBox = new ListBox();
		InitializeComponent();
	}
	private void InitializeComponent() {
		rankBox.Size = new Size (formLength, formHeight);
		rankBox.Location = new Point(0, 0);
		rankBox.Font = new Font ("Serif", 10);
		rankBox.KeyDown += new KeyEventHandler (this.keyDown);
		
		for(int i = 0; i < Two.rank.length; i++) {
			rankBox.Items.Add((i+1) + "등 " + Two.rank.scores[i] + " ");
		}
		
		this.Controls.Add(rankBox);
		this.Text = "당신의 점수는???";
		this.Size = new Size (formLength, formHeight);
		this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
	}
	private void keyDown (object sender, KeyEventArgs e) {
		if (e.KeyCode == Keys.Escape) {
			this.Close();
		}
	}
}

public class View : Form{
	private bool move_flag;
	private Model model;
	private Label back_label, score_label;
	private Timer timer;

	public View (){
		model = new Model();
		move_flag = false;
		back_label = new Label();
		score_label = new Label();
		InitializeComponent();
	}
	private void InitializeComponent(){
		back_label.Size = new Size(Two.block_size * Two.block_num + Two.margin * (Two.block_num-1), Two.block_size * Two.block_num + Two.margin * (Two.block_num-1));
		back_label.Location = new Point(Two.padding + Two.margin, Two.block_size + Two.margin * 2 + Two.padding);
		back_label.BackColor = Color.LightGray;
		
		score_label.Size = new Size(Two.block_size * Two.block_num + Two.margin * (Two.block_num-1), 40);
		score_label.Location = new Point(Two.padding + Two.margin, Two.padding + Two.margin);
		score_label.BackColor = Color.SteelBlue;
		score_label.ForeColor = Color.White;
		score_label.Font = new Font("Serif", 10, FontStyle.Bold);
		score_label.TextAlign = ContentAlignment.MiddleCenter;
		score_label.Text = "점수 : " + model.score + "점";
		for(int j = 0; j < Two.block_num; j++) {
			for(int i = 0; i < Two.block_num; i++) {
				this.Controls.Add (model.blocks [j,i]);
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
	private void timer_diplay(object sender, EventArgs e) {
		int[] vel = {0,0};
		int cnt = 0;
		if (move_flag) {
			for(int j = 0; j < Two.block_num; j++){
				for(int i = 0; i < Two.block_num; i++) {
					if (!model.blocks[j,i].is_stop()) {
						cnt++;
						vel[1] = model.blocks[j,i].vel[1];
						vel[0] = model.blocks[j,i].vel[0];
						model.blocks[j,i].Location = new Point(model.blocks[j,i].Location.X + vel[1], model.blocks[j,i].Location.Y + vel[0]);
					}
				}
			}
			if (cnt == 0) {
				score_label.Text = " 점수 : " + model.score + "점";
				move_flag = false;
				if (model.make_flag) {
					model.make_rnd_block();
					model.make_flag = false;
				}else{
					model.check_over();
				}
			}
		}
	}
	private void keyDown (object sender, KeyEventArgs e) {
		if (!move_flag) {
			int[] a = new int[2];
			int[] b = new int[2];
			int[] start = new int[2];
			switch(e.KeyCode) {
				case Keys.Up:
					a[0] = 1; a[1] = 0;
					b[0] = 0; b[1] = 1;
					start[0] = 0; start[1] = 0;
					move_flag = true;
					model.move_block(a, b, start);
					break;
				case Keys.Down:
					a[0] = -1; a [1] = 0;
					b[0] = 0; b[1] = -1;
					start[0] = Two.block_num-1; start[1] = Two.block_num-1;
					move_flag = true;
					model.move_block (a, b, start);
					break;
				case Keys.Right:
					a[0] = 0; a [1] = -1;
					b[0] = -1; b[1] = 0;
					start[0] = Two.block_num-1; start[1] = Two.block_num-1;
					move_flag = true;
					model.move_block(a, b, start);
					break;
				case Keys.Left:
					a[0] = 0; a [1] = 1;
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
	public void restart() {
		for(int j = 0; j < Two.block_num; j++) {
			for(int i = 0; i < Two.block_num; i++) {
				this.Controls.Remove (model.blocks[j,i]);
			}
		}
		model = new Model();
		score_label.Text = "점수 : " + model.score + "점";
		for(int j = 0; j < Two.block_num; j++) {
			for(int i = 0; i < Two.block_num; i++) {
				this.Controls.Add(model.blocks[j,i]);
				model.blocks[j,i].BringToFront();
			}
		}
	}
}
public class Model{
	public Block[,] blocks;
	public bool moveable, make_flag;
	public int score;
	private int block_cnt;
	private Random rnd;
	private int[]a, b, start;

	public Model() {
		moveable = true;
		make_flag = false;
		score = 0;
		block_cnt = 0;
		rnd = new Random();
		blocks = new Block[Two.block_num, Two.block_num];
		for(int j = 0; j < Two.block_num; j++) {
			for(int i = 0; i < Two.block_num; i++) {
				blocks[j,i] = new Block (j, i);
			}
		}
		make_rnd_block();
		make_rnd_block();
	}
	public void make_rnd_block(){
		int rnd_num = rnd.Next(0, Two.block_num * Two.block_num - block_cnt);
		int cnt = 0;
		for(int j = 0; j < Two.block_num; j++) {
			for(int i = 0; i < Two.block_num; i++) {
				if (blocks[j,i].num == 0) {
					if(rnd_num == (cnt++)) {
						blocks[j,i].Visible = true;
						blocks[j,i].set_num(2);
						block_cnt++;
					}
				}
			}
		}
	}
	public void move_block (int[]a, int[]b, int[]start) {
		this.a = a;
		this.b = b;
		this.start = start;
		for(int j = 0; j < Two.block_num; j++) {
			// shift
			for(int i = Two.block_num-2; i >= 0; i--) {
				if (blocks[get_index(i,j)[0],get_index(i, j)[1]].num == 0) {
					shift_block(get_index(i,j)[0],get_index(i,j)[1]);
				}
			}
			//add
			for(int i = 0; i < Two.block_num-1; i++) {
				int num = blocks[get_index(i,j)[0], get_index(i,j)[1]].num;
				if (num != 0 && num == blocks[get_index(i+1,j)[0],get_index(i+1,j) [1]].num) {
					add_block(i, j);
					score += num;
				}
			}
		}
	}
	public int[] get_index(int i, int j){
		int[] index = new int[2];
		index[0] = start[0] + a[0]*i + b[0]*j;
		index[1] = start[1] + a[1]*i + b[1]*j;
		return index;
	}
	public void shift_block(int y, int x){
		Block temp = new Block(Two.block_num, Two.block_num);
		int fun_num, shift_a;
		shift_a = a[0] + a[1];
		if(shift_a > 0) {
			fun_num = Two.block_num - (y * a [0]) - (x * a [1]) - 1;
		} else{
			fun_num = ((y * a[0]) + (x * a[1])) * (-1);
		}
		for(int i = 0; i < fun_num; i++) {
			int[] index_1 = {y + (a[0] * i),x + (a[1] * i)};
			int[] index_2 = {y + (a[0] * (i+1)),x + (a[1] * (i+1))};
			if ((blocks[index_2[0], index_2[1]].num) > 0) {
				make_flag = true;
				temp = blocks[index_1[0], index_1[1]];
				blocks[index_1[0], index_1[1]] = blocks [index_2[0], index_2[1]];
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
	public void add_block(int i, int j) {
		make_flag = true;
		blocks[get_index(i,j)[0], get_index(i,j)[1]].level_up();
		blocks[get_index (i+1,j)[0],
		get_index(i+1,j)[1]].added_to(get_index(i,j)[0], get_index(i,j) [1]);
		for(int k = Two.block_num-2; k >= 0; k--) {
			if(blocks[get_index(k,j)[0], get_index(k, j)[1]].num == 0) {
				shift_block(get_index(k,j)[0],get_index(k,j)[1]);
			}
		}
		block_cnt--;
	}
	public void check_over(){
		moveable = false;
		for(int j = 0; j < Two.block_num; j++) {
			for(int i = 0; i < Two.block_num; i++) {
				int num = blocks[j,i].num;
				if(i < Two.block_num-1 && j < Two.block_num-1){
					if(num == blocks[j,i+1].num || (num * blocks[j,i+1].num) == 0) moveable = true;
					else if (num == blocks [j+1,i].num || (num * blocks [j+1,i].num) == 0) moveable = true;
				}else if(j < Two.block_num-1){
					if(num==blocks[j+1,i].num || (num * blocks[j+1,i].num) == 0)
						moveable = true;
				}else if(i < Two.block_num-1) {
					if(num==blocks[j,i+1].num || (num * blocks[j,i+1].num) == 0)
						moveable = true;
				}
			}
		}
		if(!moveable){
			MessageBox.Show("Game Over\n" + score + " 점\n" + "R 키로 재시작");
			Two.rank.add_score(score) ;
			Two.save();
			RankView rankView = new RankView();
			rankView.ShowDialog();
		}	
	}
}
public class Block : Label{
	public int[] vel; //0 y, 1 x
	public int x, y, goal_x, goal_y, num;
	private bool stop_flag;
	
	public Block(int y, int x){
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
		this.BackColor = Color.FromArgb(248,229,208);
		this.ForeColor = Color.DimGray;
		this.Font = new Font("Serif", 12, FontStyle.Bold);
		this.TextAlign = ContentAlignment.MiddleCenter;
		this.Visible = false;
	}
	public void set_num(int num) {
		this.num = num;
		this.Text = ""+this.num;
	}
	public void set_location (){
		this.Location = new Point(Two.padding + Two.margin * (x + 1) + Two.block_size * x, Two.padding + Two.margin * (y + 2) + Two.block_size * (y + 1));
		goal_x = Two.padding + Two.margin * (x + 1) + Two.block_size * x;
		goal_y = Two.padding + Two.margin * (y + 2) + Two.block_size * (y + 1);
	}
	public void set_goal (int y, int x) {
		stop_flag = false;
		this.goal_x = (Two.padding + Two.margin * (x + 1) + Two.block_size * x);
		this.goal_y = (Two.padding + Two.margin * (y + 2) + Two.block_size * (y + 1));
		vel[1] = (this.goal_x - this.Location.X) / Two.interval_num;
		vel[0] = (this.goal_y - this.Location.Y) / Two.interval_num;
	}
	public void level_up() {
		this.BringToFront();
		set_num(num*2);
		switch(num){
			case 2: 	this.BackColor = Color.FromArgb(248,229,208); break;
			case 4: 	this.BackColor = Color.FromArgb(221,221,255); break;
			case 8: 	this.BackColor = Color.FromArgb(211,243,237); break;
			case 16:	this.BackColor = Color.FromArgb(158,221,208); break;
			case 32:	this.BackColor = Color.FromArgb(255,189,189); break;
			case 64:	this.BackColor = Color.FromArgb(255,221,221); break;
			case 128:	this.BackColor = Color.FromArgb(187,209,231); break;
			case 256:	this.BackColor = Color.FromArgb(194,221,186); break;
			case 512: 	this.BackColor = Color.FromArgb(255,255,189); break;
			case 1024: 	this.BackColor = Color.FromArgb(255,169,176); break;
			case 2048: 	this.BackColor = Color.FromArgb(171,149,212); break;
			default :
				if(num > 10000) this.Font = new Font ("Serif", 10, FontStyle.Bold);
				Random rnd = new Random();
				this.BackColor = Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));
				break;
		}
	}
	public void added_to(int y, int x){
		num = 0;
		set_goal(y, x);
		this.Visible = true;
		this.Font = new Font("Serif", 12, FontStyle.Bold);
	}	
	public bool is_stop(){
		if((goal_x == Location.X) && (goal_y == Location.Y)) {
			vel[0] = 0;
			vel[1] = 0;
			stop_flag = true;
			if (num == 0) {
				this.Text = "" + this.num;
				this.Visible = false;
				this.BackColor = Color.FromArgb(248,229,208);
				set_location();
			}
		}
		return stop_flag;
	}
}