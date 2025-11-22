// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================
    
using System;
using UnityEngine;
using System.Globalization;
using System.Text;
// using TMPro;
// using UnityEngine.Splines;
// using DG.Tweening;

namespace Chuozt.Template.ProUtils
{
    // REQUIRES DOTWEEN

    // REQUIRES UNITY's SPLINE
    // public static class SplineMovementUtils
    // {
    //     public static Tween MoveAlongSpline(Transform transform, SplineContainer spline, float duration, Ease ease = Ease.InQuint, bool lookForward = true)
    //     {
    //         float t = 0f;

    //         Tween tween = DOTween.To(() => t, x =>
    //         {
    //             t = x;
    //             spline.Evaluate(t, out var pos, out var tangent, out var up);
    //             transform.position = pos;

    //             if (lookForward)
    //                 transform.rotation = Quaternion.LookRotation(tangent, up);

    //         }, 1f, duration).SetEase(ease);

    //         return tween;
    //     }
    // }

    // REQUIRES TMPro
    // public static class NumberTransitor
    // {
    //     public static Tweener TweenTextValue(TMP_Text text, int from, int to, float duration, RectTransform parentRectTransform = null, bool isConvertFormat = false)
    //     {
    //         float currentValue = from;
    //         int lastDigitCount = from.ToString().Length;

    //         return DOTween.To(() => currentValue, x =>
    //         {
    //             currentValue = x;
    //             int intValue = Mathf.FloorToInt(currentValue);

    //             if (isConvertFormat)
    //                 text.text = CurrencyFormatter.Convert(intValue);
    //             else
    //                 text.text = intValue.ToString();

    //             int currentDigitCount = text.text.Length;
    //             if (text.rectTransform != null && currentDigitCount != lastDigitCount)
    //             {
    //                 lastDigitCount = currentDigitCount;
    //                 LayoutRebuilder.ForceRebuildLayoutImmediate(text.rectTransform);

    //                 if (parentRectTransform != null)
    //                     LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);
    //             }

    //         }, to, duration);
    //     }
    // }

    public static class CurrencyFormatter
    {
        private static string[] format = new string[]
        {
            "K",
            "M",
            "B",
            "T",
        };

        public static string Convert(int input)
        {
            if (input < 10000.0)
                return Mathf.Round(input).ToString();

            float num = 0f;
            for (int i = 0; i < format.Length; i++)
            {
                num = input / Mathf.Pow(1000f, (i + 1));
                if (num < 1000f)
                    return System.Math.Round(num, 1).ToString() + format[i];
            }
            return num.ToString();
        }
    }

    public static class TimeFormatter
    {
        // private static string[] units = new string[]
        // {
        //     "s",  // Seconds
        //     "m",  // Minutes
        //     "h",  // Hours
        //     "d",  // Days
        //     "w",  // Weeks
        //     "mo", // Months (approx.)
        //     "y"   // Years (approx.)
        // };

        // public static string Convert(float seconds)
        // {
        //     float[] thresholds = new float[]
        //     {
        //         60f,        // 60 seconds -> 1 minute
        //         60f * 60f,  // 3600 seconds -> 1 hour
        //         86400f,     // 86400 seconds -> 1 day
        //         604800f,    // 7 days -> 1 week
        //         2628000f,   // ~1 month
        //         31536000f   // ~1 year
        //     };

        //     for (int i = thresholds.Length - 1; i >= 0; i--)
        //     {
        //         if (seconds >= thresholds[i])
        //         {
        //             float value = seconds / thresholds[i];
        //             return Math.Floor(value).ToString() + units[i + 1];
        //         }
        //     }

        //     return Mathf.Round(seconds) + units[0]; // Less than 60 → show seconds
        // }

        private static readonly string[] units = new string[]
        {
            "s",  // Seconds
            "m",  // Minutes
            "h",  // Hours
            "d"   // Days
        };

        private static readonly float[] thresholds = new float[]
        {
            1f,             // Seconds
            60f,            // Minutes
            3600f,          // Hours
            86400f          // Days
        };

        /// <summary>
        /// Converts time into human-readable format (e.g., "2d 3h").
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <param name="maxUnitIndex">Max unit index to show (0=s, 1=m, 2=h, 3=d)</param>
        /// <param name="maxParts">How many parts to show (e.g., 2 → "1d 2h")</param>
        /// <returns>Formatted string</returns>
        public static string Convert(double seconds, int maxUnitIndex = 3, int maxParts = 1)
        {
            seconds = Math.Max(0, seconds); // Clamp to non-negative

            StringBuilder result = new StringBuilder();
            int partsShown = 0;

            for (int i = maxUnitIndex; i >= 0 && partsShown < maxParts; i--)
            {
                double unitValue = Math.Floor(seconds / thresholds[i]);
                if (unitValue >= 1 || (i == 0 && partsShown == 0)) // Show at least seconds if nothing added
                {
                    result.Append((int)unitValue).Append(units[i]).Append(" ");
                    seconds -= unitValue * thresholds[i];
                    partsShown++;
                }
            }

            return result.ToString().Trim();
        }
    }

    public static class UIHelper
    {
        public static void SetUIToWorldPosition(RectTransform uiElement, Vector3 worldPosition, Canvas canvas, Camera camera = null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

            RectTransform canvasRect = canvas.transform as RectTransform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera,
                out Vector2 localPoint
            );

            uiElement.localPosition = localPoint;
        }
    }

    public static class DateTimeHelper
    {
        public static DateTime ParseDate(string dateToParse)
        {
            return DateTime.ParseExact(dateToParse, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        }
    }

    public static class StringHelper
    {
        //Turn a string into "snake_case"
        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (char.IsUpper(c))
                {
                    if (i > 0)
                        result.Append('_');

                    result.Append(char.ToLower(c));
                }
                else
                    result.Append(c);
            }

            return result.ToString();
        }

        //Turn a string into "PascalCase"
        public static string ToPascalCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.Replace("_", " ").Replace("-", " ");

            var words = input.Split(' ');
            StringBuilder sb = new StringBuilder();

            foreach (var word in words)
            {
                if (string.IsNullOrEmpty(word))
                    continue;

                sb.Append(char.ToUpperInvariant(word[0]));
                if (word.Length > 1)
                    sb.Append(word.Substring(1).ToLowerInvariant());
            }

            return sb.ToString();
        }
    }
}
