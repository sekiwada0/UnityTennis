/*
https://blog.applibot.co.jp/2018/05/25/how-to-use-unity-videoplayer/
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
	// Use this for initialization
	void Start(){
//GigaTrax.Debug.Log( "ED:Start");
		Camera mainCamera = Camera.main;

		GameObject obj = this.gameObject;
		m_audioSource = obj.AddComponent<AudioSource>();
		m_videoPlayer = obj.AddComponent<VideoPlayer>();

		// 表示設定
		m_videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
		m_videoPlayer.targetCamera = mainCamera;
		m_videoPlayer.aspectRatio = VideoAspectRatio.FitInside;
		m_videoPlayer.playOnAwake = false;

		// サウンド設定
		//TODO: エディタだと音声が出力されない(Unity 2017.2.1f1)
		//	  PrepareCompleted後に設定すると2回目以降は再生される…
		m_audioSource.playOnAwake = false;
		m_videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		m_videoPlayer.controlledAudioTrackCount = 1;
		m_videoPlayer.EnableAudioTrack(0, true);
		m_videoPlayer.SetTargetAudioSource(0, m_audioSource);

		// イベント設定
		m_videoPlayer.errorReceived += OnErrorReceived;
		m_videoPlayer.prepareCompleted += OnPrepareCompleted;
		m_videoPlayer.started += OnPlayStart;
		m_videoPlayer.loopPointReached += OnPlayEnd;

		m_videoPlayer.enabled = false;
		//m_scene_time = 0;
	}
	public void Dispose()
	{
		// イベント設定解除
		m_videoPlayer.errorReceived -= OnErrorReceived;
		m_videoPlayer.prepareCompleted -= OnPrepareCompleted;
		m_videoPlayer.started -= OnPlayStart;
		m_videoPlayer.loopPointReached -= OnPlayEnd;
		//GameObject.Destroy(m_objMovie);
	}
	// Update is called once per frame
	void Update(){
		if( !m_videoPlayer.enabled ){
//GigaTrax.Debug.Log( "ED:enabled=true");
			m_videoPlayer.enabled = true;
			//string filePath = Application.streamingAssetsPath + "/GevelLogo2.mp4";
			string filePath = Application.streamingAssetsPath + "/" + MainFrame.getSysString("EDMovie");
			//obj.name = "Movie_" + System.IO.Path.GetFileName( filePath );
//GigaTrax.Debug.Log( "filePath=" + filePath);

			m_videoPlayer.url = filePath;
			m_videoPlayer.Prepare();
		}
	}
	private void OnErrorReceived(VideoPlayer player, string message)
	{
	}
	private void OnPrepareCompleted(VideoPlayer player)
	{
		m_videoPlayer.isLooping = false;
		m_videoPlayer.Play();
	}
	private void OnPlayStart(VideoPlayer player)
	{
	}
	private void OnPlayEnd(VideoPlayer player)
	{
		SceneManager.LoadScene( strDemoScene );
	}
	//private const string strDemoScene = "AnalyseDemo3";
	//private const string strDemoScene = "AnalyseMenu";
	private const string strDemoScene = "AnalyseMenu2";

	private VideoPlayer m_videoPlayer = null;
	private AudioSource m_audioSource = null;
}
