using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor; // Solo necesario para dibujar el cono en el editor
#endif

public class Enemigo : MonoBehaviour
{
    [Header("Estadisticas")]
    public float saludActual = 100;

    [Header("Visión")]
    public Transform puntoDeVision; // <-- ¡NUEVO! Aquí arrastramos el hijo
    public float visionRange = 20f;
    public float visionAngle = 60f; // Ángulo total (ej. 30 grados a cada lado del centro)
    public Color conoColor = new Color(1, 0, 0, 0.2f); // Color rojo transparente para debug

    private NavMeshAgent agent;
    private Transform player;

    public void RecibirDano(float cantidaDano)
    {
        saludActual -= cantidaDano;
        Debug.Log("Enemigo Herido!! salud restante: " + saludActual);

        if (saludActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log("Enemigo Abatido!");
        Destroy(gameObject);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Alerta por si te olvidas de asignar el objeto
        if (puntoDeVision == null)
        {
            Debug.LogError("¡OJO! No asignaste el 'PuntoDeVision' en el Inspector del enemigo: " + gameObject.name);
        }
    }

    void Update()
    {
        // 1. Verificamos que todo exista y que tengamos "ojos" (puntoDeVision)
        if (player != null && agent != null && puntoDeVision != null)
        {
            float distanceToPlayer = Vector3.Distance(puntoDeVision.position, player.position);

            // 2. ¿Está dentro de la distancia máxima de visión?
            if (distanceToPlayer <= visionRange)
            {
                // Dirección desde los "ojos" hacia el jugador
                Vector3 directionToPlayer = (player.position - puntoDeVision.position).normalized;

                // 3. ¿El jugador está dentro del cono frontal (ángulo)?
                // Comparamos el frente de los OJOS (puntoDeVision.forward) con la dirección al jugador
                float angleToPlayer = Vector3.Angle(puntoDeVision.forward, directionToPlayer);

                // Dividimos visionAngle por 2 porque Angle() mide el desvío desde el centro
                if (angleToPlayer <= visionAngle * 0.5f)
                {
                    RaycastHit hit;

                    // 4. Disparamos el láser desde los OJOS (puntoDeVision.position)
                    if (Physics.Raycast(puntoDeVision.position, directionToPlayer, out hit, visionRange))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            agent.SetDestination(player.position);
                        }
                    }
                }
            }
        }
    }

    // --- ESTO DIBUJA EL CONO EN EL EDITOR ---
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (puntoDeVision == null) return;

        // Dibujamos el cono visual
        Handles.color = conoColor;

        // Calculamos la dirección de inicio del cono (rotada hacia la izquierda)
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle * 0.5f, 0) * puntoDeVision.forward;

        // Dibujamos el arco del cono
        Handles.DrawSolidArc(puntoDeVision.position, Vector3.up, leftBoundary, visionAngle, visionRange);

        // Dibujamos el rayo hacia el jugador (si existe y está en rango) solo para testear
        if (player != null)
        {
            float dist = Vector3.Distance(puntoDeVision.position, player.position);
            if (dist <= visionRange)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(puntoDeVision.position, player.position);
            }
        }
    }
#endif
}
