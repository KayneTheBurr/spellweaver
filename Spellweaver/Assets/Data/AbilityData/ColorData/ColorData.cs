using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Scriptable Objects/ColorData")]
public class ColorData : ScriptableObject
{
    public ElementType element;
    public Color baseColor;
    public Color hoverColor;
    public Color selectedColor;
}
