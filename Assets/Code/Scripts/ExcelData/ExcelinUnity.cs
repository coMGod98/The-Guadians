using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExcelinUnity : MonoBehaviour
{



    public TextAsset csvFile;
    public static string[,] data;

    void Start()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV file is not assigned.");
            return;
        }
        string[] lines = csvFile.text.Split(new char[] { '\n' }); // 줄 단위로 데이터 분할
        data = new string[lines.Length, 2]; // 2열로 고정된 배열

        for (int i = 0; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(new char[] { ',' }); // 쉼표 단위로 데이터 분할
                if (row.Length >= 2)
                {
                    data[i, 0] = row[0].Trim(); // 첫 번째 열 데이터 저장
                    data[i, 1] = row[1].Trim(); // 두 번째 열 데이터 저장
                }
                else
                {
                    Debug.LogWarning("Row does not contain enough columns: " + lines[i]);
                }
               
            }
            
        }
    }
}
// Start is called before the first frame update


    // Update is called once per frame
   

