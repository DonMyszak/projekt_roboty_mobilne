using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Image image, image2;
        Rectangle rect;
        public Pozycja objPozycja;
        public int wsp_x = 50;
        public int wsp_y = 50;
        bool przycisk = false;
    

        public enum Pozycja
        {
            Lewo, Prawo, Gora, Dol, Stop
        }

        public Form1()
        {
            InitializeComponent();
            image = WindowsFormsApplication1.Properties.Resources.osypiuk;
            image2 = WindowsFormsApplication1.Properties.Resources.osypiuk2;
            
            Wspolrzedne punkt = new Wspolrzedne(wsp_x,wsp_y);
            objPozycja = Pozycja.Dol;
           // rect = new Rectangle(wsp_x,wsp_y, 50, 50);
            
        }

        public class Wspolrzedne
        {

            public int wsp_x;
            public int wsp_y;

            public Wspolrzedne(int x, int y)
            {

                wsp_x = x;
                wsp_y = y;

            }

 /*           public void LosoweWspolrzedne()
            {
                Random rnd = new Random();
                
                    wsp_x = rnd.Next(100, 400);
                    wsp_y = rnd.Next(100, 400);
                
            } */
        }

        

    private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (przycisk)
            {
                g.DrawImage(image2, wsp_x, wsp_y, 50, 50);
            }
            else g.DrawImage(image, wsp_x, wsp_y, 50, 50);
        }



    private void timer1_Tick(object sender, EventArgs e)
        {

            if(objPozycja == Pozycja.Prawo)
            {
                wsp_x += 10;
            }
            else if(objPozycja == Pozycja.Lewo)
            {
                wsp_x -= 10;
            }
            else if(objPozycja == Pozycja.Gora)
            {
                wsp_y -= 10;
            }
            else if(objPozycja == Pozycja.Dol)
            {
                wsp_y += 10;
            }

            if(wsp_x >= 450 || wsp_y >=450 || wsp_x <= 0 || wsp_y <= 0)
            {
                objPozycja = Pozycja.Stop;
            }

            Invalidate();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            if(e.Button == MouseButtons.Left)
            {
                Point punkt = new Point(e.X, e.Y);
                if (((wsp_x-50) <= punkt.X & punkt.X <= (wsp_x + 50)) & ((wsp_y - 50) <= punkt.Y & punkt.Y <= wsp_y + 50))
                {
                    przycisk = !przycisk;
                }
                
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                objPozycja = Pozycja.Lewo;
            }

            else if (e.KeyCode == Keys.Right)
            {
                objPozycja = Pozycja.Prawo;
            }

            else if (e.KeyCode == Keys.Up)
            {
                objPozycja = Pozycja.Gora;
            }

            else if (e.KeyCode == Keys.Down)
            {
                objPozycja = Pozycja.Dol;
            }

            else if (e.KeyCode == Keys.Space)
            {
                objPozycja = Pozycja.Stop;
            }
        }
    }
}
