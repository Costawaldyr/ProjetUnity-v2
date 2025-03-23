using UnityEngine;

public class FireMagic : MonoBehaviour
{    
    public int damage = 2;
    public float speed = 5f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().life -= damage;
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        Invoke("Disable", 1f);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}

