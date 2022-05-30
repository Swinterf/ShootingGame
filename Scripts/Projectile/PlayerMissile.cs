using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [Header("---- SFX ----")]
    [SerializeField] AudioData targetAcquireVoice = null;

    [Header("---- SPEED CHANGE ----")]
    [SerializeField] float lowSpeed = 8f;
    [SerializeField] float highSpeed = 15f;
    [SerializeField] float variableSpeedDelayTime = 0.5f;

    WaitForSeconds waitVariableSpeedDelayTime;

    private protected override void Awake()
    {
        base.Awake();
        waitVariableSpeedDelayTime = new WaitForSeconds(variableSpeedDelayTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(variableSpeedCoroutine));
    }

    IEnumerator variableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;

        yield return waitVariableSpeedDelayTime;

        moveSpeed = highSpeed;

        if(target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquireVoice);
        }
    }

}
