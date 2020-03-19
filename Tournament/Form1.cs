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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Turnuva kupa;

        //Kontrol hareket ettirme fonksiyonları baslangıç

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

        //bitiş

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int takimSay = int.Parse(textBox1.Text);
                int randomSeed = int.Parse(textBox2.Text);

                if (takimSay < 0 || randomSeed < 0) throw new negatifSayiHatasi("Negatif sayi girilemez");
                if (takimSay != 32 && takimSay != 64 && takimSay != 128) throw new takimSayisiHatasi("Sayı , turnuvanın eleme usulüyle ilerlemesinden dolayı 2^n formatında olmalıdır.\n\n n = [5,7]");

                Turnuva TınaztepeCup = new Turnuva(takimSay, randomSeed);//Program ayaklanır

                kupa = TınaztepeCup;

                //Arayüz düzenleme ayarları

                Turnuva.PuanTablosuBastir(TınaztepeCup, dataGridView1);

                textBox1.Visible = textBox2.Visible = label1.Visible = label2.Visible = pictureBox1.Visible = false;
                this.Size = new System.Drawing.Size(787, 623);
                panel1.Visible = panel2.Visible = panel3.Visible = panel5.Visible = true;
                pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + 15);

                dataGridView2.Columns.Add("Gruplar", "Gruplar");
                dataGridView2.Columns["Gruplar"].SortMode = DataGridViewColumnSortMode.NotSortable;
                for (int i = 0; i < kupa.Takımlar.Count() / 8; i++)
                {
                    dataGridView2.Rows.Add("Grup" + (i + 1));
                }
                dataGridView2.Rows.Add("Final");

                dataGridView3.Columns.Add("Ev Sahibi", "Ev Sahibi");
                dataGridView3.Columns.Add("E", "E");
                dataGridView3.Columns.Add("D", "D");
                dataGridView3.Columns.Add("Deplasman", "Deplasman");
            }
            catch (takimSayisiHatasi error)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                MessageBox.Show(error.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (negatifSayiHatasi error)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                MessageBox.Show(error.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FormatException error)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                MessageBox.Show(error.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception error)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                MessageBox.Show(error.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Slider buttonları ve ayarları
        private void button1_Click_1(object sender, EventArgs e)
        {
            panel4.Location = button1.Location;
            panel5.Visible = dataGridView1.Visible = true;
            label5.Visible = pictureBox6.Visible = textBox3.Visible = dataGridView2.Visible = textBox4.Visible = dataGridView3.Visible = label7.Visible = pictureBox7.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label5.Visible = pictureBox6.Visible = textBox3.Visible = true;
            panel4.Location = button2.Location;
            dataGridView2.Visible = dataGridView1.Visible = textBox4.Visible = dataGridView3.Visible = label7.Visible = pictureBox7.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel4.Location = button3.Location;
            textBox4.Visible = label7.Visible = pictureBox7.Visible = true;
            label5.Visible = pictureBox6.Visible = textBox3.Visible = dataGridView2.Visible = dataGridView1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = true;

            panel4.Location = button4.Location;
            label5.Visible = pictureBox6.Visible = textBox3.Visible = dataGridView1.Visible = textBox4.Visible = dataGridView3.Visible = label7.Visible = pictureBox7.Visible = false;
        }

        // Programdan çıkış buttonu
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Kazanan Ekranı buttonu
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            MiniForm KazananEkranı = new MiniForm(kupa, this);
            KazananEkranı.Show();
            panel5.Enabled = false;
            pictureBox5.Image = Resource1.grayMedal;

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)//Eğer row,column indexi negtaif sayi değilse çalışır bu sayede headerlar kullanımdışı bırakılmış olur
            {
                if (e.RowIndex < kupa.Takımlar.Count() / 8)
                {
                    int istenenGrup = e.RowIndex;
                    MiniForm yeni = new MiniForm(kupa, this, istenenGrup, 8);
                    dataGridView2.Enabled = false;
                    yeni.Show();
                }
                else
                {
                    int istenenGrup = e.RowIndex;
                    MiniForm yeni = new MiniForm(kupa, this, istenenGrup, kupa.Takımlar.Count() / 8);
                    dataGridView2.Enabled = false;
                    yeni.Show();
                }
            }
        }

        //Bilgi buttonu

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kazanan madalyasına basarak,turnuvanın kazanan bilgilerini ekranda görebilirsiniz.\n\nTakımlar sekmesinde istediğiniz takımın adını Takım[x] şeklinde girerek,o takımla ilgili bilgileri görebilirsiniz.([x]=takım no)\n\nTurlar sekmesinde istediğiniz turun numarasını girerek,o turda oynanan tüm maç bilgilerini görebilirsiniz.\n\nGruplar sekmesinde bulunan tablodan istediğiniz grubun ismine tıklayarak o grubun turnuva fikstürünü(chartını) görebilirsiniz ve karşınıza çıkacak chart üzerinde herhangi bir takım isminin üzerine gelirseniz,açılacak tooltipte o takımın hangi turun,kaçıncı oyununun kazananı olduğu bilgisi de yer almaktadır.", "Kullanım Klavuzu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //İstenilen turu bastirmak için kullanıcıdan girdi alan arayüz

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            dataGridView3.Visible = true;
            try
            {
                if (Int32.TryParse(textBox4.Text, out int result))
                {
                    int tur = Convert.ToInt32(textBox4.Text);
                    if (tur > (int)Math.Log(kupa.Takımlar.Count(), 2) || tur <= 0)
                    {
                        throw new turHatasi("Girdiğiniz değer varolan tur değerinden yüksek veya düşüktü.");
                    }
                    else
                    {
                        tur--;
                        Turnuva.turBastir(kupa, dataGridView3, tur);
                    }
                }
                else throw new turHatasi("Sayisal bir değer giriniz.");
            }
            catch (turHatasi error)
            {
                MessageBox.Show(error.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                textBox4.Text = "";
            }
        }

        //İstenilen takım bilgilerini getirmek için kullanıcıdan girdi alan arayüz

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            try
            {
                int flag = 0;
                for (int i = 0; i < kupa.Takımlar.Count(); i++)
                {
                    if (kupa.Takımlar[i].takım_isim == textBox3.Text)
                    {
                        MiniForm yeni = new MiniForm(kupa, this, textBox3.Text);
                        yeni.Show();
                        pictureBox6.Enabled = false;
                        textBox3.Enabled = false;
                        label5.Enabled = false;
                        flag = 1;
                        break;
                    }
                }
                if (flag == 0) throw new takimIsmiHatasi("Girilen isimde bir takim bulunamadi.");
            }
            catch (takimIsmiHatasi error)
            {
                MessageBox.Show(error.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                textBox3.Text = "";
            }
        }



    }
}
