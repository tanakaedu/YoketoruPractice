using UnityEngine;

/// <summary>
/// 反射移動を制御するクラス。
/// </summary>
public class ReflectionMover : MonoBehaviour, IStartStop
{
    static float MinimumSpeed => 0.01f;

    [Tooltip("移動方向"), SerializeField]
    Vector3 firstDirection = Vector3.right;
    [Tooltip("移動速度"), SerializeField]
    float speed = 2;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // TODO: 速度を維持する
    }

    public void OnGameStarted()
    {
        Debug.Log($"{name} 移動開始");
    }

    public void OnGameStopped()
    {
        Debug.Log($"{name} 移動停止");
    }
}
