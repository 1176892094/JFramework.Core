// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:43
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