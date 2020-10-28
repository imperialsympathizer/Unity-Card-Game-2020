using System.Collections;
using UnityEngine;

public class ObjectBlink : MonoBehaviour {

    public float blinkSpeed = 0.5f;
    public bool stop = false;

    private bool state = false;
    private GameObject blinkObject;

    // Start is called before the first frame update
    void Start() {
        blinkObject = this.transform.GetChild(0).gameObject;
        StartCoroutine(Blink());
    }

    private IEnumerator Blink() {
        while (!stop) {
            blinkObject.SetActive(state);
            state = !state;
            yield return new WaitForSeconds(blinkSpeed);
        }

        yield break;
    }
}
