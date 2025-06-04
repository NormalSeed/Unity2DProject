using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isGameOver;
    public bool isGameCleared;
    public int score = 0;
    public float timeRemain = 300f;
    public TextMesh scoreText;
    private static string previousSceneName;

    private void Awake() => Init();
    private void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_TITLE);
    }
    private void Init()
    {
        base.SingletonInit();
        isGameOver = false;
        
    }

    private void Update()
    {

    }

    public void LoadScene(string sceneName)
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadPreviousScene()
    {
        if (!string.IsNullOrEmpty(previousSceneName))
        {
            SceneManager.LoadScene(previousSceneName);
        }
        if (isGameOver) isGameOver = false;
    }

    public void OnStartButtonClicked()
    {
        LoadScene("Stage1");
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_STAGE);
    }

    public void OnRestartButtonClicked()
    {
        LoadPreviousScene();
        if (isGameOver) isGameOver = false;
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_STAGE);
    }

    public void OnTitleButtonClicked()
    {
        LoadScene("Title");
        if (isGameOver) isGameOver = false;
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_TITLE);
    }
    public void AddScore(int newScore)
    {
        if (!isGameOver)
        {
            score += newScore;
        }
    }
}
