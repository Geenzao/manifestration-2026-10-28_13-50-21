using UnityEngine;

public enum ItemType
{
    Normal,
    Flamable,
    Pencil,
}
public interface IItem
{
    // Type of the item
    public ItemType ItemType { get; }
}
