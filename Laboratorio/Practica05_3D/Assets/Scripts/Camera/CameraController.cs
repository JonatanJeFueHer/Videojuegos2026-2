using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // El objeto que la cámara seguirá.
    public Transform pivot; // El punto alrededor del cual la cámara rotará.
    public float sensitivity = 3f; // Sensibilidad de la rotación de la cámara.
    public float minY = -20f; // Ángulo mínimo de rotación vertical.
    public float maxY = 60f; // Ángulo máximo de rotación vertical.

    float rotX; // Rotación horizontal acumulada.
    float rotY; // Rotación vertical acumulada.

    // Start is called before the first frame update
    void Start()
    {
        Cursor.LockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla.        
    }

    // Update is called once per frame 
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity; // Obtiene el movimiento horizontal del mouse.
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity; // Obtiene el movimiento vertical del mouse.

        rotX += mouseX; // Acumula la rotación horizontal.
        rotY -= mouseY; // Acumula la rotación vertical (invertida para un control más natural).
        rotY = Mathf.Clamp(rotY, minY, maxY); // Limita la rotación vertical.

        transform.rotation = Quaternion.Euler(0, rotX, 0); // Rota la cámara horizontalmente.
        pivot.LocalRotation = Quaternion.Euler(rotY, 0, 0); // Rota la cámara verticalmente alrededor del pivot.

        transform.position = target.position; // Mantiene la cámara en la posición del objetivo.
    }
}
