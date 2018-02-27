//Código desarrollado por Jesús Mayor. Código completamente libre, salvo eliminación o edición de esta línea.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase contempla varios tipos de movimiento, tanto movimiento instantaneo como suavizado. 
/// Varios tipos de InteractiveItem pueden contenerla.
/// </summary>
public class Displacement : MonoBehaviour {

    public Transform player;                //Referencia al jugador principal para ser desplazado.
    public bool instant = true;             //¿El movimiento es instantaneo o suavizado?
    public float speedIfNotIntant = 1.4f;   //Si es suavizado, a qué velocidad?
    public Material hoverMat;               //Material al pasar por encima del objeto.
    public Material basicMat;               //Material al dejar de pasar por encima del objeto.
    public MeshRenderer rend;               //Renderer sobre el cual se aplican los materiales.
    private bool isMoving = false;          //Esta variable indica si se está aplicando un desplazamiento suavizado en este momento.
    private Vector3 targetDest;             //Destino que alcanzará el player tras el desplazamiento.
    private InteractiveItem ii;             //Referencia privada al InteractiveItem que contiene esta acción.
    private Laser laser;                    //Laser que ha apuntado a este objeto.

    void Start()
    {
        ii = GetComponent<InteractiveItem>();

    }

    /// <summary>
    /// Movimiento propio del teletransporte.
    /// Este movimiento cogerá el punto en el cual ha chocado el laser de la mano para desplazarse a el.
    /// </summary>
    public void DoActionLaser()
    {
        if (ii != null)
            laser = ii.selector as Laser;
        if (laser != null)
        {
            targetDest = new Vector3(laser.hitPosition.x, player.position.y, laser.hitPosition.z);
            if (instant)
                player.position = targetDest;
            else
                isMoving = true;
        }
    }

    /// <summary>
    /// Este tipo de desplazamiento moverá al usuario al centro del objeto apuntado y no a donde apunte el laser.
    /// </summary>
    public void DoActionPointOfInterest()
    {
        targetDest = new Vector3(transform.position.x, player.position.y, transform.position.z);
        if (instant)
            player.position = targetDest;
        else
            isMoving = true;
    }

    /// <summary>
    /// Cambia de material al pasar por encima para dar feedback
    /// </summary>
    public void DoEnter()
    {
        rend.material = hoverMat;
    }
    
    /// <summary>
    /// Devuelve el material cambiado a su estado natural.
    /// </summary>
    public void DoExit()
    {
        rend.material = basicMat;
    }

    /// <summary>
    /// Este update es usado para realizar los movimientos suavizados a través de una función Lerp.
    /// </summary>
    void Update()
    {
        if (isMoving == true)
        {
            if ((player.position - targetDest).magnitude > 0.3)
                player.position = Vector3.Lerp(player.position, targetDest, speedIfNotIntant * Time.deltaTime);
            else
                isMoving = false;
        }
    }
}
