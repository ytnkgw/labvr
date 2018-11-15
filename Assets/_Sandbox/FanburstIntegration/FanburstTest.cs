using MyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
//using System.Web;
using UnityEngine;
using UnityEngine.Networking;

namespace LABVR.FanburstIntegration
{

    public class FanburstTest : MonoBehaviour
    {
		//private readonly string BASE_URL = "https://api.fanburst.com";
		private readonly string AUTH_URL = 
			"https://fanburst.com/oauth/authorize?" +
			"client_id={0}&" +
			"redirect_uri={1}&" +
			"response_type=code";
		private readonly string ACCESS_TOKEN_BASE_URL = 
			"https://fanburst.com/oauth/token?" +
			"client_id={0}&" +
			"client_secret={1}&" +
			"grant_type=authorization_code&" +
			"code={2}&" +
			"redirect_uri={3}";
		private readonly string ACCESS_TOKEN = "564f7b17ebe9f2eaa902f42e20d0708ff0602cfd532afdd9e54011be2b638fb9";

		private readonly string ME_URL = 
			"https://api.fanburst.com/me/?" +
			"access_token={0}&" +
			"client_id={1}";

		[SerializeField]
        private string m_ClientId = "";
        [SerializeField]
        private string m_ClientSecret = "";
		[SerializeField]
		private string m_RedirectURL = "";
		[SerializeField]
		private int m_ListenPort = 8080;

		private HttpListener authListener = null;
		private Thread authThread = null;


		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Space))
			{
				//StartCoroutine(GetAccessToken(authCode));
				StartCoroutine(Auth());
			}
		}

		private void OnApplicationQuit()
		{
			if (authListener != null) authListener.Abort();
			if (authThread != null) authThread.Abort();
		}

		private IEnumerator Me(Action<string> callback)
		{
			var request = new UnityWebRequest();
			request.url = string.Format(ME_URL, ACCESS_TOKEN, m_ClientId);
			request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
			request.SetRequestHeader("Accept-Version", "v1");
			request.method = UnityWebRequest.kHttpVerbGET;
			request.downloadHandler = new DownloadHandlerBuffer();

			yield return request.SendWebRequest();
		}

		/*
		 * PROBLEM ::
		 * In current fanburst circumstance, you have to log in before call auth.
		 * fanburst doesn't jump to callback url after login process.
		 */
		private IEnumerator Auth()
		{
			bool hasAuthCode = false;
			string authCode = string.Empty;

			// Build url
			string redirectURI = m_RedirectURL + ":" + m_ListenPort + "/";
			string authURL = string.Format(AUTH_URL, m_ClientId, WWW.EscapeURL(redirectURI));

			// Create listener
			authListener = new HttpListener();
			authListener.Prefixes.Add(redirectURI);
			authListener.Start();

			// Start thread for listener
			authThread = new Thread(
				() =>
				{
					HttpListenerContext context = authListener.GetContext();
					Dictionary<string, string> query = Utils.ParseQueryString(context.Request.Url.ToString());
					authCode = query["code"];
					hasAuthCode = true;
					Debug.Log("Auth Code : " + authCode);

					// Kill listeners
					authListener.Abort();
					authListener = null;
					authThread.Abort();
					authThread = null;
				}
			);
			authThread.Start();

			// Open Auth Page
			// TODO :: Use webview
			Application.OpenURL(authURL);

			while (!hasAuthCode)
			{
				yield return 0;
			}

			StartCoroutine(GetAccessToken(authCode));
		}

		private IEnumerator GetAccessToken(string authCode)
		{
			string redirectURI = WWW.EscapeURL(m_RedirectURL + ":" + m_ListenPort + "/");
			string url = string.Format(ACCESS_TOKEN_BASE_URL, m_ClientId, m_ClientSecret, authCode, redirectURI);

			var request = new UnityWebRequest();
			request.url = url;
			request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
			request.SetRequestHeader("Accept-Version", "v1");
			request.method = UnityWebRequest.kHttpVerbPOST;
			request.downloadHandler = new DownloadHandlerBuffer();

			yield return request.SendWebRequest();

			// TODO :: Error handling
			if (request.isNetworkError)
			{
				// Network error handling
				Debug.LogWarning("[Fanburst Test] Network error : " + request.error.ToString());
			}
			else if (request.isHttpError)
			{
				// Http error handling
				Debug.LogWarning("[Fanburst Test] Http error : " + request.error.ToString());
			}
			else
			{
				if (request.responseCode == 200)
				{
					if (request.downloadHandler != null)
					{
						Debug.Log("Access Token : " + request.downloadHandler.text);
					}
					else
					{
						Debug.Log("Can't access to the result");
					}
				}
			}

			request.Abort();

		}

    }

}