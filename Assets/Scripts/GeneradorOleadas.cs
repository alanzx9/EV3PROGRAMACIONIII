using System.Collections;
using UnityEngine;
using TMPro;

public class GeneradorOleadas : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject[] prefabsEnemigos; // Arreglo con los prefabs de los enemigos (ej. EnemigoPB1 y Perro Robot)
    public Transform[] puntosDeSpawn;
    public float tiempoEntreSpawns = 2f;
    public float tiempoDescanso = 5f;

    [Header("Condición de Victoria")]
    public int oleadaFinal = 3; // "duracion" del nivel

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
        ComenzarOleada(); //empieza la oleada
    }

    void Update()
    {
        if (textoOleadaActual != null)
        {
            // Muestra "OLEADA: 1 / 3"
            textoOleadaActual.text = "OLEADA: " + oleadaActual + " / " + oleadaFinal;
        }

        if (textoEnemigosRestantes != null)
        {
            // Sumamos los enemigos que están caminando + los que aún faltan por nacer
            int totalRestantes = enemigosVivos + enemigosPorGenerar;
            textoEnemigosRestantes.text = "Enemigos restantes: " + totalRestantes;
        }
        // si no quedan monos x salir, cortamos el ciclo y se prepara la siguiente ronda
        if (enemigosPorGenerar == 0 && enemigosVivos == 0 && oleadaEnCurso)
        {
            oleadaEnCurso = false;
            StartCoroutine(PrepararSiguienteOleada());
        }
    }

    void ComenzarOleada()
    {
        oleadaEnCurso = true;
        enemigosPorGenerar = oleadaActual * 5; //cantidas de monos que salen, x5
        StartCoroutine(GenerarEnemigos()); // uno por uno...
    }

    IEnumerator GenerarEnemigos()
    {
        while (enemigosPorGenerar > 0)
        {
            Transform puntoAleatorio = puntosDeSpawn[Random.Range(0, puntosDeSpawn.Length)]; //un punto de spawn al azar

            if (puntoAleatorio == null) yield break;

            // Verificamos que el arreglo tenga enemigos asignados antes de spawnear
            if (prefabsEnemigos != null && prefabsEnemigos.Length > 0)
            {
                // Selecciona un enemigo al azar de la lista
                GameObject enemigoAleatorio = prefabsEnemigos[Random.Range(0, prefabsEnemigos.Length)];

                // Creación del enemigo seleccionado en el punto de spawn
                Instantiate(enemigoAleatorio, puntoAleatorio.position, puntoAleatorio.rotation);
            }
            else
            {
                Debug.LogError("¡Olvidaste asignar los prefabs de los enemigos en el Inspector del GameManager!");
                yield break;
            }

            enemigosVivos++; //actualizacion de contadores
            enemigosPorGenerar--;

            yield return new WaitForSeconds(tiempoEntreSpawns); //esperamos unos segundos para crear el siguiente
        }
    }

    IEnumerator PrepararSiguienteOleada()
    {
        yield return new WaitForSeconds(tiempoDescanso);

        oleadaActual++;

        if (oleadaActual > oleadaFinal) // si pasamos la ronda final, le decimos al gamemanager que se gano el nivel
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Victoria();
            }
        }
        else
        {
            ComenzarOleada(); //si no empieza la siguiente ronda
        }
    }

    public void EnemigoMuerto()
    {
        enemigosVivos--;
    }
}
