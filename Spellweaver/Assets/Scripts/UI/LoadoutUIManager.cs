using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadoutUIManager : MonoBehaviour
{
    public static LoadoutUIManager instance;

    [Header("UI Panels")]
    public GameObject abilitySelectionPanel;
    public GameObject abilityInfoPanel;

    [Header("UI Elements")]
    public TextMeshProUGUI abilityNameText;
    public Image abilityIcon;
    public TextMeshProUGUI abilityDescriptionText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI baseDamageText;
    


    [Header("Slot Assignment")]
    public Button[] slotButtons;
    private int selectedSlot = -1;
    public Button assignButton;
    public Button backButton;

    [Header("Ready Button")]
    public Button readyButton;

    [Header("Confirm Panel Parts")]
    public Image[] abilityIcons = new Image[4];
    public TextMeshProUGUI[] abilityNames = new TextMeshProUGUI[4];
    public Image basicAttackIcon;
    public TextMeshProUGUI basicAttackName;
    private Coroutine currentFadeRoutine;
    public GameObject confirmationPanel;
    public Button confirmButton;
    public Button cancelButton;
    public Button quitButton;

    private AbilityButton assignedBasicAttack;
    public AbilityButton currentAbilityButton;
    private AbilityData selectedAbility;
    private AbilityData selectedBasicAttack;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        abilityInfoPanel.SetActive(false);
        abilitySelectionPanel.SetActive(true);
        readyButton.gameObject.SetActive(false);
        confirmationPanel.SetActive(false);

        PlayerManager.instance.playerAbilityManager.ResetSpells();
        ResetLoadout();
        ResetSlotButtons();

        backButton.onClick.AddListener(ReturnToSelection);
        assignButton.onClick.AddListener(AssignAbility);
        readyButton.onClick.AddListener(ShowConfirmationPanel);
        confirmButton.onClick.AddListener(StartCombatPhase);
        cancelButton.onClick.AddListener(CancelConfirmation);
        quitButton.onClick.AddListener(Application.Quit);
    }
    public void DisplayAbilityInfo(AbilityData ability, AbilityButton button)
    {
        selectedAbility = ability;
        currentAbilityButton = button;

        foreach (var slotButton in slotButtons)
        {
            slotButton.gameObject.SetActive(true);
        }

        selectedSlot = -1;
        UpdateSlotSelectVisuals();

        abilityNameText.text = ability.abilityName;
        abilityIcon.sprite = ability.abilityIcon;
        abilityDescriptionText.text = ability.description;
        cooldownText.text = $"Cooldown: {ability.cooldown} sec";
        baseDamageText.text = $"Damage: {ability.baseDamage}";
        

        if (currentAbilityButton.isAssigned)
        {
            assignButton.GetComponentInChildren<TextMeshProUGUI>().text = "Remove";
        }
        else
        {
            assignButton.GetComponentInChildren<TextMeshProUGUI>().text = "Assign";
        }

        abilityInfoPanel.SetActive(true);
    }
    public void DisplayBasicAttackInfo(AbilityData basicAttack, AbilityButton button)
    {
        selectedAbility = basicAttack;
        currentAbilityButton = button;

        foreach (var slotButton in slotButtons)
        {
            slotButton.gameObject.SetActive(false);
        }

        abilityNameText.text = basicAttack.abilityName;
        abilityIcon.sprite = basicAttack.abilityIcon;
        abilityDescriptionText.text = basicAttack.description;
        cooldownText.text = "";
        baseDamageText.text = $"Base Damage: {basicAttack.baseDamage}";
        

        assignButton.GetComponentInChildren<TextMeshProUGUI>().text = currentAbilityButton.isAssigned ? "Remove" : "Assign";
        abilityInfoPanel.SetActive(true);
    }
    private void UpdateSlotSelectVisuals()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            Button button = slotButtons[i];
            bool slotOccupied = PlayerManager.instance.playerAbilityManager.spellList[i] != null;

            
            if (slotOccupied)
            {
                button.interactable = false; 
                var colors = button.colors;
                colors.normalColor = Color.gray; 
                button.colors = colors;

                AbilityButton assignedButton = GetAbilityButtonAssignedToSlot(i);
                if (assignedButton != null)
                {
                    assignedButton.slotIndicatorText.gameObject.SetActive(true);
                    assignedButton.slotIndicatorText.text = $"{i + 1}";
                }

            }
            else
            {
                button.interactable = true; 
                var colors = button.colors;
                colors.normalColor = Color.white; 
                button.colors = colors;
            }
        }
    }
    public void SelectSlot(int slot)
    {
        selectedSlot = slot;

        if (slotButtons[selectedSlot].interactable)
        {
            UpdateSlotSelectVisuals();
            var colors = slotButtons[selectedSlot].colors;
            colors.selectedColor = new Color(0.7764707f, 0.7294118f, 0.5137255f); 
            slotButtons[selectedSlot].colors = colors;
        }
    }
    public void UpdateSlotButtons()
    {
        //disable button if they have something assigned to thier slot 
        for (int i = 0; i < slotButtons.Length; i++)
        {
            bool slotOccupied = PlayerManager.instance.playerAbilityManager.spellList[i] != null;
            slotButtons[i].interactable = !slotOccupied;
        }
    }
    private void ReturnToSelection()
    {
        abilityInfoPanel.SetActive(false);
        abilitySelectionPanel.SetActive(true);
    }
    public void AssignAbility()
    {
        if (selectedAbility != null)
        {
            if(selectedAbility.abilityType == AbilityType.BasicAttack)
            {
                AssignBasicAttack();
                //Debug.Log("Do basic attack stuff instead");
                return;
            }
            
            if (!currentAbilityButton.isAssigned)
            {
                if (selectedSlot != -1)//if i have selected a slot button
                {
                    AbilityButton previousAssignedButton = GetAbilityButtonAssignedToSlot(selectedSlot);
                    if (previousAssignedButton != null)
                    {
                        previousAssignedButton.DeselectSlot();
                    }

                    PlayerManager.instance.playerAbilityManager.AssignAbility(selectedSlot, selectedAbility);
                    currentAbilityButton.AssignSlot(selectedSlot);
                    assignButton.GetComponentInChildren<TextMeshProUGUI>().text = "Remove";
                    currentAbilityButton.isAssigned = true;

                    UpdateSlotButtons();
                    ReadyCheckForCombat();
                }
                else
                {
                    Debug.LogWarning("set the slot first");
                }
            }
            else
            {
                PlayerManager.instance.playerAbilityManager.RemoveAbility(currentAbilityButton.assignedSlot);
                currentAbilityButton.DeselectSlot();
                assignButton.GetComponentInChildren<TextMeshProUGUI>().text = "Assign";
                currentAbilityButton.isAssigned = false;

                UpdateSlotButtons();
                ReadyCheckForCombat();
            }
        }
    }
    private void AssignBasicAttack()
    {
        if (!currentAbilityButton.isAssigned)
        {
            if (assignedBasicAttack != null)
            {
                assignedBasicAttack.isAssigned = false;
                assignedBasicAttack.highlightBorder.gameObject.SetActive(false); 
            }

            
            PlayerManager.instance.playerAbilityManager.AssignBasicAttack(selectedAbility);
            currentAbilityButton.isAssigned = true;
            currentAbilityButton.highlightBorder.gameObject.SetActive(true); 
            assignButton.GetComponentInChildren<TextMeshProUGUI>().text = "Remove";

            
            assignedBasicAttack = currentAbilityButton;
            ReadyCheckForCombat();
        }
        else
        {
            
            PlayerManager.instance.playerAbilityManager.AssignBasicAttack(null);
            currentAbilityButton.isAssigned = false;
            currentAbilityButton.highlightBorder.gameObject.SetActive(false); 
            assignButton.GetComponentInChildren<TextMeshProUGUI>().text = "Assign";

            assignedBasicAttack = null;
            ReadyCheckForCombat();
        }
    }
    private AbilityButton GetAbilityButtonAssignedToSlot(int slot)
    {
        AbilityButton[] allAbilityButtons = FindObjectsByType<AbilityButton>(FindObjectsSortMode.None);
        foreach (AbilityButton button in allAbilityButtons)
        {
            if (button.isAssigned && button.GetAssignedSlot() == slot)
            {
                return button;
            }
        }
        return null;
    }
    public void ReadyCheckForCombat()
    {
        //check spell list
        bool allSlotsFilled = true;
        for (int i = 0; i < PlayerManager.instance.playerAbilityManager.spellList.Length; i++)
        {
            if (PlayerManager.instance.playerAbilityManager.spellList[i] == null)
            {
                allSlotsFilled = false;
                break;
            }
        }
        //check basic attack 
        bool basicAttackAssigned = PlayerManager.instance.playerAbilityManager.basicAttack != null;

        //if all slots have spells and a basic attack is assigned, reveal the ready button
        //otherwise hide the button
        readyButton.gameObject.SetActive(basicAttackAssigned && allSlotsFilled);
    }
    public void StartCombatPhase()
    {
        //Debug.Log("Starting combat dps phase");
        PlayerManager.instance.playerAbilityManager.ConfirmLoadout();
        SceneManager.LoadScene(2);
    }
    private void UpdateConfirmationPanel()
    {
        var abilityManager = PlayerManager.instance.playerAbilityManager;

        // update name/icons for abilities selected 
        for (int i = 0; i < abilityIcons.Length; i++)
        {
            if (abilityManager.spellList[i] != null)
            {
                abilityIcons[i].sprite = abilityManager.spellList[i].abilityIcon;
                abilityNames[i].text = abilityManager.spellList[i].abilityName;
                abilityIcons[i].gameObject.SetActive(true);
            }
            else
            {
                abilityIcons[i].gameObject.SetActive(false);
                abilityNames[i].text = "";
            }
        }

        // update basic attack info 
        if (abilityManager.basicAttack != null)
        {
            basicAttackIcon.sprite = abilityManager.basicAttack.abilityIcon;
            basicAttackName.text = abilityManager.basicAttack.abilityName;
            basicAttackIcon.gameObject.SetActive(true);
        }
        else
        {
            basicAttackIcon.gameObject.SetActive(false);
            basicAttackName.text = "";
        }
    }
    public void ShowConfirmationPanel()
    {
        UpdateConfirmationPanel();
        confirmationPanel.SetActive(true);
    }
    public void CancelConfirmation()
    {
        confirmationPanel.SetActive(false);
    }
    private void ResetLoadout()
    {
        PlayerManager.instance.ResetAllAbilities();

        foreach (var icon in abilityIcons)
        {
            icon.gameObject.SetActive(false);
        }

        foreach (var nameText in abilityNames)
        {
            nameText.text = "";
        }

        basicAttackIcon.gameObject.SetActive(false);
        basicAttackName.text = "";

        readyButton.gameObject.SetActive(false);

        ResetSlotButtons();

        UpdateSlotButtons();
    }
    public void ResetSlotButtons()
    {
        for (int i = 0; i < slotButtons.Length; i++) //also need to reset the slot buttons here
        {
            slotButtons[i].interactable = true;
            var colors = slotButtons[i].colors;
            colors.normalColor = Color.white;
            slotButtons[i].colors = colors;
        }
    }
    
}
