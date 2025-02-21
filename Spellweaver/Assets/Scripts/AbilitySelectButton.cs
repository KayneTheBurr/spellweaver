using UnityEngine;
using UnityEngine.UI;

public class AbilitySelectButton : MonoBehaviour
{
    public AbilityData ability;
    public int spellListIndex;

    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(AssignAbilitySlot);
    }

    private void AssignAbilitySlot()
    {
        PlayerManager.instance.AssignAbility(spellListIndex, ability);
        
    }

}
