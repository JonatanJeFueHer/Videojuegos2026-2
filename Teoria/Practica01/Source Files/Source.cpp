//Base_Pong.cpp : A bouncing ball 

//#include <windows.h> //the windows include file, required by all windows applications
#include <GL/glut.h> //the glut file for windows operations
					 // it also includes gl.h and glu.h for the openGL library calls
#include <math.h>

#define PI 3.1415926535898 

double xpos, ypos, ydir, xdir;         // x and y position for house to be drawn
double sx, sy, squash;          // xy scale factors
double rot, rdir;             // rotation
double ball_speed;

// --- PALETAS ---
#define PADDLE_W  5.0f
#define PADDLE_H  25.0f

float paddle1_y = 60.0f;   // Jugador 1 (izquierda)
float paddle2_y = 60.0f;   // Jugador 2 (derecha)
float paddle_speed = 3.0f;

// --- VARIABLES DE TECLAS PRESIONADAS ---
bool key_w = false, key_s = false;
bool key_up = false, key_down = false;


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

GLfloat RadiusOfBall = 15.;
// Draw the ball, centered at the origin
void draw_ball() {
	glColor3f(0.6, 0.3, 0.);
	MyCircle2f(0., 0., RadiusOfBall);

}

void draw_paddle(float x, float y) {
	glBegin(GL_QUADS);
	glVertex2f(x, y - PADDLE_H / 2);
	glVertex2f(x + PADDLE_W, y - PADDLE_H / 2);
	glVertex2f(x + PADDLE_W, y + PADDLE_H / 2);
	glVertex2f(x, y + PADDLE_H / 2);
	glEnd();
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

	// Mover paleta 1 (W/S) con límites de pantalla
	if (key_w && paddle1_y + PADDLE_H / 2 < 120) paddle1_y += paddle_speed;
	if (key_s && paddle1_y - PADDLE_H / 2 > 0)   paddle1_y -= paddle_speed;

	// Mover paleta 2 (flechas) con límites de pantalla
	if (key_up && paddle2_y + PADDLE_H / 2 < 120) paddle2_y += paddle_speed;
	if (key_down && paddle2_y - PADDLE_H / 2 > 0)   paddle2_y -= paddle_speed;


	// swap the buffers
	glutSwapBuffers();

	//clear all pixels with the specified clear color
	glClear(GL_COLOR_BUFFER_BIT);
	// 160 is max X value in our world


	  // Shape has hit the ground! Stop moving and start squashing down and then back up 
	if (ypos == RadiusOfBall && ydir == -1) {
		sy = sy * squash;

		if (sy < 0.8)
			// reached maximum suqash, now unsquash back up 
			squash = 1.1;
		else if (sy > 1.) {
			// reset squash parameters and bounce ball back upwards
			sy = 1.;
			squash = 0.9;
			ydir = 1;
		}
		sx = 1. / sy;

		// 120 is max Y value in our world

	}
	else {

		// Mover pelota en X
		xpos += xdir * ball_speed;

		// Rebotar en techo y suelo
		if (ypos >= 120 - RadiusOfBall) ydir = -1;
		if (ypos <= RadiusOfBall)       ydir = 1;

		// Colisión con paleta izquierda (Jugador 1)
		if (xpos - RadiusOfBall <= 10.0f &&
			ypos >= paddle1_y - PADDLE_H / 2 &&
			ypos <= paddle1_y + PADDLE_H / 2) {
			xdir = 1;
		}

		// Colisión con paleta derecha (Jugador 2)
		if (xpos + RadiusOfBall >= 150.0f &&
			ypos >= paddle2_y - PADDLE_H / 2 &&
			ypos <= paddle2_y + PADDLE_H / 2) {
			xdir = -1;
		}


		// set Y position to increment 1.5 times the direction of the bounce
		ypos += ydir * ball_speed;

		// If ball touches the top, change direction of ball downwards
		if (ypos == 120 - RadiusOfBall) {
			ydir = -1;
		}
		// If ball touches the bottom, change direction of ball upwards
		else if (ypos < RadiusOfBall)
			ydir = 1;
	}

	/*  //reset transformation state
	  glLoadIdentity();

	  // apply translation
	  glTranslatef(xpos,ypos, 0.);

	  // Translate ball back to center
	  glTranslatef(0.,-RadiusOfBall, 0.);
	  // Scale the ball about its bottom
	  glScalef(sx,sy, 1.);
	  // Translate ball up so bottom is at the origin
	  glTranslatef(0.,RadiusOfBall, 0.);
	  // draw the ball
	  draw_ball();
	*/

	//Translate the bouncing ball to its new position
	T[12] = xpos;
	T[13] = ypos;
	glLoadMatrixf(T);

	T1[13] = -RadiusOfBall;
	// Translate ball back to center
	glMultMatrixf(T1);
	S[0] = sx;
	S[5] = sy;
	// Scale the ball about its bottom
	glMultMatrixf(S);

	T1[13] = RadiusOfBall;
	// Translate ball up so bottom is at the origin

	glMultMatrixf(T1);

	draw_ball();
	// Dibujar paletas
	glLoadIdentity();
	glColor3f(1.0f, 1.0f, 1.0f);
	draw_paddle(5.0f, paddle1_y);   // Paleta izquierda
	draw_paddle(150.0f, paddle2_y);   // Paleta derecha

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
	ball_speed = 1.5;

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