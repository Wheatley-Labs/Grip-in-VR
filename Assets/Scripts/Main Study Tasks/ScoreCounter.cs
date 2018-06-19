using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK.Examples;
using UnityEngine.SceneManagement;

public class ScoreCounter : MonoBehaviour
{
    public int score = 0;
    public int maxScore = 10;

    public delegate void scoreEventHandler(int currentScore, GameObject item);
    public event scoreEventHandler OnScore;

    private Text text;
    private Color targetCyan = new Color32(137, 255, 255, 226);
    private Color successGreen = new Color32(54, 255, 54, 226);

    public SessionManager sessionManager;

    private void Start()
    {
        text = GetComponent<Text>();

        text.color = targetCyan;
        text.text = score + " von " + maxScore;
    }

    public void UpdateScore(bool increase, GameObject glass)
    {
        if (increase)
            score += 1;
        else
            score -= 1;

        if (score < maxScore)
        {
            text.color = targetCyan;
            text.text = score + " von " + maxScore;
        }
        else
        {
            text.color = successGreen;
            text.text = score + " von " + maxScore;
            if (!SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                sessionManager.LevelFinished();
            }
        }
        
        if(OnScore != null)
        {
            OnScore(score, glass);
        }
    }


}