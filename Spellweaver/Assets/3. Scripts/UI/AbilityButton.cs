using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [Header("Ability Data")]
    public AbilityData abilityData;

    [Header("UI Elements")]
    public Image buttonBackground;
    public Image highlightBorder;
    
    public TextMeshProUGUI abilityNameText;
    public TextMeshProUGUI slotIndicatorText;

    

    public bool isAssigned = false;
    public int assignedSlot = -1;
    private Button button;
    

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void Start()
    {
        if (abilityData != null)
        {
            SetupButton(abilityData);
        }

        slotIndicatorText.gameObject.SetActive(false);
        highlightBorder.gameObject.SetActive(false);
        
    }

    public void SetupButton(AbilityData ability)
    {
        abilityData = ability;
        abilityNameText.text = ability.abilityName;
        SetButtonColors();
    }

    private void SetButtonColors()
    {
        
        ColorBlock colors = button.colors;
        colors.normalColor = abilityData.colorData.baseColor;
        colors.highlightedColor = abilityData.colorData.hoverColor;
        colors.pressedColor = abilityData.colorData.selectedColor;
        colors.selectedColor = abilityData.colorData.selectedColor;

        button.colors = colors;
    }

    private void OnButtonClick()
    {
        if (abilityData.abilityType == AbilityType.BasicAttack)
        {
            LoadoutUIManager.instance.DisplayBasicAttackInfo(abilityData, this);
        }
        else
        {
            LoadoutUIManager.instance.DisplayAbilityInfo(abilityData, this);
        }
    }

    public void AssignSlot(int slotNumber)
    {
        isAssigned = true;
        assignedSlot = slotNumber;
        slotIndicatorText.text = $"{slotNumber + 1}";
        slotIndicatorText.gameObject.SetActive(true);
        highlightBorder.gameObject.SetActive(true);
        
    }

    public void DeselectSlot()
    {
        isAssigned = false;
        assignedSlot = -1;
        slotIndicatorText.gameObject.SetActive(false);
        highlightBorder.gameObject.SetActive(false);
    }
    public int GetAssignedSlot()
    {
        return assignedSlot;
    }
    
}
