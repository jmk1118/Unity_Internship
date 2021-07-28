using UnityEngine;

/// <summary>
/// by MK, |ES 파일에서 나온 데이터를 가지고 있는 클래스. IES 라이브러리를 구성한다.
/// </summary>
public class IES_Prefab : MonoBehaviour 
{
    Texture2D texture; // spot light에 사용하는 텍스쳐
    Cubemap cubemap; // point light에 사용하는 큐브맵
    float Lumen; // 루멘값
    [SerializeField] string IESname; // ies 파일명

    /// <summary>
    /// IES파일에서 나온 텍스쳐를 저장하는 함수
    /// </summary>
    /// <param name="IESTexture"></param>
    public void SaveTexture(Texture2D IESTexture)
    {
        texture = IESTexture;
    }

    /// <summary>
    /// IES파일에서 나온 큐브맵을 저장하는 함수
    /// </summary>
    /// <param name="IESCubemap"></param>
    public void SaveCubemap(Cubemap IESCubemap)
    {
        cubemap = IESCubemap;
    }

    /// <summary>
    /// IES파일에서 나온 Intensity값을 저장하는 함수
    /// </summary>
    /// <param name="LumenData"></param>
    public void SaveLumen(float LumenData)
    {
        Lumen = LumenData;
    }

    /// <summary>
    /// IES파일의 이름을 저장하는 함수
    /// </summary>
    /// <param name="IESName"></param>
    public void SaveName(string IESName)
    {
        IESname = IESName;
    }

    /// <summary>
    /// 저장된 텍스쳐를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public Texture2D LoadTexture()
    {
        return texture;
    }

    /// <summary>
    /// 저장된 큐브맵을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public Cubemap LoadCubemap()
    {
        return cubemap;
    }

    /// <summary>
    /// 저장된 Intensity값을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float LoadLumen()
    {
        return Lumen;
    }

    /// <summary>
    /// 저장된 IES파일의 이름을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public string LoadName()
    {
        return IESname;
    }
}
