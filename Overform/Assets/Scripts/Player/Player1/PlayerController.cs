using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
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
    public int life;
    public int coin;

    [Header("Ground Check")]
    public LayerMask groundLayer; // camada do chão
    public float groundCheckDistance; // tamanho da linha do raycast
    public Vector2 groundCheckOffsetLeft; // para verificacao da esquerda
    public Vector2 groundCheckOffsetRight; // para verificacao da direita


    [Header("Bool")] 
    public bool isGrounded;
    public bool isPause;

    [Header("UI Components")]
    public TextMeshProUGUI textLife;
    public TextMeshProUGUI textCoin;

    [Header("Game Objects")]
    public GameObject gameOver;
    public GameObject pause;

    [Header("Level")]
    public string levelName;

    
    private void Awake() // is called before the start
    {
        //verifacte if the game was loaded
        if(PlayerPrefs.GetInt("wasLoaded") == 1)
        {
            life = PlayerPrefs.GetInt("Life", 0);
            levelName = PlayerPrefs.GetString("levelSaved", "Default");
            Debug.Log("Game loaded");
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colliderPlayer = GetComponent<CapsuleCollider2D>();
        Time.timeScale = 1;
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        textLife.text = life.ToString();
        textCoin.text = coin.ToString();

        if (life <= 0)
        {
            this.enabled= false;
            colliderPlayer.enabled = false;
            rb.gravityScale = 0;
            anim.Play("Die", -1);
            gameOver.SetActive(true);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            PauseScreen();
        }
        
        //Save Game
        if (Input.GetKeyDown(KeyCode.P))
        {
            string activeScene = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("levelSaved", activeScene);
            PlayerPrefs.SetInt("Life", life);
            Debug.Log("Game saved");
        }

        if (isGrounded)
        {
            addJump = 1;

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && addJump > 0)
            {
                addJump--;
                Jump();
            }
        }

        Attack();
    }

    void FixedUpdate()
    {
        Move();

        //Check if the player is grounded
        CheckGroundedStatus();

    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y); 

        if (moveX > 0) // lado direito
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            anim.SetBool("IsRun", true);

            groundCheckOffsetLeft.x = - Mathf.Abs(groundCheckOffsetLeft.x);
            groundCheckOffsetRight.x = Mathf.Abs(groundCheckOffsetRight.x);
        }
        else if (moveX < 0) // Se o player estiver olhando para o lado esquerdo
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            anim.SetBool("IsRun", true);

            groundCheckOffsetLeft.x = Mathf.Abs(groundCheckOffsetLeft.x);
            groundCheckOffsetRight.x = - Mathf.Abs(groundCheckOffsetRight.x);
        }
        else // Se o player não estiver se movendo
        {
            anim.SetBool("IsRun", false);
        }
        
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); 
        anim.SetBool("IsJump", true);
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetBool("IsAttack", true);
        }
        else
        {
            anim.SetBool("IsAttack", false);
        }

        /*if (Input.GetButtonDown("Fire1"))
        {
            anim.Play("Attack", -1);
        }*/
        
    }

    void PauseScreen()
    {
        if(isPause)
        {
            isPause = false;
            Time.timeScale = 1;
            pause.SetActive(false);
        }
        else
        {
            isPause = true;
            Time.timeScale = 0;
            pause.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        isPause = false;
        Time.timeScale = 1;
        pause.SetActive(false);
    }

    public void BackMenu()
    {   
        SceneManager.LoadScene(0);
    }

    public void CheckGroundedStatus()
    {
        // calcula os pontos de origem + os raycast
        Vector2 position = transform.position;
        Vector2 groundCheckPosLeft = position + groundCheckOffsetLeft;
        Vector2 groundCheckPosRight = position + groundCheckOffsetRight;

        RaycastHit2D hitLeft = Physics2D.Raycast(groundCheckPosLeft, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(groundCheckPosRight, Vector2.down, groundCheckDistance, groundLayer);

        isGrounded = hitLeft.collider != null || hitRight.collider != null;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("IsJump", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}


