using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoNubes : MonoBehaviour
{
    public float velocidadMovimiento = 1.0f; // Velocidad a la que se moverán las nubes
    public float inicioDeGeneracion = 10f; // Posición en la que comenzarán a generarse las nubes
    public float longitudPantalla = 20f; // Longitud de la pantalla en unidades del juego
    public float velocidadBorrado = 1.0f; // Velocidad de borrado de las nubes
    public bool borrarCachoPorCacho = true; // Indica si se debe borrar cacho por cacho

    private float tiempoInicio; // Tiempo en el que se instanció el objeto
    private float distanciaRecorrida = 0f; // Distancia recorrida desde la creación
    private SpriteRenderer spriteRenderer; // Componente de renderizado de las nubes

    void Start()
    {
        // Guarda el tiempo de inicio
        tiempoInicio = Time.time;

        // Obtiene el componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Calcula la distancia recorrida desde el inicio
        distanciaRecorrida = velocidadMovimiento * (Time.time - tiempoInicio);

        // Mueve las nubes hacia la izquierda
        transform.Translate(Vector3.left * velocidadMovimiento * Time.deltaTime);

        // Si las nubes salen completamente de la pantalla por la izquierda
        if (transform.position.x < -longitudPantalla)
        {
            // Destruye gradualmente las nubes
            if (borrarCachoPorCacho)
            {
                BorrarCachoPorCacho();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // Método para borrar cacho por cacho las nubes
    void BorrarCachoPorCacho()
    {
        // Calcula la cantidad de transparencia que se debe aplicar
        float transparencia = velocidadBorrado * Time.deltaTime;

        // Reduce la opacidad del sprite gradualmente
        Color colorActual = spriteRenderer.color;
        colorActual.a -= transparencia;
        spriteRenderer.color = colorActual;

        // Si la opacidad es menor o igual a cero, destruye el objeto
        if (colorActual.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
