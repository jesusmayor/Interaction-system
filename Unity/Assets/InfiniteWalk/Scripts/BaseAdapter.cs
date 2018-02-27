
using UnityEngine;

public class BaseAdapter : MonoBehaviour {
    public virtual Vector3 GetLocalPosition() { return Vector3.zero; }
    public virtual Quaternion GetLocalRotation() { return Quaternion.identity; }
    public virtual bool IsPressing() { return false; }
}
