using UnityEngine;

public class EnterBossZone : MonoBehaviour
{
    public BossPattern bossPattern;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Checkpoint.hasCheckpoint = true;
            Checkpoint.respawnPosition = transform.position;
            Checkpoint.bossFightStarted = true;

            other.transform.position = transform.position;

            bossPattern.ChangeState(BossState.Move);

            gameObject.SetActive(false);
        }
    }
}
