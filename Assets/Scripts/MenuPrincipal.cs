using UnityEngine;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Paneles de la Interfaz")]
    public GameObject panelMenuPrincipal;

    [Header("Referencias")]
    public CargadorNiveles scriptCargador;

    public void EmpezarJuego()
    {
        // 1. Apagamos el menú principal para que desaparezca por completo
        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(false);
        }

        // 2. Encendemos la pantalla de carga e iniciamos el viaje de escena
        if (scriptCargador != null)
        {
            scriptCargador.CargarNivel(1);
        }
        else
        {
            Debug.LogError("Falta conectar el script CargadorNiveles en el Inspector");
        }
    }

    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}
