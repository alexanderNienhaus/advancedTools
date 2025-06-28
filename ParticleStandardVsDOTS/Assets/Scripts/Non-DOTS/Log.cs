using System.Collections;
using TMPro;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class Log : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textFieldFps;
    [SerializeField] private TextMeshProUGUI textFieldParticlesDOTS;
    [SerializeField] private TextMeshProUGUI textFieldParticlesStandard;
    [SerializeField] private float interval = 0.5f;
    [SerializeField] private bool doLog = false;

    private float count = 0;
    private float duration = 0;
    private float numberOfParticlesDOTS = 0;
    private float numberOfParticlesStandard = 0;
    private string pathFilename;

    private IEnumerator Start()
    {
        pathFilename = Application.dataPath + @"\TestResults\" + SceneManager.GetActiveScene().name + "_" + DateTime.Now.ToString("yyyy-dd-M_HH-mm-ss") + ".txt";
        while (true)
        {
            count = Mathf.Round(1f / Time.unscaledDeltaTime);
            textFieldFps.SetText("FPS: " + count);
            if(doLog) SaveLogToTxt();

            duration += interval;

            textFieldParticlesDOTS.SetText("# DOTS: " + numberOfParticlesDOTS);
            textFieldParticlesStandard.SetText("# Standard: " + numberOfParticlesStandard);

            yield return new WaitForSeconds(interval);
        }
    }

    private void SaveLogToTxt()
    {
        if (!File.Exists(pathFilename))
        {
            File.WriteAllText(pathFilename, "Start Log - Format: duration;count;numberOfParticlesDOTS;numberOfParticlesStandard\n");
        }
        
        string content = duration + ";" + count + ";" + numberOfParticlesDOTS + ";" + numberOfParticlesStandard + "\n";
        File.AppendAllText(pathFilename, content);
    }

    private void OnNumberOfParticlesChangedDOTS(OnNumberOfParticlesChangedDOTS pOnNumberOfParticlesChanged)
    {
        numberOfParticlesDOTS += pOnNumberOfParticlesChanged.numberOfParticlesChange;
    }

    private void OnNumberOfParticlesChangedStandard(OnNumberOfParticlesChangedStandard pOnNumberOfParticlesChangedStandard)
    {
        numberOfParticlesStandard += pOnNumberOfParticlesChangedStandard.numberOfParticlesChange;
    }

    private void OnEnable()
    {
        EventBus<OnNumberOfParticlesChangedDOTS>.OnEvent += OnNumberOfParticlesChangedDOTS;
        EventBus<OnNumberOfParticlesChangedStandard>.OnEvent += OnNumberOfParticlesChangedStandard;
    }

    private void OnDisable()
    {
        EventBus<OnNumberOfParticlesChangedDOTS>.OnEvent -= OnNumberOfParticlesChangedDOTS;
        EventBus<OnNumberOfParticlesChangedStandard>.OnEvent -= OnNumberOfParticlesChangedStandard;
    }
}
