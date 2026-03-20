using UnityEngine;

//Super clase Personaje para un enemigo básico.
public class Personaje : MonoBehaviour
{
    public float velocidad = 2f;
    public int vida = 1;

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public virtual void RecibirDaño(int cantidad)
    {
        vida -= cantidad;

        if (vida <= 0)
            Morir();
    }

    protected virtual void Morir()
    {
        Destroy(gameObject);
    }
}
