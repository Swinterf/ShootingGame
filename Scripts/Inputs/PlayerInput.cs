using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject,InputActions.IGamePlayActions,InputActions.IPauseMenuActions
{
    public event UnityAction<Vector2> onMove = delegate { };     //对时间赋一个空值委托，来确保之后在调用的时候不用做非空判断
    public event UnityAction onStop = delegate { };

    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };

    public event UnityAction onDodge = delegate { };

    public event UnityAction onOverdrive = delegate { };
    //public event UnityAction onStopDodge = delegate { };
    public event UnityAction onPause = delegate { };

    public event UnityAction onUnpause = delegate { };

    InputActions inputActions;

    private void OnEnable()
    {
        inputActions = new InputActions();

        //每次添加一个新的动作表都要来添加一次他的回调函数
        inputActions.GamePlay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableAllInputs();
    }

    private void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();

        if (isUIInput)
        {
            Cursor.visible = true;      
            Cursor.lockState = CursorLockMode.None;      
        }
        else
        {
            Cursor.visible = false;      
            Cursor.lockState = CursorLockMode.Locked;     
        }
    }

    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    /// <summary>
    /// 将全部的动作表禁用
    /// </summary>
    public void DisableAllInputs() => inputActions.Disable();

    /// <summary>
    /// 实现玩家动作表的启用
    /// </summary>
    public void EnableGamePlayInput() => SwitchActionMap(inputActions.GamePlay, false);

    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);

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
        if (context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnpause.Invoke();
        }
    }
}
