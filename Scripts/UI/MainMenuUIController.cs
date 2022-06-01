using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    [Header("---- CANVAS ----")]
    [SerializeField] Canvas mainMenuCanvas;
    
    [Header("---- BUTTON ----")]
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonOptions;
    [SerializeField] Button buttonExit;


    private void OnEnable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClip);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnButtonOptionsClip);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonExit.gameObject.name, OnButtonExitClip);
    }

    private void OnDisable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }

    private void Start()
    {
        Time.timeScale = 1;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }

    void OnButtonStartClip()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGamePlayScene();
    }

    void OnButtonOptionsClip()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }

    void OnButtonExitClip()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
