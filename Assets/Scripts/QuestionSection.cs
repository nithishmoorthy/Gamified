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

        if (selectedOption == correctAnswer)
        {
            Debug.Log("CORRECT ANSWER!");

            if (correctSound != null &&
                audioSource != null)
            {
                audioSource.PlayOneShot(correctSound);
            }
        }
        else
        {
            Debug.Log("WRONG ANSWER!");

            if (wrongSound != null &&
                audioSource != null)
            {
                audioSource.PlayOneShot(wrongSound);
            }
        }

        // Wait before destroying the QuestionSection
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        // Wait 3 seconds so the sound can finish
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