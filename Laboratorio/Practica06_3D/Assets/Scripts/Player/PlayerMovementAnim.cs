using UnityEngine;

public class PlayerMovementAnim : MonoBehaviour
{
    public Transform cameraTransform;
    Animator anim;
    CharacterController controller;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        Vector3 moveDir = camForward * z + camRight * x;
        float speedValue = moveDir.magnitude;
        anim.SetFloat("Speed", speedValue);
            

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        anim.SetFloat("Speed", speedValue);
        Vector3 direction = new Vector3(x, 0, z);
        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * 10f
            );
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }

        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 10f
            );
        }
    }
}