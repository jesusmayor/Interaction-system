
using UnityEngine;

public class BaseAdapter {
    public virtual Vector3 GetLocalPosition(InputManager.Hands hand) { return Vector3.zero; }
    public virtual Quaternion GetLocalRotation(InputManager.Hands hand) { return Quaternion.identity; }
    public virtual bool IsPressing(InputManager.Hands hand) { return false; }
    public virtual Vector3 GetHeadsetPosition() { return Vector3.zero; }
    public virtual Quaternion GetHeadsetRotation() { return Quaternion.identity; }

    public virtual void Update() {}
    public virtual void FixedUpdate() {}
}
