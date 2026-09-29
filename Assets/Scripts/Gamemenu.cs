using UnityEngine;
using UnityEngine.SceneManagement;
public class Gamemenu: MonoBehaviour
{
    public GameObject FirstScreen;
    public GameObject JEEScreen;
    public GameObject NEETScreen;
    public GameObject UnderConstructionScreen;
    public void Play()
{
    SceneManager.LoadScene("Level1");
}
    public void quit()
    {
        Application.Quit();
    }

    public void GetJEEScreen()
    {
        JEEScreen.SetActive(true);
        FirstScreen.SetActive(false);
        
    }

    public void GetNEETScreen()
    {
        NEETScreen.SetActive(true);
        FirstScreen.SetActive(false);
    }

    public void CloseJEEScreen()
    {
        FirstScreen.SetActive(true);
        JEEScreen.SetActive(false);
    }

    public void CloseNEETScreen()
    {
        FirstScreen.SetActive(true);
        NEETScreen.SetActive(false);
    }

    public void GetUnderConstructionScreen()
    {
        UnderConstructionScreen.SetActive(true);
    }

    public void CloseUnderConstructionScreen()
    {
        UnderConstructionScreen.SetActive(false);
    }
}