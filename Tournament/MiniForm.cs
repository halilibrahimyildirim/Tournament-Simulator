using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tournament
{
    public partial class MiniForm : Form
    {
        private Form1 papa;//Kendisini çağıran form

        public MiniForm()
        {
            InitializeComponent();
        }

        internal MiniForm(Turnuva tournament, Form1 ebeveyn)//Kazanan takımı takımı gösteren form constructor
        {
            this.papa = ebeveyn;
            InitializeComponent();
            Control[] c = new Control[6];
            c[0] = label4;//kazanan
            c[1] = label6;//atılan gol
            c[2] = label5;//yenilen gol
            c[3] = label1;//averaj
            c[4] = label2;//pr
            c[5] = dataGridView1;
            Turnuva.takımBastir(tournament, c, tournament.Kazanan.takım_isim);
        }

        internal MiniForm(Turnuva tournament, Form1 ebeveyn, string istenen)//İstenilen takımı gösteren arayüz form constructor
        {
            this.papa = ebeveyn;
            InitializeComponent();
            Control[] c = new Control[6];
            c[0] = label4;//kazanan
            c[1] = label6;//atılan gol
            c[2] = label5;//yenilen gol
            c[3] = label1;//averaj
            c[4] = label2;//pr
            c[5] = dataGridView1;
            pictureBox2.Click -= pictureBox2_Click;
            pictureBox2.Click += takım_pictureBox2_Click;
            Turnuva.takımBastir(tournament, c, istenen);
        }

        internal MiniForm(Turnuva tournament, Form1 ebeveyn, int istenen, int takımSay)//Turnuva chartını gösteren arayüz form constructor
        {
            Control[] c = new Control[(takımSay*2)-1];// (takımSay*2)-1 Girilen takım sayısından oluşan control sayısı

            ToolTip tip = new ToolTip();
            int orjinalDeger = takımSay;
            this.papa = ebeveyn;
            InitializeComponent();

            this.Controls.Remove(label4);//sahip olduğu gereksiz controlleri kaldırdık
            this.Controls.Remove(label6);
            this.Controls.Remove(label5);
            this.Controls.Remove(label1);
            this.Controls.Remove(label2);
            this.Controls.Remove(dataGridView1);
            
            //Düzenli chart oluşturan algoritma

            this.Height = 150 + (40*takımSay);
            Point baslangicNoktasi = new Point(25, 120);
            int y_artis = 40;
            int tepeBosluk = 20;
            int yatayBosluk = 100;
            int base1 = 25;
            int base2 = 120;
            int sayac = 0;
            int turSayac = 0;
            while(takımSay!=0)
            { 
                for (int i = 0; i < takımSay; i++)
                {
                    Button b = new Button();
                    b.FlatStyle = FlatStyle.Flat;
                    b.BackColor = Color.FromArgb(18, 61, 10);
                    b.Location = baslangicNoktasi;
                    baslangicNoktasi.Y += y_artis;
                    b.ForeColor = Color.FromArgb(242, 214, 70);
                    if (istenen < tournament.Takımlar.Count() / 8)
                    {
                        if (takımSay != orjinalDeger) tip.SetToolTip(b, turSayac + ". Tur " + ((i + 1) + (istenen * takımSay)) + ". Oyun Galibi"); //Tooltip içeriği
                    }
                    else
                    {
                            tip.SetToolTip(b, turSayac+3 + ". Tur " + (i + 1)  + ". Oyun Galibi"); //Tooltip içeriği
                    }
                    
                    this.Controls.Add(b);
                    c[sayac] = b;
                    sayac++;
                }
                y_artis *= 2;
                base2 += tepeBosluk;
                base1 += yatayBosluk;
                baslangicNoktasi = new Point(base1, base2);
                yatayBosluk += 8;
                tepeBosluk *= 2;
                takımSay /= 2;
                turSayac++;
            }
            this.Width = 150 + (100 *turSayac);
            panel1.Width = this.Width;
            pictureBox2.Click -= pictureBox2_Click;
            pictureBox2.Click += grup_pictureBox2_Click;

            Turnuva.GrupBastir(tournament, c, istenen);
        }

        private void pictureBox2_Click(object sender, EventArgs e)//Kazanan ekranından çıkmayı sağlayan x click fonskiyonu
        {
            this.Close();
            foreach (Control c in papa.Controls)
            {
                if (c.Name == "panel5")
                {
                    c.Enabled = true;
                    foreach (Control k in c.Controls)
                    {
                        if (k.Name == "pictureBox5")
                        {
                            ((PictureBox)k).Image = Resource1.Kazanan;
                        }
                    }
                }
            }

        }

        private void grup_pictureBox2_Click(object sender, EventArgs e)//Grup ekranından çıkmayı sağlayan x click fonskiyonu
        {
            this.Close();
            foreach (Control c in papa.Controls)
            {
                if (c.Name == "dataGridView2") c.Enabled = true;
            }

        }

        private void takım_pictureBox2_Click(object sender, EventArgs e)//Takım ekranından çıkmayı sağlayan x click fonskiyonu
        {
            this.Close();
            foreach (Control c in papa.Controls)
            {
                if (c.Name == "pictureBox6") c.Enabled = true;
                else if (c.Name == "textBox3") c.Enabled = true;
                else if (c.Name == "label5") c.Enabled = true;
            }

        }

        //Kontrol hareket ettirme fonksiyonları


        int hareketEttir = 0;
        int x, y;
        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                hareketEttir = 0;
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (hareketEttir == 1)
            {
                this.Location = new Point(System.Windows.Forms.Cursor.Position.X - x, System.Windows.Forms.Cursor.Position.Y - y);
            }
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = System.Windows.Forms.Cursor.Position.X - this.Location.X;
                y = System.Windows.Forms.Cursor.Position.Y - this.Location.Y;
                hareketEttir = 1;
            }
        }

    }
}
