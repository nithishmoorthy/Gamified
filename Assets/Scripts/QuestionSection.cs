using UnityEngine;

public class QuestionSection : MonoBehaviour
{
    public int questionIndex;
    public int correctAnswer;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Setup(int index, int correctAnswerFromFirebase)
    {
        questionIndex = index;
        correctAnswer = correctAnswerFromFirebase;
    }

    public void CheckAnswer(int selectedOption)
    {
        if (selectedOption == correctAnswer)
        {
            Debug.Log("Q" + (questionIndex + 1) + " - CORRECT ANSWER!");

            if (correctSound != null)
            {
                audioSource.PlayOneShot(correctSound);
            }
        }
        else
        {
            Debug.Log("Q" + (questionIndex + 1) + " - WRONG ANSWER!");

            if (wrongSound != null)
            {
                audioSource.PlayOneShot(wrongSound);
            }
        }
    }
}