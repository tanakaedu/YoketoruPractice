using UnityEngine;

/// <summary>
/// ウェイポイントを設定して、そのルートを巡回する。
/// </summary>
public class WayPointMover : MonoBehaviour, IStartStop
{
    static float GizmoRadius => 0.1f;
    static readonly Vector3 GizmoOffset = 0.5f * Vector3.back;

    public enum Type
    {
        /// <summary>
        /// 行ったり来たり
        /// </summary>
        PingPong,
        /// <summary>
        /// 端についたら、スタートから
        /// </summary>
        Loop,
    }

    [SerializeField, Tooltip("移動速度")]
    float speed = 2f;
    [SerializeField, Tooltip("通過点")]
    Vector3[] wayPoints = default;
    [SerializeField, Tooltip("終端についた時の動作")]
    Type type = Type.PingPong;
    [SerializeField, Tooltip("スタートしたら、目指す先のインデックス")]
    int startNextIndex = 1;

    int nextIndex;
    Rigidbody rb;

    /// <summary>
    /// インデックスの加算、減算方向
    /// </summary>
    int indexStep = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nextIndex = startNextIndex;
    }

    public void Move(float delta)
    {
        // 移動ベクトルを求める

        // 目的地までの残りの距離を求める

        // 1回分の移動距離を求める

        // 次の移動で到着するなら、目的地を切り替える
        // 到着しないなら、1回分の距離を移動

        // rb.MovePositionを使って移動。現在座標は、rb.positionで参照
    }

    void OnDrawGizmosSelected()
    {
        if (wayPoints == null) { return; }

        Gizmos.color = Color.red;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            Gizmos.DrawSphere(wayPoints[i] + GizmoOffset, GizmoRadius);
        }
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
