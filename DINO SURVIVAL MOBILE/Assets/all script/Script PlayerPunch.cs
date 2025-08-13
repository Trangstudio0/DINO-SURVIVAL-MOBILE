using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public Camera playerCamera;
    public float punchRange = 3f;
    public Inventory inventory; // Tham chiếu Inventory
    public PlayerStats playerStats; // Script máu / đói

    public int woodAmountPerPunch = 1; // Lượng gỗ mỗi cú đấm
    public float damageToPlayer = 5f;  // Mất máu khi đấm cây tay không
    public float punchCooldown = 1f;   // Thời gian giữa các cú đấm

    private float lastPunchTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastPunchTime >= punchCooldown)
        {
            lastPunchTime = Time.time;
            Punch();
        }
    }

    void Punch()
    {
        Debug.Log("Player đấm!");
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, punchRange))
        {
            Resource resource = hit.collider.GetComponent<Resource>();
            if (resource != null)
            {
                // Thêm ít gỗ
                inventory.AddItem(resource.resourceName, woodAmountPerPunch);

                // Mất máu nhân vật
                if (playerStats != null)
                {
                    playerStats.currentHealth -= damageToPlayer;
                    Debug.Log("Player mất " + damageToPlayer + " máu vì đấm cây!");
                }
            }
        }
    }
}
