Shader "Custom/DistanceTesselation"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _SpecColor("Spec Color", color) = (0.5, 0.5, 0.5, 0.5)
        _NormalMap ("Normal Map", 2D) = "bump" {}
        
        _Tess ("Tesselation", Range(1,32)) = 4
        _DispTex ("Disp Texture", 2D) = "gray" {}
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3
        [IntRange] _LOD("Texture LOD", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 300

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessDistance nolightmap

        // 4.6 for tesselation
        #pragma target 4.6

        #include "Tessellation.cginc"

        struct appdata {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
        };

        float _Tess;    

        float4 tessDistance(appdata v0, appdata v1, appdata v2) {
            float minDist = 10.0;
            float maxDist = 25.0;
            return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
        }

        sampler2D _DispTex;
        float _Displacement;
        float _LOD;

        void disp(inout appdata v) {
            float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, _LOD)).r * _Displacement;
            v.vertex.xyz += v.normal * d;
        }

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        sampler2D _NormalMap;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Specular = 0.2;
            o.Gloss = 1.0;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
