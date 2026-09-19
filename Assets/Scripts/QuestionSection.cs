using UnityEngine;

public class QuestionSection : MonoBehaviour
{
    public int questionIndex;
    public int correctAnswer;

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
        }
        else
        {
            Debug.Log("Q" + (questionIndex + 1) + " - WRONG ANSWER!");
        }
    }
}