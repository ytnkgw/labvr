using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web;
using UnityEngine;
using UnityEngine.Networking;

namespace LABVR.FanburstIntegration
{

    public class FanburstTest : MonoBehaviour
    {
		private readonly string BASE_URL = "https://api.fanburst.com";
		private readonly string AUTH_URL = 
			"https://fanburst.com/oauth/authorize?" +
			"client_id={0}&" +
			"redirect_uri={1}&" +
			"response_type=code";
		private readonly string ACCESS_TOKEN_BASE_URL = "https://fanburst.com/oauth/token?" +
			"client_id={0}&" +
			"client_secret={1}&" +
			"grant_type=code&" +
			"code={2}&" +
			"redirect_uri={3}";

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
				StartCoroutine(Auth());
			}
		}

		private void OnApplicationQuit()
		{
			if (authListener != null) authListener.Abort();
			if (authThread != null) authThread.Abort();
		}

		/*
		 * PROBLEM ::
		 * In current fanburst circumstance, you have to log in before call auth.
		 * fanburst doesn't jump to callback url after login process.
		 */
		private IEnumerator Auth()
		{
			// Build url
			string redirectURI = m_RedirectURL + ":" + m_ListenPort + "/";

			Debug.Log("URL : " + WWW.EscapeURL(redirectURI));
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
					System.Uri uri = context.Request.Url;
					var query = uri.Query;
					//query.key

					Debug.Log("auth code : " + context.Request.Url);
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

			yield return null;
		}

		private IEnumerator GetAccessToken(HttpListener context)
		{
			string redirectURI = WWW.EscapeURL(m_RedirectURL + ":" + m_ListenPort + "/");
			string code = "";
			string url = string.Format(ACCESS_TOKEN_BASE_URL, m_ClientId, m_ClientSecret, code, redirectURI);

			var request = new UnityWebRequest();
			request.url = url;
			request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
			request.SetRequestHeader("Accept-Version", "v1");
			request.method = UnityWebRequest.kHttpVerbPOST;

			yield return request.SendWebRequest();

			if (request.isNetworkError)
			{
				// Network error handling
			}
			else if (request.isHttpError)
			{
				// Http error handling
			}
			else
			{
				if (request.responseCode == 200)
				{
					Debug.Log("Access Token : " + request.downloadHandler.text);
				}
			}

		}

    }

}