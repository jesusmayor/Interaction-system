using UnityEngine;

public class VRHand : MonoBehaviour {
    InputManager inputManager;
    public InputManager.Hands hand = InputManager.Hands.Left;
    
    void Start()
    {
        inputManager = InputManager.GetInstance();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = inputManager.GetLocalPosition(hand);
        transform.rotation = inputManager.GetLocalRotation(hand);
    }
}
