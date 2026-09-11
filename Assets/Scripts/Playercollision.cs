using UnityEngine;
public class Playercollision : MonoBehaviour
{
    public Gamemanagerscript gamemanager;
    public Playermovment movement;
    public AudioSource correct;
    public AudioSource wrong;
    void OnCollisionEnter(Collision collisioninfo)
    {
        if (collisioninfo.collider.tag == "Obstacle")
        {
            movement.enabled = false;
            gamemanager.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CorrectAnswer"))
        {
            correct.Play();
            Debug.Log("Correct");
        }

        if (other.CompareTag("WrongAnswer"))
        {
            wrong.Play();
            Debug.Log("Wrong");
        }
    }

}
