using UnityEngine;

public class Arma : MonoBehaviour
{
    [Header("Audio del arma")]
    public AudioSource fuenteDeAudio;
    public AudioClip sonidoDisparo;

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
        if(fuenteDeAudio != null && sonidoDisparo != null)
        {
            fuenteDeAudio.pitch = Random.Range(0.9f, 1.1f);
            fuenteDeAudio.PlayOneShot(sonidoDisparo);
        }

        RaycastHit impacto;

        if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out impacto, rango)) //lazer hacia delante
        {
            Debug.Log("¡Pum! Le diste a: " + impacto.transform.name);

            if(prefabRastroBala != null && puntoDeDisparo != null)
            {
                GameObject nuevoRastro = Instantiate(prefabRastroBala, puntoDeDisparo.position, Quaternion.identity);
                TiroVisual tiro = nuevoRastro.GetComponent<TiroVisual>();
                if (tiro != null)
                {
                    tiro.ConfigurarRastro(puntoDeDisparo.position, impacto.point);
                }
            }

            //GameObject nuevoRastro = Instantiate(prefabRastroBala, puntoDeDisparo.position, Quaternion.identity);

            //nuevoRastro.GetComponent<TiroVisual>().ConfigurarRastro(puntoDeDisparo.position, impacto.point); //laser visual

            Debug.DrawLine(camaraJugador.transform.position, impacto.point, Color.red, 0.5f);

            Enemigo objetivo = impacto.transform.GetComponentInParent<Enemigo>(); // obtenemos el script Enemigo

            if (objetivo != null) // si tiene el script Enemigo
            {
                objetivo.RecibirDano(dano); // le hacemos dano de nuestra arma
            }
        }
    }
}
