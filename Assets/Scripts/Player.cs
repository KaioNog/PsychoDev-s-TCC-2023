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

    //andar e pular
    public float Speed;
    public float JumpForce;

    //pulo duplo
    public bool isJumping;
    public bool doubleJump;

    private Rigidbody2D rig;
    public Animator anim;

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

    //pulo e pulo duplo
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

    //Verifica ground para pular
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            anim.SetBool("Jump", false);
        }    
    }

    //Dano ao colidir com espinho
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spike")
        {
            Dano();           
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
                
                GameController.instance.ShowGameOver();
                Destroy(gameObject);
            }
    }
}
