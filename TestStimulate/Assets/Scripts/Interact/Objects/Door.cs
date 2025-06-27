using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject, IOpenable
{
    [SerializeField] Vector3 openRotation = new Vector3(0, 90, 0); // Rotation to open the door
    [SerializeField] Vector3 closedRotation = new Vector3(0, 0, 0); // Rotation to close the door
    [SerializeField] float openDuration = 2f; // Duration to open the door
    [SerializeField] float closeDuration = 2f; // Duration to close the door
    private bool _isOpen;
    public bool IsOpen => _isOpen;

    override protected void OnInteract(GameObject interactor)
    {
        // Logic to handle door interaction
        if (IsOpen)
        {
            Close(interactor);
        }
        else
        {
            Open(interactor);
        }
    }


    public void Close(GameObject interactor)
    {
        _isOpen = false;
        // Add logic to visually close the door
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(closedRotation), closeDuration);
    }

    public void Open(GameObject interactor)
    {
        _isOpen = true;
        // Add logic to visually open the door
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(openRotation), openDuration);
    }

    

}
