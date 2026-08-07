using UnityEngine;
using UnityEngine.Rendering.LookDev;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public string changedName;
    public Sprite itemIcon;
    public Sprite ItemDetailIcon;

    [TextArea]
    public string itemDescription;

    [TextArea]
    public string changedDescription;
}
