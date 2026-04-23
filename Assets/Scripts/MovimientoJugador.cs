using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public CharacterController controller;
    [Header("Movimiento (Game Fell")]

    public float velocidadMaxima = 6f;

    [Tooltip("0 = Instantaneo (DOOM), 0.5 = Lento")]

    public float tiempoAceleracion = 0.1f;
    public float tiempoFrenado = 0.15f;

    [Header("Fisicas")]
    public float gravedad = -15f;
    public float fuerzaSalto = 2f;

    private Vector3 velocidadMovimientoActual;
    private Vector3 refVelocidad;
    private Vector3 velocidadCaida;

    void Update()
    {
        // 1. Obtener entradas del teclado (WASD / Flechas), usamos raw para evitar resbalos
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // normalizamos para no correr mas rapido en diagonal
        Vector3 direccionDeseada = (transform.right * x + transform.forward * z).normalized;
        Vector3 velocidadObjetivo = direccionDeseada * velocidadMaxima;
        
        //suavizado entre doom y movimiento normal
        float tiempoSuavizado = (direccionDeseada.magnitude > 0f) ? tiempoAceleracion : tiempoFrenado;
        velocidadMovimientoActual = Vector3.SmoothDamp(velocidadMovimientoActual, velocidadObjetivo, ref refVelocidad, tiempoSuavizado);

        //movemos el personaje
        controller.Move(velocidadMovimientoActual * Time.deltaTime);

        //gravedad y salto
        if (controller.isGrounded && velocidadCaida.y < 0f)
        {
            velocidadCaida.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocidadCaida.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
        }

        velocidadCaida.y += gravedad * Time.deltaTime;

        controller.Move(velocidadCaida * Time.deltaTime);
    }
}