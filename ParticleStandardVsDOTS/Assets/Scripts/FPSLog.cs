using System.Collections;
using TMPro;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class FPSLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private float interval = 0.5f;
    [SerializeField] private bool doLog = false;

    private float count = 0;
    private float duration = 0;
    private string pathFilename;

    private IEnumerator Start()
    {
        pathFilename = Application.dataPath + @"\TestResults\" + SceneManager.GetActiveScene().name + "_" + DateTime.Now.ToString("yyyy-dd-M_HH-mm-ss") + ".txt";
        while (true)
        {
            count = Mathf.Round(1f / Time.unscaledDeltaTime);
            textField.SetText("FPS: " + count);
            if(doLog) SaveLogToTxt();

            duration += interval;

            yield return new WaitForSeconds(interval);
        }
    }

    private void SaveLogToTxt()
    {
        if (!File.Exists(pathFilename))
        {
            File.WriteAllText(pathFilename, "Start Log\n");
        }
        
        string content = duration + ";" + count + "\n";
        File.AppendAllText(pathFilename, content);
    }
}
