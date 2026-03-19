using UnityEngine;

public class EnemigoIA:Personaje
{
    public float direccion = -1f; //-1 para izquierda, 1 para derecha.

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);
    }
}
