using System.Collections;
using TMPro;
using UnityEngine;

public class FPSLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private float interval = 0.5f;

    private float count;

    private IEnumerator Start()
    {
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            textField.SetText("FPS: " + Mathf.Round(count));

            yield return new WaitForSeconds(interval);
        }
    }
}
