Shader "Custom/SpriteBlur" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _BlurAmount ("Blur Amount", Range(0.0, 1.0)) = 0.5
    }
 
    SubShader {
        Tags { "Queue" = "Transparent" }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float _BlurAmount;
 
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
 
                // Размытие
                fixed4 blurredColor = fixed4(0, 0, 0, 0);
                float blurSize = 0.01 * _BlurAmount; // Размер размытия
 
                for (int x = -1; x <= 1; x++) {
                    for (int y = -1; y <= 1; y++) {
                        float2 offset = float2(x, y) * blurSize;
                        blurredColor += tex2D(_MainTex, i.uv + offset);
                    }
                }
 
                blurredColor /= 9.0;
 
                // Смешивание исходного цвета и размытого цвета
                col = lerp(col, blurredColor, _BlurAmount);
 
                return col;
            }
 
            ENDCG
        }
    }
}