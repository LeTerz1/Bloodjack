using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public BossState currentState;

    private void Start()
    {
        ChangeState(BossState.Idle);
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

            case BossState.Attack1:
                break;
        }
    }

}

public enum BossState
{
    Idle,
    Move,
    Attack1,
}