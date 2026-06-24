using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzParpadeante : MonoBehaviour
{
    [Header("Configuracion del parpadeo")]
    public float intensidadMaxima = 2;
    public float intensidadMinima = 0;
    public float tiempoMinimoPausa = 0.2f;
    public float tiempoMaximoPausa = 0.05f;


    private AudioSource audioSource;
    private Light luz;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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
            float nuevaIntensidad = Random.Range(intensidadMinima, intensidadMaxima);

            float tiempoEspera = Random.Range(tiempoMinimoPausa, tiempoMaximoPausa);

            if(Random.value > 0.8f)
            {
                nuevaIntensidad = intensidadMaxima;
                tiempoEspera = Random.Range(0.5f, 2f);

            }

            luz.intensity = nuevaIntensidad;

            if (audioSource != null)
            {
                audioSource.volume = (nuevaIntensidad / intensidadMaxima) * 0.8f;
            }

            yield return new WaitForSeconds(tiempoEspera);
        }
    }
}
