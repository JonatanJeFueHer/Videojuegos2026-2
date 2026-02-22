using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        Debug.Log("MOVIMIENTO HORIZONTAL: " + h);

        float v = Input.GetAxis("Vertical");
        Debug.Log("MOVIMIENTO VERTICAL: " + v);
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SALTANDO.");
        }
    }
}
