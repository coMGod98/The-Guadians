using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAOE : MonoBehaviour
{
    public Button button;
    public GameObject mouseFollowPrefab;
    private bool isFollowing = false;

    // Start is called before the first frame update
    void Start()
    {
        // 버튼 클릭 이벤트 설정
        button.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            FollowMouse();
        }
    }

    // 버튼 클릭 시 호출되는 메서드
    void OnButtonClick()
    {
        // isFollowing 플래그를 true로 설정하여 Update 메서드에서 마우스 위치를 따라가도록 함
        isFollowing = true;
        // mouseFollowPrefab 활성화 (비활성화 상태일 수 있으므로)
        mouseFollowPrefab.SetActive(true);
    }

    // 마우스를 따라가도록 하는 메서드
    void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane; // 카메라의 근평면 거리 설정

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseFollowPrefab.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0); // Z 값을 0으로 설정
    }
}
