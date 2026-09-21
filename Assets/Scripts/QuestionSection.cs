using UnityEngine;

public class QuestionSection : MonoBehaviour
{
    public int questionIndex;
    public int correctAnswer;

    public AudioClip correctSound;
    public AudioClip wrongSound;

    private AudioSource audioSource;
    private string questionText;

    private bool answered = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Setup(int index, string question, int correctAnswerFromFirebase)
    {
        questionIndex = index;
        questionText = question;
        correctAnswer = correctAnswerFromFirebase;

        answered = false;
    }

    public void ShowQuestion()
    {
        QuestionDisplay display = FindAnyObjectByType<QuestionDisplay>();

        if (display != null)
        {
            display.ShowQuestion(questionText);
        }
    }

    public void CheckAnswer(int selectedOption)
    {
        if (answered)
            return;

        answered = true;

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