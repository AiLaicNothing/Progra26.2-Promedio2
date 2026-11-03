using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class InsertScore_Controller : MonoBehaviour
{
    private const string url = "http://localhost/Progra26-2-Promedio2/InsertScore.php";

    public void SendRequest(string nickname, int score,
        bool hasWon, int level, Action<bool> callback)
    {
        StartCoroutine(Send(nickname, score, hasWon, level, callback));
    }

    private IEnumerator Send(string nickname, int score, bool hasWon, int level, Action<bool> callback)
    {
        WWWForm form = new WWWForm();

        form.AddField("nickname", nickname);
        form.AddField("score", score);
        form.AddField("hasWon", hasWon ? 1 : 0);
        form.AddField("level", level);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            bool success = www.result == UnityWebRequest.Result.Success;

            if (success)
            {
                Debug.Log("Se insertó con éxito");
            }
            else
            {
                Debug.LogError( "Error: " + www.error + " " + www.downloadHandler.text);
            }

            callback?.Invoke(success);
        }
    }
}
