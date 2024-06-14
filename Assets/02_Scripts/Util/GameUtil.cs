using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameUtil : SingletonMono<GameUtil>
{
    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    public void StartWatch()
    {
#if UNITY_EDITOR
        stopwatch.Start();
#endif
    }

    public void StopWatch(string param = "")
    {
#if UNITY_EDITOR
        stopwatch.Stop();
        Debug.Log(string.Format("Time Elapse {0} {1} mSec", param, stopwatch.ElapsedMilliseconds.ToString()));
#endif
    }

    public void StopAndStart(string param = "")
    {
#if UNITY_EDITOR
        stopwatch.Stop();
        Debug.Log(string.Format("Time Elapse {0} {1} mSec", param, stopwatch.ElapsedMilliseconds.ToString()));
        stopwatch.Reset();
        stopwatch.Start();
#endif
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

#if UNITY_EDITOR
    public static bool IsSimbol(string _simbol, BuildTargetGroup _targetGruop)
    {
        bool isSimbol = false;
        string[] simbols;
        PlayerSettings.GetScriptingDefineSymbolsForGroup(_targetGruop, out simbols);

        foreach (var simbol in simbols)
        {
            if (simbol == _simbol)
            {
                isSimbol = true;
                break;
            }
        }

        return isSimbol;
    }

    public static void AddSimbol(string _simbol, BuildTargetGroup _targetGruop)
    {
        string[] simbols;
        PlayerSettings.GetScriptingDefineSymbolsForGroup(_targetGruop, out simbols);
        List<string> simbolList = new List<string>(simbols);
        simbolList.Add(_simbol);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(_targetGruop, simbolList.ToArray());
    }

    public static void RemoveSimbol(string _simbol, BuildTargetGroup _targetGruop)
    {
        string[] simbols;
        PlayerSettings.GetScriptingDefineSymbolsForGroup(_targetGruop, out simbols);
        List<string> simbolList = new List<string>(simbols);
        simbolList.Remove(_simbol);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(_targetGruop, simbolList.ToArray());
    }
#endif
}
