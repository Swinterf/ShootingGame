using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class UIInput : Singleten<UIInput>
{
    [SerializeField] PlayerInput playerInput;

    InputSystemUIInputModule UIInputModule;

    protected override void Awake()
    {
        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }

    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }

    public void DisableAllUIInputs()
    {
        playerInput.DisableAllInputs();
        UIInputModule.enabled = false;
    }

}
