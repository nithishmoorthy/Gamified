using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class QuestionManager : MonoBehaviour
{
    public Transform player;
    public Text scoretext;

    private FirebaseFirestore db;

    private QuestionSection[] questionSections;

    private bool levelCompleted = false;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        // Find all QuestionSection prefabs in the scene
        questionSections =
            FindObjectsByType<QuestionSection>(FindObjectsSortMode.None);

        // Arrange them according to their position on the track
        System.Array.Sort(questionSections, (a, b) =>
            a.transform.position.z.CompareTo(b.transform.position.z));

        Debug.Log("QuestionSections found: " + questionSections.Length);

        LoadQuestions();
    }

    void LoadQuestions()
    {
        for (int i = 0; i < questionSections.Length; i++)
        {
            string documentID = "Q" + (i + 1).ToString("000");

            int sectionIndex = i;

            db.Collection("Questions")
                .Document(documentID)
                .GetSnapshotAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        DocumentSnapshot snapshot = task.Result;

                        if (snapshot.Exists)
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

                            // Check if this is the final question
                            bool isLastQuestion =
                                sectionIndex == questionSections.Length - 1;

                            questionSections[sectionIndex].Setup(
                                fullQuestion,
                                correctAnswer,
                                isLastQuestion
                            );

                            Debug.Log(
                                documentID +
                                " loaded into QuestionSection " +
                                (sectionIndex + 1)
                            );
                        }
                        else
                        {
                            Debug.LogError(
                                "Firebase document not found: " +
                                documentID
                            );
                        }
                    }
                    else
                    {
                        Debug.LogError(
                            "Failed to load " +
                            documentID
                        );
                    }
                });
        }
    }

    // Called by the final QuestionSection
    public void CompleteLevel()
    {
        if (levelCompleted)
            return;

        levelCompleted = true;

        scoretext.text = "Level Complete!";

        Debug.Log("All questions answered! Level Complete!");
    }
}