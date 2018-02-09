using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LABVR.WebView
{

	public class OpenBrowser : MonoBehaviour
	{

		[SerializeField]
		private string url = "";

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Space))
			{
				Application.OpenURL(url);
			}
		}

	}

}
