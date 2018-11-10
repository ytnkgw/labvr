using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sandbox.DjKit
{
	public class DebugView : MonoBehaviour
	{
		[SerializeField]
		private DjKit _djKit;

		[SerializeField]
		private Button _playButton;

		[SerializeField]
		private Button _pauseButton;


		private void Start()
		{
			_playButton.onClick.AddListener(() => {
				_djKit.Play();
			});

			_pauseButton.onClick.AddListener(() => {
				_djKit.Pause();
			});
		}
	}
}