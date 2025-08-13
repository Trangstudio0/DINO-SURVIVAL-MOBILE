using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxHunger = 100f;
    public float currentHunger;
    public float hungerDecayRate = 1f; // Mất bao nhiêu hunger mỗi phút

    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
    }

    void Update()
    {
        currentHunger -= hungerDecayRate * Time.deltaTime;

        if (currentHunger <= 0)
        {
            currentHealth -= 5f * Time.deltaTime; // Mất máu khi đói
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Eat(float foodValue)
    {
        currentHunger = Mathf.Min(currentHunger + foodValue, maxHunger);
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // Thêm game over UI
    }
}
