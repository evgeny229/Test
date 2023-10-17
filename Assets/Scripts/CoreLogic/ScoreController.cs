using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public int score;
    public Text ScoreCount;
    public Text MaxScoreCount;
    private void Start()
    {
        //MaxScoreCount.text = "max: " + 0;
    }
}
