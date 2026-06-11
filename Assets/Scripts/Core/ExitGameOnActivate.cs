using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitGameOnActivate : MonoBehaviour
{
    [SerializeField] private float delayBeforeQuit = 0.2f;

    private bool quitScheduled;

    private void OnEnable()
    {
        if (quitScheduled)
            return;

        quitScheduled = true;
        StartCoroutine(QuitRoutine());
    }

    private IEnumerator QuitRoutine()
    {
        if (delayBeforeQuit > 0f)
            yield return new WaitForSeconds(delayBeforeQuit);

        // Ждём конец кадра, чтобы Timeline закончил обработку PlayableGraph
        yield return new WaitForEndOfFrame();

#if UNITY_EDITOR
        EditorApplication.delayCall += StopPlayMode;
#else
        Application.Quit();
#endif
    }

#if UNITY_EDITOR
    private void StopPlayMode()
    {
        EditorApplication.isPlaying = false;
    }
#endif
}