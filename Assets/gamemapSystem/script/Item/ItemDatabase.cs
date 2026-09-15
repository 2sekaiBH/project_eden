using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Objects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    private static ItemDatabase _instance;
    public static ItemDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                ItemDatabase[] databases = Resources.FindObjectsOfTypeAll<ItemDatabase>();
                if (databases.Length > 0)
                {
                    _instance = databases[0];
                }
                else
                {
                    _instance = Resources.Load<ItemDatabase>("ItemDatabase");
                }
            }
            return _instance;
        }
    }

    [SerializeField] private ItemData[] items;

    public void Initialize()
    {
        _instance = this;
    }

    public ItemData GetItemByID(int id)
    {
        if (items == null) return null;

        foreach (var item in items)
        {
            if (item == null) continue;
            if (item.id == id) return item;
        }
        return null;
    }
}