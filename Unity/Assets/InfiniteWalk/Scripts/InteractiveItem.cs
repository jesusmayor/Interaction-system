//Código desarrollado por Jesús Mayor. Código completamente libre, salvo eliminación o edición de esta línea.

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Clase que define un objeto con el cual se puede interactuar.
/// Si se quere añadir funcionalidad, se podría heredar de este objeto con la finalidad de añadirla o modificarla.
/// </summary>
public class InteractiveItem : MonoBehaviour {

    [SerializeField]
    private bool isHoverAccumulative = false;   //Define si el objeto acumula el tiempo de carga (al pasar por encima) o si vuelve a cero tras percer el foco.
    [SerializeField]
    private bool isPressAccumulative = false;   //Define si el objeto acumula el tiempo de carga (al presionar bontón) o si vuelve a cero tras percer el foco.
    public float hoverActivateTime = 2f;        //Tiempo de activación al pasar por encima del objeto.
    public float pressActivateTime = 2f;        //Tiempo de presionado de un botón para accionar el objeto.
    [SerializeField]
    private bool isHoverRepeatable = false;     //Indica si la acción final es repetible solo pasando por encima.
    [SerializeField]
    private bool isPressRepeatable = false;     //Indica si la acción final es repetible presionando un botón.

    struct InteractionMarks                     //Parcas que indican diferentes estados y tiempo acumulado para press y hover.
    {                                           //Al no ser expuestas en el editor, se han agrupado en una estructura para dar legibilidad.
        public float accumulatedTime;           //Tiempo acumulado en esta acción
        public bool isPerforming;               //¿Se está realizando esta acción?
        public bool isDone;                     //¿Se ha realizado la acción? Si isDone = true, no podremos realizarla de nuevo y se dejará de contar su tiempo.
    }

    InteractionMarks hover;                     //Marcas de hover.
    InteractionMarks press;                     //Marcas de press.

    [System.Serializable]
    public struct InteractionMoments            //Esta estructura define tres tipos de listas de acciones (Funciones que se ejecutarán)
    {
        public UnityEvent select;               //Al finalizar la cuenta del tiempo de espera.
        public UnityEvent enter;                //Al comenzar la interacción.
        public UnityEvent exit;                 //Al al finalizar la interacción.
    }

    public InteractionMoments onHover;          //Acciones cuando pasamos por encima del objeto.
    public InteractionMoments onPress;          //Acciones cuando presionamos el botón indicado.

    [HideInInspector]
    public Selector selector;                   //Referencia al Selector que está interactuando con este objeto.

    //Getters y setters para las variables privadas de este objeto. (Controll de acceso a ellas)
    public float HoverAccumulatedTime
    {
        get { return hover.accumulatedTime; }
        set { hover.accumulatedTime = value; }
    }
    public float PressAccumulatedTime
    {
        get { return press.accumulatedTime; }
        set { press.accumulatedTime = value; }
    }
    public bool IsHoverDone
    {
        get { return hover.isDone; }
    }
    public bool IsPressDone
    {
        get { return press.isDone; }
    }
    public bool IsHovering
    {
        get { return hover.isPerforming; }
    }
    public bool IsPressing
    {
        get { return press.isPerforming; }
    }

    /// <summary>
    /// Inicializando las variables de las estructuras usadas, ya que no quiero hacer un constructor.
    /// </summary>
    void Start()
    {
        press.accumulatedTime = 0f;
        press.isPerforming = false;
        press.isDone = false;
        hover.accumulatedTime = 0f;
        hover.isPerforming = false;
        hover.isDone = false;
    }

    /// <summary>
    /// El selector llama a esta función al pasar por encima sin hacer click
    /// </summary>
    public void HoverDown(Selector selector) {
		hover.isPerforming = true;
        this.selector = selector;
	}


    /// <summary>
    /// El selector llama a esta función al dejar de pasar por encima sin hacer click
    /// </summary>
    public void HoverUp()
    {
        if (!isHoverAccumulative)
            hover.accumulatedTime = 0f;
        hover.isPerforming = false;
        this.selector = null;
    }

    /// <summary>
    /// Se llama una vez realizada las acciones de seleccion al pasar por encima
    /// </summary>
    public void HoverDone()
    {
        if (!isHoverRepeatable)
            hover.isDone = true;
        hover.accumulatedTime = 0f;
    }

    /// <summary>
    /// El selector llama a esta función al pasar por encima y hacer click
    /// </summary>
    public void PressDown(Selector selector)
    {
        press.isPerforming = true;
        this.selector = selector;
    }


    /// <summary>
    /// El selector llama a esta función al dejar de pasar por encima y hacer click
    /// </summary>
    public void PressUp()
    {
        if (!isPressAccumulative)
            press.accumulatedTime = 0f;
        press.isPerforming = false;
    }

    /// <summary>
    /// Se llama una vez realizada las acciones de seleccion al pasar por encima y hacer click
    /// </summary>
    public void PressDone()
    {
        if (!isHoverRepeatable)
            hover.isDone = true;
        hover.accumulatedTime = 0f;
    }
}
