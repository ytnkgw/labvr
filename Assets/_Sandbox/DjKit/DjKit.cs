using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sandbox.DjKit
{
	public class DjKit : MonoBehaviour
	{
		[SerializeField]
		private AudioSource _audioSource;

		private List<System.IDisposable> _streams = new List<System.IDisposable>();


		#region MonoBehaviour functions
		private void Awake()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 90;
		}

		private void OnEnable()
		{
			
		}

		private void OnDisable()
		{
			
		}

		private void Update()
		{
			// TEST
			// TODO:C : 本番はUniRxで管理してUpdate外で管理します
			if (Input.GetKeyUp(KeyCode.P))
			{
				if (_audioSource.isPlaying)
				{
					Pause();
				}
				else
				{
					Play();
				}
			}
		}
		#endregion // MonoBehaviour functions


		public void Play()
		{
			_audioSource.Play();
		}

		public void Pause()
		{
			_audioSource.Pause();
		}
	}
}