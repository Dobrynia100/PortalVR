using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [SerializeField]
    public float moveSpeed = 5f;
    private bool isJumping = false;
    private Rigidbody rb;
    

 
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        if (Input.GetButtonDown("XRI_Right_SecondaryButton") && !isJumping)
        {
            Jump();
        }

        if (rb.velocity.y < 0)
        {
           
            rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {

            rb.velocity += Vector3.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (!isJumping)
        {
            
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

    }

    private void Jump()
    {
        Debug.Log("прыжок");
        isJumping = true;
        rb.velocity=Vector3.up * jumpForce;
    }
    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("на полу");
            isJumping = false;
        }
    }
}
