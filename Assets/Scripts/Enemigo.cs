using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{

    // ESTE CODIGO SE USARA PARA LA VIDA DE ENEMIGO Y DAÑO QUE CAUSARA AL JUGADOR
    [Header("Estadisticas")]
    public float saludActual = 100;

    public void RecibirDano(float cantidaDano) //funcion que llamamos desde el script Arma
    {
        saludActual -= cantidaDano; // se resta la salud
        Debug.Log("Enemigo Herido!! salud restante: " + saludActual);

        if (saludActual <=0) //comprobamos si su vida llego a 0
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log("Enemigo Abatido!");

        Destroy(gameObject); //se destruye el objeto
    }
}
