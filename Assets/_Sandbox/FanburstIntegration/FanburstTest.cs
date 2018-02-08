using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace LABVR.FanburstIntegration
{

    public class FanburstTest : MonoBehaviour
    {
		private readonly string BASE_URL = "https://api.fanburst.com";

		private string m_AccessToken = "b25531438172b74d74841e6676f45cb54a711a43b544dcb1098aaf53589c389a";

		[SerializeField]
        private string m_ClientId = "";
        [SerializeField]
        private string m_ClientSecret = "";


		private IEnumerator ExecuteAPI ()
        {
            UnityWebRequest request = UnityWebRequest.Get("https://example.com");

            yield return null;
        }

    }

}