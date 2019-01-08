using UnityEngine;
using System.Collections;

public class TimerDestroy : MonoBehaviour {

    private float Timer = 0;
    private float mTimer = 0;
	// Use this for initialization
	public void Init (float Timer) {
        this.Timer = Timer;
	}
	
	// Update is called once per frame
	void Update () {
        this.mTimer += Time.deltaTime;
        if (mTimer >= Timer) {
            Destroy(gameObject);
        }
	}
}
