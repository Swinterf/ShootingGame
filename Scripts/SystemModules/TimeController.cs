using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleten<TimeController>
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;


    float defaultFixedDeltaTime;
    float timeScaleBeforePause;

    float t;

    protected override void Awake()
    {
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        base.Awake();
    }

    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;
    }

    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }

    public void BulletTime(float inDuration, float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }

    public void BulletTime(float inDuration,float keepingDuration, float outDuration)
    {
        SlowInKeepAndOut(inDuration, keepingDuration, outDuration);
    }


    /// <summary>
    /// ʵ�ֻ��������ӵ�ʱ��ĺ���
    /// </summary>
    /// <param name="inDuration">��������ʱ��</param>
    public void SlowIn(float inDuration)
    {
        StartCoroutine(SlowInCoroutine(inDuration));
    }

    /// <summary>
    /// ʵ�ֻ����˳��ӵ�ʱ��ĺ���
    /// </summary>
    /// <param name="inDuration">�˳�����ʱ��</param>
    public void SlowOut(float outDuration)
    {
        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    /// <summary>
    /// ʵ�ֻ��������ӵ�ʱ��Ȼ�󱣳�һ��ʱ����ٻ����˳��ĺ���
    /// </summary>
    /// <param name="inDuration">��������ʱ��</param>
    /// <param name="KeepingDuration">����ʱ��</param>
    /// <param name="outDuration">�˳�����ʱ��</param>
    public void SlowInKeepAndOut(float inDuration, float keepingDuration, float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutCoroutine(inDuration, keepingDuration, outDuration));
    }

    IEnumerator SlowInKeepAndOutCoroutine(float inDuration,float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);

        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0f;

        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)      //ֻ�е���Ϊ��ͣ״̬�²�ʵ���ӵ�ʱ���Ч��
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }

            yield return null;
        }
    }

    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0f;

        while(t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)      //ֻ�е���Ϊ��ͣ״̬�²�ʵ���ӵ�ʱ���Ч��
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }

            yield return null;
        }
    }
}
