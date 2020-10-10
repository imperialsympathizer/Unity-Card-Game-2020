using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public static LevelLoader SharedInstance;

    public Animator transition;

    public float transitionTime = 1f;

    private void Awake() {
        SharedInstance = this;
    }

    public void LoadLevel(Level level) {
        StartCoroutine(LoadScene(level));
    }

    private IEnumerator LoadScene(Level level) {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        SceneManager.LoadScene((int)level);

        yield break;
    }

    public enum Level {
        MAIN_MENU = 0,
        BATTLE = 1
    }
}
