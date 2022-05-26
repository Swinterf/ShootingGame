using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;

    [SerializeField] float fillSpeed = 0.1f;

    [SerializeField] bool isDelayFill = true;
    [SerializeField] float fillDelay = 0.5f;

    float currentFillAmount;
    protected float targetFillAmount;
    float previousFillAmount;


    Canvas canvas;

    WaitForSeconds waitForDelayFill;

    /// <summary>
    /// 用于辅助携程实现计时的辅助参数
    /// </summary>
    float t;

    /// <summary>
    /// 防止多个携程同时被调用，设置一个参数方便停用携程
    /// </summary>
    Coroutine bufferFillingCoroutine;

    private void Awake()
    {
        if(TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }

        waitForDelayFill = new WaitForSeconds(fillDelay);

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 初始化状态条
    /// </summary>
    /// <param name="currentValue">当前状态值</param>
    /// <param name="maxValue">最大值</param>
    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;

        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = targetFillAmount;
    }

    /// <summary>
    /// 更新状态条
    /// </summary>
    /// <param name="currentValue">当前状态值</param>
    /// <param name="maxValue">最大值</param>
    public void UpdateState(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;

        //防止多个携程同时被调用，在下一一次携程启用前先将其停用
        if (bufferFillingCoroutine != null)
        {
            StopCoroutine(bufferFillingCoroutine);
        }

        //************* 其中缓慢填充用携程（Coroutine）来实现
        // if 状态为减少
        if (currentFillAmount > targetFillAmount)
        {
            //前面图片的填充值（fillImageFront.fillAmount）立即变为目标填充值（targetFillAmount）
            fillImageFront.fillAmount = targetFillAmount;
            //后面图片的填充值（fillImageBack.fillAmount）缓慢减少至目标填充值（targetFillAmount）
            if (gameObject.activeSelf)
            {
                bufferFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageBack));
            }
        }
        // if 状态为增加
        else if (currentFillAmount < targetFillAmount)
        {
            //后面图片的填充值（fillImageBack.fillAmount）立即变为目标填充值（targetFillAmount）
            fillImageBack.fillAmount = targetFillAmount;
            //前面图片的填充值（fillImageFront.fillAmount）缓慢变为目标填充值（targetFillAmount）
            if (gameObject.activeSelf)
            {
                bufferFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageFront));
            }
        }
    }

    /// <summary>
    /// 使用线性插值来实现逐帧缓慢填充的效果
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator BufferFillingCoroutine(Image image)
    {
        //当延迟填充开关开启时
        if (isDelayFill)
        {
            yield return waitForDelayFill;
        }

        previousFillAmount = currentFillAmount;

        t = 0f;
        while (t < 1f)
        {
            t += Time.fixedDeltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;

            yield return null;
        }
    }
}
