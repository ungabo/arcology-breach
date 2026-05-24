Shader "BrassworksBreach/UWMC09FakeLitTexture"
{
    Properties
    {
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
        LOD 100

        Pass
        {
            Cull Back
            ZWrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWorld : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normalWorld = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 normalWorld = normalize(i.normalWorld);
                float3 warmKey = normalize(float3(-0.45, 0.68, -0.58));
                float3 coolFill = normalize(float3(0.55, 0.28, 0.65));
                float warm = saturate(dot(normalWorld, warmKey)) * 0.78;
                float cool = saturate(dot(normalWorld, coolFill)) * 0.18;
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
                float rim = pow(1.0 - saturate(dot(viewDir, normalWorld)), 3.0) * 0.28;
                fixed4 tex = tex2D(_MainTex, i.uv) * _Color;
                float light = 0.30 + warm + cool + rim;
                float3 brassWarmth = float3(1.06, 0.93, 0.78);
                return fixed4(saturate(tex.rgb * light * brassWarmth), tex.a);
            }
            ENDCG
        }
    }
    Fallback Off
}
