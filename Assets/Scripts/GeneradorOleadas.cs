using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorOleadas : MonoBehaviour
{
    [Header("Configuracion")]
    public GameObject prefabEnemigo;
    public Transform[] puntosDeSpawn;
    public float tiempoEntreSpawns = 2f;
    public float tiempoDescanso = 5;

    [Header("Estado Actual")]
    public int oleadaActual = 1;
    public int enemigosVivos = 0;
    private int enemigosPorGenerar = 0;
    private bool oleadaEnCurso = false;

    private void Start()
    {
        ComenzarOleada(); //iniciamos pesadilla
    }

    void Update()
    {
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

        Debug.Log("Empieza la pesadilla" + oleadaActual + "!");
        StartCoroutine(GenerarEnemigos());
    }

    IEnumerator GenerarEnemigos()
    {
        Debug.Log("Entrando a la corrutina. Enemigos por generar: " + enemigosPorGenerar);

        // Mientras falten enemigos por salir en esta oleada...
        while (enemigosPorGenerar > 0)
        {
            //Debug.Log("Intentando buscar un punto de spawn...");

            // Elegimos un punto al azar
            int indiceAlAzar = Random.Range(0, puntosDeSpawn.Length);
            Transform puntoAleatorio = puntosDeSpawn[indiceAlAzar];

            // Verificamos si por accidente hay un hueco vacío en la lista
            if (puntoAleatorio == null)
            {
                //Debug.LogError("¡ERROR! El punto de spawn número " + indiceAlAzar + " está vacío en el Inspector.");
                yield break; // Detenemos la corrutina para no crashear
            }

            //Debug.Log("Punto encontrado: " + puntoAleatorio.name + ". Creando enemigo...");

            // Creamos al enemigo en ese punto
            GameObject nuevoEnemigo = Instantiate(prefabEnemigo, puntoAleatorio.position, puntoAleatorio.rotation);

            if (nuevoEnemigo != null)
            {
                //Debug.Log("¡Enemigo creado con éxito!");
            }

            enemigosVivos++;
            enemigosPorGenerar--;

            // Esperamos unos segundos antes de sacar al siguiente
            yield return new WaitForSeconds(tiempoEntreSpawns);
        }
    }

    IEnumerator PrepararSiguienteOleada()
    {
        Debug.Log("Oleada superada...");

        yield return new WaitForSeconds(tiempoDescanso);

        oleadaActual++;
        ComenzarOleada();
    }

    public void EnemigoMuerto()
    {
        enemigosVivos--;
    }
}
