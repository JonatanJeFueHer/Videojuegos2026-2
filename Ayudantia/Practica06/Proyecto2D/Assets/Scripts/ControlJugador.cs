using UnityEngine;

public class ControlJugador : Personaje
{
    //Variables para velocidad, aceleración, gravedad y estado vertical.
    public float velocidadActual = 0f;
    public float velocidadMax = 5f;
    public float aceleracion = 10f;
    public float velocidadVertical = 0f;
    public float gravedad = -20f;
    public float tiempoMaxSalto = 0.2f;
    private float tiempoSaltoActual = 0f;
    private float tiempoAnterior;

    //Variables para movimiento avanzado.
    public float desaceleracion = 8f;
    public float gravedadCaida = -30f;
    public float tiempoCoyote = 0.1f;
    public float tiempoBufferSalto = 0.1f;
    private float coyoteTimer = 0f;
    private float bufferTimer = 0f;
    public float fuerzaSalto = 10f;

    //Variables para animaciones.
    public bool estaCaminando;
    public bool estaSaltando;
    public bool estaCayendo;

    //variable para acceder al script jugador.
    private Jugador jugador;

    //Variable para acceder al Animator.
    private Animator anim;

    //Variables para efectos de sonido.
    public AudioClip sonidoSalto;
    public AudioClip sonidoPaso;
    public AudioSource audioSrc;

    //Variables para disparo.
    public GameObject prefabProyectil;
    public Transform puntoDisparo;

    //Inicializamos referencia a Jugador, Animator y AudioSource.
    void Awake()
    {
        jugador = GetComponent<Jugador>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jugador = GetComponent<Jugador>();
    }

    // Update is called once per frame
    void Update()
    {
        //Disparo.
        if (Input.GetButtonDown("Fire1"))
        {
            Disparar();
        }

        void Disparar()
        {
            GameObject p = Instantiate(prefabProyectil, puntoDisparo.position, Quaternion.identity);

            Vector2 dir = sr.flipX ? Vector2.left : Vector2.right;
            
            float velocidadJugador = velocidadActual;

            p.GetComponent<Proyectil>().Inicializar(dir, velocidadJugador);
        }

        //Actualización del estado para las variables para animaciones.
        estaCaminando = Mathf.Abs(velocidadActual) > 0.1f;
        estaSaltando = velocidadVertical > 0.1f;
        estaCayendo = velocidadVertical < -0.1f;

        //Evitar errores si no hay Animator, pero actualizar las variables de estado de todas formas.
        if (anim != null)
        {
            anim.SetBool("Caminando", estaCaminando);
            anim.SetBool("Saltando", estaSaltando);
            anim.SetBool("Cayendo", estaCayendo);
        }

        //Calculo manual de deltaTime.
        float delta = Time.time - tiempoAnterior;
        tiempoAnterior = Time.time;

        //Movimiento lateral con aceleración.
        float h = Input.GetAxis("Horizontal");
        velocidadActual += h * aceleracion * delta;
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMax, velocidadMax);
        transform.position += new Vector3(velocidadActual * delta, 0, 0);

        // —> Girar sprite según velocidad (negativa = izquierda)
        if (jugador != null)
        {
            jugador.SetFlipX(velocidadActual < 0f);
        }

        //Sonido de pasos.
        if(estaCaminando && jugador.enSuelo)
        {
            if(!audioSrc.isPlaying)
                audioSrc.PlayOneShot(sonidoPaso);
        }

        //Salto
        if (bufferTimer > 0)
        { //Salto con coyote time y buffer.
            if (coyoteTimer > 0)
            {
                velocidadVertical = fuerzaSalto;
                jugador.enSuelo = false;
                audioSrc.PlayOneShot(sonidoSalto);
                bufferTimer = 0;
                coyoteTimer = 0;
            }

            if (tiempoSaltoActual < tiempoMaxSalto)
            {
                velocidadVertical += -gravedad * delta;
                tiempoSaltoActual += delta;
            }
        }
        if (Input.GetAxis("Jump") == 0)
        {
            tiempoSaltoActual = tiempoMaxSalto;
        }
        //Gravedad y movimiento vertical.
        if (jugador.enSuelo)
        {
            velocidadVertical = 0;
        }
        else //Este else lo puse como en la clase, con la gravedad mejorada dentro
        {
            //Gravedad mejorada.
            if (velocidadVertical < 0)
                velocidadVertical += gravedadCaida * delta;
            else
                velocidadVertical += gravedad * delta;
        }
        transform.position += new Vector3(0, velocidadVertical * delta, 0);

        //Desaceleración automática.
        if (h == 0)
        {
            if (velocidadActual > 0)
                velocidadActual -= desaceleracion * delta;
            else if (velocidadActual < 0)
                velocidadActual += desaceleracion * delta;
            if (Mathf.Abs(velocidadActual) < 0.1f)
                velocidadActual = 0;
        }

        //Coyote time.
        if (jugador.enSuelo)
            coyoteTimer = tiempoCoyote;
        else
            coyoteTimer -= delta;

        //Jump buffering.
        if (Input.GetAxis("Jump") > 0)
            bufferTimer = tiempoBufferSalto;
        else
            bufferTimer -= delta;
    }
}