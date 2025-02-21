using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public float damage;
    public ElementType element;

    public abstract void Execute();
}
