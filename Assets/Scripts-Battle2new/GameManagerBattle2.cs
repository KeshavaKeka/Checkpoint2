using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerBattle2 : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    float elapsedTime;
    int minutes;
    int seconds;
    public bool isActive;
    public Button pause;
    public Button resume;
    public TextMeshProUGUI gameover;
    public TextMeshProUGUI levelCompleted;
    public Button restart;
    public Button quit;
    public GameObject background;
    public GameObject charac;

    private void Start()
    {
        resume.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        restart.gameObject.SetActive(false);
        background.SetActive(false);
        charac.SetActive(false);
        isActive = true;
        elapsedTime = 120.5f;
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Ensure buttons are set correctly at the start
        pause.onClick.AddListener(Pause);
        resume.onClick.AddListener(Resume);
        resume.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isActive)
        {
            elapsedTime -= Time.deltaTime;
            if (elapsedTime > 0)
            {
                CDown(elapsedTime);
            }
            else
            {
                isActive = false;
                GameOver();
            }
        }
    }

    public void CDown(float elapsedTime)
    {
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void GameOver()
    {
        pause.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        if (isActive)
        {
            levelCompleted.gameObject.SetActive(true);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
        else
        {
            gameover.gameObject.SetActive(true);
        }
        quit.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        background.SetActive(true);
        charac.SetActive(true);
    }

    public void Pause()
    {
        Debug.Log("Pause button clicked.");
        pause.gameObject.SetActive(false);
        resume.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        background.SetActive(true);
        charac.SetActive(true);
        Time.timeScale = 0;

        // Ensure NavMeshAgents are paused if necessary
        UnityEngine.AI.NavMeshAgent[] agents = FindObjectsOfType<UnityEngine.AI.NavMeshAgent>();
        foreach (UnityEngine.AI.NavMeshAgent agent in agents)
        {
            agent.isStopped = true;
        }
    }

    public void Resume()
    {
        Debug.Log("Resume button clicked.");
        pause.gameObject.SetActive(true);
        resume.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        restart.gameObject.SetActive(false);
        background.SetActive(false);
        charac.SetActive(false);
        Time.timeScale = 1;

        // Resume NavMeshAgents if they were paused
        UnityEngine.AI.NavMeshAgent[] agents = FindObjectsOfType<UnityEngine.AI.NavMeshAgent>();
        foreach (UnityEngine.AI.NavMeshAgent agent in agents)
        {
            agent.isStopped = false;
        }
    }

    public void Restart()
    {
        resume.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        restart.gameObject.SetActive(false);
        background.SetActive(false);
        charac.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
