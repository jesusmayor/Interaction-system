using UnityEngine;

public class InputManager : MonoBehaviour {
	
    public enum SupportedDevices
    {
        Oculus,
        HTCVive
    };

    public enum Hands
    {
        Left,
        Right
    };

    [SerializeField]
    SupportedDevices device = SupportedDevices.Oculus;

    BaseAdapter adapter;
    static InputManager instance;

    public static InputManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }

    void Start()
    {
        switch (device)
        {
            case SupportedDevices.HTCVive:
                break;
            case SupportedDevices.Oculus:
            default:
                adapter = new OculusAdapter(gameObject);
                break;
        }
    }

    void Update(){ adapter.Update(); }
    void FixedUpdate(){ adapter.FixedUpdate(); }

    public Vector3 GetLocalPosition(Hands hand) { return adapter.GetLocalPosition(hand); }
    public Quaternion GetLocalRotation(Hands hand) { return adapter.GetLocalRotation(hand); }
    public bool IsPressing(Hands hand) { return adapter.IsPressing(hand); }
    public Vector3 GetHeadsetPosition() { return adapter.GetHeadsetPosition(); }
    public Quaternion GetHeadsetRotation() { return adapter.GetHeadsetRotation(); }
}
