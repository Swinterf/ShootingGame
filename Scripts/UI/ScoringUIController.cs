using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringUIController : MonoBehaviour
{
    [Header("---- SCORING ----")]
    [SerializeField] Text scoreText;
    [SerializeField] Canvas scoringScreenCanvs;
    [SerializeField] Button buttonMainMenu;

    [Header("---- BACKGROUND ----")]
    [SerializeField] Sprite[] backgroundImages;

    Image background;

    private void Awake()
    {
        background = GameObject.Find("Background").GetComponent<Image>();
    }

    private void Start()
    {
        ShowRandomBackground();
        ShowScoringScreen();

        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonMainMenu.gameObject.name, OnButtonMainMenuClip);

        GameManager.GameState = GameState.Scoring;
    }

    private void OnDisable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }

    private void ShowRandomBackground()
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
    }

    private void ShowScoringScreen()
    {
        scoringScreenCanvs.enabled = true;
        scoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // TODO: Update high score leaderboard UI
    }

    private void OnButtonMainMenuClip()
    {
        scoringScreenCanvs.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
