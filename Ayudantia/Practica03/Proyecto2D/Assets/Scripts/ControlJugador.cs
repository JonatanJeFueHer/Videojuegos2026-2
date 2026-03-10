using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    //Variables para velocidad, aceleración, gravedad y estado vertical.
    public float velocidadActual = 0f;
    public float velocidadMax = 5f;
    public float aceleración = 10f;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jugador = GetComponent<Jugador>();
    }

    // Update is called once per frame
    void Update()
    {
        //Actualización del estado para las variables para animaciones.
        estaCaminando = Mathf.Abs(velocidadActual) > 0.1f;
        estaSaltando = velocidadVertical > 0.1f;
        estaCayendo = velocidadVertical < -0.1f;

        //Calculo manual de deltaTime.
        float delta = Time.time - tiempoAnterior;
        tiempoAnterior = Time.time;

        //Movimiento lateral con aceleración.
        float h = Input.GetAxis("Horizontal");
        velocidadActual += h * aceleración * delta;
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMax, velocidadMax);
        transform.position += new Vector3(velocidadActual * delta, 0, 0);

        //Salto
        if (Input.GetAxis("Jump") > 0 && jugador.enSuelo)
        {
            velocidadVertical = 10f;
            jugador.enSuelo = false;
            tiempoSaltoActual = 0f;
        }
        if (!jugador.enSuelo && Input.GetAxis("Jump") > 0)
        {
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
        if (jugador.enSuelo)
        {
            velocidadVertical = 0;
        }
        else
        {
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

        //Salto con coyote time y buffer.
        if (bufferTimer > 0 && coyoteTimer > 0)
        {
            velocidadVertical = fuerzaSalto;
            jugador.enSuelo = false;
            bufferTimer = 0;
            coyoteTimer = 0;
        }

        //Gravedad mejorada.
        if (velocidadVertical < 0)
            velocidadVertical += gravedadCaida * delta;
        else
            velocidadVertical += gravedad * delta;

        //Movimiento final.
        transform.position += new Vector3(velocidadActual * delta, velocidadVertical * delta, 0);
    }
}
