using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这是一个单例模式设计的类!!
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

        //通过mainCamera.ViewportToWorldPoint 函数来取得视口下真正的世界坐标位置
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
    /// 该方法将Player 移动位置限制在一定范围内
    /// </summary>
    /// <param name="playerPosition">player 当前位置</param>
    /// <returns>返回限制过后的位置</returns>
    public Vector3 PlayerMovablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);

        return position;
    }

    /// <summary>
    /// 随机生成敌人位置（默认的生成位置在视口的最右边镜头之外）
    /// </summary>
    /// <param name="paddingX">X轴的偏移</param>
    /// <param name="paddingY">Y轴的偏移</param>
    /// <returns>生成的位置</returns>
    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }

    /// <summary>
    /// 限制敌人只在视口的右半部分移动
    /// </summary>
    /// <param name="paddingX">X轴的偏移</param>
    /// <param name="paddingY">Y轴的偏移</param>
    /// <returns>敌人目前的位置</returns>
    public Vector3 RandomEnemyHalfRightPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(middleX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }
}
