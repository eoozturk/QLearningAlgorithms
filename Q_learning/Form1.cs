using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;


namespace Yazlab_Q_learning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                openFileDialog1.FileName = "Okunacak Dosyayı Seçiniz.";
                openFileDialog1.Filter = "Yazı Dosyaları (*txt)|*.txt";
                openFileDialog1.ShowDialog();

                StreamReader okumaislemi = new StreamReader(openFileDialog1.FileName);
                string satir=okumaislemi.ReadLine();

                for (i=0;satir!=null;i++) 
                {
                    listBox1.Items.Add(satir);
                    satir=okumaislemi.ReadLine();
                }
                
                okumaislemi.Close();

            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
          
        }


        ArrayList yoldizi=new ArrayList();
        int timer = 1;
        private void button2_Click(object sender, EventArgs e)
        {
            int row = listBox1.Items.Count;
            int[,] R = new int[row, row];
            double[,] Q = new double[row, row];
            for (int k = 0; k < row; k++)
            {
                for (int l = 0; l < row; l++)
                {
                    R[k, l] = -1;
                    Q[k, l] = 0.0;
                }
            }

            for (int s = 0; s < row; s++)
            {
                string[] parcala = listBox1.Items[s].ToString().Split(',');
                for (int j = 0; j < parcala.Length; j++)
                {
                    if (Convert.ToInt32(parcala[j]) == Convert.ToInt32(Txthedef.Text))
                        R[s, Convert.ToInt32(parcala[j])] = 100;                          
                    else
                        R[s, Convert.ToInt32(parcala[j])] = 0;
                }
            }
            R[Convert.ToInt32(Txthedef.Text),Convert.ToInt32(Txthedef.Text)] = 100;
            
            // R MATRİSİ
            for (int k = 0; k < row; k++)
            {
                for (int l = 0; l < row; l++)
                {
                    textBox1.AppendText(R[k, l] + "  ");
                }
                textBox1.AppendText(Environment.NewLine);
            }

            int iterasyon= Convert.ToInt32(textIterasyon.Text);
            int baslangic= Convert.ToInt32(textBaslangic.Text);
            int durum;
            int g;
            ArrayList c = new ArrayList();
            int x;
            Random rnd=new Random();
            int act = 0;
            int hedef = Convert.ToInt32(Txthedef.Text);
           for (int k = 0; k < 3000; k++)
            {
                durum = rnd.Next(row);
              while(durum!=hedef)
              {
                  c.Clear();
                  for ( g = 0; g < row ; g++)
			        {
                         if (R[durum, g] != -1)
                            {
                                c.Add(g); 
                            }
                    }
                     x = rnd.Next(c.Count);
                     act =Convert.ToInt32(c[x]);
                    // Q maximum
                     double maxx = -1;
                         for (g = 0; g < row; g++)
                         {
                             if (R[act, g] != -1)
                             {
                                 if (maxx < Q[act, g])
                                 {
                                     maxx = Q[act, g];
                                 }
                             }
                         }
                    
                  // Q maximum
                  Q[durum, act] = R[durum,act]+ 0.8 * maxx;
                  durum=act;
          }
      }

            // Q MATRİSİ YAZDIRMA
           for (int k = 0; k < row; k++)
            {
                for (int l = 0; l < row; l++)
                {
                    textBox5.AppendText(Math.Round(Q[k, l],1) + " - ");
                }
                textBox5.AppendText(Environment.NewLine);
            }
            durum=baslangic;
            double yolmax=0.0;
            int nextpath=0;
            textBox2.AppendText(baslangic + " - ");
            yoldizi.Add(baslangic);
            while (durum != hedef)
            {
                for (g = 0; g < row; g++)
                {
                    if (Q[durum, g] > yolmax)
                        {
                            yolmax = Q[durum, g];
                            nextpath = g;
                           
                        }
                    
                }
                yoldizi.Add(nextpath);
                textBox2.AppendText(nextpath + " - " );
                durum = nextpath;
                
            }

        }

        private void Txthedef_TextChanged(object sender, EventArgs e)
        {
            int s = listBox1.Items.Count;
            try
            {
                if (s <= Convert.ToInt32(Txthedef.Text))
                {
                    MessageBox.Show("Geçersiz Bir Hedef Girdiniz");
                    Txthedef.Text = "0";
                }
            }
            catch (Exception)
            {
                

            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StreamWriter outR = new StreamWriter("C:/Users/ibokan/Desktop/outR.txt ");
            outR.WriteLine(textBox1.Text);
            outR.Close();

            StreamWriter outQ = new StreamWriter("C:/Users/ibokan/Desktop/outQ.txt ");
            outQ.WriteLine(textBox5.Text);
            outQ.Close();

            StreamWriter outPath = new StreamWriter("C:/Users/ibokan/Desktop/outPath.txt");
            outPath.WriteLine(textBox2.Text);
            outPath.Close();
            MessageBox.Show("Veriler Masaüstüne Kayıt Edildi");
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Graphics g;
            Form2 frm2 = new Form2();
            frm2.Show();
            g = frm2.CreateGraphics();
            Pen firca = new Pen(System.Drawing.Color.Black, 2);
            Pen yolfirca = new Pen(System.Drawing.Color.Blue, 5);
            int rw = listBox1.Items.Count;
            double r2w=Math.Sqrt(rw);
            int gen=0,yuk=0,sayac=0;
            int xx=20, yy=20;
            int[] find=new int[50];
            int fp = 0;
            SolidBrush Brush = new SolidBrush(Color.Red);
            Font yazi = new Font("Georgia",12,FontStyle.Regular);
            while(sayac<r2w)
            {
                for (int i = 0; i <r2w ; i++)
                {      
                    g.DrawRectangle(firca, xx + gen, yy + yuk, 50, 50);
                    g.DrawString(fp.ToString(), yazi, Brush, xx + gen, yy + yuk);
                    gen = gen + 50;
                    fp++;
                }
                yuk = yuk + 50;
                gen = 0;
                sayac += 1;
                }
            int x1,x2,y1,y2;
            for (int i = 0; i < yoldizi.Count-1 ; i++)
            {
                y1 = Convert.ToInt32(Math.Floor(Convert.ToInt32(yoldizi[i]) / r2w));
                x1 = Convert.ToInt32(Convert.ToInt32(yoldizi[i]) % r2w);
                y2 = Convert.ToInt32(Math.Floor(Convert.ToInt32(yoldizi[i + 1]) / r2w));
                x2 = Convert.ToInt32(Convert.ToInt32(yoldizi[i + 1]) % r2w);
                g.DrawLine(yolfirca,50+x1*50,50+y1*50,50+x2*50,50+y2*50);
            }
                
            }

        }

    }

