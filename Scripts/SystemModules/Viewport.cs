using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����һ������ģʽ��Ƶ���!!
/// </summary>
public class Viewport : Singleten<Viewport>
{
    float minX;
    float middleX;
    float maxX;

    float minY;
    float middleY;
    float maxY;

    private void Start()
    {
        Camera mainCamera = Camera.main;

        //ͨ��mainCamera.ViewportToWorldPoint ������ȡ���ӿ�����������������λ��
        Vector2 buttomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));
        Vector2 middle = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));

        minX = buttomLeft.x;
        minY = buttomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
        middleX = middle.x;
        middleY = middle.y;
    }

    /// <summary>
    /// �÷�����Player �ƶ�λ��������һ����Χ��
    /// </summary>
    /// <param name="playerPosition">player ��ǰλ��</param>
    /// <returns>�������ƹ����λ��</returns>
    public Vector3 PlayerMovablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);

        return position;
    }

    /// <summary>
    /// ������ɵ���λ�ã�Ĭ�ϵ�����λ�����ӿڵ����ұ߾�ͷ֮�⣩
    /// </summary>
    /// <param name="paddingX">X���ƫ��</param>
    /// <param name="paddingY">Y���ƫ��</param>
    /// <returns>���ɵ�λ��</returns>
    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }

    /// <summary>
    /// ���Ƶ���ֻ���ӿڵ��Ұ벿���ƶ�
    /// </summary>
    /// <param name="paddingX">X���ƫ��</param>
    /// <param name="paddingY">Y���ƫ��</param>
    /// <returns>����Ŀǰ��λ��</returns>
    public Vector3 RandomEnemyHalfRightPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(middleX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }
}
