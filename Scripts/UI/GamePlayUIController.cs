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

        //�����еİ�ť��Ӷ�Ӧ�Ĺ��ܺ�����ע���ֵ䣩
        ButtonPressedBehaviour.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClip);
        ButtonPressedBehaviour.buttonFunctionTable.Add(optionButton.gameObject.name, OnOptionButtonClip);
        ButtonPressedBehaviour.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClip);
    }

    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        ButtonPressedBehaviour.buttonFunctionTable.Clear();     //���л�����ʱ����ֵ�
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        waveUICanvas.enabled = false;
        hUDCanvas.enabled = false;
        menuCanvas.enabled = true;
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();
        UIInput.Instance.SelectUI(resumeButton);    //����ͣ�˵�һ�򿪾ͻ��Զ�ѡ��resume��ť��
        AudioManager.Instance.PlayeSFX(pauseSFX);
    }

    private void Unpause()
    {
        resumeButton.Select();      //ѡ��resume��ť
        resumeButton.animator.SetTrigger(buttonPressedParameterID);    //����preseed����
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
