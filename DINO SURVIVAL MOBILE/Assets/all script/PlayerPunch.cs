using UnityEngine;

[DisallowMultipleComponent]
public class PlayerPunchCustom : MonoBehaviour
{
    [Header("References (can auto-find in Awake)")]
    public Camera playerCamera;
    public Inventory inventory;
    public PlayerStatsCustom playerStats;

    [Header("Punch Settings")]
    public float punchRange = 3f;
    public int woodAmountPerPunch = 1;
    public float damageToPlayer = 5f;
    public float punchCooldown = 1f;

    float lastPunchTime;

    void Awake()
    {
        if (playerCamera == null) playerCamera = Camera.main;
        if (inventory == null) inventory = GetComponent<Inventory>();
        if (playerStats == null) playerStats = GetComponent<PlayerStatsCustom>();
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
        if (playerCamera == null)
        {
            Debug.LogWarning("PlayerPunchCustom: playerCamera null (set it in Inspector or tag MainCamera).");
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, punchRange))
        {
            Resource res = hit.collider.GetComponent<Resource>();
            if (res != null)
            {
                if (inventory != null)
                    inventory.AddItem(res.resourceName, woodAmountPerPunch);
                else
                    Debug.LogWarning("PlayerPunchCustom: Inventory not found on Player.");

                if (playerStats != null)
                    playerStats.TakeDamage(damageToPlayer);
                else
                    Debug.LogWarning("PlayerPunchCustom: PlayerStatsCustom not found on Player.");

                Debug.Log($"Đấm cây: +{woodAmountPerPunch} {res.resourceName}, -{damageToPlayer} máu");
            }
        }
    }
}
