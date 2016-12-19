using UnityEngine;
using System.Collections;

public class PersistingPauseBool : MonoBehaviour {
    public bool Paused = true;
	// Use this for initialization
	void Start () {
        if (Paused == null)
            Paused = true;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
