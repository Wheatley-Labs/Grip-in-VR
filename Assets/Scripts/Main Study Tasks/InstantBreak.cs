using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantBreak : MonoBehaviour {
    private bool broken = false;
    private BreakableWindow BreakableWindow;

	// Use this for initialization
	void Start () {
        BreakableWindow = GetComponent<BreakableWindow>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!broken){
            broken = true;
            BreakableWindow.breakWindow();
        }
	}
}
