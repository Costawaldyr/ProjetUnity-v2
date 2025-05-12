using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class RogueController : MonoBehaviour
{
    [Header("Player Components")]
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D colliderPlayer;
    private float moveX;

    [Header("Atributtes")]
    public float speed;
    public int addJump;
    public float jumpForce;
    public float highJumpForce;



    [Header("Jump Settings")]
    public bool isGrounded;
    private int jumpCount = 0; // Compteur pour les pressions sur la touche de saut
    private float jumpTimer = 0f; // Timer pour détecter les doubles pressions
    public float doublePressTime = 0.3f; // Temps maximum entre deux pressions pour un saut élevé


    [Header("Ground Check")]
    public LayerMask groundLayer; // camada do chão
    public float groundCheckDistance; // tamanho da linha do raycast
    public Vector2 groundCheckOffsetLeft; // para verificacao da esquerda
    public Vector2 groundCheckOffsetRight; // para verificacao da direita



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colliderPlayer = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal");

         // Gestion du mouvement
        if (Mathf.Abs(moveX) > 0.1f) // Si le joueur se déplace
        {
            if (Input.GetKey(KeyCode.LeftShift)) // Si le joueur court
            {
                anim.SetBool("IsRunning", true);
                anim.SetBool("IsWalking", false);
            }
            else // Si le joueur marche
            {
                anim.SetBool("IsWalking", true);
                anim.SetBool("IsRunning", false);
            }
        }
        else // Si le joueur est immobile
        {
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsRunning", false);
        }


        // Gestion des attaques
        if (Input.GetButtonDown("Fire1")) 
        {
            if (anim.GetBool("IsWalking")) 
            {
                anim.Play("WalkAttack", -1); 
            }
            else if (anim.GetBool("IsRunning")) 
            {
                anim.Play("RunAttack", -1); 
            }
            else 
            {
                anim.Play("Attack", -1);
            }
        }
        else if (Input.GetButtonDown("Fire2")) 
        {
            anim.Play("AttackExtra", -1);
        }

        //Gestion du saut
        if(Input.GetButtonDown("Jump"))
        {
            jumpCount++;

            // premiere pression sur la touche de saut 
            if(jumpCount == 1) 
            {
                Jump();
                //Demarre le timer pour detecter si le joueur a appuyé 2 fois rapidement
                jumpTimer = Time.time;  
            }
            else if(jumpCount == 2 && Time.time - jumpTimer <= doublePressTime)
            {
                HighJump();
                jumpCount = 0;
            }
        }
        // si le joueur n'appuie pas une deuxieme fois dans le temps impartie, le compteur de saut est remis à 0
        if (Time.time - jumpTimer > doublePressTime) 
        {
            jumpCount = 0;
        }


    }

    void FixedUpdate()
    {
        Move();
        CheckIfGrounded();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y); 

        if(moveX > 0)
        {
            transform.eulerAngles = new Vector3(0f,0f,0f);
            anim.SetBool("IsWalking", true);


            groundCheckOffsetLeft.x = - Mathf.Abs(groundCheckOffsetLeft.x);
            groundCheckOffsetRight.x = Mathf.Abs(groundCheckOffsetRight.x);
        }
        else if(moveX < 0)
        {
            transform.eulerAngles = new Vector3(0f,180f,0f);
            anim.SetBool("IsWalking", true);


            groundCheckOffsetLeft.x = Mathf.Abs(groundCheckOffsetLeft.x);
            groundCheckOffsetRight.x = - Mathf.Abs(groundCheckOffsetRight.x);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }

    void Jump()
    {
        if (isGrounded) // Vérifie si le joueur est au sol
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.Play("Jump", -1);
        }
    }
    void HighJump()
    {
        if (isGrounded) // Vérifie si le joueur est au sol
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, highJumpForce);
            anim.Play("HighJump", -1);
        }
    }

    void Climb()
    {
        anim.Play("Climb", -1);
        rb.linearVelocity = new Vector2(0, Input.GetAxis("Vertical") * speed);
    }

    public void Die()
    {
        anim.Play("Die", -1);
        rb.linearVelocity = Vector2.zero;
        this.enabled = false;
    }


private void ReloadScene()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder")) // Si le joueur touche une échelle
        {
            Climb();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder")) // Si le joueur quitte l'échelle
        {
            anim.SetBool("IsClimbing", false); // Arrête l'animation d'escalade
            rb.linearVelocity = Vector2.zero; // Arrête le mouvement vertical
        }
    }


    public void CheckIfGrounded()
    {
        // Position centrale du joueur
        Vector2 position = transform.position;

        // Points de départ des raycasts (gauche et droite)
        Vector2 groundCheckPosLeft = position + groundCheckOffsetLeft;
        Vector2 groundCheckPosRight = position + groundCheckOffsetRight;

        // Raycasts vers le bas pour vérifier le sol
        RaycastHit2D hitLeft = Physics2D.Raycast(groundCheckPosLeft, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(groundCheckPosRight, Vector2.down, groundCheckDistance, groundLayer);

        // Si un des raycasts touche un objet dans la couche "groundLayer", le joueur est au sol
        isGrounded = hitLeft.collider != null || hitRight.collider != null;

        // Débogage visuel dans la scène
        Debug.DrawLine(groundCheckPosLeft, groundCheckPosLeft + Vector2.down * groundCheckDistance, Color.red);
        Debug.DrawLine(groundCheckPosRight, groundCheckPosRight + Vector2.down * groundCheckDistance, Color.red);

        // Met à jour l'animation
        anim.SetBool("IsJump", !isGrounded);
    }

    public void OnDrawGizmos()
    {
        Vector3 groundCheckPosLeft = transform.position + new Vector3(groundCheckOffsetLeft.x, groundCheckOffsetLeft.y, 0f);
        Vector3 groundCheckPosRight = transform.position + new Vector3(groundCheckOffsetRight.x, groundCheckOffsetRight.y, 0f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheckPosLeft, groundCheckPosLeft + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(groundCheckPosRight, groundCheckPosRight + Vector3.down * groundCheckDistance);
    }

}