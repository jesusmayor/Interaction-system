using UnityEngine;

public class VRHand : MonoBehaviour {

    InputManager inputManager;
    
    void Start()
    {
        inputManager = InputManager.GetInstance();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = inputManager.GetLocalPosition();
        transform.rotation = inputManager.GetLocalRotation();
    }
}
