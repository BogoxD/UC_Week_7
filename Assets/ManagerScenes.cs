using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ManagerScenes : MonoBehaviour
{
    public TextMeshProUGUI Username_Field;
    public static ManagerScenes instance;
    public SaveDataClass dataClass;
    private void Awake()
    {
        if (instance)
            Destroy(gameObject);

        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    public void OnPlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnUsernameWrote()
    {
        dataClass.userName = Username_Field.text;
        dataClass.SaveData();
    }
    public void SetHighScore()
    {
        dataClass.highScore = FindObjectOfType<MainManager>().GetHighScore();
    }
    private void OnApplicationQuit()
    {
        if (Username_Field)
            dataClass.userName = Username_Field.text;
        dataClass.SaveData();
    }
    private void OnLevelWasLoaded(int level)
    {
        dataClass.LoadData();
        GameObject userText = GameObject.Find("Username");
        GameObject highScoreText = GameObject.Find("HighScore");

        if (dataClass.HasUserName())
        {
            userText.GetComponent<Text>().text = dataClass.userName;
            highScoreText.GetComponent<Text>().text += dataClass.highScore;
        }
    }
}
[Serializable]
public class SaveDataClass
{
    public string userName;
    public int highScore;

    public void SaveData()
    {
        SaveDataClass data = new SaveDataClass();
        data.userName = userName;
        data.highScore = highScore;

        string JSON = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", JSON);

    }
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string JSON = File.ReadAllText(path);

            SaveDataClass data = JsonUtility.FromJson<SaveDataClass>(JSON);

            userName = data.userName;
            highScore = data.highScore;
        }
    }
    public bool HasUserName()
    {
        if (userName.Length > 0)
            return true;
        else
            return false;
    }
}
