using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public BossState currentState;

    private void Start()
    {
        ChangeState(BossState.Projectiles);
    }

    public void ChangeState(BossState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case BossState.Idle:
                break;

            case BossState.Move:
                break;

            case BossState.Projectiles:
                GetComponent<BossProjectiles>().SpawnProjectiles();
                break;
        }
    }

}

public enum BossState
{
    Idle,
    Move,
    Projectiles,
}