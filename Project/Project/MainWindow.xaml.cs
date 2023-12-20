﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading.Tasks;
using Circle;
namespace Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer gametimer = new DispatcherTimer();
        private Canvas canvas;
        private bool MousePressed = false;
        private float speed = 2, speedx, speedy;
        private MouseButtonEventArgs mouseEventArgs;
        private int tickcount = 0;

        public MainWindow()
        {
            InitializeComponent();
            /// initzialize the canvas
            /// 

            canvas = new Canvas();

            GameScreen.Focus();
            Window.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseDown), true);
            Window.AddHandler(PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseUp), true);
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += GameTick;
            timer.Start();

        }
        private void GameTick(object sender, EventArgs e)
        {
            if (MousePressed == true)
            {   // get the mouse position
                tickcount ++;

                float speed = 0.2f;
                double centerX = mouseEventArgs.GetPosition(GameScreen).X;
                double centerY = mouseEventArgs.GetPosition(GameScreen).Y;
                // get the player position
                double playerX = Canvas.GetLeft(Player);
                double playerY = Canvas.GetTop(Player);
                // get the angle between the mouse and the player
                double dx = centerX - playerX;
                double dy = centerY - playerY;
                double angle = Math.Atan2(dy, dx);
                double oppositeAngle = angle + Math.PI;
                // move the player to the opposite side
                double oppositeX = (playerX + Math.Cos(oppositeAngle));
                double oppositeY = (playerY + Math.Sin(oppositeAngle));
                // set the player position in the canvas
                Canvas.SetLeft(Player, oppositeX);
                Canvas.SetTop(Player, oppositeY);

                if (tickcount > 8)
                {
                    tickcount = 0;
                    // Code to execute when the gametimer reaches 500 milliseconds
                    double radius = 10;
                    IShapeFactory factory = new CircleFactory(radius);
                    Shape circle = factory.CreateShape();
                    // Move the circle to the opposite of the player side

                    double circleX = (playerX - Math.Cos(oppositeAngle) - radius * 2);
                    double circleY = (playerY - Math.Sin(oppositeAngle) - radius * 2);

                    Canvas.SetLeft(circle, circleX);
                    Canvas.SetTop(circle, circleY);
                    GameScreen.Children.Add(circle);
                }
            }
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                MousePressed = true;
                mouseEventArgs = e;
                
                // set the circle position in the canvas
                
            }
        }
        public void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                MousePressed = false;
                Console.WriteLine("mouse released");
            }
        }
    }
    


    
}