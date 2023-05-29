using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Chambers4-5");
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
