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
    /// ���ڸ���Я��ʵ�ּ�ʱ�ĸ�������
    /// </summary>
    float t;

    /// <summary>
    /// ��ֹ���Я��ͬʱ�����ã�����һ����������ͣ��Я��
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
    /// ��ʼ��״̬��
    /// </summary>
    /// <param name="currentValue">��ǰ״ֵ̬</param>
    /// <param name="maxValue">���ֵ</param>
    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;

        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = targetFillAmount;
    }

    /// <summary>
    /// ����״̬��
    /// </summary>
    /// <param name="currentValue">��ǰ״ֵ̬</param>
    /// <param name="maxValue">���ֵ</param>
    public void UpdateState(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;

        //��ֹ���Я��ͬʱ�����ã�����һһ��Я������ǰ�Ƚ���ͣ��
        if (bufferFillingCoroutine != null)
        {
            StopCoroutine(bufferFillingCoroutine);
        }

        //************* ���л��������Я�̣�Coroutine����ʵ��
        // if ״̬Ϊ����
        if (currentFillAmount > targetFillAmount)
        {
            //ǰ��ͼƬ�����ֵ��fillImageFront.fillAmount��������ΪĿ�����ֵ��targetFillAmount��
            fillImageFront.fillAmount = targetFillAmount;
            //����ͼƬ�����ֵ��fillImageBack.fillAmount������������Ŀ�����ֵ��targetFillAmount��
            if (gameObject.activeSelf)
            {
                bufferFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageBack));
            }
        }
        // if ״̬Ϊ����
        else if (currentFillAmount < targetFillAmount)
        {
            //����ͼƬ�����ֵ��fillImageBack.fillAmount��������ΪĿ�����ֵ��targetFillAmount��
            fillImageBack.fillAmount = targetFillAmount;
            //ǰ��ͼƬ�����ֵ��fillImageFront.fillAmount��������ΪĿ�����ֵ��targetFillAmount��
            if (gameObject.activeSelf)
            {
                bufferFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageFront));
            }
        }
    }

    /// <summary>
    /// ʹ�����Բ�ֵ��ʵ����֡��������Ч��
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator BufferFillingCoroutine(Image image)
    {
        //���ӳ���俪�ؿ���ʱ
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
