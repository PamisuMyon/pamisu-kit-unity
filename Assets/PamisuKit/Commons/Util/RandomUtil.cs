using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pamisu.Commons
{
    public static class RandomUtil
    {
        public static float RandomNum(float num, float randomness)
        {
            return num + Random.Range(-num * randomness, num * randomness);
        }

        public static int RandomSigned(int start, int end)
        {
            int sign = RandomSign();
            var num = sign * Random.Range(start, end);
            return num;
        }

        public static float RandomSigned(float start, float end)
        {
            int sign = RandomSign();
            var num = sign * Random.Range(start, end);
            return num;
        }

        public static int RandomSign()
        {
            return Random.Range(0, 2) > 0 ? -1 : 1;
        }

        public static T RandomItem<T>(this T[] collections)
        {
            return collections[Random.Range(0, collections.Length)];
        }

        public static T RandomItem<T>(this List<T> collections)
        {
            return collections[Random.Range(0, collections.Count)];
        }

        public static void PlayRandomPitch(this AudioSource source)
        {
            source.pitch = Random.Range(.8f, 1.2f);
            source.Play();
        }

        public static void PlayRandomPitch(this AudioSource source, float pitch, float randomValue)
        {
            source.pitch = pitch + RandomSigned(0, randomValue);
            source.Play();
        }

        public static void PlayRandomPitch(this AudioSource source, AudioClip clip)
        {
            source.pitch = Random.Range(.8f, 1.2f);
            source.clip = clip;
            source.Play();
        }

        public static Quaternion RandomYRotation()
        {
            var angle = Random.Range(0, 360f);
            return Quaternion.Euler(0, angle, 0);
        }

        public static Vector2 InsideAnnulus(float minRadius, float maxRadius)
        {
            var dir = Random.insideUnitCircle.normalized;
            var minR2 = minRadius * minRadius;
            var maxR2 = maxRadius * maxRadius;
            // ICDF(x) = √(x*(rmax^2 - rmin^2)+rmin^2)
            return dir * Mathf.Sqrt(Random.value * (maxR2 - minR2) + minR2);
        }
        
    }
}