using UnityEngine;
using System.Collections.Generic;
using SFB;

/// <summary>
/// by MK, csv파일을 가져와서 프로그램 내에 저장하는 클래스
/// </summary>
public class CSV_Reader : MonoBehaviour
{
    /// <summary>
    /// CSV파일을 가져오는 함수 (나중에 path를 가져오는 것응로 수정)
    /// </summary>
    public void CSVOutput()
    {
        string selectFBXPath;

        // 탐색기를 열어 파일명을 가져오는 오픈소스
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select Folder", "", "", false);

        if (paths.Length > 0)
        {
            selectFBXPath = paths[0];
            List<Dictionary<string, object>> data = CSVReader.Read(selectFBXPath);

            for (var i = 0; i < data.Count; i++)
            {
                Debug.Log("index " + (i).ToString() + " : " + data[i]["PROJ_NO"] + " " + data[i]["EQP_KEY"] + " " + data[i]["Equip Type"] + " " + data[i]["OID"] + " " + data[i]["COG_X"] + " " + data[i]["COG_Y"] + " " + data[i]["COG_Z"]);
            }
        }
        //List<Dictionary<string, object>> data = CSVReader.Read(@"C:\Users\minki.jung\Downloads\Simple\Lighting.csv");
    }
}
