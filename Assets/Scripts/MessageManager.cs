using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

    public GameObject messagePanel;
    public Text messageText;
    public void showMessage(string message) {
        messageText.text = message;
        messagePanel.SetActive(true);
    }
}
