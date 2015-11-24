using System;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace practica2 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Thread gameThread;
        private bool gameStarted = false;
        private bool gamePaused = false;
        private bool newGame = true;
        private System.Timers.Timer newBallTimer;
        private int countdown = 3;

        private Jugador playerOne, playerTwo;
        private Pelota ball;

        public MainWindow() {
            InitializeComponent();
        }

        private void initializeGame() {
            GameCanvas.Children.Clear();
            newBallTimer = new System.Timers.Timer();
            newBallTimer.Elapsed += new ElapsedEventHandler(countdownTick);
            newBallTimer.Interval = 1000;

            playerOne = new Jugador(Jugador.Side.Player1);
            playerOne.initiatePosition(GameCanvas.Width, GameCanvas.Height);
            playerTwo = new Jugador(Jugador.Side.Player2);
            playerTwo.initiatePosition(GameCanvas.Width, GameCanvas.Height);
            ball = new Pelota();
            ball.resetBall(GameCanvas.Width, GameCanvas.Height);

            GameCanvas.Children.Add(ball.getShape());
            GameCanvas.Children.Add(playerOne.getPaddleShape());
            GameCanvas.Children.Add(playerTwo.getPaddleShape());
            GameCanvas.Children.Add(playerOne.getMarkShape());
            GameCanvas.Children.Add(playerTwo.getMarkShape());

            startThread();            
        }

        /// <summary>
        /// Inicialización de la hebra del juego
        /// </summary>
        private void startThread() {
            gameThread = new Thread(new ThreadStart(this.GameLoop));
            gameThread.IsBackground = true;
        }

        private void NewRound() {
            // Pause the game
            gamePaused = true;
            // Start the countdown timer
            countdown = 3;
            gameLabel.Content = countdown;
            newBallTimer.Start();
        }

        private void Draw() {
            // Move the paddles to their new positions
            Canvas.SetLeft(playerOne.getPaddleShape(), playerOne.getPaddlePosition().X);
            Canvas.SetTop(playerOne.getPaddleShape(), playerOne.getPaddlePosition().Y);
            Canvas.SetLeft(playerTwo.getPaddleShape(), playerTwo.getPaddlePosition().X);
            Canvas.SetTop(playerTwo.getPaddleShape(), playerTwo.getPaddlePosition().Y);

            // Move the ball to its new position
            Canvas.SetLeft(ball.getShape(), ball.getPosition().X);
            Canvas.SetTop(ball.getShape(), ball.getPosition().Y);
        }

        private void GameLoop() {
            while (true) {
                if (!gamePaused && gameStarted) {
                    Thread.Sleep(1000 / Utilities.FRAME_RATE);
                    this.Dispatcher.Invoke((Action)(() => {
                        Update();
                        Draw();
                    }));
                }
            }
        }

        private void Update() {
            // If it's a new game, start with a countdown
            if (newGame) {
                NewRound();
                newGame = false;
            }

            // Move the ball forward
            ball.updatePosition();

            // Bounce off the side walls
            if (ball.getPosition().Y < 0 || ball.getPosition().Y > GameCanvas.Height - Utilities.BALL_RADIUS) {
                ball.bounceBallVertical();
            }

            if(checkIfGoalOver(playerOne)) {
                ball.resetBall(GameCanvas.Width, GameCanvas.Height);
                playerTwo.increaseScore();
                scoreTwoLabel.Content = playerTwo.getScore().ToString();
                NewRound();
            } else if(checkIfGoalOver(playerTwo)){
                ball.resetBall(GameCanvas.Width, GameCanvas.Height);
                playerOne.increaseScore();
                scoreOneLabel.Content = playerOne.getScore().ToString();
                NewRound();
            }

            // Check for a winner
            if (playerOne.getScore() >= Utilities.WIN_SCORE) {
                ToggleStart();
                gameLabel.Content = "¡Gana Jugador 1!";
            } else if (playerTwo.getScore() >= Utilities.WIN_SCORE) {
                ToggleStart();
                gameLabel.Content = "¡Gana Jugador 2!";
            }
        }

        /// <summary>
        /// Comprueba si el jugador pasado por parámetro ha recibido un tanto
        /// </summary>
        /// <param name="rival"></param>
        /// <param name="ball"></param>
        /// <returns></returns>
        private bool checkIfGoalOver(Jugador rival) {
            bool isInRivalArea = false, isScore = false;
            switch (rival.getSide()) {
                case Jugador.Side.Player1:
                    isInRivalArea = ball.getPosition().X - Utilities.BALL_RADIUS < Utilities.MARGIN_OFFSET + Utilities.PADDLE_WIDTH - Utilities.BALL_RADIUS;
                    isScore = ball.getPosition().X - Utilities.BALL_RADIUS < 0;
                break;
                case Jugador.Side.Player2:
                    isInRivalArea = ball.getPosition().X + Utilities.BALL_RADIUS > GameCanvas.Width - Utilities.MARGIN_OFFSET - Utilities.PADDLE_WIDTH;
                    isScore = ball.getPosition().X + Utilities.BALL_RADIUS > GameCanvas.Width;
                break;
            }

            if (isInRivalArea) {
                if (ball.getPosition().Y < (rival.getPaddlePosition().Y + Utilities.PADDLE_HEIGHT + (Utilities.BALL_RADIUS / 2)) &&
                    ball.getPosition().Y > (rival.getPaddlePosition().Y - (Utilities.BALL_RADIUS / 2))) {
                    ball.bounceBallHorizontal(rival, GameCanvas.Height);
                    //Console.WriteLine("Posición Balón: ["+ball.getPosition().X +", "+ball.getPosition().Y+"]");
                    //Console.WriteLine("Posición Barra: [" + rival.getPaddlePosition().X + ", " + rival.getPaddlePosition().Y + "]");
                    //Console.WriteLine("Posición Jugador: [" + rival.getPlayerPosition().X + ", " + rival.getPlayerPosition().Y + "]");
                    //Console.WriteLine("Canvas| Altura: " + GameCanvas.Height + " / Anchura: " + GameCanvas.Width );
                    Console.WriteLine("Rebote");
                } else if(isScore){
                    //Console.WriteLine("Posición Balón: [" + ball.getPosition().X + ", " + ball.getPosition().Y + "]");
                    //Console.WriteLine("Posición Jugador: [" + rival.getPlayerPosition().X + ", " + rival.getPlayerPosition().Y + "]");
                    //Console.WriteLine("Posición Jugador: [" + rival.getPlayerPosition().X + ", " + rival.getPlayerPosition().Y + "]");
                    //Console.WriteLine("Canvas| Altura: " + GameCanvas.Height + " / Anchura: " + GameCanvas.Width);
                    Console.WriteLine("Gol!");
                    return true;
                }
            }
            return false;
        }

        private void countdownTick(object o, ElapsedEventArgs e) {
            // Decrement the countdown timer
            countdown--;
            // If the countdown is finished, unpuase the game and begin
            if (countdown == 0) {
                gamePaused = false;
                this.Dispatcher.Invoke((Action)(() => {
                    gameLabel.Content = "";
                    //pauseGameButton.IsEnabled = true;
                }));
                newBallTimer.Stop();
            }
            // If the countdown is still going, update the label on the screen
            else {
                this.Dispatcher.Invoke((Action)(() => {
                    gameLabel.Content = countdown;
                }));
            }

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (gameThread != null) {
                gameThread.Abort();
            }
        }

        private void startGameButton_Click(object sender, RoutedEventArgs e) {
            ToggleStart();
        }

        private void ToggleStart() {
            // Iniciamos el juego en el caso de que no lo esté
            if (!gameStarted) {
                // Reiniciando el juego
                newGame = true;
                gameStarted = true;

                initializeGame();
                // Arrancamos la hebra del juego
                gameThread.Start();
                gameLabel.Content = "";
                startGameButton.Background = new SolidColorBrush(Colors.Red);
                startGameButton.Content = "Parar";
            } else {
                // Ponemos el estado del juego a parado
                gameStarted = false;
                // Limpiamos el canvas
                GameCanvas.Children.Clear();
                // Finalizamos la hebra
                gameThread.Abort();
                gameLabel.Content = "Presiona Iniciar para comenzar";
                startGameButton.Background = new SolidColorBrush(Colors.Green);
                startGameButton.Content = "Iniciar";
            }
        }
    }
}
