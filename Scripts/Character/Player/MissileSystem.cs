using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defaultAmount = 5;
    [SerializeField] float cooldownTime = 2f;
    [SerializeField] GameObject missilePrefab = null;
    [SerializeField] AudioData launchSFX = null;


    int amount;

    bool isReady = true;

    private void Awake()
    {
        amount = defaultAmount;
    }

    private void Start()
    {
        MissileDisPlay.UpdateAmountText(amount);

    }

    public void Launch(Transform muzzleTransform)
    {
        if (amount == 0 || !isReady) return;    //TODO: Add a SFX & VFX here

        isReady = false;
        //Release a missile prefab in pool
        PoolManager.Release(missilePrefab, muzzleTransform.position);
        //Play missile launch SFX
        AudioManager.Instance.PlayRandomSFX(launchSFX);
        amount--;
        MissileDisPlay.UpdateAmountText(amount);

        if (amount == 0)
        {
            MissileDisPlay.UpdateCooldownImage(1f);
        }
        else
        {
            StartCoroutine(nameof(CooldownCoroutine));
        }
    }

    IEnumerator CooldownCoroutine()
    {
        var cooldownValue = cooldownTime;
        while(cooldownValue > 0f)
        {
            MissileDisPlay.UpdateCooldownImage(cooldownValue / cooldownTime);
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);

            yield return null;
        }
        isReady = true;
    }
}
