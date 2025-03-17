using UnityEngine;

public interface IInteractable
{
    public void StartInteraction(PlayerController player);
    public void EndInteraction(PlayerController player);
}
