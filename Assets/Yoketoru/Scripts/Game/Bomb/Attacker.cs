using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 接触相手がプレイヤーなら、ゲームオーバーを要求して、自分を爆破する。
/// </summary>
public class Attacker : MonoBehaviour, IGameOverEmitter
{
    [SerializeField]
    Explosion explosionPrefab = default(Explosion);

    public UnityEvent GameOverRequest { get; } = new();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"爆発");
    }
}
