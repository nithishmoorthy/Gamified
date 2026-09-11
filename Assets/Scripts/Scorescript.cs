using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class Scorescript : MonoBehaviour
{
    public Transform player;
    public Text scoretext;

    private FirebaseFirestore db;
    private string[] questions = new string[5];
    private int currentQuestion = -1;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        LoadQuestions();
    }

    void LoadQuestions()
    {
        for (int i = 0; i < 5; i++)
        {
            string id = "Q00" + (i + 1);
            int index = i;

            db.Collection("Questions").Document(id).GetSnapshotAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        DocumentSnapshot snapshot = task.Result;

                        if (snapshot.Exists)
                        {
                            string question = snapshot.GetValue<string>("Question");
                            string option1 = snapshot.GetValue<string>("Option 1");
                            string option2 = snapshot.GetValue<string>("Option 2");
                            string option3 = snapshot.GetValue<string>("Option 3");

                            questions[index] =
                                question + "\n" +
                                "1. " + option1 + "      " +
                                "2. " + option2 + "      " +
                                "3. " + option3;

                            Debug.Log(id + " loaded!");
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to load " + id);
                    }
                });
        }
    }

    void Update()
    {
        if (player.position.z >= 500)
        {
            scoretext.text = "Level Complete!";
            return;
        }

        int questionNumber = Mathf.FloorToInt(player.position.z / 100);

        if (questionNumber != currentQuestion &&
            questionNumber < questions.Length)
        {
            if (!string.IsNullOrEmpty(questions[questionNumber]))
            {
                currentQuestion = questionNumber;
                scoretext.text = questions[questionNumber];
            }
        }
    }
}