using System.Media;
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
using System.IO;

namespace Pac_Man_WPF_2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] map = new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
            { 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
            { 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        private GameSettings settings;
        private PacMan pacMan;
        private InputHandler inputHandler;
        private List<Ghost> ghosts;
        private DispatcherTimer timer;



        
        private void PlaySound()
        {
            
            string soundFilePath = "Sounds/background.wav"; 
            SoundPlayer player = new SoundPlayer(soundFilePath);
            player.Play();
        }
        public MainWindow()
        {
            InitializeComponent();
        
            Settings s = new Settings();
            s.ShowDialog();
            inputHandler = new InputHandler();

            settings = s.settings; // Alapértelmezett beállítások
            ghosts = new List<Ghost>(); // Initialize ghost list
            Thread.Sleep(800);
            PlaySound();
            DrawMap();

            // Create PacMan and add it to the Canvas
            pacMan = new PacMan(1, 1);
            GameCanvas.Children.Add(pacMan.Shape);

            // Create ghosts and add them to the canvas
            ghosts.Add(new Ghost(10, 4, Brushes.Blue, 1)); // Example ghost at (10,4)

            // Add ghost shapes to the canvas
            foreach (var ghost in ghosts)
            {
                var ghostShape = new Ellipse
                {
                    Width = settings.CellSize,
                    Height = settings.CellSize,
                    Fill = ghost.Color
                };
                Canvas.SetLeft(ghostShape, ghost.X * settings.CellSize);
                Canvas.SetTop(ghostShape, ghost.Y * settings.CellSize);
                GameCanvas.Children.Add(ghostShape);
            }

            // Set up the timer for updating ghost movement
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200) // Update every 200ms
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            // Move each ghost
            foreach (var ghost in ghosts)
            {
                ghost.Move(map); // Move ghost based on the current map
                // Update ghost position on the canvas
                var ghostShape = GameCanvas.Children.OfType<Ellipse>()
                                                     .FirstOrDefault(g => g.Fill == ghost.Color);
                if (ghostShape != null)
                {
                    Canvas.SetLeft(ghostShape, ghost.X * settings.CellSize);
                    Canvas.SetTop(ghostShape, ghost.Y * settings.CellSize);
                }
            }
        }


        private void DrawMap()
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = settings.CellSize,
                        Height = settings.CellSize,
                        Fill = map[y, x] == 1 ? settings.WallColor : settings.PathColor
                    };
                    Canvas.SetLeft(rect, x * settings.CellSize);
                    Canvas.SetTop(rect, y * settings.CellSize);
                    GameCanvas.Children.Add(rect);
                }
            }
        }

        private void Window_KeyDownRegi(object sender, KeyEventArgs e)
        {
            int deltaX = 0, deltaY = 0;

            // Nyílbillentyűk kezelése
            switch (e.Key)
            {
                case Key.Up:
                    deltaY = -1;
                    break;
                case Key.Down:
                    deltaY = 1;
                    break;
                case Key.Left:
                    deltaX = -1;
                    break;
                case Key.Right:
                    deltaX = 1;
                    break;
            }

            pacMan.Move(deltaX, deltaY, map); // PacMan mozgatása
        } //törölni később


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            inputHandler.HandleKey(e.Key); // Handle the input key

            pacMan.Move(inputHandler.DeltaX, inputHandler.DeltaY, map); // Move PacMan based on input
        }
    }
}