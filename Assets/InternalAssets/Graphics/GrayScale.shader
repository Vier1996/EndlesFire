Shader "Custom/GreyScale" {
     Properties {
           _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Alpha Color Key", Color) = (0,0,0,1)
     }
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf Lambert
 
         sampler2D _MainTex;
 
         struct Input {
             float2 uv_MainTex;
         };
 
         void surf (Input IN, inout SurfaceOutput o) {
         
             half4 tex = tex2D (_MainTex, IN.uv_MainTex);
             
             half brightness = dot(tex.rgb, half3(1, 1, 1));
 
             o.Albedo = brightness;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }
