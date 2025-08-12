using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class QueueSystem : MonoBehaviour
{
    [SerializeField] private Transform[] queuePositions;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private GameObject personPrefab;
    [SerializeField] private Transform endMovePosotion;

    private List<GameObject> peopleInQueue = new List<GameObject>();
    private bool ready;

    [Inject] private MoneyManager moneyManager;

    void Start()
    {
        for (int i = 0; i < queuePositions.Length; i++)
        {
            AddPersonToQueue();
        }

        ready = true;
    }

    public bool PurchaseReady()
    {
        return ready;
    }

    public void AddPersonToQueue()
    {
        if (peopleInQueue.Count >= queuePositions.Length) return;

        GameObject newPerson = Instantiate(personPrefab, queuePositions[peopleInQueue.Count].position, Quaternion.identity);
        peopleInQueue.Add(newPerson);
    }

    public void ProcessPurchase(Transform lastGrass)
    {
        if (peopleInQueue.Count == 0) return;

        ready = false;

        SoldGrass grass = lastGrass.GetComponent<SoldGrass>();
        Vector3 pos = (Vector3.up + Vector3.forward) / 2f;
        grass.Jump(peopleInQueue[0].transform, pos, .75f, () =>
        {
            FxManager.PlayFx(FxType.grass, lastGrass.position + Vector3.up, Vector3.zero);
            Destroy(lastGrass.gameObject);

            StartCoroutine(PlayPurchaseEffects(peopleInQueue[0]));

            peopleInQueue.RemoveAt(0);

            for (int i = 0; i < peopleInQueue.Count; i++)
            {
                StartCoroutine(MoveToPosition(peopleInQueue[i], queuePositions[i].position));
            }

            AddPersonWithDelay();

            ready = true;
        });
    }

    private IEnumerator PlayPurchaseEffects(GameObject person)
    {
        // Анимация или эффекты покупки
        moneyManager.AddMoney(1);

        SoundManager.PlaySound(SoundType.buy);        

        StartCoroutine(MoveToPosition(person, endMovePosotion.position));

        yield return new WaitForSeconds(5f);
        Destroy(person);
    }

    // Можно вызывать при необходимости
    public void AddPersonWithDelay()
    {
        StartCoroutine(DelayedAddPerson(Random.Range(.5f, 1f)));
    }

    private IEnumerator DelayedAddPerson(float delay)
    {
        yield return new WaitForSeconds(delay);
        AddPersonToQueue();
    }

    // Плавное перемещение к позиции
    private IEnumerator MoveToPosition(GameObject person, Vector3 targetPosition)
    {
        Animator animator = person.GetComponent<Animator>();

        if (animator != null) animator.SetBool("IsWalking", true);

        while (Vector3.Distance(person.transform.position, targetPosition) > 0.01f)
        {
            person.transform.position = Vector3.MoveTowards(person.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            var movedirection = targetPosition - person.transform.position;
            movedirection.Normalize();

            if (movedirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movedirection, Vector3.up);
                person.transform.rotation = Quaternion.RotateTowards(person.transform.rotation, toRotation, 850 * Time.deltaTime);
            }

            yield return null;
        }

        person.transform.position = targetPosition;

        if (animator != null) animator.SetBool("IsWalking", false);
    }
}