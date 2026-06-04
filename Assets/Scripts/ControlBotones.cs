using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlBotones : MonoBehaviour
{
    public void JugarDeNuevo() //esto concecta al boton volver a intentar
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void SiguienteNivel()
    {
        SceneManager.LoadScene("Nivel2");
    }

    public void IrAlMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}
