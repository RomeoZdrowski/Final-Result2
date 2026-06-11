using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSceneLoader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health bossHealth;

    [Header("Scene")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private float delayBeforeLoad = 2f;

    private bool sceneLoading;

    private void Awake()
    {
        if (bossHealth == null)
            bossHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (sceneLoading)
            return;

        if (bossHealth != null && bossHealth.IsDead)
        {
            sceneLoading = true;
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        SceneManager.LoadScene(nextSceneName);
    }
}