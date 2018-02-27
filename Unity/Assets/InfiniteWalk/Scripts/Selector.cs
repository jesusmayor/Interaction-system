//Código desarrollado por Jesús Mayor. Código completamente libre, salvo eliminación o edición de esta línea.

using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour
{

    // Campos públicos
    public InteractiveItem lastInteractable;            // Último objeto interactivo utilizado en la iteración de FixedUpdateAnterior
    private Coroutine selectionHoverCounterCoroutine;   // Referencia a la corrutina que cuenta el tiepo transcurrido pasando por encima.
    private Coroutine selectionPressCounterCoroutine;   // Referencia a la corrutina que cuenta el tiepo transcurrido presionando un botón y pasando por encima.

    /// <summary>
    /// Evento FixedUpdate de MonoBehavior.
    /// Se ejecuta cada frame de físicas. Debería detectar la interacción con el InteractiveItem si pasa por 
    /// en principio solo al tocar (Llamando a InteractivityHover [Sin parámetro cuando no haya interacción])
    /// y si hay algun tipo de acción (Llamando a InteractivityPress [Sin parámetro cuando no haya pressión]).
    /// </summary>
    //virtual public void FixedUpdate() { } //AVISO IMPORTANTE, la logica del selector va aquí o en el Update.

    /// <summary>
    /// Al pasar por encima de un nuebo objeto se avisa utilizando esta función.
    /// </summary>
    /// <param name="interactable">Interactable con el que se interactuó</param>
    virtual protected void InteractivityHover(InteractiveItem interactable)
    {
        // Comprobamos si el objeto con el que hemos colisionado es interactivo en la función que lo llama.
        if (interactable != lastInteractable)
        {
            if (interactable && !interactable.IsHoverDone)
            {
                //Pasamos por encima,entonces realicemos las acciones del HoverDown.
                interactable.onHover.enter.Invoke(); //Ejecutamos la lista de acciones
                interactable.HoverDown(this);
                StartHoverSelection(interactable);
            }

            //Esto solo ocurre la primera vez que cambiamos el foco, por lo que desactivamos el interactable anterior solo una vez.
            DeactivateLastInteractible();
        }
        lastInteractable = interactable;
    }

    /// <summary>
    /// Función que avisa al sistema de que se ha terminado de interactuar (Pasar por encima) en el lastInteractable.
    /// </summary>
    virtual protected void InteractivityHover()
    {
        // Desactivamos el último elemento interactivo en caso de que haya uno.
        if (!lastInteractable)
            return;

        DeactivateLastInteractible();
    }

    /// <summary>
    /// Al pasar por encima y estar presionando un botón se avisa utilizando esta función. (En el caso de que la interacción tenga boton)
    /// </summary>
    /// <param name="interactable">Interactable detectado</param>
    virtual protected void InteractivityPress(InteractiveItem interactable)
    {
        if (!lastInteractable)
            return;

        // Comprobamos si el objeto con el que hemos colisionado es interactivo en la función que lo llama.
        if (interactable && interactable == lastInteractable && interactable.IsHovering && !interactable.IsPressDone && !interactable.IsPressing)
        {
            //Pasamos por encima,entonces realicemos las acciones del HoverDown.
            interactable.PressDown(this);
            interactable.onPress.enter.Invoke(); //Ejecutamos la lista de acciones
            StartPressSelection(interactable);
        }
    }

    /// <summary>
    /// Función que avisa al sistema de que se ha terminado de interactuar (Pasar por encima y presionar un botón) en el lastInteractable.
    /// </summary>
    virtual protected void InteractivityPress()
    {
        // Desactivamos el último elemento interactivo en solo en el caso de que haya uno.
        if (!lastInteractable)
            return;

        lastInteractable.onPress.exit.Invoke(); //Ejecutamos la lista de acciones
        lastInteractable.PressUp();
        StopPressSelection();
    }

    /// <summary>
    /// Función que es llamada por la parte encargada de contar si el tiempo de espera se ha cumplido.
    /// Esta función realiza las acciones finales de selección al pasar por encima.
    /// </summary>
    virtual protected void HoverSelectionComplete()
    {
        lastInteractable.onHover.select.Invoke();  //Ejecutamos la lista de acciones
        lastInteractable.HoverDone();
        DeactivateLastInteractible();
    }

    /// <summary>
    /// Función que es llamada por la parte encargada de contar si el tiempo de espera se ha cumplido.
    /// Esta función realiza las acciones finales de selección al pulsar el botón pasando por encima.
    /// </summary>
    virtual protected void PressSelectionComplete()
    {
        lastInteractable.onPress.select.Invoke();  //Ejecutamos la lista de acciones
        lastInteractable.PressDone();
    }

    /// <summary>
    /// Esta función es llamada cada vez que haya un cambio de interactive item o se haya completado la acción.
    /// Sirve para desactivar el objeto seleccionado.
    /// </summary>
    virtual protected void DeactivateLastInteractible()
    {
        if (lastInteractable == null)
            return;
        
        lastInteractable.onHover.exit.Invoke();  //Ejecutamos la lista de acciones
        StopHoverSelection();
        lastInteractable.HoverUp();
        lastInteractable = null;
    }

    //Sacado del UI selector
    /// <summary>
    /// Inicia el temporizador de selección al pasar por encima. Esta función es llamada al inicio de la interacción.
    /// </summary>
    virtual protected void StartHoverSelection(InteractiveItem interactable)
    {
        StopHoverSelection();
        //Si el tiempo es negativo se entiende que es infinito y no se cuenta.
        if (interactable.hoverActivateTime >= 0)
            selectionHoverCounterCoroutine = StartCoroutine(HoverCount());
    }

    /// <summary>
    /// Detiene el temporizador de selección.
    /// </summary>
    virtual protected void StopHoverSelection()
    {
        if (selectionHoverCounterCoroutine != null)
        {
            //Si el tiempo es negativo se entiende que es infinito y no se cuenta.
            if (lastInteractable.hoverActivateTime >= 0)
            {
                StopCoroutine(selectionHoverCounterCoroutine);
                selectionHoverCounterCoroutine = null;
            }
        }
    }

    /// <summary>
    /// Corrutina que controla el temporizador para comparar que se supera el tiempo marcado (ActivateTime).
    /// Una ver completado el temporizador llama al evento OnSelectionComplete.
    /// </summary>
    virtual protected IEnumerator HoverCount()
    {
        if (lastInteractable == null)
            yield return null;

        while (lastInteractable.HoverAccumulatedTime < lastInteractable.hoverActivateTime)
        {
            lastInteractable.HoverAccumulatedTime += Time.deltaTime; //ESTO NO ES CORRECTO (Aunque da el pego). 
            yield return null;                                       //Habría que evitar usar corutinas y hacerlo todo en Uptade, pues Time.deltaTime es para ello.
        }
        HoverSelectionComplete();
    }

    /// <summary>
    /// Inicia el temporizador de selección al pasar por encima presionando un botón. Esta función es llamada al inicio de la interacción.
    /// </summary>
    virtual protected void StartPressSelection(InteractiveItem interactable)
    {
        StopPressSelection();
        //Si el tiempo es negativo se entiende que es infinito y no se cuenta.
        if (interactable.pressActivateTime >= 0)
            selectionPressCounterCoroutine = StartCoroutine(PressCount());
    }

    /// <summary>
    /// Detiene el temporizador de selección.
    /// </summary>
    virtual protected void StopPressSelection()
    {
        if (selectionPressCounterCoroutine != null)
        {
            //Si el tiempo es negativo se entiende que es infinito y no se cuenta.
            if (lastInteractable.pressActivateTime >= 0)
            {
                StopCoroutine(selectionPressCounterCoroutine);
                selectionPressCounterCoroutine = null;
            }
        }
    }

    /// <summary>
    /// Corrutina que controla el temporizador de presión de una tecla.
    /// Una vez completado el temporizador llama al evento OnSelectionComplete.
    /// </summary>
    virtual protected IEnumerator PressCount()
    {
        if (lastInteractable == null)
            yield return null;

        while (lastInteractable.PressAccumulatedTime < lastInteractable.pressActivateTime)
        {
            lastInteractable.PressAccumulatedTime += Time.deltaTime; //ESTO NO ES CORRECTO (Aunque da el pego). 
            yield return null;                                       //Habría que evitar usar corutinas y hacerlo todo en Uptade, pues Time.deltaTime es para ello.
        }
        PressSelectionComplete();
    }
}
