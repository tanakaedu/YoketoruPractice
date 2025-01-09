using UnityEngine;

public class Clear : OverlapScene
{
    enum State
    {
        None,
        WaitStart,
        WaitInput,
        NextStage,
    }

    SimpleState<State> state = new(State.None);

    private void Start()
    {
        state.SetNextState(State.WaitStart);
    }

    private void Update()
    {
        InitState();
        UpdateState();
    }

    void InitState()
    {
        if (!state.ChangeState())
        {
            return;
        }

        switch (state.CurrentState)
        {
            case State.NextStage:
                gameInstance.RequestNextStage();
                break;
        }
    }

    void UpdateState()
    {
        switch(state.CurrentState)
        {
            case State.WaitStart:
                if (canControl)
                {
                    state.SetNextState(State.WaitInput);
                }
                break;

            case State.WaitInput:
                if (Input.GetButtonDown("Submit"))
                {
                    state.SetNextState(State.NextStage);
                }
                break;
        }
    }
}
