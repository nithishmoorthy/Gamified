using UnityEngine;

public class QuestionSection : MonoBehaviour
{
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private AudioSource audioSource;

    private string questionText;
    private int correctAnswer;

    private bool answered = false;
    private bool isLastQuestion = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Setup(
        string question,
        int correctAnswerFromFirebase,
        bool lastQuestion
    )
    {
        questionText = question;
        correctAnswer = correctAnswerFromFirebase;
        isLastQuestion = lastQuestion;

        answered = false;

        Debug.Log(
            "Question loaded into " + gameObject.name
        );
    }

    public void ShowQuestion()
    {
        QuestionDisplay display =
            FindAnyObjectByType<QuestionDisplay>();

        if (display != null)
        {
            display.ShowQuestion(questionText);
        }
        else
        {
            Debug.LogError("QuestionDisplay not found!");
        }
    }

    public void CheckAnswer(int selectedOption)
    {
        if (answered)
            return;

        answered = true;

        if (selectedOption == correctAnswer)
        {
            Debug.Log("CORRECT ANSWER!");

            if (correctSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(correctSound);
            }
        }
        else
        {
            Debug.Log("WRONG ANSWER!");

            if (wrongSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(wrongSound);
            }
        }

        // If this is the final question
        if (isLastQuestion)
        {
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        QuestionManager questionManager =
            FindAnyObjectByType<QuestionManager>();

        if (questionManager != null)
        {
            questionManager.CompleteLevel();
        }
        else
        {
            Debug.LogError("QuestionManager not found!");
        }
    }
}