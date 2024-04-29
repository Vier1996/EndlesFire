using UnityEngine;

namespace InternalAssets.Codebase.Library.Extension.Colors
{
    public static class ColorTools
    {
        public static Color TransparentColor = new(1f, 1f, 1f, 0f);

        public static string ToHtml(this Color color) => 
            $"#{(int)(color.r * 255):X2}{(int)(color.g * 255):X2}{(int)(color.b * 255):X2}";
    }
}