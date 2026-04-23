using UnityEngine;

public class Arma : MonoBehaviour
{


    //ESTE CODIGO SERA PARA TODAS LAS ARMAS, SE PODRA CAMBIAR EL DAÑO Y CADENCIA DE ARMA
    [Header("Estadísticas del Arma")]
    public float dano = 10f;
    public float rango = 100f;

    [Header("Referencias")]
    public Camera camaraJugador;
    public GameObject prefabRastroBala;
    public Transform puntoDeDisparo;

    void Update()
    {
        // "Fire1" en Unity suele ser el clic izquierdo del ratón
        if (Input.GetButtonDown("Fire1"))
        {
            Disparar();
        }
    }

    void Disparar()
    {
        RaycastHit impacto;

        // Lanzamos el "láser" invisible desde la cámara, hacia adelante
        if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out impacto, rango))
        {
            Debug.Log("¡Pum! Le diste a: " + impacto.transform.name);

            // Creamos una instancia del rastro en el PUNTO DE DISPARO (el cañón)
            GameObject nuevoRastro = Instantiate(prefabRastroBala, puntoDeDisparo.position, Quaternion.identity);

            // Trazamos la línea visual desde el cañón hasta el impacto real
            nuevoRastro.GetComponent<TiroVisual>().ConfigurarRastro(puntoDeDisparo.position, impacto.point);

            // Esta línea roja solo la verás tú en la pestaña "Scene" para comprobar que la cámara apunta bien
            Debug.DrawLine(camaraJugador.transform.position, impacto.point, Color.red, 0.5f);

            Enemigo objetivo = impacto.transform.GetComponent<Enemigo>(); // obtenemos el script Enemigo

            if(objetivo != null) // si tiene el script Enemigo
            {
                objetivo.RecibirDano(dano); // le hacemos dano de nuestra arma
            }
        }
    }
}
