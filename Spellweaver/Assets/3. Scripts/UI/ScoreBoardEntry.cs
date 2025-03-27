using UnityEngine;
using TMPro;

public class ScoreBoardEntry : MonoBehaviour
{
    public TextMeshProUGUI NameLabel;
    public TextMeshProUGUI ScoreLabel;
    public TextMeshProUGUI rankLabel;

    public void Setup(HighScoreManager.NameAndScore s, int rank)
    {
        rankLabel.text = $"{rank}.";
        NameLabel.text = s.Name;
        ScoreLabel.text = $"{s.Score:N0}";
    }
}
