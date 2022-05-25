using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject,InputActions.IGamePlayActions
{
    public event UnityAction<Vector2> onMove = delegate { };     //对时间赋一个空值委托，来确保之后在调用的时候不用做非空判断
    public event UnityAction onStop = delegate { };

    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };

    public event UnityAction onDodge = delegate { };
    //public event UnityAction onStopDodge = delegate { };

    InputActions inputActions;

    private void OnEnable()
    {
        inputActions = new InputActions();

        //每次添加一个新的动作表都要来添加一次他的回调函数
        inputActions.GamePlay.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableAllInputs();
    }

    /// <summary>
    /// 将全部的动作表禁用
    /// </summary>
    public void DisableAllInputs()
    {
        inputActions.GamePlay.Disable();
    }
    /// <summary>
    /// 实现玩家动作表的启用
    /// </summary>
    public void EnableGamePlayInput()
    {
        //启用GamePlaye动作表
        inputActions.GamePlay.Enable();

        //设置鼠标状态
        Cursor.visible = false;      //将鼠标显示为不可见
        Cursor.lockState = CursorLockMode.Locked;      //将鼠标设置为锁定状态

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //当玩家按下绑定按键的时候调用onMovw 事件
        if(context.phase == InputActionPhase.Performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        //当玩家松开绑定按键的时候调用onStop 事件
        if(context.phase == InputActionPhase.Canceled)
        {
            onStop.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)     //OR context.performed
        {
            onFire.Invoke();
        }
        if(context.phase == InputActionPhase.Canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
        //if (context.canceled)
        //{

        //}
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
