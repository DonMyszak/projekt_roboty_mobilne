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
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace ProjektRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Robot[] robot = new Robot[5];
        Random rand = new Random();
        Ellipse[] ellipse = new Ellipse[5];
        Line myLine = new Line();
        TextBlock wsp_x_info = new TextBlock();
        TextBlock wsp_y_info = new TextBlock();
        Line myLine1 = new Line();
        TextBlock wsp_x_info1 = new TextBlock();
        TextBlock wsp_y_info1 = new TextBlock();
        Line myLine2 = new Line();
        TextBlock wsp_x_info2 = new TextBlock();
        TextBlock wsp_y_info2 = new TextBlock();
        Line myLine3 = new Line();
        TextBlock wsp_x_info3 = new TextBlock();
        TextBlock wsp_y_info3 = new TextBlock();
        Line myLine4 = new Line();
        TextBlock wsp_x_info4 = new TextBlock();
        TextBlock wsp_y_info4 = new TextBlock();
        private System.Windows.Threading.DispatcherTimer timer;
        public Pozycja objPozycja;
        public int id_przycisku;
        public bool[] przycisk = new bool[5];
        public bool[] rysLinii = new bool[5];
        public bool[] robot_dostepny = new bool[5];
        TcpClient client = new TcpClient();
        Thread mThread;




        public enum Pozycja
        {
            Lewo, Prawo, Gora, Dol, Stop
        }

        public class Robot
        {

            public int wsp_x;
            public int wsp_y;
            public string identyfikator;
            public int bateria;
            public double kat;
            public int predkosc;
            public bool led;

            public Robot(int x, int y, string id)
            {
                wsp_x = x;
                wsp_y = y;
                identyfikator = id;
            }


        }

        public MainWindow()
        {
            InitializeComponent();
            int i = 0;
            robot_dostepny[0] = true;
            robot_dostepny[1] = true;
            robot_dostepny[2] = true;
            robot_dostepny[3] = true;
            robot_dostepny[4] = true;
            przycisk[0] = false;
            przycisk[1] = false;
            przycisk[2] = false;
            przycisk[3] = false;
            przycisk[4] = false;
            for (i = 0; i < robot.Length; i++)
            {
                if (robot_dostepny[i])
                {
                    robot[i] = new Robot(50 + i * 50, 50 + i * 50, Convert.ToString(i + 1));

                }
            }
            objPozycja = Pozycja.Stop;
            //Initialize the timer class 
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Start();
            timer.Interval = TimeSpan.FromSeconds(0.1); //Set the interval period here.
            timer.Tick += timer1_Tick;

        }
        private void ledChangeColorGreen(Ellipse x)
        {

            x.Fill = new SolidColorBrush(Colors.ForestGreen);
        }

        private void ledChangeColorRed(Ellipse x)
        {

            x.Fill = new SolidColorBrush(Colors.Red);
        }

        private void robotJeden_Click(object sender, RoutedEventArgs e)
        {
            if (robot[0] != null)
            {
                identyfikatorRobota.Text = robot[0].identyfikator;
            }
        }

        private void robotDwa_Click(object sender, RoutedEventArgs e)
        {
            if (robot[1] != null)
            {
                identyfikatorRobota.Text = robot[1].identyfikator;
            }

        }

        private void robotTrzy_Click(object sender, RoutedEventArgs e)
        {
            if (robot[2] != null)
            {
                identyfikatorRobota.Text = robot[2].identyfikator;
            }
        }

        private void robotCztery_Click(object sender, RoutedEventArgs e)
        {
            if (robot[3] != null)
            {
                identyfikatorRobota.Text = robot[3].identyfikator;
            }
        }

        private void robotPiec_Click(object sender, RoutedEventArgs e)
        {
            if (robot[4] != null)
            {
                identyfikatorRobota.Text = robot[4].identyfikator;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (objPozycja == Pozycja.Prawo)
            {
                robot[0].wsp_x += 10;
            }
            else if (objPozycja == Pozycja.Lewo)
            {
                robot[0].wsp_x -= 10;
            }
            else if (objPozycja == Pozycja.Gora)
            {
                robot[0].wsp_y -= 10;
            }
            else if (objPozycja == Pozycja.Dol)
            {
                robot[0].wsp_y += 10;
            }

            if (robot[0].wsp_x >= 480 || robot[0].wsp_y >= 480 || robot[0].wsp_x <= 0 || robot[0].wsp_y <= 0)
            {
                objPozycja = Pozycja.Stop;
            }


            //Remove the previous ellipse from the paint canvas.



            //Add the ellipse to the canvas

            int i = 0;
            for (i = 0; i < robot.Length; i++)
            {
                PaintCanvas.Children.Remove(ellipse[i]);
                ellipse[i] = CreateAnEllipse(20, 20, przycisk[i]);
                PaintCanvas.Children.Add(ellipse[i]);
                Canvas.SetLeft(ellipse[i], robot[i].wsp_x);
                Canvas.SetTop(ellipse[i], robot[i].wsp_y);
            }



        }

        // Customize your ellipse in this method
        public Ellipse CreateAnEllipse(int height, int width, bool przycisk)
        {

            SolidColorBrush fillBrush = new SolidColorBrush();
            SolidColorBrush borderBrush = new SolidColorBrush();

            if (przycisk)
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
            if ((robot[0].wsp_x <= x & x <= (robot[0].wsp_x + 20)) & (robot[0].wsp_y <= y & y <= robot[0].wsp_y + 20))
            {
                przycisk[0] = !przycisk[0];
                rysLinii[0] = true;
            }
            else
            {
                rysLinii[0] = false;
            }

            if ((robot[1].wsp_x <= x & x <= (robot[1].wsp_x + 20)) & (robot[1].wsp_y <= y & y <= robot[1].wsp_y + 20))
            {
                przycisk[1] = !przycisk[1];
                rysLinii[1] = true;
            }
            else
            {
                rysLinii[1] = false;
            }

            if ((robot[2].wsp_x <= x & x <= (robot[2].wsp_x + 20)) & (robot[2].wsp_y <= y & y <= robot[2].wsp_y + 20))
            {
                przycisk[2] = !przycisk[2];
                rysLinii[2] = true;
            }
            else
            {
                rysLinii[2] = false;
            }

            if ((robot[3].wsp_x <= x & x <= (robot[3].wsp_x + 20)) & (robot[3].wsp_y <= y & y <= robot[3].wsp_y + 20))
            {
                przycisk[3] = !przycisk[3];
                rysLinii[3] = true;
            }
            else
            {
                rysLinii[3] = false;
            }

            if ((robot[4].wsp_x <= x & x <= (robot[4].wsp_x + 20)) & (robot[4].wsp_y <= y & y <= robot[4].wsp_y + 20))
            {
                przycisk[4] = !przycisk[4];
                rysLinii[4] = true;
            }
            else
            {
                rysLinii[4] = false;
            }

        }



        private void PaintCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (przycisk[0] & (objPozycja == Pozycja.Stop) & rysLinii[0])
            {

                Point p = e.GetPosition(PaintCanvas);

                double x = p.X;
                double y = p.Y;

                PaintCanvas.Children.Remove(myLine);
                PaintCanvas.Children.Remove(wsp_x_info);
                PaintCanvas.Children.Remove(wsp_y_info);

                myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                myLine.X1 = x;
                myLine.X2 = robot[0].wsp_x + 10;
                myLine.Y1 = y;
                myLine.Y2 = robot[0].wsp_y + 10;
                myLine.StrokeThickness = 2;

                PaintCanvas.Children.Add(myLine);
                ledChangeColorGreen(Led1);

                wsp_x_info.Text = "X:" + Convert.ToString(x * 0.004) + "m";
                wsp_x_info.Foreground = new SolidColorBrush(Colors.Black);
                wsp_x_info.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_x_info, x + 15);
                Canvas.SetTop(wsp_x_info, y);
                PaintCanvas.Children.Add(wsp_x_info);

                wsp_y_info.Text = "Y:" + Convert.ToString(y * 0.004) + "m";
                wsp_y_info.Foreground = new SolidColorBrush(Colors.Black);
                wsp_y_info.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_y_info, x + 15);
                Canvas.SetTop(wsp_y_info, y + 20);
                PaintCanvas.Children.Add(wsp_y_info);

            }

            if (przycisk[1] & (objPozycja == Pozycja.Stop) & rysLinii[1])
            {

                Point p = e.GetPosition(PaintCanvas);

                double x = p.X;
                double y = p.Y;

                PaintCanvas.Children.Remove(myLine1);
                PaintCanvas.Children.Remove(wsp_x_info1);
                PaintCanvas.Children.Remove(wsp_y_info1);

                myLine1.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                myLine1.X1 = x;
                myLine1.X2 = robot[1].wsp_x + 10;
                myLine1.Y1 = y;
                myLine1.Y2 = robot[1].wsp_y + 10;
                myLine1.StrokeThickness = 2;

                PaintCanvas.Children.Add(myLine1);
                ledChangeColorGreen(Led2);

                wsp_x_info1.Text = "X:" + Convert.ToString(x * 0.004) + "m";
                wsp_x_info1.Foreground = new SolidColorBrush(Colors.Black);
                wsp_x_info1.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_x_info1, x + 15);
                Canvas.SetTop(wsp_x_info1, y);
                PaintCanvas.Children.Add(wsp_x_info1);

                wsp_y_info1.Text = "Y:" + Convert.ToString(y * 0.004) + "m";
                wsp_y_info1.Foreground = new SolidColorBrush(Colors.Black);
                wsp_y_info1.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_y_info1, x + 15);
                Canvas.SetTop(wsp_y_info1, y + 20);
                PaintCanvas.Children.Add(wsp_y_info1);

            }

            if (przycisk[2] & (objPozycja == Pozycja.Stop) & rysLinii[2])
            {

                Point p = e.GetPosition(PaintCanvas);

                double x = p.X;
                double y = p.Y;

                PaintCanvas.Children.Remove(myLine2);
                PaintCanvas.Children.Remove(wsp_x_info2);
                PaintCanvas.Children.Remove(wsp_y_info2);

                myLine2.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                myLine2.X1 = x;
                myLine2.X2 = robot[2].wsp_x + 10;
                myLine2.Y1 = y;
                myLine2.Y2 = robot[2].wsp_y + 10;
                myLine2.StrokeThickness = 2;

                PaintCanvas.Children.Add(myLine2);
                ledChangeColorGreen(Led3);

                wsp_x_info2.Text = "X:" + Convert.ToString(x * 0.004) + "m";
                wsp_x_info2.Foreground = new SolidColorBrush(Colors.Black);
                wsp_x_info2.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_x_info2, x + 15);
                Canvas.SetTop(wsp_x_info2, y);
                PaintCanvas.Children.Add(wsp_x_info2);

                wsp_y_info2.Text = "Y:" + Convert.ToString(y * 0.004) + "m";
                wsp_y_info2.Foreground = new SolidColorBrush(Colors.Black);
                wsp_y_info2.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_y_info2, x + 15);
                Canvas.SetTop(wsp_y_info2, y + 20);
                PaintCanvas.Children.Add(wsp_y_info2);

            }

            if (przycisk[3] & (objPozycja == Pozycja.Stop) & rysLinii[3])
            {

                Point p = e.GetPosition(PaintCanvas);

                double x = p.X;
                double y = p.Y;

                PaintCanvas.Children.Remove(myLine3);
                PaintCanvas.Children.Remove(wsp_x_info3);
                PaintCanvas.Children.Remove(wsp_y_info3);

                myLine3.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                myLine3.X1 = x;
                myLine3.X2 = robot[3].wsp_x + 10;
                myLine3.Y1 = y;
                myLine3.Y2 = robot[3].wsp_y + 10;
                myLine3.StrokeThickness = 2;

                PaintCanvas.Children.Add(myLine3);
                ledChangeColorGreen(Led4);

                wsp_x_info3.Text = "X:" + Convert.ToString(x * 0.004) + "m";
                wsp_x_info3.Foreground = new SolidColorBrush(Colors.Black);
                wsp_x_info3.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_x_info3, x + 15);
                Canvas.SetTop(wsp_x_info3, y);
                PaintCanvas.Children.Add(wsp_x_info3);

                wsp_y_info3.Text = "Y:" + Convert.ToString(y * 0.004) + "m";
                wsp_y_info3.Foreground = new SolidColorBrush(Colors.Black);
                wsp_y_info3.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_y_info3, x + 15);
                Canvas.SetTop(wsp_y_info3, y + 20);
                PaintCanvas.Children.Add(wsp_y_info3);

            }

            if (przycisk[4] & (objPozycja == Pozycja.Stop) & rysLinii[4])
            {

                Point p = e.GetPosition(PaintCanvas);

                double x = p.X;
                double y = p.Y;

                PaintCanvas.Children.Remove(myLine4);
                PaintCanvas.Children.Remove(wsp_x_info4);
                PaintCanvas.Children.Remove(wsp_y_info4);

                myLine4.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                myLine4.X1 = x;
                myLine4.X2 = robot[4].wsp_x + 10;
                myLine4.Y1 = y;
                myLine4.Y2 = robot[4].wsp_y + 10;
                myLine4.StrokeThickness = 2;

                PaintCanvas.Children.Add(myLine4);
                ledChangeColorGreen(Led5);

                wsp_x_info4.Text = "X:" + Convert.ToString(x * 0.004) + "m";
                wsp_x_info4.Foreground = new SolidColorBrush(Colors.Black);
                wsp_x_info4.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_x_info4, x + 15);
                Canvas.SetTop(wsp_x_info4, y);
                PaintCanvas.Children.Add(wsp_x_info4);

                wsp_y_info4.Text = "Y:" + Convert.ToString(y * 0.004) + "m";
                wsp_y_info4.Foreground = new SolidColorBrush(Colors.Black);
                wsp_y_info4.Background = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(wsp_y_info4, x + 15);
                Canvas.SetTop(wsp_y_info4, y + 20);
                PaintCanvas.Children.Add(wsp_y_info4);

            }

        }

        private void PaintCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {



            ledChangeColorRed(Led1);
            ledChangeColorRed(Led2);
            ledChangeColorRed(Led3);
            ledChangeColorRed(Led4);
            ledChangeColorRed(Led5);

            PaintCanvas.Children.Remove(myLine);
            PaintCanvas.Children.Remove(wsp_x_info);
            PaintCanvas.Children.Remove(wsp_y_info);
            PaintCanvas.Children.Remove(myLine1);
            PaintCanvas.Children.Remove(wsp_x_info1);
            PaintCanvas.Children.Remove(wsp_y_info1);
            PaintCanvas.Children.Remove(myLine2);
            PaintCanvas.Children.Remove(wsp_x_info2);
            PaintCanvas.Children.Remove(wsp_y_info2);
            PaintCanvas.Children.Remove(myLine3);
            PaintCanvas.Children.Remove(wsp_x_info3);
            PaintCanvas.Children.Remove(wsp_y_info3);
            PaintCanvas.Children.Remove(myLine4);
            PaintCanvas.Children.Remove(wsp_x_info4);
            PaintCanvas.Children.Remove(wsp_y_info4);

            int i = 0;
            for (i = 0; i < robot.Length; i++)
            {
                rysLinii[i] = false;
                przycisk[i] = false;
            }

        }

        private void identyfikatorRobota_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void portServer_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            mThread = new Thread(new ThreadStart(Connect));
            int port = Convert.ToInt32(portServer.Text);
            client.Connect(ipServer.Text, port);
            mThread.Start();
        }

        public void Connect()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                stream = client.GetStream();
                Byte[] data = System.Text.Encoding.ASCII.GetBytes("120");
                for (int i = 0; i != data.Length; i++)
                {
                    data[i] -= 48;
                }
                stream.Write(data, 0, data.Length);

                Byte[] odpowiedz = new Byte[2];
                odpowiedz[0] = (byte)stream.ReadByte();
                odpowiedz[1] = (byte)stream.ReadByte();

                if (odpowiedz[0] == 1 & odpowiedz[1] == 7)
                {
                    odpowiedz = new Byte[120];

                    while (true)
                    {
                        data = System.Text.Encoding.ASCII.GetBytes("3");

                        for (int i = 0; i != data.Length; i++)
                        {
                            data[i] -= 48;
                        }

                        stream.Write(data, 0, data.Length);

                        for (int i = 0; i < odpowiedz.Length; i++)
                        {
                            odpowiedz[i] = (byte)stream.ReadByte();
                        }

                        string odpowiedz_string;

                        for (int i = 0; i < odpowiedz.Length; i++)
                        {
                            odpowiedz_string = odpowiedz[i].ToString();
                        }

                        if (odpowiedz.Length > 3)
                        {
                            if (odpowiedz[0] == '4')
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    robot[i].identyfikator = Math.Round((BitConverter.ToSingle(odpowiedz, 14 * i + 1))).ToString();
                                    robot[i].wsp_x = Convert.ToInt16((BitConverter.ToSingle(odpowiedz, 14 * i + 3)));
                                    robot[i].wsp_y = Convert.ToInt16((BitConverter.ToSingle(odpowiedz, 14 * i + 7)));
                                    robot[i].kat = Convert.ToDouble((BitConverter.ToSingle(odpowiedz, 14 * i + 11)));
                                }

                            }
                        }

                    }
                }
                else MessageBox.Show("brak mmonitora");
            }
            catch (Exception)
            {
                MessageBox.Show("serwer rozłączony");
            }

        }
    }
}
