using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzParpadeante : MonoBehaviour
{
    private Light luz;

    [Header("Configuracion del parpadeo")]
    public float intensidadMaxima = 2;
    public float intensidadMinima = 0;
    public float tiempoMinimoPausa = 0.2f;
    public float tiempoMaximoPausa = 0.05f;

    void Start()
    {
        luz = GetComponent<Light>();

        if(luz!= null)
        {
            StartCoroutine(EfectoParpadeo());
        }
    }

    IEnumerator EfectoParpadeo()
    {
        while(true)
        {
            luz.intensity = Random.Range(intensidadMinima, intensidadMaxima);

            float tiempoEspera = Random.Range(tiempoMinimoPausa, tiempoMaximoPausa);

            if(Random.value > 0.8f)
            {
                luz.intensity = intensidadMaxima;
                tiempoEspera = Random.Range(0.5f, 2f);

            }
            yield return new WaitForSeconds(tiempoEspera);
        }
    }
}
