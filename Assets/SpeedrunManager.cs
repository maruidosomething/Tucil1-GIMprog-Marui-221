using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpeedrunManager : MonoBehaviour
{
    public static SpeedrunManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI leaderboardText;

    [Header("Settings")]
    public float countdownTime = 3f;
    public bool isPlaying = false;

    private float currentTime = 0f;
    private string sceneName;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        sceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        UpdateLeaderboardUI();
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(sceneName);
        }

        if (isPlaying)
        {
            currentTime += Time.deltaTime;
            UpdateTimerUI(currentTime, timerText, "");
        }
    }

    private IEnumerator StartCountdown()
    {
        isPlaying = false;
        countdownText.gameObject.SetActive(true);

        float t = countdownTime;
        while (t > 0)
        {
            countdownText.text = Mathf.Ceil(t).ToString();
            yield return new WaitForSeconds(1f);
            t -= 1f;
        }

        isPlaying = true;
        currentTime = 0f;

        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }

    private void UpdateTimerUI(float time, TextMeshProUGUI textElement, string prefix)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = (time % 1) * 1000;
        textElement.text = string.Format("{0}{1:00}:{2:00}.{3:000}", prefix, minutes, seconds, milliseconds);
    }

    public void FinishLevel()
    {
        if (!isPlaying) return;
        
        isPlaying = false;
        SaveScore(currentTime);
        UpdateLeaderboardUI();
    }

    private void SaveScore(float time)
    {
        float bestTime = PlayerPrefs.GetFloat(sceneName + "_BestTime", float.MaxValue);
        if (time < bestTime)
        {
            PlayerPrefs.SetFloat(sceneName + "_BestTime", time);
            PlayerPrefs.Save();
        }
    }

    private void UpdateLeaderboardUI()
    {
        float bestTime = PlayerPrefs.GetFloat(sceneName + "_BestTime", float.MaxValue);
        if (bestTime == float.MaxValue)
        {
            leaderboardText.text = "Best Time: --:--.---";
        }
        else
        {
            UpdateTimerUI(bestTime, leaderboardText, "Best Time: ");
        }
    }
}