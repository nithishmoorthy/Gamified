using UnityEngine;

public class AnswerTrigger : MonoBehaviour
{
    public int optionNumber;

    private QuestionSection questionSection;

    void Awake()
    {
        questionSection = GetComponentInParent<QuestionSection>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (questionSection == null)
        {
            Debug.LogError("QuestionSection not found!");
            return;
        }

        Debug.Log("Player selected option " + optionNumber);

        questionSection.CheckAnswer(optionNumber);
    }
}