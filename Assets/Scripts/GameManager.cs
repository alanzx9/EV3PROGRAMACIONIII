using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Pantallas de interfaz (Canvas)")] //paneles de victoria y derrota
    public GameObject pantallaVictoria;
    public GameObject pantallaDerrota;

    private bool juegoTerminado = false; //bool para que no salten los dos paneles a la vez

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f; //el tiempo empieza a correr
        if (pantallaVictoria != null) pantallaVictoria.SetActive(false); // se apagan los paneles victoria/derrota
        if (pantallaDerrota != null) pantallaDerrota.SetActive(false);
    }
    void Update()
    {
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R)) // con r se reinicia el juego si pierdes o ganas
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Victoria()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        Debug.Log("¡Victoria Absoluta!");
        if (pantallaVictoria != null) pantallaVictoria.SetActive(true); //prendemos panel de win y congelamos el juego

        Time.timeScale = 0f;
    }

    public void Derrota()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        Debug.Log("¡Has caído en combate!");
        if (pantallaDerrota != null) pantallaDerrota.SetActive(true); //panel de derrota

        Time.timeScale = 0f;
    }
}
