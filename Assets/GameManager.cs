using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int maxRounds = 10;
    public TextMeshProUGUI roundText;
    public GameObject roundScreen;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public Transform enemySpawnPoint;
    public Transform bossSpawnPoint;
    public PlayerHealth playerHealth;
    public Image fadeImage;

    public float startSpawnDelay = 2.0f;
    public float fadeInDuration = 1.0f;

    private int currentRound = 1;
    private int enemiesRemaining;
    private bool roundInProgress = false;
    private Vector3 initialPlayerPosition;

    void Start()
    {
        initialPlayerPosition = playerHealth.transform.position;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return StartCoroutine(DisplayRoundScreen("Ronda " + currentRound));
        StartRound();
    }

    void StartRound()
    {
        roundInProgress = true;
        roundText.text = "Ronda " + currentRound;

        StartCoroutine(DelayedStartSpawn());
    }

    IEnumerator DelayedStartSpawn()
    {
        yield return new WaitForSeconds(startSpawnDelay);

        if (currentRound < maxRounds)
        {
            enemiesRemaining = Random.Range(3, 5);
            StartCoroutine(SpawnEnemies());
        }
        else
        {
            SpawnBoss();
        }
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesRemaining; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
            enemy.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        boss.SetActive(true);
        enemiesRemaining = 1;
    }

    public void EnemyKilled()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0 && roundInProgress)
        {
            roundInProgress = false;
            EndRound();
        }
    }

    void EndRound()
    {
        StartCoroutine(FadeScreen(true, 0.5f));

        if (currentRound % 1 == 0 || currentRound == maxRounds)
        {
            StartCoroutine(HealPlayerAndResetPosition(1.0f));
        }

        currentRound++;
        if (currentRound <= maxRounds)
        {
            StartCoroutine(DisplayRoundScreen("Ronda " + currentRound));
            StartRound();
        }
        else
        {
            Debug.Log("Game Over: You completed all rounds!");
            SceneManager.LoadScene("Pantalla_FIN");

        }
    }

    IEnumerator DisplayRoundScreen(string text)
    {
        roundText.text = text;
        roundScreen.SetActive(true);

        StartCoroutine(FadeScreen(false, fadeInDuration));

        float countdownTime = 3f;
        while (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            yield return null;
        }

        roundScreen.SetActive(false);
        StartRound();
    }

    IEnumerator FadeScreen(bool fadeOut, float duration)
    {
        Color targetColor = fadeOut ? Color.black : Color.clear;
        Color initialColor = fadeImage.color;
        float timer = 0.0f;

        while (timer < duration)
        {
            fadeImage.color = Color.Lerp(initialColor, targetColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = targetColor;
    }

    IEnumerator HealPlayerAndResetPosition(float delay)
    {
        yield return new WaitForSeconds(delay);

        playerHealth.Heal(playerHealth.maxHealth);
        playerHealth.transform.position = initialPlayerPosition;

        StartCoroutine(FadeScreen(false, fadeInDuration));
    }
}
