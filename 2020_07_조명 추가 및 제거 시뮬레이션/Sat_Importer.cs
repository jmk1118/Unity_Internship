using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using PiXYZ.Import;
using PiXYZ.Config;
using PiXYZ.Samples;
using PiXYZ.Utils;
using SFB;

/// <summary>
/// by MK, 오픈소스를 활용해 SAT파일을 import하여 오브젝트로 만드는 클래스
/// </summary>
public class Sat_Importer : MonoBehaviour 
{
    ProcessStartInfo processStartInfo = new ProcessStartInfo(); // Sat파일들을 하나의 SAT파일로 합치는 과정에서 필요한 cmd호출용
    Process process = new Process(); // Sat파일들을 하나의 SAT파일로 합치는 과정에서 필요한 cmd호출용

    static string SatSaveFolder; // 합쳐진 SAT파일이 임시적으로 저장될 폴더 경로
    int fileCount; // 폴더내의 파일 번호(sat파일 탐색시 사용)
    bool importFinish; // sat import중일 경우 false, 아무것도 import하고있지 않을 경우 true

    [SerializeField] GameObject spinner; // sat import중일 경우 돌아가는 모래시계역할.
    [SerializeField] GameObject LicenseManager; // Pixyz 라이선스 매니저 창
    [SerializeField] GameObject PointLight; // 조명 SAT 오브젝트에 자식으로 붙여줄 조명 오브젝트
    List<Dictionary<string, object>> CSVdata; // CSV파일에서 읽어들인 메타데이터
    string[] LogData; // 로그 파일에서 읽어들인 SAT파일들의 import 순서

    [SerializeField] GameObject IESChanger; //조명 오브젝트의 IES파일 변경을 담당하는 클래스

    // 초기화
    private void Start()
    {
        processStartInfo.FileName = @"cmd";
        processStartInfo.CreateNoWindow = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardInput = true;
        processStartInfo.RedirectStandardError = true;
        process.StartInfo = processStartInfo;

        SatSaveFolder = UnityEngine.Application.dataPath + "\\SatData"; //합쳐진 SAT 파일 저장폴더
        fileCount = 0;
        importFinish = false;
    }

    /// <summary>
    /// import가 끝났을 경우 importFinish가 true가 되어 다음 sat파일을 import한다.
    /// </summary>
    private void Update()
    {
        if (importFinish)
        {
            SatImporter(SatSaveFolder);
            importFinish = false;
        }
    }

    /// <summary>
    /// SAT, CSV 파일 import를 위한 모든 과정을 관리하는 함수
    /// </summary>
    public void StartSatImport()
    {
        if (importFinish) // import중이면 종료
            return;

        // sat 다운로드 프로그램 라이센스를 가지고 있어야 실행
        if (Configuration.CheckLicense() == false)
        {
            LicenseManager.SetActive(true);
            return;
        }

        if (Directory.Exists(SatSaveFolder) == false) // 합쳐진 sat파일을 보관할 폴더를 생성
            Directory.CreateDirectory(SatSaveFolder);

        string[] paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", false); // 윈도우 탐색기 오픈

        if (paths.Length < 1)
            return;

        FindCSVinDirectory(paths[0]); // 입력받은 폴더에서 csv 파일을 탐색
        FindSATinDirectory(paths[0]); // 입력받은 폴더에서 sat 파일을 탐색

        // 기존에 있었던 오브젝트는 파괴
        for (int i = 0; i < this.transform.childCount; i++)
            Destroy(this.transform.GetChild(i).gameObject);

        fileCount = 0; // 폴더 내 파일 카운트 초기화
        SatImporter(SatSaveFolder); // sat import 시작
    }

    // path 경로에서 csv 파일을 탐색하는 함수
    private void FindCSVinDirectory(string path)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(path); // 경로에 있는 폴더 할당

        // 폴더 내 폴더 모두 csv 파일 탐색
        if (Directory.Exists(path))
        {
            foreach (var item in directoryInfo.GetDirectories())
                FindCSVinDirectory(item.FullName);
        }

        // csv 파일이 존재하면, csv를 프로그램 내로 읽어들인다
        if (Directory.GetFiles(path, "*.csv").Length > 0)
        {
            CSVImporter(path);
        }
    }

    /// <summary>
    /// path내에 있는 폴더마다 합쳐진 sat파일을 생성하는 함수
    /// </summary>
    /// <param name="path">폴더 경로</param>
    private void FindSATinDirectory(string path)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(path); // 경로에 있는 폴더 할당

        // 폴더 내 폴더 모두 sat 파일 탐색
        if (Directory.Exists(path))
        {
            foreach (var item in directoryInfo.GetDirectories())
                FindSATinDirectory(item.FullName);
        }

        // sat파일이 존재하는 폴더면 모든 sat를 합친다.
        if (Directory.GetFiles(path, "*.sat").Length > 0)
        {
            SumSat(path, directoryInfo.Parent.Name + "_" + directoryInfo.Name);
        }
    }

    /// <summary>
    /// path 폴더에 존재하는 sat파일을 모두 합쳐서 filename.sat, filename.log를 생성하는 함수
    /// </summary>
    /// <param name="path">filename을 저장하는 경로</param>
    /// <param name="filename">저장할 파일명</param>
    private void SumSat(string path, string filename)
    {
        process.Start();
        process.StandardInput.Write("cd " + path + Environment.NewLine);

        // sat 파일을 이어붙여 하나로 만든다
        process.StandardInput.Write("type *.sat > " + SatSaveFolder + "\\" + filename + ".sat 2> " + SatSaveFolder + "\\" + filename + ".log" + Environment.NewLine);
        process.StandardInput.Close();

        process.WaitForExit();
        process.Close();
    }

    /// <summary>
    /// path로 입력받은 sat파일을 import하는 함수
    /// </summary>
    /// <param name="path">sat 파일 경로</param>
    private void SatImporter(string path)
    {
        // 폴더에 있던 sat파일을 모두 지운다
        if (Directory.GetFiles(path, "*.sat").Length <= fileCount)
        {
            Directory.Delete(path, true);

            importFinish = false;
            return;
        }
         
        // 로딩을 알리는 오브젝트를 회전시킨다
        spinner.GetComponent<Spinner>().spin();
        // sat 파일의 순서를 기록한 로그 파일을 가져온다
        LogData = File.ReadAllLines(Directory.GetFiles(path, "*.log")[fileCount]);
    
        // 오픈소스를 활용하여 sat import
        Importer importer = new Importer(Directory.GetFiles(path, "*.sat")[fileCount], null);
        importer.completed += callback;
        importer.run();
    }

    /// <summary>
    /// path폴더에 있는 csv파일을 읽어들이는 함수
    /// </summary>
    /// <param name="path">폴더 경로</param>
    private void CSVImporter(string path)
    {
        CSVdata = CSVReader.Read(path + "\\Lighting.csv");
    }

    /// <summary>
    /// import가 끝날시 호출되는 트리거 함수
    /// </summary>
    /// <param name="gameObject"></param>
    private void callback(GameObject gameObject)
    {
        Vector3 gameObjectCenter;

        // 각 오브젝트들의 센터를 0,0에서 오브젝트의 센터로 바꾼다.
        for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++) 
        {
            if (gameObject.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>() == null)
                continue;
            gameObjectCenter = gameObject.transform.GetChild(0).GetChild(i).gameObject.GetBoundsWorldSpace(true).center;
            gameObject.transform.GetChild(0).GetChild(i).position = gameObjectCenter;

            Vector3 delta = Vector3.zero - gameObjectCenter;
            Vector3 localDelta = gameObject.transform.GetChild(0).GetChild(i).InverseTransformVector(delta);
            gameObject.transform.GetChild(0).GetChild(i).position = gameObjectCenter;
            foreach (Transform child in gameObject.transform.GetChild(0).GetChild(i))
            {
                child.position += delta;
            }

            // 메쉬를 조정한다
            MeshFilter meshFilter = gameObject.transform.GetChild(0).GetChild(i).gameObject.GetComponent<MeshFilter>();
            if (meshFilter)
            {
                Mesh mesh = Mesh.Instantiate(meshFilter.sharedMesh);
                mesh.name = meshFilter.sharedMesh.name;
                var vertices = mesh.vertices;
                for (int j = 0; j < vertices.Length; j++)
                {
                    vertices[j] += localDelta;
                }
                mesh.vertices = vertices;
                mesh.RecalculateBounds();
                meshFilter.sharedMesh = mesh;
            }
        }

        // 조명일 경우 빛과 데이터 추가
        if (gameObject.name.Contains("Lighting"))  
        {
            IESChanger.GetComponent<IES_Changer>().SetSatLighting(gameObject.transform.GetChild(0).gameObject);

            int count = 0;
            for (int i = 0; i < LogData.Length; i++)
            {
                if (LogData[i] == "")
                    continue;
                
                // csv 파일에 있던 데이터를 조명 오브젝트에 기록
                for (int o = 0; o < CSVdata.Count; o++)
                {
                    if (CSVdata[o]["OID"].ToString() == Path.GetFileNameWithoutExtension(LogData[i]))
                    {
                        gameObject.transform.GetChild(0).GetChild(count).gameObject.AddComponent<CSV_Data>();
                        gameObject.transform.GetChild(0).GetChild(count).gameObject.GetComponent<CSV_Data>().AddData(CSVdata[o]["PROJ_NO"].ToString(), CSVdata[o]["EQP_KEY"].ToString(), CSVdata[o]["Equip Type"].ToString(), CSVdata[o]["OID"].ToString(), CSVdata[o]["COG_X"].ToString(), CSVdata[o]["COG_Y"].ToString(), CSVdata[o]["COG_Z"].ToString());

                        break;
                    }
                }
                count++;
            }

            for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
            {
                Instantiate(PointLight, gameObject.transform.GetChild(0).GetChild(i));
                if (IESChanger.GetComponent<IES_Changer>().SetLightData(gameObject.transform.GetChild(0).GetChild(i).gameObject, gameObject.transform.GetChild(0).GetChild(i).GetComponent<CSV_Data>().WhatEquipType()) == false)
                    UnityEngine.Debug.Log(gameObject.transform.GetChild(0).GetChild(i).name);
            }

            // csv기준으로 위치 변경
            for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++) 
            {
                gameObject.transform.GetChild(0).GetChild(i).position = gameObject.transform.GetChild(0).GetChild(i).GetComponent<CSV_Data>().ChangePosition();
            }
        }
        // 조명 외의 오브젝트들은 -x, z, y로 위치를 변경시킨다.
        else
        {
            Transform nowChild;
            for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
            {
                nowChild = gameObject.transform.GetChild(0).GetChild(i);
                nowChild.position = new Vector3(nowChild.position.x * -1, nowChild.position.y, nowChild.position.z);
            }
        }

        fileCount++;
        spinner.GetComponent<Spinner>().stop();
        gameObject.transform.parent = this.transform;
        importFinish = true;
    }
}
