using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int vida;
    private int vidaMaxima = 3;
    [SerializeField] Image vidaOn;
    [SerializeField] Image vidaOff;
    [SerializeField] Image vidaOn2;
    [SerializeField] Image vidaOff2;

    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        vida = vidaMaxima;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.eulerAngles = new Vector3(0f,0f,0f);    
            //transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            //transform.localScale = new Vector3(-1, 1, 1,);
            transform.eulerAngles = new Vector3(0f,180f,0f);

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spike")
        {
            Dano();           
        }
    }


    private void Dano()
    {
        vida -= 1;

            if(vida == 2)
            {
                vidaOn2.enabled = true;   //primeiro coração apagado
                vidaOff2.enabled = false; 
            }
            else
            {
                vidaOn2.enabled = false;
                vidaOff2.enabled = true;                    
            }

            if(vida == 1)
            {
                vidaOn2.enabled = true;
                vidaOff2.enabled = false;      

                vidaOn.enabled = true; 
                vidaOff.enabled = false;            
            }
            else
            {
                vidaOn.enabled = false; 
                vidaOff.enabled = true;    
            }

            if(vida <= 0)
            {
                Debug.Log("Game Over");
                Destroy(gameObject);
                GameController.instance.ShowGameOver();
            }
    }
}
