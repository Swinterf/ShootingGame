using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    [SerializeField] Button startGameButton;

    private void OnEnable()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClip);
    }

    private void OnDisable()
    {
        startGameButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    void OnStartGameButtonClip()
    {
        SceneLoader.Instance.LoadGamePlayScene();
    }
}
