using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public UserInfo getAppData() {
        if (PlayerPrefs.HasKey("appData"))
            return JsonUtility.FromJson<UserInfo>(PlayerPrefs.GetString("appData"));
        return null;
    }
    public void saveAppData(UserInfo appData) {
        string dataInJson = JsonUtility.ToJson(appData);
        PlayerPrefs.SetString("appData", dataInJson);
    }
}
