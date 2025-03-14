using UnityEngine;
using UnityEngine.Serialization;

public class InteractableBoneToPick : MonoBehaviour
{
    [FormerlySerializedAs("isActive")] [SerializeField]
    private bool _isActive = false;
    [FormerlySerializedAs("wasUsed")] [SerializeField] public bool WasUsed = false; //Note: does this need to be public? private and method to pass value instead

    private void Update()
    {
        var questObject = QuestManager.GetQuestObject("BoneToPick");
        var q = QuestManager.Instance.GetQuest(questObject);
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _isActive && q.State.Equals(QuestState.InProgress))
        {
            WasUsed = true;
            FindObjectOfType<CollectableQuestStepBoneToPick>().CollectableProgress();
            Debug.Log("object found!");
            Destroy(gameObject);
        }
        else if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _isActive && q.State.Equals(QuestState.Finished))
        {
            WasUsed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _isActive = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _isActive = false;
    }
}
