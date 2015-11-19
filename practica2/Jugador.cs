using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace practica2 {
    class Jugador{

        /// <summary>
        /// Enumerador con el número de jugadores
        /// </summary>
        public enum Side { Player1, Player2}

        /// <summary>
        /// Rectangulo que representa la barra
        /// </summary>
        private Rectangle paddle;

        /// <summary>
        /// Posición de la barra
        /// </summary>
        private Point paddlePosition;

        /// <summary>
        /// Posición anterior de la barra
        /// </summary>
        private Point paddlePositionPast;

        /// <summary>
        /// Posición del jugador
        /// </summary>
        private Point position;

        /// <summary>
        /// Elipse que representa el jugador
        /// </summary>
        private Ellipse mark;


        /// <summary>
        /// Puntuación del jugador
        /// </summary>
        private int score;

        /// <summary>
        /// Contiene el número del jugador
        /// </summary>
        private Side side;


        /// <summary>
        /// Inicializa el Jugador a partir de su <see cref="Side"/>
        /// </summary>
        /// <param name="side"></param>
        public Jugador(Side side) {
            //Inicializamos las variables
            paddlePosition = new Point();
            paddlePositionPast = new Point();
            position = new Point();
            this.side = side;
            score = 0;

            //Creamos el puntero para ver donde se encuentra la mano del jugador
            mark = new Ellipse();
            mark.Width = Utilities.PLAYER_ELLIPSE_RADIUS;
            mark.Height = Utilities.PLAYER_ELLIPSE_RADIUS;

            //Creamos la barra del jugador
            paddle = new Rectangle();
            paddle.Width = Utilities.PADDLE_WIDTH;
            paddle.Height = Utilities.PADDLE_HEIGHT; ;
            paddle.Stroke = new SolidColorBrush(Colors.Black);
            paddle.StrokeThickness = 2;

            //Coloreamos según el jugador que sea
            switch (side) {
                case Side.Player1:
                mark.Fill = new SolidColorBrush(Colors.Blue);
                paddle.Fill = new SolidColorBrush(Colors.Blue);
                break;
                case Side.Player2:
                mark.Fill = new SolidColorBrush(Colors.Red);
                paddle.Fill = new SolidColorBrush(Colors.Red);
                break;
            }
           
        }
        
        /// <summary>
        /// Inicia la posición del jugador a partir del tamaño del canvas en el que jugamos
        /// </summary>
        /// <param name="canvasWidth">Anchura del canvas</param>
        /// <param name="canvasHeight">Altura del canvas</param>
        public void initiatePosition(double canvasWidth, double canvasHeight) {
            switch (side) {
                case Side.Player1:
                paddlePosition.X = Utilities.MARGIN_OFFSET;
                break;
                case Side.Player2:
                paddlePosition.X = canvasWidth - paddle.Width - Utilities.MARGIN_OFFSET;
                break;
            }
            paddlePosition.Y = canvasHeight / 2 - Utilities.PADDLE_HEIGHT / 2;
            paddlePositionPast = paddlePosition;
        }


        /// <summary>
        /// Actualiza la posición del jugador a partir de la posición de la cabeza y de la mano derecha
        /// </summary>
        /// <param name="positionHead">Coordenadas de la cabeza</param>
        /// <param name="positionHand">Coordenadas de la mano</param>
        public void updatePosition(Point positionHead, Point positionHand) {
            paddlePositionPast = paddlePosition;
            paddlePosition.Y = positionHand.Y - paddle.Height / 2;

            position.X = positionHead.X - mark.Width / 2;
            position.Y = positionHead.Y - mark.Height / 2;
        }


        /// <summary>
        /// Devuelve un <see cref="Ellipse"/> posicionado en las coordenadas del jugador
        /// </summary>
        /// <returns>Elipse con la posición del jugador</returns>
        public Ellipse getMark() {
            return mark;
        }


        /// <summary>
        /// Devuelve un <see cref="Point"/> con la posición del jugador
        /// </summary>
        /// <returns></returns>
        public Point getPosition() {
            return position;
        }

        /// <summary>
        /// Devuelve la puntuación del jugador
        /// </summary>
        /// <returns></returns>
        public int getScore() {
            return score;
        }

        /// <summary>
        /// Incrementa la puntuación del jugador, por defecto en 1
        /// </summary>
        /// <param name="cant"></param>
        public void increaseScore(int cant = 1) {
            score += cant;
        }

        /// <summary>
        /// Devuelve un <see cref="Point"/> con la posición de la barra
        /// </summary>
        /// <returns></returns>
        public Point getPaddlePosition() {
            return paddlePosition;
        }


        /// <summary>
        /// Devuelve un <see cref="Point"/> con la posición anterior de la barra
        /// </summary>
        /// <returns></returns>
        public Point getPaddlePositionPast() {
            return paddlePositionPast;
        }

        /// <summary>
        /// Devuelve el lado del jugador
        /// </summary>
        /// <returns></returns>
        public Side getSide() {
            return side;
        }
    }
}
