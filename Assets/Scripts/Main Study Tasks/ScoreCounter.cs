using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour {
    public int score = 0;

    private Text text;
    private Color targetCyan = new Color32(137,255,255,226);
    private Color successGreen = new Color32(54, 255, 54, 226);

    private void Start()
    {
        text = GetComponent<Text>();

        text.color = targetCyan;
        text.text = " " + score + " von 10";
    }

    public void UpdateScore(bool increase)
    {
        if (increase)
            score += 1;
        else
            score -= 1;

        if (score < 10)
        {
            text.color = targetCyan;
            text.text = " " + score + " von 10";
        }
        else
        {
            text.color = successGreen;
            text.text = score + " von 10";
        }
    }
}
