using UnityEngine;

[DisallowMultipleComponent]
public class PlayerPunchAxe : MonoBehaviour
{
    public Camera playerCamera;
    public Inventory inventory;
    public PlayerStatsCustom playerStats;

    [Header("Punch (fist)")]
    public float punchRange = 3f;
    public int woodPerPunch = 1;
    public float damageWhenPunch = 5f;
    public int treeDamagePunch = 3; // cây mất máu khi đấm tay
    public float punchCooldown = 1f;

    [Header("Axe settings")]
    public string axeItemName = "Axe";      // tên trong Inventory
    public int woodPerAxeHit = 6;
    public int treeDamageAxe = 10;          // cây mất máu khi dùng rìu
    public int axeDurabilityMax = 10;

    int axeDurability;
    float lastPunchTime;

    void Awake()
    {
        if (playerCamera == null) playerCamera = Camera.main;
        if (inventory == null) inventory = GetComponent<Inventory>();
        if (playerStats == null) playerStats = GetComponent<PlayerStatsCustom>();
        axeDurability = axeDurabilityMax;
    }

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
        if (playerCamera == null) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, punchRange))
        {
            Resource res = hit.collider.GetComponent<Resource>();
            if (res == null) return;

            bool hasAxe = inventory != null && inventory.GetQuantity(axeItemName) > 0;

            if (hasAxe)
            {
                // Dùng rìu: nhiều gỗ, cây mất máu nhanh, không mất máu nhân vật
                inventory.AddItem(res.resourceName, woodPerAxeHit);
                res.TakeHit(treeDamageAxe);

                axeDurability--;
                Debug.Log($"Chặt bằng rìu: +{woodPerAxeHit} {res.resourceName} (độ bền: {axeDurability}/{axeDurabilityMax})");

                if (axeDurability <= 0)
                {
                    inventory.RemoveItem(axeItemName, 1);
                    Debug.Log("Rìu đã vỡ!");
                    axeDurability = axeDurabilityMax;
                }
            }
            else
            {
                // Đấm tay: ít gỗ, cây mất máu ít, nhân vật mất máu
                inventory.AddItem(res.resourceName, woodPerPunch);
                res.TakeHit(treeDamagePunch);

                if (playerStats != null) playerStats.TakeDamage(damageWhenPunch);
                Debug.Log($"Đấm tay: +{woodPerPunch} {res.resourceName}, -{damageWhenPunch} HP");
            }
        }
    }
}
