using System.Collections;
using TMPro;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class Log : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textFieldFps;
    [SerializeField] private TextMeshProUGUI textFieldParticlesDOTS;
    [SerializeField] private TextMeshProUGUI textFieldParticlesStandard;
    [SerializeField] private float interval = 0.5f;
    [SerializeField] private bool doLog = false;

    private float fps = 0;
    private float duration = 0;
    private float numberOfParticlesDOTS = 0;
    private float numberOfParticlesStandard = 0;
    private string pathFilename;

    private int iterations = 0;
    private int cumulativeFps = 0;

    private IEnumerator Start()
    {
        pathFilename = Application.dataPath + @"\TestResults\" + SceneManager.GetActiveScene().name + "_" + DateTime.Now.ToString("yyyy-dd-M_HH-mm-ss") + ".txt";
        while (true)
        {
            yield return new WaitForSeconds(interval);

            fps = cumulativeFps / iterations;
            iterations = 0;
            cumulativeFps = 0;

            textFieldFps.SetText("FPS: " + fps);
            if(doLog) SaveLogToTxt();

            duration += interval;

            textFieldParticlesDOTS.SetText("# DOTS: " + numberOfParticlesDOTS);
            textFieldParticlesStandard.SetText("# Standard: " + numberOfParticlesStandard);
        }
    }

    private void Update()
    {
        iterations++;
        cumulativeFps += (int)Mathf.Round(1f / Time.unscaledDeltaTime);
    }

    private void SaveLogToTxt()
    {
        if (!File.Exists(pathFilename))
        {
            File.WriteAllText(pathFilename, "Time;Number of Particles (DOTS);Number of Particles (Standard);FPS\n");
        }

        string content = duration + ";" + numberOfParticlesDOTS + ";" + numberOfParticlesStandard + ";" + fps + "\n";
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
