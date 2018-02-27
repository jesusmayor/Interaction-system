using UnityEngine;

public class Recenter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		UnityEngine.XR.InputTracking.Recenter ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.R))
			UnityEngine.XR.InputTracking.Recenter ();
	
	}
}
