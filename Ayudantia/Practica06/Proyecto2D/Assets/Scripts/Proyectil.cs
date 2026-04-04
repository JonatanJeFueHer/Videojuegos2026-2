using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidadBase = 10f;
    public float tiempoVida = 2f;
    public int daño = 1;

    private Vector2 direccion;
    private float velocidadExtra = 0f;

    public void Inicializar(Vector2 dir, float velocidadJugador)
    {
        direccion = dir.normalized;
        velocidadExtra = velocidadJugador;
        Destroy(gameObject, tiempoVida);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float velocidadFinal = velocidadBase + velocidadExtra;
        transform.position += (Vector3)(direccion * velocidadFinal * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemigo"))
        {
            col.GetComponent<EnemigoIA>().RecibirDaño(daño);
            Destroy(gameObject);
        }

        if (col.CompareTag("Pared") || col.CompareTag("Obstaculo"))
        { 
            Destroy(gameObject);
        }
    }
}
