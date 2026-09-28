using UnityEngine;
using UnityEngine.SceneManagement;

public class Playermovment : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardforce = 4000f;
    public float sideforce = 50f;

    // Mobile button states
    private bool mobileForward;
    private bool mobileLeft;
    private bool mobileRight;

    void FixedUpdate()
    {
        // PC controls
        if (Input.GetKey("w") || mobileForward)
        {
            rb.AddForce(0, 0, forwardforce * Time.deltaTime);
        }

        if ((Input.GetKey("d") || mobileRight))
        {
            rb.AddForce(sideforce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if ((Input.GetKey("a") || mobileLeft))
        {
            rb.AddForce(-sideforce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (rb.position.y < 0)
        {
            FindAnyObjectByType<Gamemanagerscript>().GameOver();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    // MOBILE BUTTONS

    public void ForwardDown()
    {
        mobileForward = true;
        Debug.Log("PRESSED");
    }

    public void ForwardUp()
    {
        mobileForward = false;
    }

    public void LeftDown()
    {
        mobileLeft = true;
    }

    public void LeftUp()
    {
        mobileLeft = false;
    }

    public void RightDown()
    {
        mobileRight = true;
    }

    public void RightUp()
    {
        mobileRight = false;
    }
}