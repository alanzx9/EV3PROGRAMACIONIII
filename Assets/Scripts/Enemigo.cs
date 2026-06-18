using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemigo : MonoBehaviour
{
    [Header("Estadisticas")]
    public float saludActual = 100;
    private float saludMaxima;

    [Header("Ataque")]
    public float danoAtaque = 15f;
    public float rangoAtaque = 2.5f;
    public float tiempoEntreAtaques = 1.5f;
    private float temporizadorAtaque = 0f;

    [Header("Búsqueda (Patrullaje)")]
    public float radioDeBusqueda = 15f; // Qué tan lejos caminan al azar buscando al jugador
    private bool teVio = false;         // Interruptor para saber si ya te descubrió

    [Header("Visión")]
    public Transform puntoDeVision;
    public float visionRange = 20f;
    public float visionAngle = 60f;
    public Color conoColor = new Color(1, 0, 0, 0.2f);

    [Header("Interfaz")]
    public Image imagenRellenoVida;

    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        saludMaxima = saludActual;
        ActualizarBarra();

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Apenas nace, le decimos que empiece a buscar por la zona
        BuscarNuevoPunto();
    }

    public void RecibirDano(float cantidaDano)
    {
        saludActual -= cantidaDano;
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);
        ActualizarBarra();

        // Si le disparas por la espalda, se da cuenta de dónde estás automáticamente
        teVio = true;

        if (saludActual <= 0)
        {
            Morir();
        }
    }

    void ActualizarBarra()
    {
        if (imagenRellenoVida != null)
        {
            imagenRellenoVida.fillAmount = saludActual / saludMaxima;
        }
    }

    void Morir()
    {
        GeneradorOleadas generador = FindObjectOfType<GeneradorOleadas>();
        if (generador != null) generador.EnemigoMuerto();

        Destroy(gameObject);
    }

    void Update()
    {
        if (player != null && agent != null && puntoDeVision != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Si todavía no te ha visto, usa sus "ojos" para buscarte
            if (!teVio)
            {
                if (distanceToPlayer <= visionRange)
                {
                    Vector3 directionToPlayer = (player.position - puntoDeVision.position).normalized;
                    float angleToPlayer = Vector3.Angle(puntoDeVision.forward, directionToPlayer);

                    if (angleToPlayer <= visionAngle * 0.5f)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(puntoDeVision.position, directionToPlayer, out hit, visionRange))
                        {
                            if (hit.transform.CompareTag("Player"))
                            {
                                // ¡Te descubrió! Activa la persecución
                                teVio = true;
                                Debug.Log("¡Un enemigo te ha visto!");
                            }
                        }
                    }
                }
            }

            if (teVio)
            {
                // Si ya te vio, corre directo hacia ti
                agent.SetDestination(player.position);
            }
            else
            {
                // Si NO te ha visto, camina buscando por el mapa
                // Si ya llegó al punto que estaba revisando...
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    BuscarNuevoPunto(); // elige otro punto al azar para ir a investigar
                }
            }

            if (temporizadorAtaque > 0) temporizadorAtaque -= Time.deltaTime;

            if (distanceToPlayer <= rangoAtaque && teVio)
            {
                if (temporizadorAtaque <= 0)
                {
                    AtacarJugador();
                    temporizadorAtaque = tiempoEntreAtaques;
                }
            }
        }
    }

    void BuscarNuevoPunto()
    {
        // Creamos una esfera imaginaria alrededor del enemigo y elegimos un punto al azar adentro
        Vector3 direccionAleatoria = Random.insideUnitSphere * radioDeBusqueda;
        direccionAleatoria += transform.position;

        NavMeshHit navHit;
        // Le preguntamos a Unity si ese punto al azar cae dentro del suelo azul (NavMesh)
        if (NavMesh.SamplePosition(direccionAleatoria, out navHit, radioDeBusqueda, -1))
        {
            // Le decimos al enemigo que camine hacia ese punto válido
            agent.SetDestination(navHit.position);
        }
    }

    void AtacarJugador()
    {
        VidaJugador scriptVida = player.GetComponent<VidaJugador>();
        if (scriptVida != null)
        {
            scriptVida.RecibirDanoJugador(danoAtaque);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (puntoDeVision == null) return;

        Handles.color = conoColor;
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle * 0.5f, 0) * puntoDeVision.forward;
        Handles.DrawSolidArc(puntoDeVision.position, Vector3.up, leftBoundary, visionAngle, visionRange);

        if (player != null)
        {
            float dist = Vector3.Distance(puntoDeVision.position, player.position);
            if (dist <= visionRange)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(puntoDeVision.position, player.position);
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
#endif
}
