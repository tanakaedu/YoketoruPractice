using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// コインを拾う機能を提供
/// </summary>
public class Coin : MonoBehaviour, IGetCoinEmitter
{
    [Tooltip("得点"), SerializeField]
    int point = 100;

    public UnityEvent<int> CoinGot { get; } = new();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{point}点");
    }
}
