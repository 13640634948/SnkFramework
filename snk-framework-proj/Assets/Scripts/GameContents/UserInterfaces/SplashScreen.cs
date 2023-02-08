using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GAME.Contents.UserInterfaces
{
    public class SplashScreen : MonoBehaviour
    {
        private const int DISCARD_START_FRAME_NUM = 0;

        public VideoPlayer mVideoPlayer;

        public RawImage mRawImage;
        public virtual bool mFinish { get; protected set; } = false;

        protected bool _prepareCompleted = false;
        public virtual bool mPrepareCompleted => _prepareCompleted && this.mVideoPlayer.frame >= DISCARD_START_FRAME_NUM;

        private bool _lerping = false;
        
        private float _fadeOutTime = 0;
        private float currFadeOutTime = 0;
        private float progress = 0;
        private Color lerpColor = Color.white;

        [HideInInspector]
        public float FadeOutDuration = 1.0f;
        
        private void Awake()
        {
            mVideoPlayer.errorReceived += onErrorReceived;
            mVideoPlayer.frameDropped += onFrameDropped;
            mVideoPlayer.frameReady += onFrameReady;
            mVideoPlayer.loopPointReached += onLoopPointReached;
            mVideoPlayer.prepareCompleted += onPrepareCompleted;
            mVideoPlayer.seekCompleted += onSeekCompleted;
            mVideoPlayer.started += onStarted;
        }

        void Start()
        {
            mVideoPlayer.targetTexture = new RenderTexture((int)mVideoPlayer.clip.width,(int)mVideoPlayer.clip.height, 32);
            mRawImage.texture = mVideoPlayer.targetTexture;
            mRawImage.color = Color.white;
        }
        
        private void Update()
        {
            if (_lerping == false)
                return;
            
            if (progress >= 1.0f)
            {
                _lerping = false;
                mVideoPlayer.targetTexture.Release();
                this.mFinish = true;
            }

            if (currFadeOutTime < _fadeOutTime)
                currFadeOutTime += Time.unscaledDeltaTime;

            progress = currFadeOutTime / _fadeOutTime;
            if (progress > 1.0f)
            {
                progress = 1.0f;
            }

            lerpColor.a = 1-progress;
            this.mRawImage.color = lerpColor;
        }

        public void Play()
        {
            mVideoPlayer.Play();
        }

        /// <summary>
        /// 错误监听到时被执行
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        protected virtual void onErrorReceived(VideoPlayer source, string message)
        {
            
        }

        /// <summary>
        /// 有丢帧发生时被执行
        /// </summary>
        /// <param name="source"></param>
        protected virtual void onFrameDropped(VideoPlayer source)
        {
            
        }
        
        /// <summary>
        /// 新的一帧准备好时被执行
        /// </summary>
        protected virtual void onFrameReady(VideoPlayer source, long frameIdx)
        {
            Debug.Log("frameIdx:" + frameIdx);
        }

        /// <summary>
        /// 播放结束或播放到循环的点时被执行
        /// </summary>
        protected virtual void onLoopPointReached(VideoPlayer source)
        {
            this.fadeOut(FadeOutDuration);
        }

        /// <summary>
        /// 视频准备完成时被执行
        /// </summary>
        protected virtual void onPrepareCompleted(VideoPlayer source)
        {
            Debug.Log("onPrepareCompleted");
            this._prepareCompleted = true;
        }

        /// <summary>
        /// 查询帧操作完成时被执行
        /// </summary>
        protected virtual void onSeekCompleted(VideoPlayer source)
        {
        }

        /// <summary>
        /// 在Play方法调用之后立刻调用
        /// </summary>
        protected virtual void onStarted(VideoPlayer source)
        {
        }

        private void fadeOut(float time)
        {
            if(_lerping == true)
                return;
            this._fadeOutTime = time;
            this._lerping = true;
        }
    }
}