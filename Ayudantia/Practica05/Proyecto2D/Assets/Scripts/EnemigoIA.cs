using UnityEngine;

public class EnemigoIA : Personaje
{
    public float direccion = -1f; //-1 para izquierda, 1 para derecha.

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);

        // Cambia la animación a caminando si el enemigo se está moviendo.
        if (anim != null)
            anim.SetBool("Caminando", Mathf.Abs(rb.velocity.x) > 0.1f);
    }

    // Patrullaje tipo Goomba (por colisión)
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Pared") || col.gameObject.CompareTag("Obstaculo"))
        {
            direccion *= -1;
            if (sr != null)
                sr.flipX = !sr.flipX; // Voltea el sprite al cambiar de dirección.
            return;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            var jugador = col.gameObject.GetComponent<Jugador>();
            if (jugador != null)
            {
                jugador.RecibirDaño(1); // El jugador recibe daño al colisionar con el enemigo.

                // Si el jugador golpea desde arriba, el enemigo recibe daño.
                if (col.contacts != null && col.contacts.Length > 0 && col.contacts[0].normal.y < 0)
                {
                    RecibirDaño(1);
                }
            }
        }
    }
}
