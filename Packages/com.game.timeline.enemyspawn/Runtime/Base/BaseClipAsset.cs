using UnityEngine;
using UnityEngine.Playables;


namespace Game.Timeline
{
    public abstract class BaseClipAsset<T> : PlayableAsset where T : BaseBehaviour, new()
    {
        public sealed override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return BaseBehaviour.CreatePlayable<T>(graph, this);
        }
    }
}
