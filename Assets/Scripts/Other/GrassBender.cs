using UnityEngine;

public class GrassBender : MonoBehaviour
{
    [SerializeField] private float maxTiltAngle = 30f;
    [SerializeField] private float tiltSpeed = 8f;
    [SerializeField] private float returnSpeed = 4f;
    [SerializeField] private float interactionRadius = .5f;
    [SerializeField] private Transform model;
    [SerializeField] private Grass grass;

    private Quaternion originalRotation;
    private Transform playerTransform;
    private bool isTilting = false;

    private void Start()
    {
        originalRotation = model.localRotation;
    }

    private void Update()
    {
        if (isTilting && playerTransform != null && grass.IsReady())
        {
            // Получаем направление от игрока к траве (только X и Z)
            Vector3 direction = model.transform.position - playerTransform.position;
            direction.y = 0;

            if (direction.magnitude > 0.01f)
            {
                // Нормализуем направление
                direction.Normalize();

                // Создаем вращение только по горизонтальным осям
                Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, direction) * Quaternion.Euler(maxTiltAngle, 0, 0);

                // Сохраняем исходное значение Y-ротации
                Vector3 euler = targetRotation.eulerAngles;

                float x = euler.x > 180 ? euler.x - 360 : euler.x;
                float z = euler.z > 180 ? euler.z - 360 : euler.z;

                x = Mathf.Clamp(x, -maxTiltAngle, maxTiltAngle);
                z = Mathf.Clamp(z, -maxTiltAngle, maxTiltAngle);

                euler.y = originalRotation.eulerAngles.y;
                //targetRotation.eulerAngles = euler;
                targetRotation.eulerAngles = new Vector3(x, euler.y, z);

                // Плавный наклон
                model.localRotation = Quaternion.Slerp(model.localRotation, targetRotation, tiltSpeed * Time.deltaTime);
            }
        }
        else if (model.localRotation != originalRotation)
        {
            model.localRotation = Quaternion.Slerp(model.localRotation, originalRotation, returnSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl player))
        {
            isTilting = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl player))
        {
            isTilting = false;
            playerTransform = null;
        }
    }

    // Визуализация радиуса взаимодействия в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}