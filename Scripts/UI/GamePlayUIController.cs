using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIController : MonoBehaviour
{
    [Header("---- PLAYER INPUT ----")]
    [SerializeField] PlayerInput playerInput;

    [Header("---- CANVAS ----")]
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] Canvas menuCanvas;
    [SerializeField] Canvas waveUICanvas;

    [Header("---- BUTTON ----")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button mainMenuButton;

    [Header("---- AUDIO DATA ----")]
    [SerializeField] AudioData pauseSFX;
    [SerializeField] AudioData unpauseSFX;

    int buttonPressedParameterID = Animator.StringToHash("Pressed");

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        //给所有的按钮添加对应的功能函数（注册字典）
        ButtonPressedBehaviour.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClip);
        ButtonPressedBehaviour.buttonFunctionTable.Add(optionButton.gameObject.name, OnOptionButtonClip);
        ButtonPressedBehaviour.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClip);
    }

    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        ButtonPressedBehaviour.buttonFunctionTable.Clear();     //在切换场景时清空字典
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        waveUICanvas.enabled = false;
        hUDCanvas.enabled = false;
        menuCanvas.enabled = true;
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();
        UIInput.Instance.SelectUI(resumeButton);    //当暂停菜单一打开就会自动选中resume按钮了
        AudioManager.Instance.PlayeSFX(pauseSFX);
    }

    private void Unpause()
    {
        resumeButton.Select();      //选中resume按钮
        resumeButton.animator.SetTrigger(buttonPressedParameterID);    //播放preseed动画
        AudioManager.Instance.PlayeSFX(unpauseSFX);
    }

    private void OnResumeButtonClip()
    {
        Time.timeScale = 1f;
        waveUICanvas.enabled = true;
        hUDCanvas.enabled = true;
        menuCanvas.enabled = false;
        playerInput.EnableGamePlayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    private void OnOptionButtonClip()
    {
        //TODO
        UIInput.Instance.SelectUI(optionButton);
        playerInput.EnablePauseMenuInput();
    }

    private void OnMainMenuButtonClip()
    {
        menuCanvas.enabled = false;
        // Load Main Menu Scene
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
