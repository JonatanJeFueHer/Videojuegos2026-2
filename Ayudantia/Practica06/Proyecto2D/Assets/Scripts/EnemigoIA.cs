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
        rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);

        // Cambia la animación a caminando si el enemigo se está moviendo.
        if (anim != null)
            anim.SetBool("Caminando", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
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

        if (!col.gameObject.CompareTag("Player"))
            return;

        var jugador = col.gameObject.GetComponent<Jugador>();
        if (jugador == null)
            return;

        // Intentar obtener el Rigidbody2D del jugador para comprobar su velocidad vertical.
        var rbJugador = col.gameObject.GetComponent<Rigidbody2D>();

        // Determinar si el jugador golpeó al enemigo desde arriba:
        // - el jugador está por encima en Y (con un pequeño offset)
        // - y su velocidad vertical es negativa (está cayendo).
        bool jugadorEncima = false;
        if (rbJugador != null)
        {
            float offsetY = 0.2f; // ajusta si hace falta
            if (rbJugador.linearVelocity.y < 0f && col.transform.position.y > transform.position.y + offsetY)
            {
                jugadorEncima = true;
            }
        }

        // Fallback: si no hay Rigidbody o la comprobación anterior no detectó el stomp,
        // usar normales de contacto (siempre que existan).
        if (!jugadorEncima && col.contacts != null && col.contacts.Length > 0)
        {
            foreach (var contact in col.contacts)
            {
                // Si la normal del contacto apunta hacia arriba respecto al enemigo, el jugador venía desde arriba.
                if (contact.normal.y > 0.5f)
                {
                    jugadorEncima = true;
                    break;
                }
            }
        }

        if (jugadorEncima)
        {
            // El jugador pisa al enemigo: solo el enemigo recibe daño/desaparece.
            RecibirDaño(1);

            // Rebote opcional del jugador al saltar sobre el enemigo.
            if (rbJugador != null)
            {
                rbJugador.linearVelocity = new Vector2(rbJugador.linearVelocity.x, 8f); // ajusta el valor del rebote
            }
        }
        else
        {
            // Contacto lateral: el jugador recibe daño.
            jugador.RecibirDaño(1);
        }
    }
}
