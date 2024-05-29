using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pantallaInicio : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("jogo");
    }

    public void Inicio()
    {
        SceneManager.LoadScene("Pantalla_inicio");
    }

}
