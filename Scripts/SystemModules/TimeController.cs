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
    /// 实现缓慢进入子弹时间的函数
    /// </summary>
    /// <param name="inDuration">进入所需时间</param>
    public void SlowIn(float inDuration)
    {
        StartCoroutine(SlowInCoroutine(inDuration));
    }

    /// <summary>
    /// 实现缓慢退出子弹时间的函数
    /// </summary>
    /// <param name="inDuration">退出所需时间</param>
    public void SlowOut(float outDuration)
    {
        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    /// <summary>
    /// 实现缓慢进入子弹时间然后保持一段时间后再缓慢退出的函数
    /// </summary>
    /// <param name="inDuration">进入所需时间</param>
    /// <param name="KeepingDuration">保持时间</param>
    /// <param name="outDuration">退出所需时间</param>
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
            if (GameManager.GameState != GameState.Paused)      //只有当不为暂停状态下才实现子弹时间的效果
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
            if (GameManager.GameState != GameState.Paused)      //只有当不为暂停状态下才实现子弹时间的效果
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }

            yield return null;
        }
    }
}
