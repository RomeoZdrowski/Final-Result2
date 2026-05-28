using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private bool playerInside = false;

    void Update()
    {
        // Нажатие E возле двери
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Проверка что следующая сцена существует
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Это последняя сцена");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Нажмите E для перехода");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}