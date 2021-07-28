using UnityEngine;
using System.IO;
using IESLights;
using Michsky.UI.ModernUIPack;
using SFB;

/// <summary>
/// by MK, 탐색기에서 IES파일을 선택해 프로그램 내에 불러오는 클래스
/// </summary>
public class IES_Importer : MonoBehaviour
{
    Texture2D texture; // spot light용 텍스쳐
    Cubemap cubemap; // point light용 큐브맵

    [SerializeField] GameObject SpotLightList; // 스팟라이트 라이브러리
    [SerializeField] GameObject PointLightList; // 포인트라이트 라이브러리
    [SerializeField] GameObject IESPrefab; // IES 텍스처 혹은 큐브맵을 저장하는 프리팹
    GameObject newItem; // 인스턴스 명시용
    static string IESSaveFolder; // IES가 들어있는 폴더 경로 표시용 string

    [SerializeField] GameObject IESChanger; // 프로그램에 불러온 IES 파일을 기록하고 바꿔주기 위한 오브젝트

    // 초기화
    private void Start()
    {
        IESSaveFolder = Application.dataPath + "\\IESData"; // IES파일 내부 저장폴더

        if (Directory.Exists(IESSaveFolder) == false)
            Directory.CreateDirectory(IESSaveFolder);

        for (int i = 0; i < Directory.GetFiles(IESSaveFolder).Length; i++)
        {
            if (Path.GetExtension(Directory.GetFiles(IESSaveFolder)[i]) == ".ies")
                IESInLibrary(Directory.GetFiles(IESSaveFolder)[i], Path.GetFileNameWithoutExtension(Directory.GetFiles(IESSaveFolder)[i]));
        }
    }

    /// <summary>
    /// 탐색기에서 IES 파일을 선택해 프로그램 내에 불러오는 함수
    /// </summary>
    public void StartIESImport()
    {
        // 오픈소스를 통해 탐색기에서 IES파일의 경로 획득
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select IES Files", "", "ies", true);

        // 파일이 존재하면 내부경로로 파일을 복사하고, IES파일을 프로그램 내에 보여준다
        for (int i = 0; i < paths.Length; i++)
        {
            if (File.Exists(IESSaveFolder + "\\" + Path.GetFileName(Path.GetDirectoryName(paths[i])) + "_" + Path.GetFileName(paths[i])) == false)
                File.Copy(paths[i], IESSaveFolder + "\\" + Path.GetFileName(Path.GetDirectoryName(paths[i])) + "_" + Path.GetFileName(paths[i]));

            IESInLibrary(paths[i], Path.GetFileName(Path.GetDirectoryName(paths[i])) + "_" + Path.GetFileNameWithoutExtension(paths[i]));
        }
    }

    /// <summary>
    /// IES파일을 프로그램 내에 보여주는 함수
    /// </summary>
    /// <param name="path">ies파일들을 불러올 폴더 경로</param>
    private void IESInLibrary(string path, string name)
    {
        newItem = Instantiate(IESPrefab);
        newItem.GetComponent<ButtonManager>().buttonText = name; // IESPrefab을 생성하고 이름을 ies파일명으로 한다.
        newItem.GetComponent<IES_Prefab>().SaveName(name.Substring(name.LastIndexOf('_') + 1));

        RuntimeIESImporter.Import(path, out texture, out cubemap); // ies를 import하여 texture 혹은 cubemap형태로 가져온다
        if (texture != null) // 만약 texture 형식의 ies 파일일 경우, texture를 SpotLight 라이브러리 하단에 위치시킨다.
        {
            newItem.GetComponent<IES_Prefab>().SaveTexture(texture);
            newItem.transform.SetParent(SpotLightList.transform);
        }
        else if (cubemap != null) // 만약 cubemap 형식의 ies 파일일 경우, cubemap을 PointLight 라이브러리 하단에 위치시킨다.
        {
            newItem.GetComponent<IES_Prefab>().SaveCubemap(cubemap);
            newItem.transform.SetParent(PointLightList.transform);
        }

        // TILT=NONE이 적힌 line을 제대로 읽어내지 못해, 해당 라인의 첫 단어만 루멘값으로써 가져온다
        string line;
        string[] exactlyLine;
        using(StreamReader file = new StreamReader(path))
        {
            while ((line = file.ReadLine()) != null) 
            {
                if (line.Contains("TILT=NONE"))
                {
                    line = file.ReadLine();
                    exactlyLine = line.Split(' ');
                    newItem.GetComponent<IES_Prefab>().SaveLumen(float.Parse(exactlyLine[1]));

                    break;
                }
            }
        }

        IESChanger.GetComponent<IES_Changer>().SetEmptyLightData(newItem); // 추가
    }
}
