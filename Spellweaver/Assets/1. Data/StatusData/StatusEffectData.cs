using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "Scriptable Objects/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public Status statusType;
    public string effectName;
    [TextArea] public string description;
    public Sprite statusIcon;
    public ElementType causeElement1;//trigger element (burn,chill,poison, shock)
    public Status causeStatus2;//other element that combines, null for 
    public Sprite element1Icon;
    public Sprite element2Icon;

    public string element1name;
    public string element2name;
}
