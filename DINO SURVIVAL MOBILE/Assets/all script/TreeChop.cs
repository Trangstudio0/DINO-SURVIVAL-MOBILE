using UnityEngine;

public class TreeChop : MonoBehaviour
{
    public Camera playerCamera;
    public float chopRange = 5f;
    public Inventory inventory; // script Inventory bạn đã làm trước đó
    public string resourceName = "Wood";
    public int woodAmount = 1;

    private Terrain terrain;

    void Start()
    {
        terrain = Terrain.activeTerrain;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryChopTree();
        }
    }

    void TryChopTree()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, chopRange))
        {
            if (hit.collider.gameObject == terrain.gameObject)
            {
                ChopTreeAt(hit.point);
            }
        }
    }

    void ChopTreeAt(Vector3 hitPoint)
    {
        TreeInstance[] trees = terrain.terrainData.treeInstances;
        int treeIndex = -1;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < trees.Length; i++)
        {
            Vector3 treeWorldPos = Vector3.Scale(trees[i].position, terrain.terrainData.size) + terrain.transform.position;
            float distance = Vector3.Distance(hitPoint, treeWorldPos);

            if (distance < 2f && distance < closestDistance) // 2f = bán kính để coi là "trúng"
            {
                closestDistance = distance;
                treeIndex = i;
            }
        }

        if (treeIndex != -1)
        {
            // Thêm gỗ vào inventory
            inventory.AddItem(resourceName, woodAmount);

            // Xóa cây
            var treeList = new System.Collections.Generic.List<TreeInstance>(trees);
            treeList.RemoveAt(treeIndex);
            terrain.terrainData.treeInstances = treeList.ToArray();

            Debug.Log("Đã chặt cây, nhận được " + woodAmount + " " + resourceName);
        }
    }
}
