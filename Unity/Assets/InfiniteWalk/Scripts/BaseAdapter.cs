
using UnityEngine;

public class BaseAdapter {
    public virtual Vector3 GetLocalPosition() { return Vector3.zero; }
    public virtual Quaternion GetLocalRotation() { return Quaternion.identity; }
    public virtual bool IsPressing() { return false; }
}
