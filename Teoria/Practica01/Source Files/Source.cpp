//Base_Pong.cpp : A bouncing ball 

//#include <windows.h> //the windows include file, required by all windows applications
#include <GL/glut.h> //the glut file for windows operations
					 // it also includes gl.h and glu.h for the openGL library calls
#include <math.h>

#include <stdio.h>


#define PI 3.1415926535898 

double xpos, ypos, ydir, xdir;         // x and y position for house to be drawn
double sx, sy, squash;          // xy scale factors
double rot, rdir;             // rotation
double ball_speed;            //velocidad de la pelota.

// --- PALETAS ---
#define PADDLE_W  5.0f
#define PADDLE_H  25.0f

float paddle1_y = 60.0f;   // Jugador 1 (izquierda)
float paddle2_y = 60.0f;   // Jugador 2 (derecha)
float paddle_speed = 100.0f; // unidades/segundo

// --- DELTA TIME ---
double lastTime = 0.0; // tiempo anterior en segundos

// --- VARIABLES DE TECLAS PRESIONADAS ---
bool key_w = false, key_s = false;
bool key_up = false, key_down = false;

//--- VARIABLES PARA PUNTAJE ---
int score1 = 0, score2 = 0;

GLfloat T1[16] = { 1.,0.,0.,0.,\
				  0.,1.,0.,0.,\
				  0.,0.,1.,0.,\
				  0.,0.,0.,1. };
GLfloat S[16] = { 1.,0.,0.,0.,\
				 0.,1.,0.,0.,\
				 0.,0.,1.,0.,\
				 0.,0.,0.,1. };
GLfloat T[16] = { 1.,0.,0.,0.,\
				 0., 1., 0., 0.,\
				 0.,0.,1.,0.,\
				 0.,0.,0.,1. };



#define PI 3.1415926535898 
GLint circle_points = 100;
void MyCircle2f(GLfloat centerx, GLfloat centery, GLfloat radius) {
	GLint i;
	GLdouble angle;
	glBegin(GL_POLYGON);
	for (i = 0; i < circle_points; i++) {
		angle = 2 * PI * i / circle_points;
		glVertex2f(centerx + radius * cos(angle), centery + radius * sin(angle));
	}
	glEnd();
}

GLfloat RadiusOfBall = 5.;
// Draw the ball, centered at the origin
void draw_ball() {
	glColor3f(0.6, 0.3, 0.);
	MyCircle2f(0., 0., RadiusOfBall);

}
// Dibuja una paleta, con el centro en (x,y).
void draw_paddle(float x, float y) {
	glBegin(GL_QUADS);
	glVertex2f(x, y - PADDLE_H / 2);
	glVertex2f(x + PADDLE_W, y - PADDLE_H / 2);
	glVertex2f(x + PADDLE_W, y + PADDLE_H / 2);
	glVertex2f(x, y + PADDLE_H / 2);
	glEnd();
}

// Imprime marcador en consola y fuerza el flush para ver salida inmediatamente
void printScoreToConsole()
{
	printf(">> PUNTO! Jugador 1: %d  |  Jugador 2: %d\n", score1, score2);
	fflush(stdout);
}

//Funciones de teclado.
void keyboard(unsigned char key, int x, int y) {
	if (key == 'w' || key == 'W') key_w = true;
	if (key == 's' || key == 'S') key_s = true;
}

void keyboardUp(unsigned char key, int x, int y) {
	if (key == 'w' || key == 'W') key_w = false;
	if (key == 's' || key == 'S') key_s = false;
}

void specialKeys(int key, int x, int y) {
	if (key == GLUT_KEY_UP)   key_up = true;
	if (key == GLUT_KEY_DOWN) key_down = true;
}

void specialKeysUp(int key, int x, int y) {
	if (key == GLUT_KEY_UP)   key_up = false;
	if (key == GLUT_KEY_DOWN) key_down = false;
}


void Display(void)
{
    // --- DELTA TIME ---
    double currentTime = glutGet(GLUT_ELAPSED_TIME) / 1000.0;
    double deltaTime = currentTime - lastTime;
    if (deltaTime < 0.0) deltaTime = 0.0;
    if (deltaTime > 0.05) deltaTime = 0.05;
    lastTime = currentTime;

    // --- MOVER PALETAS ---
    if (key_w) paddle1_y += paddle_speed * (float)deltaTime;
    if (key_s) paddle1_y -= paddle_speed * (float)deltaTime;
    if (paddle1_y + PADDLE_H / 2 > 120.0f) paddle1_y = 120.0f - PADDLE_H / 2;
    if (paddle1_y - PADDLE_H / 2 < 0.0f)   paddle1_y = PADDLE_H / 2;

    if (key_up)   paddle2_y += paddle_speed * (float)deltaTime;
    if (key_down) paddle2_y -= paddle_speed * (float)deltaTime;
    if (paddle2_y + PADDLE_H / 2 > 120.0f) paddle2_y = 120.0f - PADDLE_H / 2;
    if (paddle2_y - PADDLE_H / 2 < 0.0f)   paddle2_y = PADDLE_H / 2;

    glClear(GL_COLOR_BUFFER_BIT);

    // --- LÓGICA DE PELOTA ---
    if (ypos <= RadiusOfBall && ydir <= -0.5) {
        sy = sy * squash;
        if (sy < 0.8)
            squash = 1.1;
        else if (sy > 1.) {
            sy = 1.;
            squash = 0.9;
            ydir = 1;
        }
        sx = 1. / sy;
    }
    else {
        xpos += xdir * ball_speed * deltaTime;
        ypos += ydir * ball_speed * deltaTime;

        // Rebotar techo y suelo
        if (ypos >= 120 - RadiusOfBall) { ypos = 120 - RadiusOfBall; ydir = -1; }
        if (ypos <= RadiusOfBall) { ypos = RadiusOfBall;        ydir = 1; }

        // Colisión paleta izquierda
        if (xpos - RadiusOfBall <= 10.0f &&
            ypos >= paddle1_y - PADDLE_H / 2 &&
            ypos <= paddle1_y + PADDLE_H / 2) {
            xdir = 1;
            xpos = 10.0f + RadiusOfBall; // evitar que quede atrapada
        }

        // Colisión paleta derecha
        if (xpos + RadiusOfBall >= 150.0f &&
            ypos >= paddle2_y - PADDLE_H / 2 &&
            ypos <= paddle2_y + PADDLE_H / 2) {
            xdir = -1;
            xpos = 150.0f - RadiusOfBall; // evitar que quede atrapada
        }

        if (xpos > 160) {
            score1++;
            printScoreToConsole();
            xpos = 80; ypos = 60;
            xdir = -1; ydir = 1;
            sx = sy = 1.; squash = 0.9;
        }
        if (xpos < 0) {
            score2++;
            printScoreToConsole();
            xpos = 80; ypos = 60;
            xdir = 1; ydir = 1;
            sx = sy = 1.; squash = 0.9;
        }
    }

    // --- DIBUJAR PELOTA ---
    T[12] = xpos;
    T[13] = ypos;
    glLoadMatrixf(T);
    T1[13] = -RadiusOfBall;
    glMultMatrixf(T1);
    S[0] = sx; S[5] = sy;
    glMultMatrixf(S);
    T1[13] = RadiusOfBall;
    glMultMatrixf(T1);
    draw_ball();

    // --- DIBUJAR PALETAS ---
    glLoadIdentity();
    glColor3f(1.0f, 1.0f, 1.0f);
    draw_paddle(5.0f, paddle1_y);
    draw_paddle(150.0f, paddle2_y);

    glutSwapBuffers();
    glutPostRedisplay();
}


void reshape(int w, int h)
{
	// on reshape and on startup, keep the viewport to be the entire size of the window
	glViewport(0, 0, (GLsizei)w, (GLsizei)h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	// keep our logical coordinate system constant
	gluOrtho2D(0.0, 160.0, 0.0, 120.0);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

}


void init(void) {
	//set the clear color
	glClearColor(0.0, 0.8, 0.0, 1.0);
	// initial position set to 0,0
	xpos = 80; ypos = RadiusOfBall; xdir = 1; ydir = 1;
	sx = 1.; sy = 1.; squash = 0.9;
	rot = 0;
	ball_speed = 60.0; // unidades/segundo

	// inicializar tiempo
	lastTime = glutGet(GLUT_ELAPSED_TIME) / 1000.0;
}


int main(int argc, char* argv[])
{

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB);
	glutInitWindowSize(320, 240);
	glutCreateWindow("Bouncing Ball");
	init();
	glutDisplayFunc(Display);
	glutReshapeFunc(reshape);
	glutKeyboardFunc(keyboard);
	glutKeyboardUpFunc(keyboardUp);
	glutSpecialFunc(specialKeys);
	glutSpecialUpFunc(specialKeysUp);
	glutMainLoop();

	return 1;
}