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
                adapter = new OculusAdapter();
                break;
        }
    }

    public Vector3 GetHandLocalPosition(Hands hand) { return adapter.GetLocalPosition(); }
    public Quaternion GetHandLocalRotation(Hands hand) { return adapter.GetLocalRotation(); }
    public bool IsHandPressing(Hands hand) { return adapter.IsPressing(); }
}
