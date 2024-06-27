using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ExcelinUnity : MonoBehaviour
{



    public TextAsset csvFile; // CSV 파일을 유니티에서 로드하기 위한 변수

    void Start()
    {
        string[] data = csvFile.text.Split(new char[] { '\n' }); // 줄 단위로 데이터 분할

        foreach (string line in data)
        {
            string[] row = line.Split(new char[] { ',' }); // 쉼표 단위로 데이터 분할

            // 예시: 첫 번째 열과 두 번째 열 데이터 출력
            Debug.Log("Column 1: " + row[0] + ", Column 2: " + row[1]);
        }
    }
}
// Start is called before the first frame update


    // Update is called once per frame
   

