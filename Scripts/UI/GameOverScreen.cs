using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] Canvas HUD_Canvas;
    [SerializeField] AudioData gamoOverSFX;

    int exitStateID = Animator.StringToHash("GameOverScreenExit");

    Canvas canvas;

    Animator animator;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();

        canvas.enabled = false;
        animator.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;

        input.onConfirmGameOver += OnConfirmGameOver;
    }

    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;

        input.onConfirmGameOver -= OnConfirmGameOver;
    }

    private void OnConfirmGameOver()
    {
        AudioManager.Instance.PlayeSFX(gamoOverSFX);
        input.DisableAllInputs();
        animator.Play(exitStateID);
        SceneLoader.Instance.LoadScoringScene();  
    }

    private void OnGameOver()
    {
        HUD_Canvas.enabled = false;
        canvas.enabled = true;
        animator.enabled = true;
        input.DisableAllInputs();
    }

    //Animation event
    private void EnableGameOverScreenInput()
    {
        input.EnableGameOverScreenInput();
    }
}
