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
        private bool isPlayerSplitting;
        private Vector playerVelocity;
        private Vector playerForceSpeed;
        private Point playerPosition;
        private Point sunPosition;
        private Point mousePosition;
        private List<Orb> enemies;
        private Orb sun, player;
        private double sunSize = 100;
        private double playerSize = 20; 
       
        public static bool CheckCollision(Orb e1, Orb e2)
        {
            var r1 = e1.Size / 2;
            var x1 = e1.Position.X + r1;
            var y1 = e1.Position.Y + r1;
            var r2 = e2.Size / 2;
            var x2 = e2.Position.X + r2;
            var y2 = e2.Position.Y + r2;
            var d = new Vector(x2 - x1, y2 - y1);
            return d.Length <= r1 + r2;
        }
        public MainWindow()
        {
            InitializeComponent();
            /// initzialize the canvas
            /// 
            
            canvas = new Canvas();
            playerPosition = new Point(100, 640);
            sunPosition = new Point(400, 640);
            sun = new Orb(1280, 800, sunSize, Colors.Yellow);
            player = new Orb(1280, 800, playerSize, Colors.Blue);
            playerVelocity = new Vector(0, 0);
            enemies = new List<Orb>();

            Window.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseDown), true);
            Window.AddHandler(PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseUp), true);
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += GameTick;
            timer.Start();
            
        }

        

        private void GameTick(object sender, EventArgs e)
        {
            UpdatePlayerPosition();
            DrawGameObjects();
            collide();
            if (MousePressed == true)
            {   // get the mouse position
                tickcount ++;
                 
                float speed = 0.2f;
                double centerX = mouseEventArgs.GetPosition(GameScreen).X;
                double centerY = mouseEventArgs.GetPosition(GameScreen).Y;
                // get the player position
                double playerX = playerPosition.X;
                double playerY = playerPosition.Y;
                // get the angle between the mouse and the player
                double dx = centerX - playerX;
                double dy = centerY - playerY;
                double angle = Math.Atan2(dy, dx);
                double oppositeAngle = angle + Math.PI;
                // move the player to the opposite side
                double oppositeX = (playerX + Math.Cos(oppositeAngle) * 4);
                double oppositeY = (playerY + Math.Sin(oppositeAngle) * 4);
                // set the player position in the canvas
                playerPosition.X = oppositeX;
                playerPosition.Y = oppositeY;
                DrawOrb(playerPosition, player.Size, player.Color);
                
                if (tickcount > 8)
                {
                    tickcount = 0;
                    // Code to execute when the gametimer reaches 500 milliseconds
                    IShapeFactory factory = new CircleFactory((playerSize)/4);
                    Shape circle = factory.CreateShape();
                    Ellipse checkellipse = circle as Ellipse;
                    // Move the circle to the opposite of the player side

                    double circleX = (playerPosition.X - Math.Cos(oppositeAngle) - (player.Size / 4) * 2 );
                    double circleY = (playerPosition.Y - Math.Sin(oppositeAngle) - (player.Size / 4) * 2 );
                    /*
                    foreach (var x in GameScreen.Children.OfType<Ellipse>().ToList())
                    {
                        if (CheckCollision(checkellipse,x))
                        {
                            if (player.Size > x.Width)
                            {
                                GameScreen.Children.Remove(x);
                                circle.Width += 2;
                                circle.Height += 2;
                            }
                            else
                            {
                                GameScreen.Children.Remove(circle);
                                x.Width += 2;
                                x.Height += 2;
                            }
                        }
                    }
                    */
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
                MouseMove += (sender, e) => mousePosition = e.GetPosition(GameScreen);
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
        public void collide()
        {
            foreach (var x in GameScreen.Children.OfType<Ellipse>().ToList())
            {
                foreach(var enemy in enemies)
                {
                    if ((string)x.Tag == "circle")
                    {
                        if (CheckCollision(player, enemy))
                        {
                            if (player.Size > x.Width)
                            {
                                GameScreen.Children.Remove(x);
                                player.Size += player.Size / 10;
                            }
                            else
                            {
                                MessageBox.Show("Game Over");
                                this.Close();
                            }
                        }
                        else if (CheckCollision(sun, enemy))
                        {
                            GameScreen.Children.Remove(x);
                            sun.Size += 2;
                        }

                    }
                    else if (CheckCollision(player, sun))
                    {
                        MessageBox.Show("Game Over");
                        this.Close();
                    }
                }
                
                
            }
        }
        
        private void UpdatePlayerPosition()
        {
            playerVelocity += (sunPosition - playerPosition) * 0.0001;
            playerPosition += playerVelocity;

            //playerPosition = ClampPosition(playerPosition);
        }
        
        private Point ClampPosition(Point position)
        {
            double x = Math.Max(0, Math.Min(1280, position.X));
            double y = Math.Max(0, Math.Min(800, position.Y));
            return new Point(x, y);
        }

        private void DrawGameObjects()
        {
            GameScreen.Children.Clear();

            DrawOrb(playerPosition, player.Size, Colors.Blue);
            DrawOrb(sunPosition, sun.Size, Colors.Yellow);

            foreach (var enemy in enemies)
            {
                DrawOrb(enemy.Position, enemy.Size, enemy.Color);
            }
        }

        private void DrawOrb(Point position, double size, Color color)
        {
            var ellipse = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = new SolidColorBrush(color)
            };

            Canvas.SetLeft(ellipse, position.X - size / 2);
            Canvas.SetTop(ellipse, position.Y - size / 2);

            GameScreen.Children.Add(ellipse);
        }
    }
    public class Orb
    {
        public Point Position { get; set; }
        public Vector Velocity { get; set; }
        public double Size { get; set; }
        public Color Color { get; set; }

        public Orb(double x, double y, double size, Color color, Vector velocity = default)
        {
            Position = new Point(x, y);
            Size = size;
            Color = color;
            Velocity = velocity;
        }

        public bool Contains(Point point, double radius)
        {
            return (Position - point).Length <= (Size / 2 + radius / 2);
        }

        public void AccelerateTowards(Point target)
        {
            var acceleration = (target - Position) * 0.001;
            Velocity += acceleration;
        }

        public void UpdatePosition()
        {
            Position += Velocity;
        }
    }
}

    
