using UnityEngine;
using UnityEngine.UI;

public class QuestionDisplay : MonoBehaviour
{
    public Text questionText;

    public void ShowQuestion(string question)
    {
        questionText.text = question;
    }
}