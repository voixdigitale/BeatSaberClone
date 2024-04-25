using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Image healthBar;

    public static GameUI instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScoreText()
    {
        scoreText.text = string.Format("SCORE\n{0}", GameManager.instance.score.ToString("D8"));
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = GameManager.instance.health;
    }
}
