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
        private MediaElement backgroundMusic; // Hátterezene
        private List<Cherry> cherries; // Lista a cseresznyékhez

        private void InitializeBackgroundMusic()
        {

            backgroundMusic = new MediaElement
            {
                Source = new Uri("Sounds/background.wav", UriKind.Relative),
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Stop,
                Volume = 0.5
            };

            GameCanvas.Children.Add(backgroundMusic);

            backgroundMusic.MediaEnded += (s, e) =>
            {
                backgroundMusic.Position = TimeSpan.Zero;
                backgroundMusic.Play();
            };
        }

        private void PlaySound()
        {
            backgroundMusic.Play();
        }

        public MainWindow()
        {
            InitializeComponent();
            Settings s = new Settings();
            s.ShowDialog();

            inputHandler = new InputHandler();
            settings = s.settings;
            ghosts = new List<Ghost>();
            coins = new List<Coin>();
            coinsCollected = 0;

            cherries = new List<Cherry>();

            DrawMap();
            GenerateCherry();

            InitializeBackgroundMusic();
            //PlaySound();

            pacMan = new PacMan(1, 1);
            GameCanvas.Children.Add(pacMan.Shape);

            ghosts.Add(new Ghost(10, 4, Brushes.Blue, 1));
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

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            // Pac-Man mozgatása az aktuális irányba
            pacMan.Move(inputHandler.DeltaX, inputHandler.DeltaY, map);

            // Ellenőrizni, hogy Pac-Man begyűjtött-e egy érmét
            Coin coin = coins.FirstOrDefault(c => c.X == pacMan.X && c.Y == pacMan.Y);
            
            if (coin != null)
            {
                coin.RemoveFromCanvas(GameCanvas);
                coins.Remove(coin);
                coinsCollected++;

                Title = $"Yellow Ball Hero’s Mystical Labyrinth Adventure 勇敢的饥饿者 - Coins Collected: {coinsCollected}";
            }

            // Ellenőrizni ütközést a szellemekkel
            foreach (var ghost in ghosts)
            {
                ghost.Move(map); // Szellemek mozgatása
                var ghostShape = GameCanvas.Children.OfType<Ellipse>()
                                                     .FirstOrDefault(g => g.Fill == ghost.Color);
                if (ghostShape != null)
                {
                    Canvas.SetLeft(ghostShape, ghost.X * settings.CellSize);
                    Canvas.SetTop(ghostShape, ghost.Y * settings.CellSize);
                }

                if (ghost.X == pacMan.X && ghost.Y == pacMan.Y)
                {
                    HandleCollision(ghost);
                }
            }

            // Ellenőrizni, hogy Pac-Man begyűjtött-e egy cseresznyét
            Cherry cherry = cherries.FirstOrDefault(c => c.X == pacMan.X && c.Y == pacMan.Y);
            if (cherry != null)
            {
                cherry.RemoveFromCanvas(GameCanvas);
                cherries.Remove(cherry);




                DispatcherTimer colorTimer = new DispatcherTimer();
                colorTimer.Interval = TimeSpan.FromSeconds(0.5);
                bool toggleColor = false; // To alternate between two colors
                colorTimer.Tick += (s, e) =>
                {
                    // Alternate colors while ghosts are eatable
                    foreach (var ghost in ghosts)
                    {
                        if (toggleColor)
                            ghost.Color = Brushes.Gray; // Alternate color
                        else
                            ghost.Color = ghost.DefaultColor; // Base color
                    }
                    toggleColor = !toggleColor;
                };

                // Start the timer
                colorTimer.Start();
                foreach(Ghost g in ghosts)
                { 
                    g.Eatable = true;
                }
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(5);
                    timer.Tick += (s, e) =>
                    {
                        // When the timer ends, make ghosts not eatable
                        foreach (Ghost g in ghosts)
                        { 
                            g.Eatable = false;
                 
                        }
                        timer.Stop(); // Stop the timer
                        colorTimer.Stop();
                    };

                    // Start the timer
                    timer.Start();








                // Növeld a pontszámot (pl. plusz 10 pont a cseresznyéért)
                coinsCollected += 10;

                Title = $"Yellow Ball Hero’s Mystical Labyrinth Adventure 勇敢的饥饿者 - Coins Collected: {coinsCollected}";
            }

        }
        private void GhostEaten(Ghost g)
        {
            g.X = 0;
            g.Y = 0;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (s, e) =>
            {
                // When the timer ends, make ghosts not eatable
                
                g.X = g.SpawnCoordinates[0, 0];
                g.Y = g.SpawnCoordinates[0, 1];
                timer.Stop(); // Stop the timer
            };

            // Start the timer
            timer.Start();
        }
        private void HandleCollision(Ghost g)
        {
            if (g.Eatable == true)
            {
                GhostEaten(g);
            }
            else
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
            // Irány megváltoztatása
            inputHandler.HandleKey(e.Key);

            // Ellenőrizni ütközést a szellemekkel
            foreach (var ghost in ghosts)
            {
              
                if (ghost.X == pacMan.X && ghost.Y == pacMan.Y)
                {
                    HandleCollision(ghost);
                }
            }
        }



        private void GenerateCherry()
        {
            // Ellenőrizni, hogy vannak-e érmék, mielőtt próbálunk választani egyet
            if (coins != null && coins.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(coins.Count);

                // Cseresznye pozíció az érme helyén
                Coin coin = coins[randomIndex];

                // Töröld az érmét és cseréld cseresznyére
                coin.RemoveFromCanvas(GameCanvas);
                coins.Remove(coin);

                Cherry cherry = new Cherry(coin.X, coin.Y, settings.CellSize);
                cherries.Add(cherry);


                // Helyezd el a cseresznyét a canvason
                Canvas.SetLeft(cherry.Shape, cherry.X * settings.CellSize + settings.CellSize / 4);
                Canvas.SetTop(cherry.Shape, cherry.Y * settings.CellSize + settings.CellSize / 4);
                GameCanvas.Children.Add(cherry.Shape);
            }

        }
    }
}