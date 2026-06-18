using UnityEngine;

public class Arma : MonoBehaviour
{
    [Header("Estadísticas del Arma")]
    public float dano = 10f;
    public float rango = 100f;

    [Header("Referencias")]
    public Camera camaraJugador;
    public GameObject prefabRastroBala;
    public Transform puntoDeDisparo;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) //click izquierdo para disparar
        {
            Disparar();
        }
    }

    void Disparar()
    {
        RaycastHit impacto;

        if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out impacto, rango)) //lazer hacia delante
        {
            Debug.Log("¡Pum! Le diste a: " + impacto.transform.name);

            GameObject nuevoRastro = Instantiate(prefabRastroBala, puntoDeDisparo.position, Quaternion.identity);

            nuevoRastro.GetComponent<TiroVisual>().ConfigurarRastro(puntoDeDisparo.position, impacto.point); //laser visual

            Debug.DrawLine(camaraJugador.transform.position, impacto.point, Color.red, 0.5f);

            Enemigo objetivo = impacto.transform.GetComponentInParent<Enemigo>(); // obtenemos el script Enemigo

            if (objetivo != null) // si tiene el script Enemigo
            {
                objetivo.RecibirDano(dano); // le hacemos dano de nuestra arma
            }
        }
    }
}
