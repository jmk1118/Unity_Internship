using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// by MK, 씬 전환 버튼용 클래스
/// </summary>
public class MenuButton : MonoBehaviour
{
    [SerializeField] GameObject car; // 차량 오브젝트
    Vector3 carPosition = new Vector3(-230, -126.2f, 34); // 산길 배경 씬 전용 차량 시작 포지션

    private void Start()
    {
        // 산길 배경 씬을 실행할 때 차량 포지션 재설정
        if(SceneManager.GetActiveScene().name != "GameStart" && SceneManager.GetActiveScene().name != "_UrbanNight" && SceneManager.GetActiveScene().name != "_UrbanDay")
        {
            car.transform.position = carPosition;
            car.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // 도시 낮 씬
    public void UrbanDay()
    {
        SceneManager.LoadScene("_UrbanDay");
    }

    // 도시 밤 씬
    public void UrbanNight()
    {
        SceneManager.LoadScene("_UrbanNight");
    }

    // 산길 여름 낮 씬
    public void SummerDay()
    {
        SceneManager.LoadScene("_SummerDay");
    }

    // 산길 여름 밤 씬
    public void SummerNight()
    {
        SceneManager.LoadScene("_SummerNight");
    }

    // 산길 겨울 낮 씬
    public void WinterDay()
    {
        SceneManager.LoadScene("_WinterDay");
    }

    // 산길 겨울 밤 씬
    public void WinterNight()
    {
        SceneManager.LoadScene("_WinterNight");
    }

    // 시작화면 씬
    public void StartScene()
    {
        SceneManager.LoadScene("GameStart");
    }
}
