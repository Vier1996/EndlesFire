namespace InternalAssets.Codebase.Library.Extension.Properties
{
    public static class PropertyExtension
    {
        public static float Normalize01(this float percent) => percent * 0.01f;

        public static float AsNormalizedPercent(this float value) => 1f + value;
    }
}