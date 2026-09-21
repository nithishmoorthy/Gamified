using UnityEngine;

public class QuestionStartTrigger : MonoBehaviour
{
    private QuestionSection questionSection;

    void Awake()
    {
        questionSection =
            GetComponentInParent<QuestionSection>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (questionSection != null)
        {
            questionSection.ShowQuestion();
        }
        else
        {
            Debug.LogError(
                "QuestionSection not found!"
            );
        }
    }
}