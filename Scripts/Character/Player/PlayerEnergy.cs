using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//由于整个项目中只会有一个玩家能量系统（PlayerEnergy）
//因此可以做成单例模式方便其他类调用
public class PlayerEnergy : Singleten<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;
    [SerializeField] float overdriveInterval = 0.1f;

    public const int MAX = 100;
    public const int PERCENT = 1;

    int energy;

    bool available = true;

    WaitForSeconds waitOverdriveInterval;
    protected override void Awake()
    {
        waitOverdriveInterval = new WaitForSeconds(overdriveInterval);
        base.Awake();
    }

    private void OnEnable()
    {
        PlayerOverdive.on += PlayerOverdriveOn;
        PlayerOverdive.off += PlayerOverdriveOff;
    }

    private void OnDisable()
    {
        PlayerOverdive.on -= PlayerOverdriveOn;
        PlayerOverdive.off -= PlayerOverdriveOff;
    }


    private void Start()
    {
        energyBar.Initialize(energy, MAX);
        Obtian(MAX);
    }

    /// <summary>
    /// 获取能量 并 更新状态条显示的函数
    /// </summary>
    /// <param name="value">获取到的能量的多少</param>
    public void Obtian(int value)
    {
        if (energy == MAX || !available || !gameObject.activeSelf) return;
        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdateState(energy, MAX);     //更新状态条的显示
    }

    /// <summary>
    /// 消耗能量 并 更新状态条显示的函数的函数
    /// </summary>
    /// <param name="value">消耗的能量的多少</param>
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateState(energy, MAX);

        if(energy == 0 && !available)
        {
            PlayerOverdive.off.Invoke();
        }
    }

    /// <summary>
    /// 判断目前能量是否经得起消耗
    /// </summary>
    /// <param name="value">需要消耗的能量</param>
    /// <returns>足够消耗返回true，反之返回false</returns>
    public bool isEnough(int value) => energy >= value;
    //只有一句的函数可以写成如此的拉姆达表达式
    //{
    //    //int restEnergy = energy - value;
    //    //if (restEnergy >= 0)
    //    //    return true;
    //    //else
    //    //    return false;
    //    //妙啊！！
    //    return energy >= value;
    //}
    private void PlayerOverdriveOn()
    {
        available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }

    private void PlayerOverdriveOff()
    {
        available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    IEnumerator KeepUsingCoroutine()
    {
        while(gameObject.activeSelf && energy > 0)
        {
            //ever 0.1 second
            yield return waitOverdriveInterval;

            //use 0.1 percent of MAX energy
            //means overdrive state last 10 seconds
            Use(PERCENT);
        }
    }

}
