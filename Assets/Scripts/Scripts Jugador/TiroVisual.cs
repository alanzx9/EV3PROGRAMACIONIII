using UnityEngine;
using System.Collections;

public class TiroVisual : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float tiempoDesvanecimiento = 0.2f;

    public void ConfigurarRastro(Vector3 inicio, Vector3 fin)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, inicio);
        lineRenderer.SetPosition(1, fin);
        StartCoroutine(Desvanecer());
    }

    IEnumerator Desvanecer()
    {
        float tiempo = 0;
        Material mat = lineRenderer.material;
        Color colorInicial = mat.color;

        while (tiempo < tiempoDesvanecimiento)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tiempo / tiempoDesvanecimiento);

            mat.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);

            yield return null;
        }

        Destroy(gameObject); // destruye el rastro cuando termina
    }
}
