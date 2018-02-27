//Código desarrollado por Jesús Mayor. Código completamente libre, salvo eliminación o edición de esta línea.
using UnityEngine;

/// <summary>
/// This class is wrapper for the Oculus' functions. You can modify this to create other adapters.
/// </summary>
public class OculusAdapter : BaseAdapter {

    public OculusAdapter(GameObject gameObject)
    {   
        if (OVRManager.instance == null)
            gameObject.AddComponent<OVRManager>();
    }

    OVRInput.Controller selectedHand;    //Mano de OVR

    public  virtual Vector3 GetLocalPosition(InputManager.Hands hand)
    {
        selectedHand = (hand == InputManager.Hands.Left) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        return OVRInput.GetLocalControllerPosition(selectedHand);
    }

    public virtual Quaternion GetLocalRotation(InputManager.Hands hand)
    {
        selectedHand = (hand == InputManager.Hands.Left) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        return OVRInput.GetLocalControllerRotation(selectedHand);
    }

    public virtual bool IsPressing(InputManager.Hands hand)
    {
        selectedHand = (hand == InputManager.Hands.Left) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        return (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, selectedHand) > 0.2);
    }
    public virtual Vector3 GetHeadsetPosition(){ return OVRManager.tracker.GetPose().position; }
    public virtual Quaternion GetHeadsetRotation(){ return OVRManager.tracker.GetPose().orientation; }
    public virtual void Update(){ OVRInput.Update(); }
    public virtual void FixedUpdate(){ OVRInput.FixedUpdate(); }
}
