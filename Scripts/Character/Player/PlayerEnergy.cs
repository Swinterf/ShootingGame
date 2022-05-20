using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����������Ŀ��ֻ����һ���������ϵͳ��PlayerEnergy��
//��˿������ɵ���ģʽ�������������
public class PlayerEnergy : Singleten<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;

    public const int MAX = 100;
    public const int PERCENT = 1;

    int energy;

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
        if (energy == MAX) return;
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
    }

    /// <summary>
    /// �ж�Ŀǰ�����Ƿ񾭵�������
    /// </summary>
    /// <param name="value">��Ҫ���ĵ�����</param>
    /// <returns>�㹻���ķ���true����֮����false</returns>
    public bool isEnough(int value) => energy >= value;     //ֻ��һ��ĺ�������д����˵���ķ����ʽ
    //{
    //    //int restEnergy = energy - value;
    //    //if (restEnergy >= 0)
    //    //    return true;
    //    //else
    //    //    return false;

    //    //�����
    //    return energy >= value;
    //}

}
