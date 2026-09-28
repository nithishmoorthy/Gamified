using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public Transform player;
    public Text scoretext;
    public GameObject questionSectionPrefab;

    private FirebaseFirestore db;

    private List<QuestionSection> questionSections =
        new List<QuestionSection>();

    private int nextQuestionNumber = 1;

    private bool levelCompleted = false;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        // Find the QuestionSections manually placed in the scene
        QuestionSection[] existingSections =
            FindObjectsByType<QuestionSection>(
                FindObjectsSortMode.None
            );

        // Sort them by Z position
        System.Array.Sort(
            existingSections,
            (a, b) =>
                a.transform.position.z.CompareTo(
                    b.transform.position.z
                )
        );

        // Store the initial sections
        foreach (QuestionSection section in existingSections)
        {
            questionSections.Add(section);
        }

        Debug.Log(
            "Initial QuestionSections: " +
            questionSections.Count
        );

        // Load the first questions
        foreach (QuestionSection section in questionSections)
        {
            LoadInitialQuestion(section);
        }
    }

    void LoadInitialQuestion(QuestionSection section)
    {
        string documentID =
            "Q" + nextQuestionNumber.ToString("000");

        int currentQuestionNumber = nextQuestionNumber;

        nextQuestionNumber++;

        db.Collection("Questions")
            .Document(documentID)
            .GetSnapshotAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (!task.IsCompletedSuccessfully)
                {
                    Debug.LogError(
                        "Failed to load " + documentID
                    );

                    return;
                }

                DocumentSnapshot snapshot = task.Result;

                // No question exists
                if (!snapshot.Exists)
                {
                    Debug.Log(
                        "No more questions found: " +
                        documentID
                    );

                    Destroy(section.gameObject);

                    questionSections.Remove(section);

                    if (questionSections.Count == 0)
                    {
                        CompleteLevel();
                    }

                    return;
                }

                SetupQuestion(
                    section,
                    snapshot,
                    documentID
                );
            });
    }

    void SetupQuestion(
        QuestionSection section,
        DocumentSnapshot snapshot,
        string documentID
    )
    {
        string question =
            snapshot.GetValue<string>("Question");

        string option1 =
            snapshot.GetValue<string>("Option 1");

        string option2 =
            snapshot.GetValue<string>("Option 2");

        string option3 =
            snapshot.GetValue<string>("Option 3");

        int correctAnswer =
            snapshot.GetValue<int>("correctAnswer");

        string fullQuestion =
            question + "\n" +
            "1. " + option1 + "      " +
            "2. " + option2 + "      " +
            "3. " + option3;

        section.Setup(
            fullQuestion,
            correctAnswer,
            false
        );

        section.SetQuestionManager(this);

        Debug.Log(
            documentID +
            " loaded into " +
            section.gameObject.name
        );
    }

    public void SpawnNextQuestion(
        QuestionSection currentSection
    )
    {
        if (levelCompleted)
            return;

        // Only the last currently spawned section
        // can request a new section.
        if (questionSections.Count == 0)
            return;

        if (currentSection !=
            questionSections[questionSections.Count - 1])
            return;

        string documentID =
            "Q" + nextQuestionNumber.ToString("000");

        // FIRST check Firebase.
        // We do NOT create a GameObject yet.
        db.Collection("Questions")
            .Document(documentID)
            .GetSnapshotAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (!task.IsCompletedSuccessfully)
                {
                    Debug.LogError(
                        "Failed to check " + documentID
                    );

                    return;
                }

                DocumentSnapshot snapshot = task.Result;

                // Question does NOT exist.
                // Therefore do NOT spawn another section.
                if (!snapshot.Exists)
                {
                    Debug.Log(
                        "No more questions. " +
                        "Level complete!"
                    );

                    CompleteLevel();

                    return;
                }

                // Question exists.
                // NOW create the QuestionSection.
                Vector3 spawnPosition =
                    currentSection.transform.position +
                    new Vector3(0f, 0f, 102f);

                GameObject newSectionObject =
                    Instantiate(
                        questionSectionPrefab,
                        spawnPosition,
                        currentSection.transform.rotation
                    );

                QuestionSection newSection =
                    newSectionObject.GetComponent<QuestionSection>();

                questionSections.Add(newSection);

                // Move to the next question number
                nextQuestionNumber++;

                SetupQuestion(
                    newSection,
                    snapshot,
                    documentID
                );

                Debug.Log(
                    documentID +
                    " spawned successfully."
                );
            });
    }

    public void RemoveQuestionSection(
        QuestionSection section
    )
    {
        if (questionSections.Contains(section))
        {
            questionSections.Remove(section);
        }

        Destroy(section.gameObject);
    }

    public void CompleteLevel()
    {
        if (levelCompleted)
            return;

        levelCompleted = true;

        scoretext.text = "Level Complete!";

        Debug.Log(
            "All questions answered! Level Complete!"
        );
    }
}