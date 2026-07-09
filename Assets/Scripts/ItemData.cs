using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite itemIcon;
    public Sprite detail_itemIcon;

    [TextArea]
    public string itemDescription;
}
