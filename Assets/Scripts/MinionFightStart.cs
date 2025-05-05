using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionFightStart : MonoBehaviour
{
    [SerializeField] private List<Animator> animators; // List of minions
    [SerializeField] private GameObject boundary;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject PowerUpMessage;
    private Queue<Animator> minionQueue = new Queue<Animator>();
    private Animator currentChasingMinion;
    private bool fightStarted = false;

    private void OnEnable()
    {
        MinionHealth.OnMinionDied += HandleMinionDeath;
    }

    private void OnDisable()
    {
        MinionHealth.OnMinionDied -= HandleMinionDeath;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!fightStarted && collision.CompareTag("Player"))
        {
            fightStarted = true;
            boundary.SetActive(true);

            foreach (Animator anim in animators)
            {
                minionQueue.Enqueue(anim);
            }

            StartCoroutine(StartNextMinion());
        }
    }

    IEnumerator StartNextMinion()
    {
        if (minionQueue.Count == 0)
        {
            Debug.Log("All minions defeated.");

            _player.GetComponent<PlayerShoot>().enabled = true;

            StartCoroutine(PowerUpMessageObject());
            yield break;
        }

        currentChasingMinion = minionQueue.Dequeue();
        currentChasingMinion.SetTrigger("StartChasing");

        // Wait until this minion dies
        yield return new WaitUntil(() => currentChasingMinion == null);

        // Start the next one
        StartCoroutine(StartNextMinion());
    }
    IEnumerator PowerUpMessageObject()
    {
        PowerUpMessage.SetActive(true);
        yield return new WaitForSeconds(3f);
        PowerUpMessage.SetActive(false);
    }
    void HandleMinionDeath(MinionHealth deadMinion)
    {
        Animator anim = deadMinion.GetComponent<Animator>();
        if (anim == currentChasingMinion)
        {
            currentChasingMinion = null;
        }
    }
}
