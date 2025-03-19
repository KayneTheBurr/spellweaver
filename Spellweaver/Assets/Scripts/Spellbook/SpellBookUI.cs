using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookUI : MonoBehaviour
{
    public Canvas spellbookCanvas;
    [SerializeField]public CanvasGroup loadoutCanvasGroup;

    public ColorData redData, blueData, purpleData, greenData;
    public GameObject pagePrefab;
    public GameObject introPagePrefab;
    public Transform pageContainer;
    public Button nextButton, prevButton, closeButton;
    public Button returnToIntroButton;
    public Button fireTab, iceTab, poisonTab, lightningTab;
    public Image fireNewStar, iceNewStar, lightningNewStar, poisonNewStar;
    public TextMeshProUGUI pageNumberText;

    public Image spellbookNewStar;

    private SpellBookPage introPage;
    private List<SpellBookPage> pages = new List<SpellBookPage>();
    private int currentPageIndex = 0;

    private int totalEffects = 27;

    private void Start()
    {
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
        closeButton.onClick.AddListener(CloseSpellbook);
        returnToIntroButton.onClick.AddListener(ReturnToIntroPage);

        SetButtonColors(fireTab, redData);
        SetButtonColors(iceTab, blueData);
        SetButtonColors(lightningTab, purpleData);
        SetButtonColors(poisonTab, greenData);

        fireTab.onClick.AddListener(() => JumpToElement(ElementType.Fire));
        iceTab.onClick.AddListener(() => JumpToElement(ElementType.Ice));
        poisonTab.onClick.AddListener(() => JumpToElement(ElementType.Poison));
        lightningTab.onClick.AddListener(() => JumpToElement(ElementType.Lightning));

        GeneratePages();
        UpdateSpellbookNotification();

        spellbookCanvas.gameObject.SetActive(false);
    }
    public void OpenSpellbook()
    {
        spellbookCanvas.gameObject.SetActive(true);
        loadoutCanvasGroup.interactable = false;
        loadoutCanvasGroup.blocksRaycasts = false;
        
        UpdateDisplay();
    }
    private void GeneratePages()
    {
        foreach (Transform child in pageContainer)
        {
            Destroy(child.gameObject);
        }
        pages.Clear();

        pages.Add(CreateIntroPage()); //add into page here for spellbook

        HashSet<Status> discoveredStatuses = StatusEffectDatabase.instance.GetAllDiscoveredEffects();
        List<StatusEffectData> allEffects = StatusEffectLibrary.instance.GetAllStatusList();

        foreach (StatusEffectData effectData in allEffects)
        {
            bool isDiscovered = discoveredStatuses.Contains(effectData.statusType); //shows info or ?????
            pages.Add(CreatePage(effectData, isDiscovered));
        }
        UpdateIntroPage();
    }
    private void SetButtonColors(Button button, ColorData colorData)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = colorData.baseColor;
        colors.highlightedColor = colorData.hoverColor;
        colors.pressedColor = colorData.selectedColor;
        colors.selectedColor = colorData.selectedColor;

        button.colors = colors;
    }
    private SpellBookPage CreateIntroPage()
    {
        introPage = Instantiate(introPagePrefab, pageContainer).GetComponent<SpellBookPage>();
        return introPage;
    }
    private SpellBookPage CreatePage(StatusEffectData effect, bool isDiscovered)
    {
        SpellBookPage page = Instantiate(pagePrefab, pageContainer).GetComponent<SpellBookPage>();
        page.SetPageData(effect, isDiscovered);
        return page;
    }
    private void UpdateDisplay()
    {
        if (pages.Count == 0) return;

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].gameObject.SetActive(i == currentPageIndex);
        }

        SpellBookPage currentPage = pages[currentPageIndex];

        if (currentPage.effectData != null)
        {
            StatusEffectDatabase.instance.MarkEffectAsViewed(currentPage.effectData.statusType);
        }

        pageNumberText.text = $"{currentPageIndex + 1} / {pages.Count}";
        prevButton.interactable = currentPageIndex > 0;
        nextButton.interactable = currentPageIndex < pages.Count - 1;

        UpdateTabNotifications();
        UpdateSpellbookNotification();
        UpdateIntroPage();
    }
    private void UpdateIntroPage()
    {
        if (introPage != null)
        {
            int discoveredCount = StatusEffectDatabase.instance.GetAllDiscoveredEffects().Count;
            totalEffects = StatusEffectLibrary.instance.GetAllStatusList().Count;
            introPage.UpdateIntroPage(discoveredCount, totalEffects);
        }
    }
    private void UpdateTabNotifications()
    {
        HashSet<Status> newEffects = StatusEffectDatabase.instance.GetNewEffects();

        fireNewStar.gameObject.SetActive(newEffects.Any(effect => 
        StatusEffectLibrary.instance.GetStatusEffectData(effect).causeElement1 == ElementType.Fire));

        iceNewStar.gameObject.SetActive(newEffects.Any(effect => 
        StatusEffectLibrary.instance.GetStatusEffectData(effect).causeElement1 == ElementType.Ice));

        poisonNewStar.gameObject.SetActive(newEffects.Any(effect => 
        StatusEffectLibrary.instance.GetStatusEffectData(effect).causeElement1 == ElementType.Poison));

        lightningNewStar.gameObject.SetActive(newEffects.Any(effect => 
        StatusEffectLibrary.instance.GetStatusEffectData(effect).causeElement1 == ElementType.Lightning));
    }
    private void UpdateSpellbookNotification()
    {
        HashSet<Status> newEffects = StatusEffectDatabase.instance.GetNewEffects();
        spellbookNewStar.gameObject.SetActive(newEffects.Count > 0); // Show notif if any effects are new
    }
    public void NextPage()
    {
        if (currentPageIndex < pages.Count - 1)
        {
            currentPageIndex++;
            UpdateDisplay();
        }
    }
    public void PrevPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdateDisplay();
        }
    }
    public void JumpToElement(ElementType type)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            SpellBookPage pageScript = pages[i].GetComponent<SpellBookPage>();
            if (pageScript != null && pageScript.effectData != null)
            {
                if (pageScript.effectData.causeElement1 == type)
                {
                    currentPageIndex = i;
                    UpdateDisplay();
                    return;
                }
            }
            else Debug.Log($"No data on page {i}");
        }
    }
    private void ReturnToIntroPage()
    {
        currentPageIndex = 0;
        UpdateDisplay();
    }
    public void CloseSpellbook()
    {
        loadoutCanvasGroup.interactable = true;
        loadoutCanvasGroup.blocksRaycasts = true;
        spellbookCanvas.gameObject.SetActive(false);
    }
}
