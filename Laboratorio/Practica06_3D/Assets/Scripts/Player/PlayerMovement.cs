using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float runSpeed = 10f;
    public float gravity = -9.8f;
    public float jumpForce = 8f;
    CharacterController controller;
    Vector3 velocity;

    public Transform cameraTransform;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Codigo viejo: moverse respecto al sistema de coordenadas local del jugador
        //Vector3 move = transform.right * x + transform.forward * z;



        //Codigo nuevo

        //Moverse respecto al sistema de coordenadas global
        // Vector3 move = new Vector3(x,0,z);

        //Moverse respecto a la camara
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        Vector3 move = camForward * z + camRight * x;


        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : speed;
        controller.Move(move * currentSpeed * Time.deltaTime);
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            velocity.y = jumpForce;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb == null || rb.isKinematic)
            return;
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0,
        hit.moveDirection.z);
        rb.velocity = pushDir * 4f;
    }

}