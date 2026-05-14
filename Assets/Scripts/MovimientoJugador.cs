using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public CharacterController controller;
    public Transform camaraJugador; //Aquí arrastraremos tu cámara

    public Transform cameraWeaponTrack;

    [Header("Movimiento (Game Feel)")]
    public float velocidadMaxima = 6f;
    public float tiempoAceleracion = 0.1f;
    public float tiempoFrenado = 0.15f;

    [Header("Fisicas")]
    public float gravedad = -15f;
    public float fuerzaSalto = 2f;

    [Header("Deslizamiento (Slide)")]
    public float impulsoSlide = 15f;
    public float friccionSlide = 15f;
    public float alturaSlide = 1f;
    public float velocidadCamara = 12f; // <-- Qué tan suave baja y sube la cabeza

    [Header("Wall Jump")]

    [Header("Items")]

    public GameObject nearItem;

    public GameObject itemPrefab;
    public Transform itemSlot;
    public GameObject crosshair;

    private float alturaNormal;
    private Vector3 centroNormal;
    private float alturaCamaraNormal;    // Para guardar la altura de los ojos

    private bool deslizando = false;
    private Vector3 direccionSlide;
    private float velocidadSlideActual;

    private Vector3 velocidadMovimientoActual;
    private Vector3 refVelocidad;
    private Vector3 velocidadCaida;

    void Start()
    {
        if (controller != null)
        {
            alturaNormal = controller.height;
            centroNormal = controller.center;
        }

        // Guardamos a qué altura estaban tus ojos al empezar a jugar
        if (camaraJugador != null)
        {
            alturaCamaraNormal = camaraJugador.localPosition.y;
        }
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 direccionDeseada = (transform.right * x + transform.forward * z).normalized;

        if ((Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftControl)))
        {
            if (controller.isGrounded && !deslizando && direccionDeseada.magnitude > 0.1f)
            {
                IniciarSlide(direccionDeseada);
            }
        }

        if (deslizando)
        {
            velocidadMovimientoActual = direccionSlide * velocidadSlideActual;
            velocidadSlideActual -= friccionSlide * Time.deltaTime;

            if (velocidadSlideActual <= velocidadMaxima)
            {
                TerminarSlide();
            }
        }
        else
        {
            Vector3 velocidadObjetivo = direccionDeseada * velocidadMaxima;
            float tiempoSuavizado = (direccionDeseada.magnitude > 0f) ? tiempoAceleracion : tiempoFrenado;
            velocidadMovimientoActual = Vector3.SmoothDamp(velocidadMovimientoActual, velocidadObjetivo, ref refVelocidad, tiempoSuavizado);
        }

        controller.Move(velocidadMovimientoActual * Time.deltaTime);

        // --- ANIMACIÓN SUAVE DE LA CÁMARA ---
        if (camaraJugador != null)
        {
            // Calculamos a qué altura deberían estar los ojos
            float alturaObjetivoCamara = deslizando ? (alturaCamaraNormal - (alturaNormal - alturaSlide)) : alturaCamaraNormal;

            // Movemos la cámara hacia ese objetivo suavemente
            Vector3 posCamara = camaraJugador.localPosition;
            posCamara.y = Mathf.Lerp(posCamara.y, alturaObjetivoCamara, velocidadCamara * Time.deltaTime);
            camaraJugador.localPosition = posCamara;
        }

        if (controller.isGrounded && velocidadCaida.y < 0f)
        {
            velocidadCaida.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocidadCaida.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
            if (deslizando) TerminarSlide();
        }

        ItemLogic();

        velocidadCaida.y += gravedad * Time.deltaTime;
        controller.Move(velocidadCaida * Time.deltaTime);
    }

    void IniciarSlide(Vector3 direccion)
    {
        deslizando = true;
        direccionSlide = direccion;
        velocidadSlideActual = impulsoSlide;

        // FÍSICA INSTANTÁNEA: Nos agachamos de golpe para que la física no se trabe
        controller.height = alturaSlide;
        controller.center = new Vector3(centroNormal.x, alturaSlide / 2f, centroNormal.z);
    }

    void TerminarSlide()
    {
        deslizando = false;
        // FÍSICA INSTANTÁNEA: Nos paramos de golpe (la cámara disimulará esto)
        controller.height = alturaNormal;
        controller.center = centroNormal;
    }

    public void ItemLogic()
    {
        if (nearItem != null && Input.GetKeyDown(KeyCode.E))
        {
            GameObject instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);

            Destroy(nearItem.gameObject);

            instantiatedItem.transform.parent = itemSlot;

            nearItem = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("Hay un item cerca");
            nearItem = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log(" ya no Hay un item cerca");
            nearItem = null;
        }
    }
}