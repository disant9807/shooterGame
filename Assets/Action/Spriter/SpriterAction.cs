using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Assets.Core.Common;

namespace Assets.Action.Spriter
{
    public class SpriterAction : MonoBehaviour, ISpriterAction
    {
        public Color colorDefault;

        private SpriteRenderer spriteRendererCache;

        public void TemporaryColorChange(Color color, float time)
        {
            StartCoroutine(AcyncColorChange(color, time));
        }

        private IEnumerator AcyncColorChange(Color color, float time)
        {
            spriteRendererCache.color = color;
            yield return new WaitForSeconds(time);
            spriteRendererCache.color = colorDefault;
        }

        public void ColorChange(Color color)
        {
            spriteRendererCache.color = color;
        }
    }
}
