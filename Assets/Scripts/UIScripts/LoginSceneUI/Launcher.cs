using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;


public class Launcher : MonoBehaviour
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField id;
    [SerializeField] TMP_InputField pw;
    [SerializeField] TMP_InputField cid;
    [SerializeField] TMP_InputField cpw;
    [SerializeField] TMP_Text errorText;
    [SerializeField] GameObject startGameButton;
    

    string loginUri = "http://127.0.0.1/login.php";
    string SetDataUri = "http://127.0.0.1/SetData.php";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public void OnClickSearch()
    {
        if (string.IsNullOrEmpty(id.text) || string.IsNullOrEmpty(pw.text))
        {
             return;
        }

        else StartCoroutine(LoginCoroutine(id.text, pw.text));
    }

    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(cid.text) || string.IsNullOrEmpty(cpw.text))
        {
            return;
        }

        else StartCoroutine(SetDataCoroutine(cid.text, cpw.text));
    }

    private IEnumerator LoginCoroutine(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", username);
        form.AddField("pw", password);


        using (UnityWebRequest www = UnityWebRequest.Post(loginUri, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if ("Wrong password.." == www.downloadHandler.text)
                {
                    errorText.text = "Wrong Password";
                    MenuManager.Instance.OpenMenu("error");
                }
                if ("ID not found.." == www.downloadHandler.text)
                {
                    errorText.text = "ID Not Found";
                    MenuManager.Instance.OpenMenu("error");
                }
                if ("Login success!!" == www.downloadHandler.text)
                {
                    MenuManager.Instance.OpenMenu("success");
                }
            }
        }
    }

    private IEnumerator SetDataCoroutine(string _id, string _pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", _id);
        form.AddField("pw", _pass);

        using (UnityWebRequest www = UnityWebRequest.Post(SetDataUri, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }

            else
            {
                if ("Success" == www.downloadHandler.text)
                {
                    errorText.text = "Create Success";
                    MenuManager.Instance.OpenMenu("error");
                }

                if ("already id exist" == www.downloadHandler.text)
                {
                    errorText.text = "Already ID Exist";
                    MenuManager.Instance.OpenMenu("error");
                }
            }
        }
    }

}