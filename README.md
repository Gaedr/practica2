## Practica2
## MS-Kinect - Pong.

### Autores
* Samuel Peregrina Morillas
* Nieves Victoria Velásquez Díaz

### Duración de la práctica.
Desde 30-Oct-2015 hasta 30-Nov-2015

### Breve descripción de la practica.
Para la realización de esta práctica se ha procedio a programar una versión del juego pong de forma que puedan jugar dos personas. En esta versión, cada jugador moverá una de sus manos hacia arriba o abajo controlando la paleta para intentar marcar en la portería del contrincante mientras intenta defender la suya. Cada portería está situada en el centro de cada uno de los laterales y asociada a cada jugador.

### Descripción del problema.
Para el desarrollo de esta práctica se han creado una serie de clases entre las que se organiza: 
* La clase **Jugador** tiene el código relacionado al funcionamiento de las palas de los jugadores así como el de las porterías y la configuración de las puntuaciones. En esta se crean las palas y se va modificando su posición según el usuario mueve las manos hacia arriba o abajo. Dentro de los métodos de esta clase tenemos:
	* **Jugador(Side side)**  el constructor de la clase. En él inicializamos las variables relacionadas con la posición actual de las palas, **paddlePosition**, y la posición previa, **paddlePositionPast**. También almacenamos la posición del jugador en **position** y su lado, en **side** para poder diferenciarlo del otro, así como su puntuación en **score**.
	Una vez tenemos las varibles inicializadas, procedemos a crear los puntos dependiendo de la posición de la mano de cada jugador, creamos cada una de las palas mediante la creación de un rectángulo, **new Rectangle()** y le asignaremos una altura y anchura determindas, un color, que sera blanco, y el tamaño del borde del rectángulo, **StrokeThickness**, que en nuestro caso será de 2. Finalmente le colorearemos dicho borde de un color u otro dependiendo del lado de la pantalla de foma que se puedan diferenciar claramente los jugadores.
	* **initiatePosition(double canvasWidth, double canvasHeight)** dadas la altura y anchura del canvas, con este método procederemos a asignar las palas a cada jugador dependiendo del lado de la pantalla en el que se encuentre y las situa a ambos lados de la pantalla.
	* **updatePosition(Point positionHead, Point positionHand)**, en este método actualizamos la posición que teniamos en un principio **paddlePositionPast**, tras esto sitúa el centro de la mano a la misma altura a la que está la mano del jugador modificando la **"y"** de la variable anterior. Por otra parte, en **position** se almacena la posición de la cabeza de cada jugador para asociarlos a cada lado. Al realizar la resta, nos aseguramos que el punto que se mostrará al detectar a los jugadores estará situado en el centro de cada una de sus cabezas. de forma que se pueda ver si ambos jugadores estan muy juntos o si falta alguno.
	* El resto de métodos de esta clase son principalmente *metodos set* con los que acceder a las variables privadas de la clase y el método **increaseScore(int cant = 1)** con el que aumentamos la puntuación del jugador que marca en uno.
* La clase **Pelota** se trata todo lo relacionado con el movimiento de la pelota por la pantalla.

###Funcionamiento.
### Errores y aspectos destacados.

### Bibliografía.
* [Curso C#: Ejercicio. Diseñar el juego Pong ](http://curso-mcts.blogspot.com.es/2011/03/ejercicio-disenar-el-juego-pong.html)
* [[P] C# + XNA Introductorio 1 PONG - Descodificando ](http://www.3djuegos.com/comunidad-foros/tema/28358171/0/p-c-xna-introductorio-1-pong/)
* [bencentra/kinect-pong ](https://github.com/bencentra/kinect-pong)
* [Tutorial: “Pong” con Wave Engine | Game Development](http://www.gamedev.es/?p=7972)
* [el método burbuja( ): Ejemplo #2: Otra versión de PONG, en JAVA](http://elmetodoburbuja.blogspot.com.es/2012/10/ejemplo-2-otra-version-de-pong-en-java.html)
* [Pong\++ Programación de un pong en C++ ](https://ullpong.wordpress.com/)
