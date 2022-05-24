using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWaveUI : MonoBehaviour
{
    #region FIELDS
    /// <summary>
    /// ��������ʱ��
    /// </summary>
    [SerializeField] float animationTime = 1f;

    [Header("---- LINE MOVE ----")]
    [SerializeField] Vector2 lineTopStartPosition = new Vector2(-1250f, 140f);
    [SerializeField] Vector2 lineTopTargetPosition = new Vector2(0f, 140f);
    [SerializeField] Vector2 lineBottomStartPosition = new Vector2(1250f, 0f);
    [SerializeField] Vector2 lineBottomTargetPosition = Vector2.zero;


    [Header("---- TEXT SCALE ----")]
    [SerializeField] Vector2 waveTextStartScale = new Vector2(1f, 0f);
    [SerializeField] Vector2 waveTextTargetScale = Vector2.one;



    RectTransform lineTop;
    RectTransform lineBottom;
    RectTransform waveText;

    /// <summary>
    /// UI�ڻ����еȴ���ͣ��ʱ�����
    /// </summary>
    WaitForSeconds waitUIStayTime;
    #endregion

    #region UNITY EVENT FUNCTIONS
    private void Awake()
    {
        //����ö�����ʵ��WaveUI �Ķ�̬Ч����ɾ������ű�
        if(TryGetComponent<Animator>(out Animator animator))
        {
            if (animator.isActiveAndEnabled)
            {
                Destroy(this);
            }
        }

        waitUIStayTime = new WaitForSeconds(EnemyManager.Instance.TimeBetweenWaves - animationTime * 2);

        lineTop = transform.Find("Line Top").GetComponent<RectTransform>();
        lineBottom = transform.Find("Line Bottom").GetComponent<RectTransform>();
        waveText = transform.Find("Wave Text").GetComponent<RectTransform>();

        //���ó�ʼλ��
        lineTop.position = lineTopStartPosition;
        lineBottom.position = lineBottomStartPosition;
        waveText.localScale = waveTextStartScale;
    }

    private void OnEnable()
    {
        StartCoroutine(LineMoveCoroutine(lineTop, lineTopTargetPosition, lineTopStartPosition));
        StartCoroutine(LineMoveCoroutine(lineBottom, lineBottomTargetPosition, lineBottomStartPosition));
        StartCoroutine(TextScaleCoroutine(waveText, waveTextTargetScale, waveTextStartScale));
    }

    #endregion

    #region LINE MOVE

    IEnumerator LineMoveCoroutine(RectTransform rect, Vector2 targetPosition, Vector2 startPosition)
    {
        yield return StartCoroutine(UIMoveCoroutine(rect, targetPosition)); //�ƶ���Ŀ��λ��
        yield return waitUIStayTime;
        yield return StartCoroutine(UIMoveCoroutine(rect, startPosition));  //�ƶ���ԭ��λ��
    }

    IEnumerator UIMoveCoroutine(RectTransform rect, Vector2 position)
    {
        float t = 0;
        Vector2 localPosition = rect.localPosition;

        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            rect.localPosition = Vector2.Lerp(localPosition, position, t);

            yield return null;
        }
    }
    #endregion

    #region TEXT SCALE

    IEnumerator TextScaleCoroutine(RectTransform rect, Vector2 waveTextTargetScale, Vector2 waveTextStartScale)
    {
        yield return StartCoroutine(UIScaleCoroutine(rect, waveTextTargetScale));
        yield return waitUIStayTime;
        yield return StartCoroutine(UIScaleCoroutine(rect, waveTextStartScale));
    }

    IEnumerator UIScaleCoroutine(RectTransform rect, Vector2 scale)
    {
        float t = 0f;
        Vector3 localScale = rect.localScale;
        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            rect.localScale = Vector2.Lerp(localScale, scale, t);

            yield return null;
        }
    }

    #endregion
}
