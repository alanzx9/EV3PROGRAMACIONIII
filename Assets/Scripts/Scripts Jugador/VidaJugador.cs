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
        vidaActual = vidaMaxima;
        ActualizarBarra();
    }

    public void RecibirDanoJugador(float cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        Debug.Log("Vida del jugador: " + vidaActual);

        ActualizarBarra();

        if (vidaActual <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Derrota();
            }
        }
    }

    void ActualizarBarra()
    {
        if (barraVidaUI != null)
        {
            barraVidaUI.fillAmount = vidaActual / vidaMaxima;
        }
    }
}
