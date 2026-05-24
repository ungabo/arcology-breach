Shader "Brassworks/UMCV09 Lit Texture"
{
    Properties
    {
        _Color ("Tint", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
        _BumpMap ("Normal", 2D) = "bump" {}
        _BumpScale ("Normal Scale", Range(0,2)) = 1
        _Metallic ("Metallic", Range(0,1)) = 0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _EmissionColor ("Emission", Color) = (0,0,0,0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _EmissionColor;
            half _Metallic;
            half _Glossiness;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWorld : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                fixed4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normalWorld = UnityObjectToWorldNormal(v.normal);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = _WorldSpaceCameraPos.xyz - worldPos;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float grain = frac(sin(dot(i.uv * float2(91.7, 47.3), float2(12.9898, 78.233))) * 43758.5453);
                float seams = step(0.965, frac(i.uv.x * 7.0)) + step(0.968, frac(i.uv.y * 9.0));
                fixed4 tex = fixed4(saturate(i.color.rgb * (0.78 + grain * 0.24) + seams * 0.035), 1.0);
                float3 n = normalize(i.normalWorld);
                float3 l1 = normalize(float3(-0.34, 0.72, -0.28));
                float3 l2 = normalize(float3(0.46, 0.28, 0.66));
                float diff1 = saturate(dot(n, l1));
                float diff2 = saturate(dot(n, l2));
                float3 v = normalize(i.viewDir);
                float3 h = normalize(l1 + v);
                float spec = pow(saturate(dot(n, h)), lerp(12.0, 72.0, _Glossiness)) * lerp(0.08, 0.55, _Glossiness);
                float3 warm = float3(1.0, 0.67, 0.34) * diff1;
                float3 cool = float3(0.42, 0.56, 0.8) * diff2 * 0.28;
                float3 ambient = float3(0.54, 0.43, 0.32);
                float3 metalSpec = lerp(float3(1.0, 0.92, 0.78), tex.rgb, _Metallic);
                float3 color = tex.rgb * (ambient + warm * 1.55 + cool * 1.15) + spec * metalSpec + _EmissionColor.rgb * 1.8;
                return fixed4(saturate(color), 1.0);
            }
            ENDCG
        }
    }

    FallBack Off
}
