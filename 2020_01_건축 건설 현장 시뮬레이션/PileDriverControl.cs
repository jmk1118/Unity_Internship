using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;

/// <summary>
/// by MK, 항타기를 기준으로 씬의 오브젝트들을 관리하며, 초기화를 담당하는 클래스
/// </summary>
public class PileDriverControl : MonoBehaviour
{
    Animator animator; // 애니메이터
    Quaternion bodyStartRotation; // 항타기 몸체 회전 초기값
    Vector3 augerStartPosition; // 오거 위치 초기값
    Vector3 casingLidStartPosition; // 케이싱리드 위치 초기값
    Vector3 casingStartPosition; // 케이싱 위치 초기값
    Vector3 pileStartPosition; // 말뚝 위치 초기값
    Quaternion pileStartRotation; // 말뚝 회전 초기값
    Vector3 wireStartPosition; // 와이어 위치 초기값
    Vector3 hammerStartPosition; // 망치 위치 초기값
    Vector3 manMasterPosition; // 감독관 위치 초기값
    Vector3 manSubPosition; // 관리자 위치 초기값
    Quaternion manSubRotation; // 관리자 회전 초기값

    //입력받는 변수
    /*
    [SerializeField] private InputField moveDistance; //이동 거리 변수 입력창
    [SerializeField] private InputField boringDepth; //천공 깊이 변수 입력창
    [SerializeField] private InputField pileLength; //말뚝 길이 변수 입력창
    */
    [SerializeField] private GameObject startScene; // 시작 화면 UI
    [SerializeField] private VariationMenu variation; // 변수를 입력받는 칸
    private int moveDistanceValue; //이동 거리 변수
    private int boringDepthValue; //천공 깊이 변수
    private int pileLengthValue; //말뚝 길이 변수

    //표시용 UI
    [SerializeField] private Text nowSequence;
    [SerializeField] private Text runningTime;
    [SerializeField] private Text remainingTime;
    private float startTime = 0;
    private int nowTime = 0;
    private int lastTime;
    [SerializeField] private GameObject subPanel;

    //항타기 움직임용
    [SerializeField] private GameObject rail;
    Vector3 railPosition = new Vector3(5.65f, 0, 0);
    bool railGo = false;
    [SerializeField] private GameObject body; //바퀴 위 차체 오브젝트
    [SerializeField] private GameObject wire; //와이어 오브젝트를 애니메이터에서 가져가기 위해 선언
    [SerializeField] private GameObject pile; //말뚝 오브젝트를 애니메이터에서 가져가기 위해 선언
    [SerializeField] private GameObject auger; //오거(드릴 윗부분)
    [SerializeField] private GameObject casingLid; //케이싱 윗부분
    [SerializeField] private GameObject casing; //케이싱
    [SerializeField] private GameObject hammer; //망치
    [SerializeField] private GameObject dirt; //흙먼지(바닥) 이펙트
    [SerializeField] private GameObject steam;
    private const float augerRotateSpeed = 0.8f; //오거 및 케이싱 회전 속도

    //기타 오브젝트 관리용
    [SerializeField] private GameObject lorry; //흙 담는 트럭
    [SerializeField] private TimelineAsset lorryDriveRIght; //오른쪽으로 주행하는 애니메이션
    [SerializeField] private TimelineAsset lorryDriveLeft; //왼쪽으로 주행하는 애니메이션
    [SerializeField] private GameObject backHoe1;
    [SerializeField] private TimelineAsset backHoe1Move;
    [SerializeField] private TimelineAsset backHoe1Vertical;
    [SerializeField] private TimelineAsset backHoe1AfterPile;
    [SerializeField] private GameObject manMaster; //작업반장
    [SerializeField] private GameObject manSub; //비계공

    /// <summary>
    /// 씬이 시작할 때 오브젝트들의 위치를 초기값으로 설정
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();

        bodyStartRotation = body.transform.rotation;
        augerStartPosition = auger.transform.position;
        casingLidStartPosition = casingLid.transform.position;
        casingStartPosition = casing.transform.position;
        pileStartPosition = pile.transform.position;
        pileStartRotation = pile.transform.rotation;
        wireStartPosition = wire.transform.position;
        hammerStartPosition = hammer.transform.position;

        manMasterPosition = manMaster.transform.position;
        manSubPosition = manSub.transform.position;
        manSubRotation = manSub.transform.rotation;

        Rail().GetComponent<Animator>().speed = 0;
    }

    /// <summary>
    /// 실행 시간 표시 업데이트
    /// </summary>
    private void Update()
    {
        nowTime = (int)(Time.time - startTime);
        runningTime.text = (nowTime / 60).ToString() + "분 " + (nowTime % 60).ToString() + "초";
        if (lastTime - nowTime >= 0) 
            remainingTime.text = ((lastTime - nowTime) / 60).ToString() + "분 " + ((lastTime - nowTime) % 60).ToString() + "초";
    }

    public void RailGo(bool go)
    {
        railGo = go;
    }
    private void LateUpdate()
    {
        if(railGo)
            rail.transform.position = transform.position + railPosition;
    }

    /// <summary>
    /// 시물레이션 시작 버튼을 누르면 호출되는 함수.
    /// 입력된 이동거리, 천공깊이, 말뚝길이를 int형으로 저장한다.
    /// 입력된 이동거리에 맞게 항타기 시작 위치를 조정해 그 곳에 천공기를 위치시킨다.
    /// 캔버스를 비활성화한다.
    /// 애니메이터에서 이동 과정을 실행한다.
    /// </summary>
    public void StartGame()
    {
        moveDistanceValue = variation.moveDistance;
        boringDepthValue = variation.boringDepth;
        pileLengthValue = variation.pileLength;

        if (boringDepthValue >= pileLengthValue) //천공 깊이가 말뚝보다 깊으면 진행되지 않는다.
            return;

        startTime = Time.time;
        lastTime = 197 + 23 * moveDistanceValue + 131 * boringDepthValue + (20 + 4 * (2 * pileLengthValue + 5)) + 10 + (int)(pileLengthValue / 0.7f) + (int)(4.8f * (pileLengthValue + 4));
        subPanel.transform.GetChild(23).GetComponent<Text>().text = ((23 * moveDistanceValue) / 60).ToString() + "' " + ((23 * moveDistanceValue) % 60).ToString() + "\"";
        subPanel.transform.GetChild(17).GetComponent<Text>().text = ((131 * boringDepthValue) / 60).ToString() + "' " + ((131 * boringDepthValue) % 60).ToString() + "\"";
        subPanel.transform.GetChild(15).GetComponent<Text>().text = ((int)(4.8f * (pileLengthValue + 4) ) / 60).ToString() + "' " + ((int)(4.8f * (pileLengthValue + 4)) % 60).ToString() + "\"";
        subPanel.transform.GetChild(13).GetComponent<Text>().text = ("0 \' "+ (int)(17 + pileLengthValue / 0.7f)).ToString() + "\"";
        subPanel.transform.GetChild(9).GetComponent<Text>().text =  ((20 + 4 * (2 * pileLengthValue + 5)) / 60).ToString() + "' " + ((20 + 4 * (2 * pileLengthValue + 5)) % 60).ToString() + "\"";

        transform.position = new Vector3(moveDistanceValue, transform.position.y, transform.position.z);

        startScene.transform.parent.GetChild(1).gameObject.SetActive(true); //판넬 활성화
        startScene.gameObject.SetActive(false);

        pile.GetComponent<Pile_Control>().NewPile(PileLength());
        animator.SetBool("StartMove", true);

        ManSub().GetComponent<ManSubControl>().MoveStart(); //비계공 이동 시작

        
    }

    /// <summary>
    /// 시뮬레이션을 종료할 때 호출하는 함수.
    /// 씬의 오브젝트들을 초기값으로 돌려놓는다.
    /// </summary>
    public void FinishGame()
    {
        startScene.transform.parent.GetChild(1).gameObject.SetActive(false); //판넬 비활성화
        startScene.transform.gameObject.SetActive(true); //시작화면 활성화

        transform.position = Vector3.zero;
        body.transform.rotation = bodyStartRotation;
        auger.transform.position = augerStartPosition;
        casingLid.transform.position = casingLidStartPosition;
        casing.transform.position = casingStartPosition;
        casing.transform.parent = casingLid.transform;
        pile.transform.position = pileStartPosition;
        pile.transform.rotation = pileStartRotation;
        pile.transform.GetChild(1).transform.Rotate(pile.transform.GetChild(1).transform.rotation.eulerAngles * -1);
        wire.SetActive(true);
        wire.transform.position = wireStartPosition;
        wire.transform.LookAt(pile.transform.GetChild(1).transform);
        wire.transform.GetChild(9).transform.position = pile.transform.GetChild(2).transform.position;
        wire.transform.GetChild(9).GetComponent<HingeJoint>().connectedBody = pile.transform.GetChild(1).GetComponent<Rigidbody>();
        hammer.transform.position = hammerStartPosition;
        dirt.SetActive(false);

        manMaster.GetComponent<ManMasterControl>().Reset();
        manMaster.transform.position = manMasterPosition;

        manSub.GetComponent<ManSubControl>().Reset();
        manSub.transform.position = manSubPosition;
        manSub.transform.rotation = manSubRotation;

        startScene.transform.parent.GetComponent<Camera_ButtonChange>().CameraChangeC1();
        Rail().GetComponent<Animator>().speed = 0;

        animator.Rebind();
    }


    // 이동 입력값을 반환한다.
    public int MoveDistance()
    {
        return moveDistanceValue;
    }

    // 천공 깊이 입력값을 반환한다.
    public int BoringDepth()
    {
        return boringDepthValue;
    }

    // 말뚝 길이 입력값을 반환한다.
    public int PileLength()
    {
        return pileLengthValue;
    }

    /// <summary>
    /// 현재 진행 단계를 표시하기 위해 진행 단계 변경 시 현재 단계를 가져오는 함수
    /// </summary>
    /// <param name="now">현재 단계를 입력하는 문자열</param>
    public void NowSequence(string now)
    {
        nowSequence.text = now;

        return;
    }

    // 서브판넬 오브젝트를 반환하는 함수
    public GameObject SubPanel()
    {
        return subPanel;
    }

    // 레일 오브젝트를 반환하는 함수
    public GameObject Rail()
    {
        return rail;
    }

    // 차체 오브젝트를 반환하는 함수
    public GameObject Body() 
    {
        return body;
    }

    // 와이어 오브젝트를 반환하는 함수
    public GameObject Wire()
    {
        return wire;
    }

    // 말뚝 오브젝트를 반환하는 함수
    public GameObject Pile()
    {
        return pile;
    }

    public GameObject Auger() //오거(드릴 윗부분) 오브젝트를 반환한다.
    {
        return auger;
    }

    public GameObject CasingLid() //케이싱리드(케이싱 윗부분) 오브젝트를 반환한다.
    {
        return casingLid;
    }

    public GameObject Casing() //케이싱 오브젝트를 반환한다.
    {
        return casing;
    }

    public GameObject Hammer() //망치 오브젝트를 반환한다.
    {
        return hammer;
    }

    public void DirtOn() //흙먼지 이펙트를 활성화한다.
    {
        dirt.SetActive(true);
    }

    public void DirtOff() //흙먼지 이펙트를 비활성화한다.
    {
        dirt.SetActive(false);
    }

    public void SteamON() //매연 이펙트를 활성화한다.
    {
        steam.GetComponent<ParticleSystem>().Play();
    }

    public float AugerRotateSpeed() //드릴이 회전하는 기준속도를 반환한다.
    {
        return augerRotateSpeed;
    }

    public GameObject Lorry() //흙 담는 트럭 오브젝트를 반환한다.
    {
        return lorry;
    }

    public TimelineAsset LorryDriveRight() //흙 담는 트럭이 오른쪽으로 지나가는 애니메이션(타임라인)을 반환한다.
    {
        return lorryDriveRIght;
    }

    public TimelineAsset LorryDriveLeft() //흙 담는 트럭이 왼쪽으로 지나가는 애니메이션(타임라인)을 반환한다.
    {
        return lorryDriveLeft;
    }

    // 백호(굴착기) 오브젝트를 반환하는 함수
    public GameObject BackHoe1()
    {
        return backHoe1;
    }

    public TimelineAsset BackHoe1Move()
    {
        return backHoe1Move;
    }

    public TimelineAsset BackHoe1Vertical()
    {
        return backHoe1Vertical;
    }

    public TimelineAsset BackHoe1AfterPile()
    {
        return backHoe1AfterPile;
    }
    
    // 작업반장 오브젝트를 반환하는 함수
    public GameObject ManMaster()
    {
        return manMaster;
    }

    // 비계공 오브젝트를 반환하는 함수
    public GameObject ManSub()
    {
        return manSub;
    }
}
