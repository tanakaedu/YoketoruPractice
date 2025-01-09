using UnityEngine;
public class TinyAudio : MonoBehaviour
{
    /// <summary>
    /// seListに設定する効果音の種類を以下に定義します。
    /// </summary>
    public enum SE
    {
        Click,
        Cancel,
        Start,
        Bound,
        Item,
        Miss,
        GameOver,
        Clear,
        CountDown,
        StartPlay,
    }
    [Tooltip("効果音のAudio Clipを、SEの列挙子と同じ順番で設定してください。"), SerializeField]
    AudioClip[] seList;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// SEで指定した効果音を再生します。
    /// </summary>
    /// <param name="se">鳴らしたい効果音</param>
    public void PlaySE(SE se)
    {
        audioSource.PlayOneShot(seList[(int)se]);
    }
}