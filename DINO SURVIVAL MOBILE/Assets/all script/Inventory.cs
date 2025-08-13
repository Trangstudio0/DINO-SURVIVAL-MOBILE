using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public int quantity;
}

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    // Thêm vật phẩm
    public void AddItem(string name, int qty)
    {
        Item item = items.Find(i => i.name == name);
        if (item != null)
        {
            item.quantity += qty;
        }
        else
        {
            items.Add(new Item { name = name, quantity = qty });
        }
        Debug.Log($"Inventory: +{qty} {name}");
    }

    // Xóa vật phẩm
    public bool RemoveItem(string name, int qty)
    {
        Item item = items.Find(i => i.name == name);
        if (item == null) return false;
        if (item.quantity < qty) return false;

        item.quantity -= qty;
        if (item.quantity <= 0)
        {
            items.Remove(item);
        }
        Debug.Log($"Inventory: -{qty} {name}");
        return true;
    }

    // Lấy số lượng item
    public int GetQuantity(string name)
    {
        Item item = items.Find(i => i.name == name);
        return item != null ? item.quantity : 0;
    }

    // Kiểm tra có item hay không
    public bool HasItem(string name)
    {
        return GetQuantity(name) > 0;
    }
}
