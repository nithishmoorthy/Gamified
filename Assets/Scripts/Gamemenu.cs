using UnityEngine;
using UnityEngine.SceneManagement;
public class Gamemenu: MonoBehaviour
{
    public void Play()
{
    SceneManager.LoadScene("Level1");
}
    public void quit()
    {
        Application.Quit();
    }
}