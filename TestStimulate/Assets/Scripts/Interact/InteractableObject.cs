using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base class for all interactable objects
public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    [SerializeField] protected string interactionText = "Interact";
    [SerializeField] protected InteractionType interactionType = InteractionType.Use;
    [SerializeField] protected bool canInteract = true;
    [SerializeField] protected Vector3 _offset_Ui;

    [Header("Audio")]
    [SerializeField] protected AudioClip interactionSound;
    protected AudioSource audioSource;
    public string GetInteractionText() => interactionText;
    public InteractionType GetInteractionType() => interactionType;
    public Transform GetTransform() => transform;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool CanInteract(GameObject interactor)
    {
        return canInteract;
    }

    public void Interact(GameObject interactor)
    {
        PlayInteractionSound();
        OnInteract(interactor);
    }

    protected virtual void OnInteract(GameObject interactor)
    {

    }
    
    protected void PlayInteractionSound()
    {
        if (interactionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(interactionSound);
        }
    }

}
