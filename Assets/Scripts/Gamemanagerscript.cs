using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanagerscript : MonoBehaviour
{
    bool gameover = false;

    public void GameOver()
    {
        if (gameover == false)
        {
            gameover = true;
            Debug.Log("GAME OVER");
            Invoke("Restart", 0.5f);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
