
using UnityEngine;

public class BoxActions : MonoBehaviour {

    //Variables publicas
    public Material hoverMat;   //Material que se aplica al pasar por encima.
    public Material basicMat;   //Material que se aplica una vez dejas de pasar por encima.
    public MeshRenderer rend;   //Renderer sobre el cual se aplica el material.
    public Rigidbody hand;      //Mano sobre la que se interactual (Debería ser privada y referenciarse a través del InteractiveItem->Selector

    //Variables privadas
    private FixedJoint joint;   //Referencia al Joint creado
    private Rigidbody rb;       //Referencia al rigidbody del objeto

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Esta función activa la gravedad al objeto seleccionado.
    /// </summary>
    public void DoSelect()
    {
        print("Acción select realizada en " + name);
        rb.useGravity = true;
    }
    /// <summary>
    /// Muestra feedback cambiando el material del objeto
    /// </summary>
    public void DoEnter()
    {
        rend.material = hoverMat;
    }    
    
    /// <summary>
    /// Muestra feedback cambiadl el material del objeto al inical
    /// </summary>
    public void DoExit()
    {
        rend.material = basicMat;
    }

    /// <summary>
    /// Agarramos el objeto con un joint
    /// </summary>
    public void PressDown()
    {
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = hand;
    }

    /// <summary>
    /// Lo soltamos destruyendo el joint y aplicando tanto la velocidad como la velocidad angular al objeto.
    /// </summary>
    public void PressUp()
    {
        DestroyImmediate(joint);
        joint = null;

        rb.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch); //Desligar el que esté metido a fuego el mando derecho
        rb.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
    }
}
