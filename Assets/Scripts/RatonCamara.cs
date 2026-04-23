using UnityEngine;

public class RatonCamara : MonoBehaviour
{
    // Le ponemos un valor por defecto de 150 para que funcione bien con Time.deltaTime
    public float sensibilidad = 150f; 
    public Transform cuerpoJugador;
    float rotacionX = 0f;

    void Start()
    {
        // Esto bloquea y oculta el mouse real en el centro de tu juego
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f); // Evita que la cámara dé la vuelta completa

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        cuerpoJugador.Rotate(Vector3.up * mouseX);
    }
}