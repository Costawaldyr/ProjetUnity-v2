using UnityEngine;

public class Life1 : MonoBehaviour
{
    public int current_life;
    public int max_Life = 100;
    [SerializeField] public Life_Bar life_Bar;
    void Start()
    {
        current_life = max_Life;
        life_Bar.UpdateLifeBar(current_life, max_Life);
    }

    public void Damage(int damage)
    {
        current_life -= damage;
        if(current_life <= 0)
        {
            current_life = 0;
            Die();
        }
        life_Bar.UpdateLifeBar(current_life, max_Life);
    }

    public void Heal(int healAmount)
    {
        current_life += healAmount;
        if (current_life > max_Life)
        {
            current_life = max_Life; 
        }
        life_Bar.UpdateLifeBar(current_life, max_Life);
    }

    public void ResetLife()
    {
        current_life = max_Life;
        life_Bar.UpdateLifeBar(current_life, max_Life);
    }

    private void Die()
    {
        Debug.Log("Le joueur est mort !");

        // Déclenche l'animation de mort
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("Die"); // Assurez-vous que l'animation "Die" existe dans votre Animator
        }

        // Désactive le contrôle du joueur
        this.enabled = false;
    }

}
