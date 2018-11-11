using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sandbox.DjKit
{
	public class DjKit : MonoBehaviour
	{
		[SerializeField]
		private AudioSource _audioSource;


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

		// TODO : イコライザー
		// TODO : 波形 : http://tips.hecomi.com/entry/2014/11/11/021147

	}
}