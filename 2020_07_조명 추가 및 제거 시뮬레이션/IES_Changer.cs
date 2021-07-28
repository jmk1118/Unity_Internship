using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// by MK, 조명 오브젝트의 정보를 IES파일을 토대로 바꾸는 클래스
/// </summary>
public class IES_Changer : MonoBehaviour 
{
    [SerializeField] GameObject SpotList; // spot 라이트 오브젝트 리스트
    [SerializeField] GameObject PointList; // point 라이트 오브젝트 리스트

    GameObject SatLighting; // Import한 Sat파일 중 Lighting 오브젝트들의 root 

    // 초기화
    private void Start()
    {
        SatLighting = null;
    }

    /// <summary>
    /// IES 라이브러리를 탐색하여 입력받은 오브젝트에 적합한 IES가 있으면 해당 IES로 조명을 바꾼다. 없으면 false를 반환한다.
    /// </summary>
    /// <param name="gameObject">조명을 자식으로 가지고 있는 조명 SAT 오브젝트</param>
    /// <param name="IESName">조명 SAT 오브젝트에 적합한 IES 파일의 이름</param>
    /// <returns></returns>
    public bool SetLightData(GameObject gameObject, string IESName)
    {
        for (int i = 1; i < SpotList.transform.childCount; i++)
        {
            if(SpotList.transform.GetChild(i).GetComponent<IES_Prefab>().LoadName() == IESName)
            {
                Spot(gameObject.transform.GetChild(0).gameObject, SpotList.transform.GetChild(i).gameObject);
                return true;
            }
        }
        for (int i = 1; i < PointList.transform.childCount; i++)
        {
            if(PointList.transform.GetChild(i).GetComponent<IES_Prefab>().LoadName() == IESName)
            {
                Point(gameObject.transform.GetChild(0).gameObject, PointList.transform.GetChild(i).gameObject);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// IES 파일을 새로 import 했을 때, 존재하는 모든 조명 SAT 오브젝트를 탐색하여 IES 파일과 적합한 조명 SAT 오브젝트에 적용하는 함수
    /// </summary>
    /// <param name="IESPrefab">새로 import한 IES가 적용된 IES Prefab</param>
    public void SetEmptyLightData(GameObject IESPrefab) //추가
    {
        if (SatLighting == null)
            return;

        for (int i = 0; i < SatLighting.transform.childCount; i++) //모든 조명 SAT 오브젝트를 탐색
        {
            // csv파일로 읽은 데이터에 기록된 ies파일과 동일하다면 실행
            if (SatLighting.transform.GetChild(i).GetComponent<CSV_Data>().WhatEquipType() == IESPrefab.GetComponent<IES_Prefab>().LoadName())
            {
                // spot light라면 집어넣는다
                if (IESPrefab.GetComponent<IES_Prefab>().LoadTexture() != null)
                {
                    Spot(SatLighting.transform.GetChild(i).GetChild(0).gameObject, IESPrefab);
                    continue;
                }
                // point light라면 집어넣는다
                else if (IESPrefab.GetComponent<IES_Prefab>().LoadCubemap() != null) 
                {
                    Point(SatLighting.transform.GetChild(i).GetChild(0).gameObject, IESPrefab);
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// 조명 컴포넌트를 point light로 바꾸고, Intensity값을 바꾸는 함수
    /// </summary>
    /// <param name="gameObject">조명 오브젝트</param>
    /// <param name="IESPrefab">조명 오브젝트에 적용할 IES를 가진 IES Prefab</param>
    void Point(GameObject gameObject, GameObject IESPrefab)
    {
        gameObject.GetComponent<Light>().type = LightType.Point;
        gameObject.GetComponent<Light>().cookie = IESPrefab.GetComponent<IES_Prefab>().LoadCubemap();
        gameObject.GetComponent<HDAdditionalLightData>().SetLightUnit(LightUnit.Lumen);
        gameObject.GetComponent<HDAdditionalLightData>().SetIntensity(IESPrefab.GetComponent<IES_Prefab>().LoadLumen());
    }

    /// <summary>
    /// 조명 컴포넌트를 spot light로 바꾸고, Intensity값을 바꾸는 함수
    /// </summary>
    /// <param name="gameObject">조명 오브젝트</param>
    /// <param name="IESPrefab">조명 오브젝트에 적용할 IES를 가진 IES Prefab</param>
    void Spot(GameObject gameObject, GameObject IESPrefab)
    {
        gameObject.GetComponent<Light>().type = LightType.Spot;
        gameObject.GetComponent<Light>().cookie = IESPrefab.GetComponent<IES_Prefab>().LoadTexture();
        gameObject.GetComponent<HDAdditionalLightData>().SetLightUnit(LightUnit.Lumen);
        gameObject.GetComponent<HDAdditionalLightData>().SetIntensity(IESPrefab.GetComponent<IES_Prefab>().LoadLumen());
    }

    /// <summary>
    /// 조명 SAT 오브젝트들의 root인 오브젝트를 입력시키는 함수
    /// </summary>
    /// <param name="SatLight">조명 SAT 오브젝트들의 root 오브젝트</param>
    public void SetSatLighting(GameObject SatLight)
    {
        SatLighting = SatLight;
    }
}
