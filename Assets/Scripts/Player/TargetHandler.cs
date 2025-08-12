using System;
using UnityEngine;
using Zenject;

public class TargetHandler : MonoBehaviour
{
    [Inject] private InputManager inputManager;

    [SerializeField] private PlayerControl playerControl;

    private void OnEnable() => inputManager.OnPointerJoystickUpChanged += JoystickUpChanged;

    private void OnDisable() => inputManager.OnPointerJoystickUpChanged -= JoystickUpChanged;

    public void JoystickUpChanged()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);

        if (hitColliders.Length == 0)
            return;

        Transform nearestObject = null;
        float minDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            IMinable minable = hitCollider.GetComponent<IMinable>();

            if (minable != null && minable.IsReady())
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObject = hitCollider.transform;
                }
            }
        }

        if (nearestObject != null)
        {
            playerControl.DoRotate(nearestObject.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ITargetable targetable))
        {
            targetable.TargetEnter(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ITargetable targetable))
        {
            targetable.TargetLeave();
        }
    }
}
