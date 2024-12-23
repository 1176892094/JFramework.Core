// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 19:12:32
// # Recently: 2024-12-22 20:12:23
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Threading.Tasks;

namespace JFramework
{
    public interface IAudioHelper : IBaseHelper
    {
        void MusicVolume(float volume);

        void AudioVolume(float volume);

        object Instantiate(string assetPath);

        Task OnDequeue<T>(string assetPath, T assetData, int assetMode);

        string OnEnqueue<T>(T assetData, bool pause);
    }
}