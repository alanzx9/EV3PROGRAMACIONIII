using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Pantallas de interfaz (Canvas)")]
    public GameObject pantallaVictoria;
    public GameObject pantallaDerrota;

    private bool juegoTerminado = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (pantallaVictoria != null) pantallaVictoria.SetActive(false);
        if (pantallaDerrota != null) pantallaDerrota.SetActive(false);
    }
    void Update()
    {
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Victoria()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        Debug.Log("¡Victoria Absoluta!");
        if (pantallaVictoria != null) pantallaVictoria.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Derrota()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        Debug.Log("¡Has caído en combate!");
        if (pantallaDerrota != null) pantallaDerrota.SetActive(true);

        Time.timeScale = 0f;
    }
}
