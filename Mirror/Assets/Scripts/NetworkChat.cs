using UnityEngine;
using Mirror;
using TMPro;
using System.Globalization;

public class NetworkChat : NetworkBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField chatInputField;
    public GameObject messagePrefab;
    public Transform messageContainer;

    private void Start()
    {
        if (!isLocalPlayer) return;

        // Only the local player should do this
        if (chatInputField == null)
            chatInputField = FindObjectOfType<TMP_InputField>();

        if (messageContainer == null)
            messageContainer = GameObject.Find("MessageContainer")?.transform;

        if (messagePrefab == null)
            messagePrefab = Resources.Load<GameObject>("MessagePrefab");
    }

    // This method is automatically called when the "Submit" action is triggered
    public void OnSubmit()
    {
        if (!isLocalPlayer) return;

        if (chatInputField.isFocused && !string.IsNullOrWhiteSpace(chatInputField.text))
        {
            SendMessageToServer(chatInputField.text);
            chatInputField.text = "";
            chatInputField.ActivateInputField();
        }
    }

    void SendMessageToServer(string message)
    {
        CmdSendMessage($"{GetPlayerName()}: {message}");
    }

    string GetPlayerName()
    {
        return $"Player {netId}";
    }

    [Command]
    void CmdSendMessage(string message)
    {
        RpcReceiveMessage(message);
    }

    [ClientRpc]
    void RpcReceiveMessage(string message)
    {
        GameObject msgObj = Instantiate(messagePrefab, messageContainer);
        msgObj.GetComponent<TMP_Text>().text = message;
    }
}
