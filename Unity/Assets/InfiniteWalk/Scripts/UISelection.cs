//Código desarrollado por Jesús Mayor. Código completamente libre, salvo eliminación o edición de esta línea.

using UnityEngine;
using UnityEngine.UI;

public class UISelection : MonoBehaviour {

    // Campos públicos
    public Image selection;					// Componente imagen sobre el que ejecutaremos la selección radial  
    
	// Campos privados
	private bool isVisible = false;         // ¿Se está mostrando la retícula ahora mismo?
    Selector selector;                      // Referencia a su padre
    // Getters y Setters que exponen algunas variables privadas.
    public bool IsVisible {
		get { return isVisible; }
	}


    /// <summary>
    /// Evento Start de MonoBehaviour
    /// </summary>
    void Start () {
		Hide ();
        selector = GetComponent<Selector>();
	}

	/// <summary>
	/// Muestra la imagen de selección e inicia el temporizador.
	/// Esta función es llamada siempre desde Reticle.cs o Laser.cs
	/// </summary>
	public void Show() {
		if (selection != null)
			selection.gameObject.SetActive (true);
        
        isVisible = true;
	}

    /// <summary>
    /// Oculta la imagen de selección y detiene el temporizador.
    /// Esta función es llamada siempre desde Reticle.cs o Laser.cs
    /// </summary>
    public void Hide() {
		if (selection != null)
			selection.gameObject.SetActive (false);

		isVisible = false;
	}
}