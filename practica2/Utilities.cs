using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practica2 {
    class Utilities {
        private const double DEG_TO_RAD = Math.PI / 180;            // Converting degrees to radians        
        public const double MARGIN_OFFSET = 25;                    // Offset of paddles from edges of canvas
        public const int FRAME_RATE = 60;                          // Frame rate for the animation
        public const int PADDLE_HEIGHT = 120;
        public const int PADDLE_WIDTH = 40;
        public const int PLAYER_ELLIPSE_RADIUS = 75;
        public const int BALL_RADIUS = 25;
        public const double BALL_SPEED = 4;
        public const JointType HANDTRACK = JointType.HandRight;

        public static double degreeToRadians(double deg) {
            return deg * DEG_TO_RAD;
        }
    }
}
