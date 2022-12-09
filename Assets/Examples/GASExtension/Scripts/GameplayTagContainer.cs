using System.Collections.Generic;
using System.Linq;
using GameplayTag.Authoring;
using Pamisu.Commons;

namespace Pamisu.GASExtension
{
    public class GameplayTagContainer
    {
        public List<GameplayTagScriptableObject> Tags { get; protected set; }

        public GameplayTagContainer()
        {
            Tags = new List<GameplayTagScriptableObject>();
        }

        public void AppendTag(GameplayTagScriptableObject appendTag)
        {
            Tags.AddUnique(appendTag);
        }

        public void AppendTags(IEnumerable<GameplayTagScriptableObject> appendTags)
        {
            Tags.AddRangeUnique(appendTags);
        }

        public void RemoveTag(GameplayTagScriptableObject appendTag)
        {
            Tags.Remove(appendTag);
        }

        public void RemoveTags(IEnumerable<GameplayTagScriptableObject> appendTags)
        {
            Tags.RemoveAll(appendTags.Contains);
        }

        public bool HasTag(GameplayTagScriptableObject tag)
        {
            return Tags.Contains(tag);
        }
    }
}