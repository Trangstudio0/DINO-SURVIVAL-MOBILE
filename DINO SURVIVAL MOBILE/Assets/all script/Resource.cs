using UnityEngine;

public class Resource : MonoBehaviour
{
    [Header("Resource Settings")]
    public string resourceName = "Wood"; // Tên resource
    public int maxHealth = 20;           // Máu của cây
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Gọi khi PlayerPunchAxe đánh trúng
    public void TakeHit(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{resourceName} bị trúng! HP còn: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Collect();
        }
    }

    // Xử lý thu thập resource (cây biến mất)
    public void Collect()
    {
        Debug.Log($"{resourceName} bị chặt xong!");
        Destroy(gameObject);
    }
}
