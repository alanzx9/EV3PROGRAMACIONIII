using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirarCamara : MonoBehaviour
{
    private Transform camaraPrincipal;

    private void Start()
    {
        camaraPrincipal = Camera.main.transform; // se busca la camara del jugador
    }

    private void LateUpdate()
    {
        if (camaraPrincipal != null)
        {
            transform.LookAt(transform.position + camaraPrincipal.forward); // hace que el canvas te "mire"
        }
    }
}
