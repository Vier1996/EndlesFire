Shader "Custom/ExclusionMask"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
 
        CGPROGRAM
        #pragma surface surf Lambert
 
        sampler2D _MainTex;
        sampler2D _MaskTex;
 
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MaskTex;
        };
 
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Читаем основную текстуру
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
 
            // Читаем маску и проверяем, если пиксель в маске полностью прозрачен, исключаем его
            fixed4 mask = tex2D(_MaskTex, IN.uv_MaskTex);
            if (mask.a < 1)
                discard;
 
            // Устанавливаем цвет пикселя для рендера
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}