using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 20f;

    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600};

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    void Awake() {
        instance = this;
        maxGameTime = 20f;
    }

    public void GameStart(int id) {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId%2); //default 장비
        isLive = true;
        Resume();
    }

    public void GameOver() {
        //바로 Stop()을 하면 사망 모션이 안나옴
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine() {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }

    public void GameVictory() {
        //바로 Stop()을 하면 사망 모션이 안나옴
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine() {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }

    public void GameRetry() {
        SceneManager.LoadScene(0); //scene의 이름 혹은 scene의 인덱스
    }

    void Update() {
        if (!isLive) return;
        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime) {
            gameTime = maxGameTime;
            GameVictory();
        }
    }
    public void GetExp() {
        if (!isLive) return;
        exp++;
        if(exp >= nextExp[Mathf.Min(level,nextExp.Length -1)]) {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop() {
        isLive = false;
        Time.timeScale = 0; //멈추기
    }

    public void Resume() {
        isLive = true;
        Time.timeScale = 1;
    }
}
