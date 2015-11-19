using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace practica2 {
    class Pelota {
        /// <summary>
        /// ´Número aleatorio para el angulo de rebote
        /// </summary>
        private Random rand; 
               
        /// <summary>
        /// <see cref="Ellipse"/> que representa la pelota
        /// </summary>
        private Ellipse ball;

        /// <summary>
        /// Posición de la pelota
        /// </summary>
        private Point position;

        /// <summary>
        /// Dirección de la pelota
        /// </summary>
        private Vector fwd;

        /// <summary>
        /// Velocidad de la pelota
        /// </summary>
        private double speed;

        /// <summary>
        /// Constructor del objeto pelota
        /// </summary>
        public Pelota() {
            ball = new Ellipse();
            position = new Point();
            fwd = new Vector();
            rand = new Random();

            ball.Height = Utilities.BALL_RADIUS;
            ball.Width = Utilities.BALL_RADIUS;
            ball.Stroke = new SolidColorBrush(Colors.Black);
            ball.StrokeThickness = 2;
            ball.Fill = new SolidColorBrush(Colors.Green);
        }

        /// <summary>
        /// Resetea la posición de la pelota al centro de la pantalla de juego
        /// </summary>
        /// <param name="canvasWidth"></param>
        /// <param name="canvasHeight"></param>
        public void resetBall(double canvasWidth, double canvasHeight) {
            position = new Point(canvasWidth / 2 - Utilities.BALL_RADIUS, canvasHeight / 2 - Utilities.BALL_RADIUS / 2);
            speed = Utilities.BALL_SPEED;

            double angle = (rand.NextDouble() > .5 ? 1 : -1) * rand.NextDouble() * 90 + 45;
            fwd = new Vector(Math.Sin(Utilities.degreeToRadians(angle)), Math.Cos(Utilities.degreeToRadians(angle)));
        }

        /// <summary>
        /// Actualiza la posición de la pelota a partir de su velocidad y dirección.
        /// </summary>
        public void updatePosition() {
            position = Vector.Add(fwd * speed, position);
        }

        /// <summary>
        /// Rebote vertical de la pelota. 
        /// Usado cuando la pelota choca contra los márgenes.
        /// </summary>
        public void bounceBallVertical() {
            fwd.Y = -fwd.Y;
        }

        /// <summary>
        /// Rebote horizontal de la pelota.
        /// Usado cuando la pelota choca contra la barra.
        /// </summary>
        /// <param name="player">Jugador contra el que choca</param>
        /// <param name="canvasHeight">Altura del canvas</param>
        public void bounceBallHorizontal(Jugador player, double canvasHeight) {
            position.X = player.getPosition().X + Utilities.PADDLE_WIDTH + (player.getSide() == Jugador.Side.Player1 ? 3 : -3);
            fwd.Y = ((player.getPosition().Y - player.getPaddlePositionPast().Y) / canvasHeight) * 2;

            fwd.X = -fwd.X;
            increaseSpeed();
        }

        /// <summary>
        /// Aumento de la velocidad de la pelota, por defecto en 0.5
        /// </summary>
        /// <param name="cant"></param>
        private void increaseSpeed(double cant = .5) {
            if (speed < Utilities.BALL_MAX_SPEED)
                speed += cant;
        }

        /// <summary>
        /// Devuelve un <see cref="Point"/> con la posición de la pelota
        /// </summary>
        /// <returns></returns>
        public Point getPosition() {
            return position;
        }

        /// <summary>
        /// Devuelve un <see cref="Vector"/> con la dirección de la pelota.
        /// </summary>
        /// <returns></returns>
        public Vector getForward() {
            return fwd;
        }
    }
}
