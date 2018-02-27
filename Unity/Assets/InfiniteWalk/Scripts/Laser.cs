//Código desarrollado por Jesús Mayor. Código completamente libre, salvo eliminación o edición de esta línea.

using UnityEngine;
using System.Collections;

public class Laser : Selector
{


    // Interacción
    public UISelection uiSelection;             // Objeto de tipo UISelection.cs - Nos permite gestionar interacción

    public Transform reticle;                   // Posición del objeto retícula
    public float defaultDistance = 10f;         // Distancia por defecto de la retícula
    public bool useNormal = false;              // Indica si rotamos la retícula usando la normal de la superficie donde se proyecta

    [HideInInspector]
    public Vector3 hitPosition;                 // Posición en la que ha impactado el Laser

    // Campos privados
    // Retícula
    Vector3 originalScale;                      // Escala inicial de la retícula
    Quaternion originalRotation;				// Orientación inicial de la retícula
    InteractiveItem interactible;               // Objeto interactivo que estamos utilizando
    private LineRenderer laser;                 // Referencia al laser creado para destruirlo después.
    private OculusHand thisHand;                // Referencia a la mano para detectar si estamos pulsando un botón o no.

    /// <summary>
    /// Evento Start de MonoBehavior. 
    /// Se ejecuta una vez al inicio.
    /// </summary>
    virtual public void Start()
    {
        if (reticle == null)
            return;

        // El componente Selection.cs también está pensado para estar asociado a la cámara.
        uiSelection = GetComponent<UISelection>();

        // Almacenamos el tamaño y la orientación original de la retícula.
        originalScale = reticle.localScale;
        originalRotation = reticle.localRotation;
        
        thisHand = GetComponent<OculusHand>();
    }

    /// <summary>
    /// Ejecuta la interacción cuando el rayo no ha colisionado con nada.
    /// </summary>
    override protected void InteractivityHover()
    {
        // Ocultamos la selección si está visible
        if (uiSelection && uiSelection.IsVisible)
            uiSelection.Hide();

        base.InteractivityHover();
    }

    /// <summary>
    /// Ejecuta la interación cuando el rayo ha colisionado con un objeto. (No muestra laser al pasar por encima)
    /// Se sobreescribe porque queremos mostrar un UI en la posición de hit.
    /// </summary>
    /// <param name="interactable">Colisión del rayo</param>
    override protected void InteractivityHover(InteractiveItem interactable)
    {

        // Comprobamos si el objeto con el que hemos colisionado es interactivo en la función que lo llama.
        if (interactable != lastInteractable)
        {
            if (interactable && !interactable.IsHoverDone)
            {
                //Pasamos por encima,entonces realicemos las acciones del HoverDown.
                interactable.onHover.enter.Invoke();
                interactable.HoverDown(this);
                StartHoverSelection(interactable);

                if (uiSelection)
                    uiSelection.Show();
            }

            DeactivateLastInteractible();
        }
        if (!interactable)
        {
            if (uiSelection && uiSelection.IsVisible)
                uiSelection.Hide();
        }
        lastInteractable = interactable;
    }

    /// <summary>
    /// Posiciona la retícula cuando el rayo no ha colisionado con nada. (UISelect)
    /// </summary>
    protected void SetPosition()
    {
        reticle.position = transform.position + transform.forward * defaultDistance;
        reticle.localScale = originalScale * defaultDistance;
        reticle.LookAt(transform);
    }

    /// <summary>
    /// Posiciona la retícula cuando el rayo ha colisionado con un objeto.
    /// </summary>
    /// <param name="hit">Colisión del rayo</param>
    protected void SetPosition(RaycastHit hit)
    {
        reticle.position = hit.point;
        reticle.localScale = originalScale * hit.distance;
        if (useNormal)
        {
            reticle.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        }
        else
        {
            reticle.LookAt(transform);
        }
    }

    /// <summary>
    /// Corrutina que controla el temporizador y nos muestra la animación
    /// de llenado de la imagen de selección.
    /// Una ver completado el temporizador llama al evento OnSelectionComplete.
    /// </summary>
    override protected IEnumerator HoverCount()
    {
        if (lastInteractable == null)
            yield return null;

        uiSelection.selection.fillAmount = 0f;

        while (lastInteractable.HoverAccumulatedTime < lastInteractable.hoverActivateTime)
        {
            uiSelection.selection.fillAmount = lastInteractable.HoverAccumulatedTime / lastInteractable.hoverActivateTime;
            lastInteractable.HoverAccumulatedTime += Time.deltaTime;
            yield return null;
        }

        uiSelection.selection.fillAmount = 1f;
        HoverSelectionComplete();
    }

    /// <summary>
    /// Sobrescribiendo el update de Selector
    /// Dependiendo de si choca con un interactable, hacemos que el laser se muestre de un color u otro
    /// </summary>
    virtual public void FixedUpdate()
    {
        if (reticle == null)
            return;

        // Utilizamos Raycast para lanzar un rayo desde el objeto hacia adelante (transform.forward)
        // y comprobamos si ese rayo impacta con algún elemento para saber dónde tenemos
        // que situar la retícula.
        // El motor de físicas sólo colisiona contra Colliders, no contra mallas visibles.
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (thisHand.IsPressing)
        {
            if (!laser)     //Si se está pulsando botón y no existe un laser anterior, creamos uno nuevo.
            {
                laser = gameObject.AddComponent<LineRenderer>();
                laser.positionCount = 2;
                laser.widthMultiplier = 0.01f;
                laser.material = new Material(Shader.Find("Particles/Additive"));
            }
        }
        else if (laser)     //Si no se está pulsando, destruimos el laser.
        {
            Destroy(laser);
            laser = null;
        }

        if (laser)  //En función de dónde impacte el laser, se pinta en rojo o en verde.
        {
            laser.SetPosition(0, transform.position);
            if (Physics.Raycast(ray, out hit) && thisHand.IsPressing && hit.collider.gameObject.layer == LayerMask.NameToLayer("Movement")) //Draw bright. 
            {
                hitPosition = hit.point;

                laser.SetPosition(1, hit.point);
                laser.startColor = Color.green;
                laser.endColor = Color.green;
                InteractiveItem ii = hit.collider.GetComponent<InteractiveItem>();
                InteractivityHover(ii);
                InteractivityPress(ii);
                SetPosition(hit);
            }
            else //Draw dark laser.
            {
                laser.SetPosition(1, transform.forward* 100f);
                laser.startColor = Color.red;
                laser.startColor = Color.red;
                InteractivityPress();
                InteractivityHover();
                SetPosition();
            }
        }
        else
        {
            InteractivityPress();
            InteractivityHover();
            SetPosition();
        }

    }
}
