using UnityEngine;

public class Jugador: MonoBehaviour
{

    //Variable para saber si el jugador esta tocando el suelo.
    public bool enSuelo = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Método para cambiar el estado de la variable enSuelo a true en caso de que el collider del suelo entre en contacto con el collider del sprite del jugador.
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    //Método para cambiar el estado de la variable enSuelo a false en caso de que el collider del suelo deje de tener contacto con el collider del sprite del jugador.
    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }
    
}
