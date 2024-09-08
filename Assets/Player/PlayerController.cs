using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 100;
    public  float jumpSpeed = 100;
    [SerializeField] float fallGravity = 10;
    [SerializeField] List<Transform> respawnPoints;

    [SerializeField] LayerMask wall;


    int currentCheckpoint = 0;
    
    bool jumping = false;
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
            print(rb.velocity);
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
                print("NEW CHECKPOINT");
                currentCheckpoint++;
            }


        }
        else
        {
            animator.SetBool("walking", false);
        }

    }

    float wallDist = 0.75f;
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
