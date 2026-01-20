Cámara con scroll que se centra en un *objeto invisible*, si colisiona con otro objeto invisible, la cámara transisiona al centro de dicho objeto.

La cámara está enfocada en el centro de la pantalla y muestra toda la sala

Cuando el personaje pasa a la siguiente sala, la cámara transiciona hacia la siguiente sala con un suavizado de movimiento

***Funcionamiento de la Cámara:***
###### ***--ETAPA 1--*** *20/01/2026*
La cámara usa el sitema de colosiones de Unity con etiquetas. 
El Objeto de la Cámara es hijo de un objeto "Controlador de Cámara" quien realiza el movimiento, el Controlador de Cámara Depende del Movimiento del Jugador(Hay que adaptar Estructura del Script de la Cámara al Script de Movimiento Final)

La cámara se centra en una pantalla colisionando con un Objeto Trasnparente del mismo tamaño. Si el Controlador de Cámara Colisiona con un objeto asociado a una pantalla, la cámara se fija en el centro del objeto, mostrando toda la Pantalla. Solo cuando el controlador de cámara colisiona con el objetode de la siguiente habitación con etiqueta de cambio de pantalla, la cámara se fija en el centro del siguiente objeto, y por ende, de la siguiente habitación.

![[Celeste_Gameplay_CameraExample.gif]]
[^1] ***Objetivo Final***
La Cámara Actual no mantiene seguimiento del jugador si es que la sala es más grande que la cámara. Corrección de este problema en siguiente etapa de progreso de Movimiento de Cámara

###### ***--ETAPA 2--***######

EN PROGRESO










###### **Bibliografía**
[^1]: Giff De Ejemplo | Gameplay de: ***Celeste***	
