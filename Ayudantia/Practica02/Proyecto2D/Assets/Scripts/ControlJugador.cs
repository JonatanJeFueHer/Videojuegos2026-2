using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    //Variables para velocidad, aceleración, gravedad y estado vertical.
    public float velocidadActual = 0f;
    public float velocidadMax = 5f;
    public float aceleración =  10f;
    public float velocidadVertical = 0f;
    public float gravedad = -20f;
    public float tiempoMaxSalto = 0.2f;
    private float tiempoSaltoActual = 0f;
    private float tiempoAnterior;

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
        //Calculo manual de deltaTime.
        float delta = Time.time - tiempoAnterior;
        tiempoAnterior = Time.time;

        //Movimiento lateral con aceleración.
        float h = Input.GetAxis("Horizontal");
        velocidadActual += h * aceleración * delta;
        velocidadActual  = Mathf.Clamp(velocidadActual, - velocidadMax, velocidadMax);
        transform.position += new Vector3(velocidadActual * delta, 0, 0);

        //Salto
        if(Input.GetAxis("Jump") > 0  && jugador.enSuelo)
        {
            velocidadVertical = 10f;
            jugador.enSuelo = false;
            tiempoSaltoActual = 0f;
        }
        if(!jugador.enSuelo && Input.GetAxis("Jump") > 0)
        {
            if(tiempoSaltoActual < tiempoMaxSalto)
            {
                velocidadVertical += -gravedad * delta;
                tiempoSaltoActual += delta;
            }
        }
        if(Input.GetAxis("Jump") == 0)
        {
            tiempoSaltoActual = tiempoMaxSalto;
        }
        if(jugador.enSuelo)
        {
            velocidadVertical = 0;
        }
        else
        {
            velocidadVertical += gravedad * delta;
        }
        transform.position += new Vector3(0,velocidadVertical * delta, 0);
    }
}
