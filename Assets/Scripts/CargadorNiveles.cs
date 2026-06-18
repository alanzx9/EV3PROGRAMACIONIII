using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CargadorNiveles : MonoBehaviour
{
    [Header("Pantalla de Carga")] // visuales de pantalla de carga
    public GameObject panelDeCarga;
    public Slider barraDeProgreso;
    public TextMeshProUGUI textoProgreso;

    [Header("Ajustes de Tiempo")]
    public float tiempoMinimoDeCarga = 2.5f; // segundos obligatorios que durará la pantalla

    void Start()
    {
        if (panelDeCarga != null) panelDeCarga.SetActive(false);
    }

    public void CargarNivel(int indiceDeLaEscena)
    {
        panelDeCarga.SetActive(true);
        StartCoroutine(CargarAsincrono(indiceDeLaEscena));
    }

    IEnumerator CargarAsincrono(int indiceDeLaEscena)
    {
        AsyncOperation operacion = SceneManager.LoadSceneAsync(indiceDeLaEscena); //carga de nivel en segundo plano
        operacion.allowSceneActivation = false; //activacion automatica desactivada de cambio de scena

        float tiempoTranscurrido = 0f;

        while (!operacion.isDone)
        {
            tiempoTranscurrido += Time.deltaTime;

            float progresoReal = Mathf.Clamp01(operacion.progress / 0.9f);
            float progresoFalso = Mathf.Clamp01(tiempoTranscurrido / tiempoMinimoDeCarga);

            float progresoMostrar = Mathf.Min(progresoReal, progresoFalso);

            if (barraDeProgreso != null) barraDeProgreso.value = progresoMostrar;
            if (textoProgreso != null) textoProgreso.text = (progresoMostrar * 100f).ToString("F0") + "%";

            if (operacion.progress >= 0.9f && tiempoTranscurrido >= tiempoMinimoDeCarga)
            {
                // escena liberada
                operacion.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
