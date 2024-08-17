using System;
using System.Collections.Generic;
using Game.Framework;
using UnityEngine;

namespace Game.Common
{
    public static class GameplayUtil
    {
        
        public static Character SelectNearest(Vector3 center, List<Character> characters, Func<bool, Character> removalPredicate = null)
        {
            var minDir = Vector3.positiveInfinity;
            Character nearest = null;
            for (var i = characters.Count - 1; i >= 0; i--)
            {
                if (removalPredicate != null)
                {
                    if (removalPredicate(characters[i]))
                    {
                        characters.RemoveAt(i);
                        continue;
                    }
                }
                else
                {
                    if (characters[i] == null || !characters[i].IsAlive)
                    {
                        characters.RemoveAt(i);
                        continue;
                    }
                }
                
                var dir = characters[i].Trans.position - center;
                if (dir.sqrMagnitude < minDir.sqrMagnitude)
                {
                    minDir = dir;
                    nearest = characters[i];
                }
            }
            return nearest;
        }
        
        // public static IInteractable SelectNearestInFieldOfView(Transform viewer, List<IInteractable> interactables, float viewAngle, bool removeNull = false)
        // {
        //     var minDir = Vector3.positiveInfinity;
        //     IInteractable nearest = null;
        //     for (var i = interactables.Count - 1; i >= 0; i--)
        //     {
        //         // Only checks null when removeNull is true
        //         if (removeNull && interactables[i] == null)
        //         {
        //             interactables.RemoveAt(i);
        //             continue;
        //         }
                
        //         var dir = interactables[i].Trans.position - viewer.position;
        //         dir.y = 0f;
        //         var angle = Vector3.Angle(viewer.forward, dir);
        //         if (angle <= viewAngle && dir.sqrMagnitude < minDir.sqrMagnitude)
        //         {
        //             minDir = dir;
        //             nearest = interactables[i];
        //         }
        //     }
        //     return nearest;
        // }

        // public static UniTask SimpleDelay(float delaySeconds, CancellationToken cancellationToken)
        // {
        //     return UniTask.Delay(TimeSpan.FromSeconds(delaySeconds), DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
        // }
        
        // public static UniTask SimpleDelay(int delayMilliseconds, CancellationToken cancellationToken)
        // {
        //     return UniTask.Delay(delayMilliseconds, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
        // }

        public static bool IsAdjacent(this Character a, Character b, float tolerance = 0f)
        {
            return IsAdjacent(a.Trans, a.Model.VisualRadius, b.Trans, b.Model.VisualRadius, tolerance);
        }

        public static bool IsAdjacent(Transform a, float aRadius, Transform b, float bRadius, float tolerance = 0f)
        {
            var dir = a.transform.position - b.transform.position;
            var maxDistanceSqr = aRadius + bRadius + tolerance;
            maxDistanceSqr *= maxDistanceSqr;
            return dir.sqrMagnitude <= maxDistanceSqr;
        }
        
    }
}