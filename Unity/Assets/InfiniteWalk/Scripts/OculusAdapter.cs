//Código desarrollado por Jesús Mayor. Código completamente libre, salvo eliminación o edición de esta línea.
using UnityEngine;

/// <summary>
/// This class is wrapper for the Oculus' functions. You can modify this to create other adapters.
/// </summary>
public class OculusAdapter : BaseAdapter {

    [SerializeField]
    OVRInput.Controller selectedHand;    //Mano de OVR

    public  virtual Vector3 GetLocalPosition()
    {
        return OVRInput.GetLocalControllerPosition(selectedHand);
    }

    public virtual Quaternion GetLocalRotation()
    {
        return OVRInput.GetLocalControllerRotation(selectedHand);
    }

    public virtual bool IsPressing()
    {
        return (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, selectedHand) > 0.2);
    }
}
