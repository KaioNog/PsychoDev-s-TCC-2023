using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //vida
    private int vida;
    private int vidaMaxima = 3;
    [SerializeField] Image vidaOn;
    [SerializeField] Image vidaOff;
    [SerializeField] Image vidaOn2;
    [SerializeField] Image vidaOff2;
    //andar
    public float Speed = defaultSpeed;
    //correr
    public float runningSpeed;
    public const float defaultSpeed = 10;  
    //pular
    public float JumpForce = 8;
    //pulo duplo
    public bool isJumping;
    public bool doubleJump;
    //componentes
    private Rigidbody2D rig;
    public Animator anim;
    private BoxCollider2D boxCollider;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        vida = vidaMaxima;    
    }

    void Update()
    {
        Move();
        Jump();
        Run();
    }

    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;

            if(Input.GetAxis("Horizontal") > 0f)
            {
                anim.SetBool("Walk", true);
                transform.eulerAngles = new Vector3(0f,0f,0f);
            }

            if(Input.GetAxis("Horizontal") < 0f)
            {
                anim.SetBool("Walk", true);
                transform.eulerAngles = new Vector3(0f,180f,0f);
            }

            if(Input.GetAxis("Horizontal") == 0f)
            {
                anim.SetBool("Walk", false);
            }
    }  

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                anim.SetBool("Jump", true);
            }
            else
            {
                if(doubleJump)
                {
                rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                doubleJump = false;
                }
            }
        }
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            Speed = runningSpeed;
        else
        {
            if(Speed != defaultSpeed)
            Speed = defaultSpeed;
        }
    }

    //Verifica ground para pular
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            anim.SetBool("Jump", false);
        }    
    }

    //Anim pulo
    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJumping = true;
        }
    }

    //Dano ao colidir com espinho
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spike")
        {
            Dano();    
            Debug.Log("Player levou dano");       
        }
    }

    private void Dano()
    {
        vida -= 1;

            if(vida == 2)
            {
                vidaOn2.enabled = true;   //primeiro coracao apagado
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

                //GameController.instance.ShowGameOver();
                Destroy(gameObject);
            }
    }
}
