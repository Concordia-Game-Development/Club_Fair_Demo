using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    //Public so that values can be get and set by slider in Player.cs
    public float speed = 100;
    public  float jumpSpeed = 100;
    [SerializeField] float fallGravity = 10;
    [SerializeField] List<Transform> respawnPoints;

    [SerializeField] LayerMask wall;

    private int currentCheckpoint = 0;
    private bool jumping = false;
    //Ray distance to check if walking into wall
    //Prevents jittering, sweet spot 0.75f
    private float wallDist = 0.75f;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;
    void Start()
    {    
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (jumping)
        {
            animator.SetBool("jumping", true);
            animator.SetFloat("jumpV", rb.velocity.y);

            if (rb.velocity.y == 0)
            {
                animator.SetBool("jumping", false);
                jumping = false;
            }

            if (rb.velocity.y < 0)
            {
                rb.velocity += fallGravity * Time.deltaTime * Vector2.down;
            }

        }
    }

    public void Move(float x_dir, bool jumpPressed)
    {
        if (jumpPressed && !jumping)
        {
            jumping = true;
            rb.velocity = Vector3.up * jumpSpeed;
            animator.SetBool("jumping", true);
        }

        if (x_dir != 0 && !CheckForWall(Vector2.right * x_dir))
        {
            Vector2 displacement = Vector2.right * speed * x_dir * Time.deltaTime;
            transform.Translate(displacement);
            animator.SetBool("walking", true);

            if (!sr.flipX && x_dir < 0 || sr.flipX && x_dir > 0)
            {
                sr.flipX = !sr.flipX;
            }


            if (currentCheckpoint < respawnPoints.Count-1    &&  transform.position.x > respawnPoints[currentCheckpoint+1].position.x)
            {
                currentCheckpoint++;
            }


        }
        else
        {
            
            animator.SetBool("walking", false);
        }

    }

    public void MoveBool(bool b)
    {
        animator.SetBool("walking", b);
    }

    
    bool CheckForWall(Vector2 direction)
    {
        return Physics2D.Raycast(transform.position, direction, wallDist, wall);

    }

    public void respawn(int position = -1)
    {
        if (position == -1) position = currentCheckpoint;
        transform.position = respawnPoints[position].position;
        rb.velocity = Vector3.zero;
        
    }

    public void Kill()
    {
        respawn(currentCheckpoint);
    }
}
