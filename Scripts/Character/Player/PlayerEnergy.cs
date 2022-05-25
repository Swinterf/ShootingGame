using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����������Ŀ��ֻ����һ���������ϵͳ��PlayerEnergy��
//��˿������ɵ���ģʽ�������������
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
    /// ��ȡ���� �� ����״̬����ʾ�ĺ���
    /// </summary>
    /// <param name="value">��ȡ���������Ķ���</param>
    public void Obtian(int value)
    {
        if (energy == MAX || !available || !gameObject.activeSelf) return;
        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdateState(energy, MAX);     //����״̬������ʾ
    }

    /// <summary>
    /// �������� �� ����״̬����ʾ�ĺ����ĺ���
    /// </summary>
    /// <param name="value">���ĵ������Ķ���</param>
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
    /// �ж�Ŀǰ�����Ƿ񾭵�������
    /// </summary>
    /// <param name="value">��Ҫ���ĵ�����</param>
    /// <returns>�㹻���ķ���true����֮����false</returns>
    public bool isEnough(int value) => energy >= value;
    //ֻ��һ��ĺ�������д����˵���ķ����ʽ
    //{
    //    //int restEnergy = energy - value;
    //    //if (restEnergy >= 0)
    //    //    return true;
    //    //else
    //    //    return false;
    //    //�����
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
