using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInteractable
{
    string GetInteractionText();
    bool CanInteract(GameObject interactor);
    void Interact(GameObject interactor);
    Transform GetTransform();
    InteractionType GetInteractionType();

}

public interface IOpenable : IInteractable
{
    void Open(GameObject interactor);
    void Close(GameObject interactor);
    bool IsOpen { get; }
}

public enum InteractionType
{
    Pickup,
    Use,
    Cook,
    Clean,
    Store,
    Activate,
    Talk,
    Open
}
public class InteractionManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _interactionRange = 2f; // Khoảng cách tương tác tối đa
    [SerializeField] private LayerMask _interactableLayer; // Lớp của các đối tượng có thể tương tác
    [SerializeField] private KeyCode _interactionKey = KeyCode.E; // Phím để tương tác

    [Header("UI References")]
    [SerializeField] private GameObject _interactionUI; // UI hiển thị khi có đối
    [SerializeField] private Text _interactionText; // Text hiển thị thông tin tương tác

    [Header("Debug")]
    [SerializeField] private IInteractable _currentInteractable; // Đối tượng hiện tại
    [SerializeField] private List<IInteractable> _nearbyInteractables = new List<IInteractable>(); // Danh sách các đối tượng có thể tương tác trong phạm vi
    [SerializeField] private Camera _mainCamera; // Camera chính để xác định hướng nhìn

    private ChefController _chefController;

    // Events
    public event Action<IInteractable> OnInteractionStarted;
    public event Action<IInteractable> OnInteractionEnded;
    public event Action<IInteractable> OnInteractableFound;
    public event Action OnInteractableLost;

    void Start()
    {
        _chefController = GetComponent<ChefController>();
        if (_interactionUI)
        {
            _interactionUI.SetActive(false); // Ẩn UI tương tác ban đầu
        }
    }

    void Update()
    {
        DetectInteractables();
        HandleInteractionInput();
        UpdateUI();
    }





    private void DetectInteractables()
    {
        _nearbyInteractables.Clear();

        // sphere cast around player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _interactionRange, _interactableLayer);
        foreach (var hitCollider in hitColliders)
        {
            IInteractable interactable = hitCollider.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract(gameObject))
            {
                _nearbyInteractables.Add(interactable);
            }
        }

        // find closest interactable
        IInteractable closestInteractable = GetClosestInteractable();

        if (closestInteractable != _currentInteractable)
        {
            if (_currentInteractable != null)
            {
                OnInteractableLost?.Invoke();
            }

            _currentInteractable = closestInteractable;

            if (_currentInteractable != null)
            {
                OnInteractableFound?.Invoke(_currentInteractable);
            }
        }
    }

    private IInteractable GetClosestInteractable()
    {
        if (_nearbyInteractables.Count == 0)
            return null;

        IInteractable closest = null;
        float closestDistance = float.MaxValue;

        foreach (var interactable in _nearbyInteractables)
        {
            float distance = Vector3.Distance(transform.position, interactable.GetTransform().position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = interactable;
            }
        }
        return closest;

    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(_interactionKey) && _currentInteractable != null)
        {
            TryInteract(_currentInteractable);
        }
    }

    public void TryInteract(IInteractable interactable)
    {
        if (interactable != null && interactable.CanInteract(gameObject))
        {
            OnInteractionStarted?.Invoke(interactable);
            interactable.Interact(gameObject);
            OnInteractionEnded?.Invoke(interactable);
        }
    }



    private void UpdateUI()
    {
        if (_interactionUI && _interactionText)
        {
            bool hasInteractable = _currentInteractable != null;
            _interactionUI.SetActive(hasInteractable);

            if (hasInteractable)
            {
                _interactionText.text = $"[{_interactionKey}] {_currentInteractable.GetInteractionText()}";
                _interactionUI.transform.position = _currentInteractable.GetTransform().position;
                _interactionUI.transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward, Vector3.up);
            }

            Debug.Log($"Interaction UI Active: {hasInteractable}, Text: {_interactionText.text}");
           
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactionRange);
    }

}
