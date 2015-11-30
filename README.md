## Práctica2
## MS-Kinect - Pong.

### Autores
* Samuel Peregrina Morillas
* Nieves Victoria Velásquez Díaz

### Duración de la práctica.
Desde 30-Oct-2015 hasta 30-Nov-2015

### Breve descripción de la practica.
Para la realización de esta práctica se ha procedio a programar una versión del juego pong de forma que puedan jugar dos personas. En esta versión, cada jugador moverá su mano derecha hacia arriba o abajo controlando la pala para intentar marcar en la portería del contrincante mientras intenta defender la suya. Cada portería está situada en cada uno de los laterales y asociada a cada jugador.

### Descripción del problema.
Para el desarrollo de esta práctica se han creado una serie de clases entre las que se organiza: 
* La clase **Jugador** tiene el código relacionado al funcionamiento de las palas de los jugadores así como el de las porterías y la configuración de las puntuaciones. En esta se crean las palas y se va modificando su posición según el jugador mueve su mano hacia arriba o abajo. Dentro de los métodos de esta clase tenemos:
	* **Jugador(Side side)**  el constructor de la clase. En él inicializamos las variables relacionadas con la posición actual de las palas, **paddlePosition**, y la posición previa, **paddlePositionPast**. También almacenamos la posición del jugador en **position** y su lado, en **side** para poder diferenciarlo del otro, así como su puntuación en **score**.
	Una vez tenemos las varibles inicializadas, procedemos a crear los puntos dependiendo de la posición de la mano de cada jugador, creamos cada una de las palas mediante la creación de un rectángulo, **new Rectangle()** y le asignaremos una altura y anchura determindas, un color, donde cada jugador tendrá el suyo propio según el lado de la pantalla en el que esté situado, de foma que se puedan diferenciar claramente los jugadores, y el tamaño del borde del rectángulo, **StrokeThickness**, que en nuestro caso será de 2. Finalmente le colorearemos dicho borde de color blanco.
	* **initiatePosition(double canvasWidth, double canvasHeight)** dadas la altura y anchura del canvas, con este método procederemos a asignar las palas a cada jugador dependiendo del lado de la pantalla en el que se encuentre y las situa a ambos lados de la pantalla.
	* **updatePosition(Point positionHead, Point positionHand)**, en este método actualizamos la posición que teniamos en un principio **paddlePositionPast**, tras esto sitúa el centro de la mano a la misma altura a la que está la mano del jugador modificando la **"y"** de la variable anterior. Por otra parte, en **position** se almacena la posición de la cabeza de cada jugador para asociarlos a cada lado. Al realizar la resta, nos aseguramos que el punto que se mostrará al detectar a los jugadores estará situado en el centro de cada una de sus cabezas. de forma que se pueda ver si ambos jugadores estan muy juntos o si falta alguno.
	* El resto de métodos de esta clase son principalmente *metodos set* con los que acceder a las variables privadas de la clase y el método **increaseScore(int cant = 1)** con el que aumentamos la puntuación del jugador que marca en uno.
	
* La clase **Pelota** tiene todo lo relacionado con el movimiento de la pelota por la pantalla, asi como su posición a lo largo de la partida y su velocidad. En estea clase, encontramos los sigueintes metodos:
	* **Pelota()**, es el constructor de la clase, en este se establecen valores como la forma de la pelota, con **shape** , la posición que va tomando en cada momento se almacena en **position**, mientras que en **fwd** almacenamos la dirección en la que se moverá la pelota. Finalmente en la variable **rand** almacenamos un número aleatorio que utilizaremos más adelante para varias cosas. En la segunda parte del constructor se establecerá la forma de la pelota asi como su color de la misma forma que hemos hecho antes para dibujar las barras. En este caso será de color verde con el borde blanco.
	* **updatePosition() ** en este método actualizamos la posición de la pelota en función de la velocidad que tiene y de su dirección.
	* **bounceBallVertical()** con este método controlamos el rebote vertical de la pelota de forma que actualizamos el valor y de la variable **fwd** a su valor pero en negativo. Ocurre cuando la pelota choca con los márgenes.
	* **bounceBallHorizontal(Jugador player, double canvasHeight) ** método con el que controlamos el rebote horizontal de la pelota, esto ocurre cuando choca contra las barras de cada jugador y es algo más complejo ya que actualiza el valor de la posicion y de **fwd** a partir de la posición y del jugador y de su pala, mientras que el valor de la coordenada x de **fwd** se actualiza a -1 y se aumenta la velocidad.
	* **increaseSpeed(double cant = .5)** con este método nos limitamos a aumentar el valor de la velocidad a la que se mueve la pelota en 0.5.
	* En los métodos restantes nos limitamos a acceder a las variables privadas de la clase.

* La clase **MainWindow** se encarga de todo lo relacionado con la ventana que va a ver el usuario a la hora de jugar al pong, por lo que es la encargada de mostrar todos los componentes necesarios, asi como un marcador o si está conectado o no el Kinect. Dentro de esta clase encontramos los siguientes métodos.
	* **MainWindow()**, este es el constructor por defecto de la clase y se escarga de inicializar los componentes, tomar los esqueletos de los jugadores para identificarlos y llama a un método que comprueba si está conectado o no el Kinect.
	* **testKinect()** es método comprueba si el Kinect está conectado, en caso de  no estarlo muestra un mensaje por pantalla indicándolo y desactiva el botón con el que poder iniciar el juego hasta que esté conectado.
	* **initializeGame()** en este método inicializamos un timer, establecemos un valor de cuenta atrás, establecemos los lados de en los que se situarán los jugadores, creamos la pelota, las palas de cada uno, y finalmente damos paso al método que ejecuta el juego.
	* **NewRound()** en este método partimos de que el juego está pausado, muestra la cuenta atrás que indica el inicio del juego 


###Funcionamiento.
### Errores y aspectos destacados.

### Bibliografía.
* [Curso C#: Ejercicio. Diseñar el juego Pong ](http://curso-mcts.blogspot.com.es/2011/03/ejercicio-disenar-el-juego-pong.html)
* [[P] C# + XNA Introductorio 1 PONG - Descodificando ](http://www.3djuegos.com/comunidad-foros/tema/28358171/0/p-c-xna-introductorio-1-pong/)
* [bencentra/kinect-pong ](https://github.com/bencentra/kinect-pong)
* [Tutorial: “Pong” con Wave Engine | Game Development](http://www.gamedev.es/?p=7972)
* [el método burbuja( ): Ejemplo #2: Otra versión de PONG, en JAVA](http://elmetodoburbuja.blogspot.com.es/2012/10/ejemplo-2-otra-version-de-pong-en-java.html)
* [Pong\++ Programación de un pong en C++ ](https://ullpong.wordpress.com/)
