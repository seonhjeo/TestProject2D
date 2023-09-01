using System.Collections.Generic;
using System.IO;
using MemoryPack;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Test : MonoBehaviour
{
    private Dictionary<int, string> _dic;
    private SaveData _res;

    [SerializeField] private TMP_Text text1;
    [SerializeField] private TMP_Text text2;
    
    public void Start()
    {
        // 시리얼라이즈 초기 설정은 필요 없음
        // 시리얼라이즈한 것을 디시리얼라이즈 해본다
        _dic = new Dictionary<int, string>();
        _dic.Add(1, "Hello");
        _dic.Add(2, "World");
        
        var data = new SaveData(1, "MemoryPack Test String", _dic, new Vector3(1.5f, 3.7f, -220.4f));
        
        // 직렬화 함수
        var serialized = MemoryPackSerializer.Serialize(data);
        // 역직렬화 함수
        var deserialized = MemoryPackSerializer.Deserialize<SaveData>(serialized);

        Debug.Log($"Id={deserialized.Id}, Message={deserialized.Message}");
        
        // 직렬화된 데이터 저장 및 불러오기
        CreateOrSaveJsonFile(Application.persistentDataPath, "SaveData", serialized);
        _res = LoadJsonFile<SaveData>(Application.persistentDataPath, "SaveData");
        
        Debug.Log($"Id={_res.Id}, Message={_res.Message}");
        Debug.Log($"Value1={_res.KeyValue[1]}, Value2={_res.KeyValue[2]}");
        Debug.Log(_res.VectorData.ToString());

        text1.text = $"Id={_res.Id}, Message={_res.Message}";
        text2.text = $"Value1={_res.KeyValue[1]}, Value2={_res.KeyValue[2]}";
    }

    // 파일 저장 함수
    public void CreateOrSaveJsonFile(string createPath, string fileName, byte[] data)
    {
        string file = string.Format("{0}/{1}.json", createPath, fileName);
            
        if (File.Exists(file))
        {
            File.Delete(file);
        }
            
        FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
    
    // 파일 불러오기 함수
    public T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open, FileAccess.Read);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        var deserialized = MemoryPackSerializer.Deserialize<T>(data);
        return deserialized;
    }
}
