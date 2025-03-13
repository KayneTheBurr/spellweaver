using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndCombatManager : MonoBehaviour
{
    [Header("UI Parts")]
    public GameObject endCombatPanel;
    public TextMeshProUGUI totalDamageText;
    public TextMeshProUGUI fireDamageText;
    public TextMeshProUGUI iceDamageText;
    public TextMeshProUGUI lightningDamageText;
    public TextMeshProUGUI poisonDamageText;

    public TextMeshProUGUI rankingText;
    public TMP_InputField playerNameInput;
    public Button submitScoreButton;
    public Button menuButton;
    public Button retryButton;
    public GameObject scoreboardContainer;
    public GameObject scoreEntryPrefab;
    public Transform scoreEntryParent;

    [Header("Game Info")]
    private Dictionary<ElementType, float> damageByElement;
    private float totalDamage;

    private void Start()
    {
        endCombatPanel.SetActive(false);

        submitScoreButton.onClick.AddListener(SubmitScore);
        menuButton.onClick.AddListener(GoToMainMenu);
        retryButton.onClick.AddListener(TryAgain);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(1);
    }
    public void ShowEndCombatPanel(Dictionary<ElementType, float> damageReport)
    {
        // get this and run it from the total combined values in damagetimemanager
        totalDamage = 0;
        damageByElement = damageReport;

        foreach (float dmg in damageByElement.Values)
        {
            totalDamage += dmg;
        }

        totalDamageText.text = $"Total Damage: {totalDamage:N0}";
        fireDamageText.text = $"Fire Damage: {GetDamage(ElementType.Fire):N0}";
        iceDamageText.text = $"Ice Damage: {GetDamage(ElementType.Ice):N0}";
        lightningDamageText.text = $"Lightning Damage: {GetDamage(ElementType.Lightning):N0}";
        poisonDamageText.text = $"Poison Damage: {GetDamage(ElementType.Poison):N0}";

        int place = HighScoreManager.instance.GetMyRank(Mathf.FloorToInt(totalDamage));

        if (place >= 0) // made the top 10 
        {
            rankingText.text = $"You placed {place + 1}! Enter your name:";
            rankingText.gameObject.SetActive(true);
            playerNameInput.gameObject.SetActive(true);
            submitScoreButton.gameObject.SetActive(true);
        }
        else
        {
            rankingText.text = $"Try again for a high score!";
            rankingText.gameObject.SetActive(true);
            playerNameInput.gameObject.SetActive(false);
            submitScoreButton.gameObject.SetActive(false);
        }

        UpdateScoreboard();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        endCombatPanel.SetActive(true);
    }
    private float GetDamage(ElementType element)
    {
        if (damageByElement.ContainsKey(element))
        {
            return damageByElement[element];
        }
        else return 0;
        
        //return damageByElement.ContainsKey(element) ? damageByElement[element] : 0;
    }
    private void SubmitScore()
    {
        string playerName = playerNameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName)) playerName = "?????";

        HighScoreManager.NameAndScore updatedScore = new HighScoreManager.NameAndScore
        {
            Score = Mathf.FloorToInt(totalDamage),
            Name = playerName
        };

        HighScoreManager.instance.AcceptNewScore(updatedScore);
        HighScoreManager.instance.SaveScores();
        UpdateScoreboard();

        playerNameInput.gameObject.SetActive(false);
        submitScoreButton.gameObject.SetActive(false);
    }
    private void UpdateScoreboard()
    {
        foreach (Transform child in scoreEntryParent)
        {
            Destroy(child.gameObject);
        }

        int scoreCount = HighScoreManager.instance.GetScoreCount();
        for (int i = 0; i < scoreCount; i++)
        {
            HighScoreManager.NameAndScore score = HighScoreManager.instance.GetScoreAt(i);
            GameObject entry = Instantiate(scoreEntryPrefab, scoreEntryParent);
            entry.GetComponent<ScoreBoardEntry>().Setup(score, i+1);
        }

        scoreboardContainer.SetActive(true);
    }
}
