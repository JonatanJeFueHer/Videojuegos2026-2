# README.md - Instrucciones para Ejecutar el Proyecto Practica03_2D en Unity

## Requisitos Previos
- Instala Unity Hub desde el sitio oficial de Unity, que gestiona las versiones del editor.
- Luego, descarga e instala la versión 6000.3.7f1

## Abrir el Proyecto
- Lanza Unity Hub y ve a la pestaña "Proyectos" 
- Agrega el proyecto desde el disco con el botón Add y selecciona "add project from disk"
- Ve a la ruta Ayudantia/Practica03
- Selecciona el proyecto llamado Proyecto2D
- Cuando termine de cargar el proyecto haz clic en "Proyecto2D" 
- Espera a que se cargue la escena.

## Ejecutar en el Editor
Una vez abierto, presiona el botón **Play** (triángulo ▶️) en la barra superior para ejecutar la escena actual en el modo de juego dentro del Editor. Usa **Pause** (⏸️) para pausar y **Stop** (⏹️) para detener. Verifica la consola para errores o mensajes de debug en scripts o assets.

## Probar el Juego
Después de presionar el botón **Play**:
- Presiona la tecla **D** para ir a la izquierda.
- Presiona la tecla **A** para ir a la derecha.
- Presiona la tecla **Espacio** para saltar un poco y dejala presionada para un poco más.
- Cuando el jugador va cayendo de una plataforma puede presionar la tecla **Espacio** para dar un salto antes de caer completamente al vacio o a otra plataforma (Coyote time).
- En esta versión ya hay una desaceleración, por lo que se puede notar que el sprite se detiene poco a poco hasta no moverse.
- El jugador tambien puede hacer un "doble salto gracias a las la combinación de coyote time  y buffer.
- Se implementa una gravedad manual para mejorar la caida.
- Se mejora el movimiento final del jugador, se alcanza a ver más dinámico.
