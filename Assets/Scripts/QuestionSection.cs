using UnityEngine;
using System.Collections;

public class QuestionSection : MonoBehaviour
{
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private AudioSource audioSource;

    private string questionText;
    private int correctAnswer;

    private bool answered = false;

    private QuestionManager questionManager;

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

        answered = false;

        Debug.Log(
            "Question loaded into " +
            gameObject.name
        );
    }

    public void SetQuestionManager(
        QuestionManager manager
    )
    {
        questionManager = manager;
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
            Debug.LogError(
                "QuestionDisplay not found!"
            );
        }

        // Spawn the next question section
        if (questionManager != null)
        {
            questionManager.SpawnNextQuestion(this);
        }
    }

    public void CheckAnswer(int selectedOption)
    {
        if (answered)
            return;

        answered = true;

        QuestionDisplay display =
            FindAnyObjectByType<QuestionDisplay>();

        if (selectedOption == correctAnswer)
        {
            Debug.Log("CORRECT ANSWER!");

            if (display != null)
            {
                display.ShowQuestion(
                    questionText + "\n\nCORRECT!"
                );
            }

            if (correctSound != null &&
                audioSource != null)
            {
                audioSource.PlayOneShot(correctSound);
            }
        }
        else
        {
            Debug.Log("WRONG ANSWER!");

            if (display != null)
            {
                display.ShowQuestion(
                    questionText + "\n\nWRONG!"
                );
            }

            if (wrongSound != null &&
                audioSource != null)
            {
                audioSource.PlayOneShot(wrongSound);
            }
        }

        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        if (questionManager != null)
        {
            questionManager.RemoveQuestionSection(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}