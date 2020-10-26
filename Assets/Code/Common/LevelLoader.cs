using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public static LevelLoader Instance;

    public Animator transition;

    public float transitionTime = 1f;

    public enum Level {
        MAIN_MENU = 0,
        BATTLE = 1
    }

    private void Awake() {
        Instance = this;
        transition = this.transform.GetChild(0).GetComponent<Animator>();
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
        if (level == Level.BATTLE) {
            
        }

        yield break;
    }
}
