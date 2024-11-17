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
        private List<Coin> coins; // Lista az érmékhez
        private int coinsCollected; // Összegyűjtött érmék száma

        public MainWindow()
        {
            InitializeComponent();
            Settings s = new Settings();
            s.ShowDialog();
            inputHandler = new InputHandler();

            settings = s.settings; // Alapértelmezett beállítások
            ghosts = new List<Ghost>(); // Initialize ghost list
            settings.CoinColor = Brushes.YellowGreen; // Példa: az érmék piros színűek lesznek
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

                // Check collision with Pac-Man
                if (ghost.X == pacMan.X && ghost.Y == pacMan.Y)
                {
                    HandleCollision();
                }
            }
        }

        private void HandleCollision()
        {
            settings.Lives--;

            if (settings.Lives <= 0)
            {
                // Játék vége
                timer.Stop();
                MessageBox.Show("Game Over! You have no lives left.", "Pac-Man", MessageBoxButton.OK, MessageBoxImage.Information);
                Close(); // Kilépés a játékból
            }
            else
            {
                // Visszaállítás kezdőpozícióba
                MessageBox.Show($"You lost a life! Remaining lives: {settings.Lives}", "Pac-Man", MessageBoxButton.OK, MessageBoxImage.Warning);
                ResetPacMan();
            }
        }

        private void ResetPacMan()
        {
            pacMan.X = 1; // Kezdő X koordináta
            pacMan.Y = 1; // Kezdő Y koordináta
            pacMan.UpdatePosition();
        }

        private void DrawMap()
        {
            coins = new List<Coin>();

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

                    if (map[y, x] == 0) // Ha az adott mező út
                    {
                        Coin coin = new Coin(x, y, settings.CellSize, settings.CoinColor);
                        coins.Add(coin);

                        Canvas.SetLeft(coin.Shape, x * settings.CellSize + settings.CellSize / 4);
                        Canvas.SetTop(coin.Shape, y * settings.CellSize + settings.CellSize / 4);
                        GameCanvas.Children.Add(coin.Shape);
                    }
                }
            }
        }



        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            inputHandler.HandleKey(e.Key);

            pacMan.Move(inputHandler.DeltaX, inputHandler.DeltaY, map);

            // Ellenőrizni, hogy Pac-Man begyűjtött-e egy érmét
            Coin coin = coins.FirstOrDefault(c => c.X == pacMan.X && c.Y == pacMan.Y);
            if (coin != null)
            {
                coin.RemoveFromCanvas(GameCanvas);
                coins.Remove(coin);
                coinsCollected++;

                // Frissíthetünk egy UI elemet, ha szeretnénk
                //Title = $"Coins Collected: {coinsCollected}";
                Title = $"Yellow Ball Hero’s Mystical Labyrinth Adventure 勇敢的饥饿者 - Coins Collected: {coinsCollected}";

            }
        }

    }
}