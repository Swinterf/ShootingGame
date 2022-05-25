using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistenSingleten<ScoreManager>
{
    int score;
    int currentScore;

    Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);

    public void RestScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateScore(score);
    }

    public void AddScore(int scorePoint)
    {
        currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine));
    }

    IEnumerator AddScoreCoroutine()
    {
        //�ڷ�������ǰ���ı��Ŵ�
        ScoreDisplay.ScaleText(scoreTextScale);

        while(score < currentScore)
        {
            score += 1;
            ScoreDisplay.UpdateScore(score);

            yield return null;
        }

        //�ڷ���������ɺ��ı���С
        ScoreDisplay.ScaleText(Vector3.one);
    }
}
