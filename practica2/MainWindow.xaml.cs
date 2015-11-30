using Microsoft.Kinect;
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
        private bool kinectEnabled = false;
        private System.Timers.Timer newBallTimer;
        private int countdown = Utilities.COUNTDOWN;
        private KinectUtilities helper;

        private Jugador playerOne, playerTwo;
        private Pelota ball;

        public MainWindow() {
            InitializeComponent();
            // Creamos un nuevo objeto de Kinect
            helper = new KinectUtilities();
            helper.ToggleSeatedMode(true);

            // Asignamos a la clase el método a ejecutar en cada cambio del esqueleto del jugador
            helper.SkeletonDataChanged += this.SkeletonDataChanged;
            SkeletonImage.Source =  helper.getSkeletonImage();

            // Comprobamos que el Kinect esté conectado
            testKinect();
        }

        /// <summary>
        /// Función que comprueba si el Kinect está conectado
        /// </summary>
        private void testKinect() {
            if (!helper.isKinectConnected()) {
                gameLabel.Content = "kinect no conectado";
                startGameButton.IsEnabled = false;                
                kinectEnabled = false;
            } else {
                kinectEnabled = true;
            }
        }

        /// <summary>
        /// Función que inicializa las variables del juego
        /// </summary>
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

            scoreOneLabel.Content = playerOne.getScore().ToString();
            scoreTwoLabel.Content = playerTwo.getScore().ToString();

            startThread();            
        }

        /// <summary>
        /// Inicialización de la hebra del juego
        /// </summary>
        private void startThread() {
            gameThread = new Thread(new ThreadStart(this.GameLoop));
            gameThread.IsBackground = true;
        }

        /// <summary>
        /// Función de nueva ronda, inicia el contador y pone en pausa el juego hasta que se inicia
        /// </summary>
        private void NewRound() {
            // Pausamos el juego
            gamePaused = true;
            // Iniciamos el contador
            countdown = Utilities.COUNTDOWN;
            gameLabel.Content = countdown;
            newBallTimer.Start();
        }

        /// <summary>
        /// Función de dibujado de los elementos
        /// </summary>
        private void Draw() {
            Canvas.SetLeft(playerOne.getPaddleShape(), playerOne.getPaddlePosition().X);
            Canvas.SetTop(playerOne.getPaddleShape(), playerOne.getPaddlePosition().Y);
            Canvas.SetLeft(playerTwo.getPaddleShape(), playerTwo.getPaddlePosition().X);
            Canvas.SetTop(playerTwo.getPaddleShape(), playerTwo.getPaddlePosition().Y);
            
            Canvas.SetLeft(ball.getShape(), ball.getPosition().X);
            Canvas.SetTop(ball.getShape(), ball.getPosition().Y);
        }

        /// <summary>
        /// Función que maneja la hebra del juego
        /// </summary>
        private void GameLoop() {
            while (true) {
                testKinect();
                    
                if (!gamePaused && gameStarted && kinectEnabled) {
                    Thread.Sleep(1000 / Utilities.FRAME_RATE);
                    this.Dispatcher.Invoke((Action)(() => {
                        Update();
                        Draw();
                    }));
                }
            }
        }

        /// <summary>
        /// Función de actualización de los elementos en cada frame
        /// </summary>
        private void Update() {
            // Si es un juego nuevo iniciamos el contador
            if (newGame) {
                NewRound();
                newGame = false;
            }

            // Actualizamos la posición de la pelota
            ball.updatePosition();

            // Comprobamos si la pelota debe rebotar
            if (ball.getPosition().Y < 0 || ball.getPosition().Y > GameCanvas.Height - Utilities.BALL_RADIUS) {
                Utilities.Play(Utilities.Sound.Bounce);
                ball.bounceBallVertical();
            }

            // Comprobamos si han marcado algún punto los jugadores
            if(checkIfGoalOver(playerOne)) {
                Utilities.Play(Utilities.Sound.Goal);
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

            // Comprobamos si hay un ganador
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
                    Utilities.Play(Utilities.Sound.Bounce);
                    ball.bounceBallHorizontal(rival, GameCanvas.Height);
                } else if(isScore){
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Función del contador
        /// Decrementa el contador hasta que llega a cero y comenzamos el juego
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void countdownTick(object o, ElapsedEventArgs e) {
            // Decrementamos el contador
            countdown--;
            // Si ha acabado el contador iniciamos el juego
            if (countdown == 0) {
                gamePaused = false;
                this.Dispatcher.Invoke((Action)(() => {
                    gameLabel.Content = "";
                }));
                newBallTimer.Stop();
            }
            // Si aún no ha acabado el contador solo actualizamos la etiqueta
            else {
                this.Dispatcher.Invoke((Action)(() => {
                    gameLabel.Content = countdown;
                }));
            }

        }

        /// <summary>
        /// Cierre de la ventana.
        /// Cerramos la hebra del juego y apagamos el Kinect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (gameThread != null) {
                gameThread.Abort();
                helper.closeKinect();
            }
        }

        /// <summary>
        /// Evento click del botón de Inicio del juego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startGameButton_Click(object sender, RoutedEventArgs e) {
            ToggleStart();
        }

        /// <summary>
        /// Función de inicialición del comienzo de un nuevo juego
        /// </summary>
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

        /// <summary>
        /// Evento de actualización del esqueleto.
        /// Reconoce la posición de los jugadores en cada frame y los muestra en la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkeletonDataChanged(object sender, KinectUtilities.SkeletonDataChangeEventArgs e) {
            if (gameThread == null)
                return;

            // Comprobamos la posición de los jugadores
            Skeleton right = null;
            Skeleton left = null;

            for (int i = 0; i < e.skeletons.Length; i++) {
                Skeleton skel = e.skeletons[i];
                if (skel.TrackingState == SkeletonTrackingState.Tracked) {
                    Point position = helper.SkeletonPointToScreen(skel.Joints[JointType.ShoulderCenter].Position);
                    
                    if ((position.X > 0 && position.X <= GameCanvas.Width / 2) && left == null)
                        left = skel;                    
                    else if ((position.X > GameCanvas.Width / 2 && position.X < GameCanvas.Width) && right == null)
                        right = skel;
                }
                
                if (left != null & right != null)
                    break;
            }

            // Mostramos el marcador del jugador solo en el caso de que esté jugando
            if (left == null) {
                playerOne.setVisibility(false);
                playerOne.isConnected = false;
            } else {
                playerOne.isConnected = true;
                Point playerOneHand = helper.SkeletonPointToScreen(left.Joints[Utilities.HANDTRACK].Position);
                Point playerOneHead = helper.SkeletonPointToScreen(left.Joints[JointType.Head].Position);
                
                playerOne.updatePosition(playerOneHead, playerOneHand);
            }

            // Mostramos el marcador del jugador solo en el caso de que esté jugando
            if (right == null) {
                playerTwo.setVisibility(false);
                playerTwo.isConnected = false;
            } else {
                playerTwo.isConnected = true;
                Point playerTwoHand = helper.SkeletonPointToScreen(right.Joints[Utilities.HANDTRACK].Position);
                Point playerTwoHead = helper.SkeletonPointToScreen(right.Joints[JointType.Head].Position);
                
                playerTwo.updatePosition(playerTwoHead, playerTwoHand);
            }
            
            this.Dispatcher.Invoke((Action)(() => {
                Canvas.SetLeft(playerOne.getMarkShape(), playerOne.getPlayerPosition().X);
                Canvas.SetTop(playerOne.getMarkShape(), playerOne.getPlayerPosition().Y);
                Canvas.SetLeft(playerTwo.getMarkShape(), playerTwo.getPlayerPosition().X);
                Canvas.SetTop(playerTwo.getMarkShape(), playerTwo.getPlayerPosition().Y);
            }));
        }
    }
}
