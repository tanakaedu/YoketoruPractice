using System;

/// <summary>
/// 簡易状態遷移クラス
/// </summary>
/// <typeparam name="T">状態遷移に使う列挙子</typeparam>
public sealed class SimpleState<T> where T : Enum
{
    public T CurrentState { get; private set; }
    T nextState;

    /// <summary>
    /// None値
    /// </summary>
    T none;

    public SimpleState(T noneValue)
    {
        none = noneValue;
        CurrentState = none;
        nextState = none;
    }

    /// <summary>
    /// 次の状態が設定されていたら、切り替えて、切り替えた状態を返す。
    /// 切り替えがなければ、Noneを返す。
    /// </summary>
    /// <returns>切り替えが発生したら、true。</returns>
    public bool ChangeState()
    {
        if (nextState.Equals(none))
        {
            return false;
        }
        CurrentState = nextState;
        nextState = none;
        return true;
    }

    /// <summary>
    /// 次の状態を設定する。すでになにか設定済みだったら、失敗して、falseを返す。
    /// </summary>
    /// <param name="state">次に設定したい状態</param>
    /// <returns>設定できたらtrue</returns>
    public bool SetNextState(T state)
    {
        if (nextState.Equals(none))
        {
            nextState = state;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 強制的に、次の状態を設定する。
    /// </summary>
    /// <param name="state"></param>
    public void SetNextStateForce(T state)
    {
        nextState = state;
    }
}
