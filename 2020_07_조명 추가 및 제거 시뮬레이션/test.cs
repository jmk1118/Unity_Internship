using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

/// <summary>
/// by MK, 테스트씬용 클래스
/// </summary>
public class test : MonoBehaviour
{
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    Process process = new Process();

    // Start is called before the first frame update
    void Start()
    {
        // 프로세스 초기화
        processStartInfo.FileName = @"cmd";
        processStartInfo.CreateNoWindow = false;
        processStartInfo.UseShellExecute = false;
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardInput = true;
        processStartInfo.RedirectStandardError = true;
        process.StartInfo = processStartInfo;

        // 프로세스 시작
        process.Start();

        // 미리 입력해둔 입력값 테스트 입력
        process.StandardInput.Write("cd C:\\Users\\jmk11\\Downloads\\1233" + Environment.NewLine);
        process.StandardInput.Write("type *.txt > C:\\Users\\jmk11\\Downloads\\123.txt" + Environment.NewLine);
        process.StandardInput.Close();

        process.WaitForExit();
        process.Close();
    }
}
