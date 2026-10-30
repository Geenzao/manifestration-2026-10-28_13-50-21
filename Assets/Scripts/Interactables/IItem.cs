using UnityEngine;

public enum ItemType
{
    Normal,
    Flamable,
    Pencil,
    Tool,
}
public interface IItem
{
    // Type of the item
    public ItemType ItemType { get; }
}
