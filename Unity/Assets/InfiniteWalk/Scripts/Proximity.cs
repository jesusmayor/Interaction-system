//Código desarrollado por Jesús Mayor. Código completamente libre, salvo eliminación o edición de esta línea.

using UnityEngine;

/// <summary>
/// Interacción definida por proximidad. Perfecta para agarrar objetos con las manos.
/// </summary>
public class Proximity : Selector
{

    private BaseAdapter thisHand;    //Este tipo de interacción necesita usar la mano para detectar si ha sido presionado el gatillo.
    public float radius = 100f;     //Radio de interacción en la mano

    /// <summary>
    /// Evento Start de MonoBehavior. 
    /// Se ejecuta una vez al inicio.
    /// </summary>
    virtual public void Start()
    {   
        thisHand = GetComponent<BaseAdapter>();
    }

    /// <summary>
    /// Sobrescribiendo el update de Selector
    /// Dependiendo de si choca con un interactable, hacemos que el laser se muestre de un color u otro
    /// </summary>
    virtual public void FixedUpdate()
    {
        // Utilizamos una esfera para detectar las colisiones cercanas al objeto.
        // Si impacta con algun objeto que contenga un InteractiveItem, nos quedamos con el.
        // El motor de físicas sólo colisiona contra Colliders, no contra mallas visibles.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        InteractiveItem firstHit = null;

        foreach (Collider collider in hitColliders)
        {
            firstHit = collider.GetComponent<InteractiveItem>();
            if (firstHit != null)
                break;
        }

        //Si hemos encontrado objeto sobre el que interactuar, lo hacemos de una manera lógica.
        if (firstHit != null)
        {
            InteractivityHover(firstHit);
            if (thisHand.IsPressing())
            {
                if (!firstHit.IsPressing)
                    InteractivityPress(firstHit);
            }
            else
            {
                if (firstHit.IsPressing)
                    InteractivityPress();
            }
        }
        else
        {
            InteractivityHover();
        }
    }
}
