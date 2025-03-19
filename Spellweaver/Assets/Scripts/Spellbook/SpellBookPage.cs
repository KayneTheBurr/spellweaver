using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookPage : MonoBehaviour
{
    public TextMeshProUGUI statusName;
    public TextMeshProUGUI statusDescription;
    public Image statusIcon;
    public Image elementIcon1;
    public TextMeshProUGUI element1Label;
    public Image elementIcon2;
    public TextMeshProUGUI element2Label;
    public TextMeshProUGUI discoveredCountText;

    public StatusEffectData effectData { get; private set; }

    public void SetPageData(StatusEffectData statusData, bool isDiscovered)
    {
        this.effectData = statusData;
        
        if (isDiscovered)
        {
            statusName.text = statusData.effectName;
            statusDescription.text = statusData.description;

            statusIcon.sprite = statusData.statusIcon;

            elementIcon1.sprite = statusData.element1Icon;
            elementIcon2.sprite = statusData.element2Icon;

            element1Label.text = statusData.element1name;
            element2Label.text = statusData.element2name;
        }
        else
        {
            statusName.text = "Unknown";
            statusDescription.text = "This effect has not been discovered yet!";
            statusIcon = null;
            elementIcon1 = null;
            elementIcon2 = null;

            element1Label.text = "?????";
            element2Label.text = "?????";
        }
    }
    public void UpdateIntroPage(int discovered, int total)
    {
        if (discoveredCountText != null)
        {
            discoveredCountText.text = $"Discovered: {discovered} / {total}";
        }
    }
}
