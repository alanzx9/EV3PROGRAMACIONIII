using System.Collections;
using UnityEngine;
using TMPro;

public class GeneradorOleadas : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject prefabEnemigo;
    public Transform[] puntosDeSpawn;
    public float tiempoEntreSpawns = 2f;
    public float tiempoDescanso = 5f;

    [Header("Condición de Victoria")]
    public int oleadaFinal = 3;

    [Header("Interfaz UI (Textos)")]
    public TextMeshProUGUI textoOleadaActual;
    public TextMeshProUGUI textoEnemigosRestantes;

    [Header("Estado Actual (No tocar)")]
    public int oleadaActual = 1;
    public int enemigosVivos = 0;
    private int enemigosPorGenerar = 0;
    private bool oleadaEnCurso = false;

    void Start()
    {
        ComenzarOleada();
    }

    void Update()
    {
        if (textoOleadaActual != null)
        {
            textoOleadaActual.text = "OLEADA: " + oleadaActual + " / " + oleadaFinal;
        }

        if (textoEnemigosRestantes != null)
        {
            int totalRestantes = enemigosVivos + enemigosPorGenerar;
            textoEnemigosRestantes.text = "Enemigos restantes: " + totalRestantes;
        }

        if (enemigosPorGenerar == 0 && enemigosVivos == 0 && oleadaEnCurso)
        {
            oleadaEnCurso = false;
            StartCoroutine(PrepararSiguienteOleada());
        }
    }

    void ComenzarOleada()
    {
        oleadaEnCurso = true;
        enemigosPorGenerar = oleadaActual * 5;
        StartCoroutine(GenerarEnemigos());
    }

    IEnumerator GenerarEnemigos()
    {
        while (enemigosPorGenerar > 0)
        {
            Transform puntoAleatorio = puntosDeSpawn[Random.Range(0, puntosDeSpawn.Length)];

            if (puntoAleatorio == null) yield break;

            Instantiate(prefabEnemigo, puntoAleatorio.position, puntoAleatorio.rotation);

            enemigosVivos++;
            enemigosPorGenerar--;

            yield return new WaitForSeconds(tiempoEntreSpawns);
        }
    }

    IEnumerator PrepararSiguienteOleada()
    {
        yield return new WaitForSeconds(tiempoDescanso);

        oleadaActual++;

        if (oleadaActual > oleadaFinal)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Victoria();
            }
        }
        else
        {
            ComenzarOleada();
        }
    }

    public void EnemigoMuerto()
    {
        enemigosVivos--;
    }
}
