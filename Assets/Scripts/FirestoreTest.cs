using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirestoreTest : MonoBehaviour
{
    void Start()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        db.Collection("Questions").Document("Q001").GetSnapshotAsync()
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
                        int correctAnswer = snapshot.GetValue<int>("correctAnswer");

                        Debug.Log("QUESTION: " + question);
                        Debug.Log("OPTION 1: " + option1);
                        Debug.Log("OPTION 2: " + option2);
                        Debug.Log("OPTION 3: " + option3);
                        Debug.Log("CORRECT ANSWER: " + correctAnswer);
                    }
                    else
                    {
                        Debug.LogError("Q001 does not exist!");
                    }
                }
                else
                {
                    Debug.LogError("Firestore read failed: " + task.Exception);
                }
            });
    }
}