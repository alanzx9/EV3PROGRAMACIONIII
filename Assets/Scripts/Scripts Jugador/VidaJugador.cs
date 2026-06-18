using UnityEngine;
using UnityEngine.UI;

public class VidaJugador : MonoBehaviour
{
    [Header("Estadísticas")]
    public float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Interfaz UI")]
    public Image barraVidaUI;

    void Start()
    {
        vidaActual = vidaMaxima; //vida llena al inicio del nivel
        ActualizarBarra(); // llena visualmente
    }

    public void RecibirDanoJugador(float cantidad)
    {
        vidaActual -= cantidad; //restamos el dano que nos hicieron
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        Debug.Log("Vida del jugador: " + vidaActual);
        //actualizamos el canvas
        ActualizarBarra();

        if (vidaActual <= 0) //si morimos...
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Derrota(); //aparece panel de derrota y congelamos el juego
            }
        }
    }

    // --- FUNCIÓN QUE MUEVE LA BARRA ---
    void ActualizarBarra()
    {
        if (barraVidaUI != null)
        {
            barraVidaUI.fillAmount = vidaActual / vidaMaxima;
        }
    }
}
