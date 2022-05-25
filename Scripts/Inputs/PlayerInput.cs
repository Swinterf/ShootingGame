using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject,InputActions.IGamePlayActions
{
    public event UnityAction<Vector2> onMove = delegate { };     //��ʱ�丳һ����ֵί�У���ȷ��֮���ڵ��õ�ʱ�������ǿ��ж�
    public event UnityAction onStop = delegate { };

    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };

    public event UnityAction onDodge = delegate { };
    //public event UnityAction onStopDodge = delegate { };

    InputActions inputActions;

    private void OnEnable()
    {
        inputActions = new InputActions();

        //ÿ�����һ���µĶ�����Ҫ�����һ�����Ļص�����
        inputActions.GamePlay.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableAllInputs();
    }

    /// <summary>
    /// ��ȫ���Ķ��������
    /// </summary>
    public void DisableAllInputs()
    {
        inputActions.GamePlay.Disable();
    }
    /// <summary>
    /// ʵ����Ҷ����������
    /// </summary>
    public void EnableGamePlayInput()
    {
        //����GamePlaye������
        inputActions.GamePlay.Enable();

        //�������״̬
        Cursor.visible = false;      //�������ʾΪ���ɼ�
        Cursor.lockState = CursorLockMode.Locked;      //���������Ϊ����״̬

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //����Ұ��°󶨰�����ʱ�����onMovw �¼�
        if(context.phase == InputActionPhase.Performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        //������ɿ��󶨰�����ʱ�����onStop �¼�
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
