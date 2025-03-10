using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D colliderPlayer;
    private float moveX;

    public float speed;
    public int addJump;
    public bool isGrounded;
    public float jumpForce;
    public int life;
    public int coin;
    public TextMeshProUGUI textLife;
    public TextMeshProUGUI textCoin;
    public GameObject gameOver;
    public GameObject pause;
    public bool isPause;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("wasLoaded") == 1)
        {
            life = PlayerPrefs.GetInt("Life", 0);
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
        
        //Salvar Game
        if (Input.GetKeyDown(KeyCode.P))
        {
            string activeScene = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("levelSaved", activeScene);
            PlayerPrefs.SetInt("Life", life);
            Debug.Log("Game saved");
        }
    }

    void FixedUpdate()
    {
        Move();
        Attack();

        if (isGrounded == true)
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
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y); 

        if (moveX > 0) // lado direito
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            anim.SetBool("IsRun", true);
        }

        if (moveX < 0) // Se o player estiver olhando para o lado esquerdo
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            anim.SetBool("IsRun", true);
        }

        if (moveX == 0)
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


