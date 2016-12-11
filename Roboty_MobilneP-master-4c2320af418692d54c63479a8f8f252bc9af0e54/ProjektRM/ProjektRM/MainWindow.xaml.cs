using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;


namespace ProjektRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private System.Windows.Threading.DispatcherTimer timer;
        Random rand = new Random();
        Ellipse ellipse = null;
        Line myLine = new Line();
        TextBlock wsp_x_info = new TextBlock();
        TextBlock wsp_y_info = new TextBlock();
        public Pozycja objPozycja;
        public int wsp_x = 50;
        public int wsp_y = 50;
        public bool przycisk = false;
        public bool rysLinii = false;


        public enum Pozycja
        {
            Lewo, Prawo, Gora, Dol, Stop
        }

        public class Robot
        {

            public int wsp_x;
            public int wsp_y;

            public Robot(int x, int y)
            {

                wsp_x = x;
                wsp_y = y;

            }

        }

        public MainWindow()
        {
            InitializeComponent();

            Robot robot = new Robot(wsp_x, wsp_y);
            objPozycja = Pozycja.Dol;
            //Initialize the timer class 
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Start();
            timer.Interval = TimeSpan.FromSeconds(0.1); //Set the interval period here.
            timer.Tick += timer1_Tick;

        }
        private void ledChangeColor(Ellipse x)
        {

            x.Fill = new SolidColorBrush(Colors.ForestGreen);
        }

        private void robotJeden_Click(object sender, RoutedEventArgs e)
        {
            ledChangeColor(Led1);
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            if (objPozycja == Pozycja.Prawo)
            {
                wsp_x += 10;
            }
            else if (objPozycja == Pozycja.Lewo)
            {
                wsp_x -= 10;
            }
            else if (objPozycja == Pozycja.Gora)
            {
                wsp_y -= 10;
            }
            else if (objPozycja == Pozycja.Dol)
            {
                wsp_y += 10;
            }

            if (wsp_x >= 480 || wsp_y >= 480 || wsp_x <= 0 || wsp_y <= 0)
            {
                objPozycja = Pozycja.Stop;
            }

            
            //Remove the previous ellipse from the paint canvas.
            PaintCanvas.Children.Remove(ellipse);


            //Add the ellipse to the canvas
            ellipse = CreateAnEllipse(20, 20, przycisk);
            PaintCanvas.Children.Add(ellipse);

            Canvas.SetLeft(ellipse, wsp_x);
            Canvas.SetTop(ellipse, wsp_y);


        }

        // Customize your ellipse in this method
        public Ellipse CreateAnEllipse(int height, int width, bool przycisk)
        {

            SolidColorBrush fillBrush = new SolidColorBrush();
            SolidColorBrush borderBrush = new SolidColorBrush();

            if (przycisk == true)
            {
                fillBrush.Color = Colors.Green;
                borderBrush.Color = Colors.Black;
            }
            else
            {
                fillBrush.Color = Colors.Red;
                borderBrush.Color = Colors.Black;
            }

            return new Ellipse()
            {
                Height = height,
                Width = width,
                StrokeThickness = 1,
                Stroke = borderBrush,
                Fill = fillBrush
            };
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            {
                if (e.Key == Key.Left)
                {
                    objPozycja = Pozycja.Lewo;
                }

                else if (e.Key == Key.Right)
                {
                    objPozycja = Pozycja.Prawo;
                }

                else if (e.Key == Key.Up)
                {
                    objPozycja = Pozycja.Gora;
                }

                else if (e.Key == Key.Down)
                {
                    objPozycja = Pozycja.Dol;
                }

                else if (e.Key == Key.Space)
                {
                    objPozycja = Pozycja.Stop;
                }
            }
        }


        private void PaintCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(PaintCanvas);
            double x = p.X;
            double y = p.Y;
            if ((wsp_x <= x & x <= (wsp_x + 20)) & (wsp_y <= y & y <= wsp_y + 20))
            {
                przycisk = !przycisk;
                rysLinii = true;
            }
            else
            {
                rysLinii = false;
            }

        }

        private void PaintCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(przycisk & (objPozycja == Pozycja.Stop) & rysLinii)
            {

               Point p = e.GetPosition(PaintCanvas);

               double x = p.X;
               double y = p.Y;

               PaintCanvas.Children.Remove(myLine);
               PaintCanvas.Children.Remove(wsp_x_info);
               PaintCanvas.Children.Remove(wsp_y_info);

                myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
               myLine.X1 = x;
               myLine.X2 = wsp_x + 10;
               myLine.Y1 = y;
               myLine.Y2 = wsp_y + 10;
               myLine.StrokeThickness = 2;

               PaintCanvas.Children.Add(myLine);


                wsp_x_info.Text = "X:" + Convert.ToString(x*0.004) + "m";
                wsp_x_info.Foreground = new SolidColorBrush(Colors.Black);
                wsp_x_info.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_x_info, x + 15);
                Canvas.SetTop(wsp_x_info, y);
                PaintCanvas.Children.Add(wsp_x_info);

                wsp_y_info.Text = "Y:" + Convert.ToString(y*0.004) + "m";
                wsp_y_info.Foreground = new SolidColorBrush(Colors.Black);
                wsp_y_info.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_y_info, x + 15);
                Canvas.SetTop(wsp_y_info, y + 20);
                PaintCanvas.Children.Add(wsp_y_info);

            }

        }

        private void PaintCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            PaintCanvas.Children.Remove(myLine);
            PaintCanvas.Children.Remove(wsp_x_info);
            PaintCanvas.Children.Remove(wsp_y_info);
            rysLinii = false;
            przycisk = false;
        }
    }
}
