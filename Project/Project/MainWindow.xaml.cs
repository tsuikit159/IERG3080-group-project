using System.Text;
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
using System;
using System.Numerics;
using System.Reflection.Metadata;
using Vector = System.Windows.Vector;
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
        Random Rnd = new Random();
        double angle = 0; // Angle of rotation
        double angularSpeed = 0.01; // Angular speed of rotation

        double radius = 100; // Radius of the circular orbit
        public static bool CheckCollision(Ellipse e1, Ellipse e2)
        {
            var r1 = e1.ActualWidth / 2;
            var x1 = Canvas.GetLeft(e1) + r1;
            var y1 = Canvas.GetTop(e1) + r1;
            var r2 = e2.ActualWidth / 2;
            var x2 = Canvas.GetLeft(e2) + r2;
            var y2 = Canvas.GetTop(e2) + r2;
            var d = new Vector(x2 - x1, y2 - y1);
            return d.Length <= r1 + r2;
        }


        public MainWindow()
        {

            InitializeComponent();
            /// initzialize the canvas
            /// 
            
            canvas = new Canvas();
            Random rnd = new Random();

            Player.Focus();
            Window.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseDown), true);
            Window.AddHandler(PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseUp), true);
            for (int i = 0; i < rnd.Next(120,130); i++)
            {
                GenerateCircle();
            }


            timer.Interval = TimeSpan.FromMilliseconds(5);
            timer.Tick += GameTick;
            timer.Start();

        }

        private void GameTick(object sender, EventArgs e)
        {

            
            collide();
            CheckCircleCollision();
            UpdatePosition();
            if (MousePressed == true)
            {   // get the mouse position
                tickcount++;
                /* set the player disance to the mouse position
                 */
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
                // Normalize the vector to get only the direction.
                Canvas.SetLeft(Player, oppositeX );
                Canvas.SetTop(Player, oppositeY );
                

                if (tickcount > 10 && (Player.Width >5))
                {
                    tickcount = 0;
                    // Code to execute when the gametimer reaches 500 milliseconds
                    IShapeFactory factory = new CircleFactory((Player.Width) / 4);
                    Shape circle = factory.CreateShape();

                    // Move the circle to the opposite of the player side


                    // calculate the relative angle by calculating the normal of the player
                    Eject();
                }
                
            }

            
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                MousePressed = true;
                mouseEventArgs = e;
           

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
        public void collide()
        {
            foreach (var x in GameScreen.Children.OfType<Ellipse>().ToList())
            {
                if ((string)x.Tag == "circle")
                {
                    if (CheckCollision(Player, x))
                    {
                        if (Player.Width > x.Width)
                        {
                            Player.Width += Math.Cbrt((x.Width / 2)); //sphere volume of x and player
                            Player.Height += Math.Cbrt((x.Width / 2));
                            GameScreen.Children.Remove(x);
                        }
                        else
                        {
                            MessageBox.Show("Game Over");
                            this.Close();
                        }
                    }
                    else if (CheckCollision(Sun, x))
                    {
                        GameScreen.Children.Remove(x);
                        Sun.Width += 2;
                        Sun.Height += 2;
                    }

                }
                else if (CheckCollision(Player, Sun))
                {
                    if (Player.Width > Sun.Width)
                    {
                        GameScreen.Children.Remove(Sun);
                        MessageBox.Show("You win");
                        this.Close();
                    }
                    else
                    {
                        GameScreen.Children.Remove(Player);
                        MessageBox.Show("Game Over");
                        this.Close();
                    }
                }
                
            }
        }
        public void CheckCircleCollision()
        {
            foreach (var circle1 in GameScreen.Children.OfType<Ellipse>().Where(x => (string)x.Tag == "circle"))
            {
                foreach (var circle2 in GameScreen.Children.OfType<Ellipse>().Where(x => (string)x.Tag == "circle" && x != circle1))
                {
                    if (CheckCollision(circle1, circle2))
                    {
                        double circle1X = Canvas.GetLeft(circle1);
                        double circle1Y = Canvas.GetTop(circle1);
                        double circle2X = Canvas.GetLeft(circle2);
                        double circle2Y = Canvas.GetTop(circle2);
                        double distance = Math.Sqrt(Math.Pow(circle2X - circle1X, 2) + Math.Pow(circle2Y - circle1Y, 2));
                        double radiusSum = circle1.ActualWidth / 2 + circle2.ActualWidth / 2;
                        if (distance <= radiusSum)
                        {
                            double newX = circle1X + radiusSum + 5; // Move the circle 10 units further
                            double newY = circle1Y + radiusSum + 5; // Move the circle 10 units further
                            Canvas.SetLeft(circle2, newX);
                            Canvas.SetTop(circle2, newY);
                        }
                    }
                }
            }
        }
        public void GenerateCircle()
        {
            Random random = new Random();
            double playerWidth = Player.Width;
            double playerHeight = Player.Height;
            double circleWidth = random.NextDouble() * (playerWidth);
            double circleHeight = random.NextDouble() * (playerHeight);
            IShapeFactory factory = new CircleFactory(circleWidth / 4);
            Shape circle = factory.CreateShape();
            double playerX = Canvas.GetLeft(Player);
            double playerY = Canvas.GetTop(Player);
            double circleX = random.NextDouble() * (GameScreen.Width);
            double circleY = random.NextDouble() * (GameScreen.Height);
            Player.Width += Math.Cbrt((circle.Width / 2));
            Player.Height += Math.Cbrt((circle.Height / 2));
            Canvas.SetLeft(circle, circleX);
            Canvas.SetTop(circle, circleY);
            GameScreen.Children.Add(circle);
        }
        // Calculate the position of the player and enemies in a circular orbit
        public void GenerateCircle(double x, double y)
        {
            Random random = new Random();
            double playerWidth = Player.Width;
            double playerHeight = Player.Height;
            double circleWidth = random.NextDouble() * (playerWidth/2);
            double circleHeight = random.NextDouble() * (playerHeight/2);
            IShapeFactory factory = new CircleFactory(circleWidth / 4);
            Shape circle = factory.CreateShape();
            double playerX = Canvas.GetLeft(Player);
            double playerY = Canvas.GetTop(Player);
            Player.Width += Math.Cbrt((circle.Width / 2));
            Player.Height += Math.Cbrt((circle.Height / 2));
            Canvas.SetLeft(circle, x);
            Canvas.SetTop(circle, y);
            GameScreen.Children.Add(circle);
        }

        public void Eject()
        {
            double playerX = Canvas.GetLeft(Player) + Player.Width / 8;
            double playerY = Canvas.GetTop(Player) + Player.Height / 8;
            double mouseX = mouseEventArgs.GetPosition(GameScreen).X;
            double mouseY = mouseEventArgs.GetPosition(GameScreen).Y;
            double dx = mouseX - playerX;
            double dy = mouseY - playerY;
            double angle = Math.Atan2(dy, dx);
            double circleX = playerX + Math.Cos(angle) * (Player.Width);
            double circleY = playerY + Math.Sin(angle) * (Player.Height);
            Player.Width -= Math.Cbrt((Player.Width / 2));
            Player.Height -= Math.Cbrt((Player.Height / 2));
            GenerateCircle(circleX, circleY);
        }
        // Update the position of the player and enemies in each frame

        private void UpdatePosition()
        {
            double angle = Math.Atan2(Canvas.GetTop(Sun) - Canvas.GetTop(Player), Canvas.GetLeft(Sun) - Canvas.GetLeft(Player));
            foreach (var x in GameScreen.Children.OfType<Ellipse>().ToList().Where(x => (string)x.Tag != "Sun"))
            {
                // Calculate the position of the player
                double speedx = (Canvas.GetLeft(x) + x.Width * Math.Cos(angle)) / 9000;
                double speedy = (Canvas.GetTop(x) + x.Height * Math.Sin(angle)) / 9000;
                // Calculate the position of the circle

                // Calculate the gravitational force
                double distance = Math.Sqrt(Math.Pow(Canvas.GetLeft(Sun) - Canvas.GetLeft(x), 2) + Math.Pow(Canvas.GetTop(Sun) - Canvas.GetTop(x), 2));
                double gravitationalForce = 1000 / Math.Pow(distance, 2);

                // Calculate the acceleration
                double accelerationX = gravitationalForce * Math.Cos(angle);
                double accelerationY = gravitationalForce * Math.Sin(angle);

                // Update the speed
                speedx += accelerationX;
                speedy += accelerationY;

                // Set the position of the player and the circle
                Canvas.SetLeft(x, Canvas.GetLeft(x) + speedx);
                Canvas.SetTop(x, Canvas.GetTop(x) + speedy);
            }

           
        }
        double GenerateRandomDouble()
        {
            return Rnd.NextDouble() * 2 - 1;
        }

    }

    // Update the position of the player and the circle in each frame


}
