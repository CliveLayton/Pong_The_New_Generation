using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private BallMainMenu ballMenuPrefab;
    private float spawnWaitTime = 0f;

    private void Update()
    {
        spawnWaitTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && spawnWaitTime > 1f)
        {
            Instantiate(ballMenuPrefab, Vector3.zero, Quaternion.identity);
            spawnWaitTime = 0f;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
