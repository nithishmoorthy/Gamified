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

    // True when Firebase tells us there are no more questions
    private bool noMoreQuestions = false;

    // The last QuestionSection that contains the final question
    private QuestionSection finalQuestionSection;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        QuestionSection[] existingSections =
            FindObjectsByType<QuestionSection>(
                FindObjectsSortMode.None
            );

        System.Array.Sort(
            existingSections,
            (a, b) =>
                a.transform.position.z.CompareTo(
                    b.transform.position.z
                )
        );

        foreach (QuestionSection section in existingSections)
        {
            questionSections.Add(section);
        }

        Debug.Log(
            "Initial QuestionSections: " +
            questionSections.Count
        );

        foreach (QuestionSection section in questionSections)
        {
            LoadInitialQuestion(section);
        }
    }

    void LoadInitialQuestion(QuestionSection section)
    {
        string documentID =
            "Q" + nextQuestionNumber.ToString("000");

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

                if (!snapshot.Exists)
                {
                    Debug.Log(
                        "No more questions found: " +
                        documentID
                    );

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

        if (noMoreQuestions)
            return;

        if (questionSections.Count == 0)
            return;

        if (currentSection !=
            questionSections[questionSections.Count - 1])
            return;

        string documentID =
            "Q" + nextQuestionNumber.ToString("000");

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

                // --------------------------------
                // NO MORE QUESTIONS
                // --------------------------------
                if (!snapshot.Exists)
                {
                    Debug.Log(
                        "No more questions. " +
                        "Waiting for the last question to be answered."
                    );

                    noMoreQuestions = true;

                    // THIS is the important part
                    finalQuestionSection = currentSection;

                    Debug.Log(
                        "Final QuestionSection is: " +
                        currentSection.gameObject.name
                    );

                    return;
                }

                // --------------------------------
                // QUESTION EXISTS
                // --------------------------------

                Vector3 spawnPosition =
                    currentSection.transform.position +
                    new Vector3(0f, 0f, 150f);

                GameObject newSectionObject =
                    Instantiate(
                        questionSectionPrefab,
                        spawnPosition,
                        currentSection.transform.rotation
                    );

                QuestionSection newSection =
                    newSectionObject.GetComponent<QuestionSection>();

                questionSections.Add(newSection);

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

        Debug.Log(
            "Removing QuestionSection: " +
            section.gameObject.name
        );

        Destroy(section.gameObject);

        // --------------------------------
        // FINAL QUESTION ANSWERED
        // --------------------------------

        if (noMoreQuestions &&
            section == finalQuestionSection)
        {
            Debug.Log(
                "FINAL QUESTION ANSWERED!"
            );

            CompleteLevel();

            return;
        }
    }

    public void CompleteLevel()
    {
        if (levelCompleted)
            return;

        levelCompleted = true;

        Debug.Log(
            "ALL QUESTIONS ANSWERED! LEVEL COMPLETE!"
        );

        scoretext.text = "Level Complete!";
    }
}