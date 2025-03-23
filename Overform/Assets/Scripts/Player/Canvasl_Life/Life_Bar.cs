using UnityEngine;
using UnityEngine.UI;

public class Life_Bar : MonoBehaviour
{
    [SerializeField] private Image life_Bar;
    [SerializeField] private Color fullLifeColor = Color.green;
    [SerializeField] private Color lowLifeColor = Color.red;

    public void UpdateLifeBar(int life, int maxLife)
    {
        float lifeRatio = (float)life / maxLife;
        life_Bar.fillAmount = lifeRatio;
        
        life_Bar.color = Color.Lerp(lowLifeColor, fullLifeColor, lifeRatio);
    }
}
    
