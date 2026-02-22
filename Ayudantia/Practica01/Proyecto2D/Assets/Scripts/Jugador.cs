using UnityEngine;

public class Jugador: MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col){
        Debug.Log("Entre en contacto con: " + col.gameObject.name);
    }

    void OnCollisionStay2D(Collision2D col){
        Debug.Log("Sigo en contacto con: " + col.gameObject.name);
    }
    
}
