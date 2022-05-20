using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar_HUD : StateBar
{
    [SerializeField] Text percentText;

    private void SetPercentText()
    {
        percentText.text = Mathf.RoundToInt(targetFillAmount * 100f) + "%";
    }

    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercentText();
    }

    protected override IEnumerator BufferFillingCoroutine(Image image)
    {
        SetPercentText();
        return base.BufferFillingCoroutine(image);
        
    }
}
