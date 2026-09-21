using UnityEngine;

public class QuestionStartTrigger : MonoBehaviour
{
    public QuestionSection questionSection;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (questionSection != null)
        {
            questionSection.ShowQuestion();
            Debug.Log("Triggered!");
        }
    }
}