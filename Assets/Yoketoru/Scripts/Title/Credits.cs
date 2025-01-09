using UnityEngine;

public class Credits : OverlapScene
{
    private void Update()
    {
        if (!canControl)
        {
            return;
        }
        
        // 閉じるチェック
        if (Input.GetButtonDown("Cancel"))
        {
            tinyAudio.PlaySE(TinyAudio.SE.Cancel);
            Hide();
        }
    }
}
