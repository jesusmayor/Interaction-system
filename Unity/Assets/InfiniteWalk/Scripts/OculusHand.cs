//Código desarrollado por Jesús Mayor. Código completamente libre, salvo eliminación o edición de esta línea.

using UnityEngine;

/// <summary>
/// Clase que hace de Wrapper a las funciones de Oculus, de tal manera que modificandola un poco es fácil hacer que todo el sitema funcione
/// para otra plataforma.
/// </summary>
public class OculusHand : MonoBehaviour {


    public OVRInput.Controller selectedHand;    //Mano de OVR

    public bool IsPressing
    {
        get { return isPressing; }
    }
    bool isPressing = false;                    //Variable que define si estamos pulsando el botón de este mando o no.
	
	// FixedUpdate is called once per frame. Reposicionamos
	void Update () {
		OVRInput.Update ();
		transform.localPosition = OVRInput.GetLocalControllerPosition (selectedHand);
		transform.localRotation = OVRInput.GetLocalControllerRotation (selectedHand);
		if (OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger, selectedHand) > 0.2) {
            if (isPressing == false)
                isPressing = true;
        }
        else if (isPressing == true)
            isPressing = false;
	}
}
