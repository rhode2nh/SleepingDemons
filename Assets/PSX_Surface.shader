Shader "Custom/PSX_Surface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _PrecisionX ("PrecisionX", float) = 80.0
        _PrecisionY ("PrecisionY", float) = 80.0
        _ScaleFactor ("Scale Factor", float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _PrecisionX;
        float _PrecisionY;
        float _ScaleFactor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert(inout appdata_full v)
        {
            float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
            float3 baseWorldPos = unity_ObjectToWorld._m03_m13_m23;
            float scaleFactor = distance(baseWorldPos, _WorldSpaceCameraPos) * _ScaleFactor;
            float4 viewPos = mul(unity_MatrixV, worldPos);
            float4 clipPos = mul(unity_CameraProjection, viewPos);
            clipPos.xyz /= clipPos.w;
            clipPos.x = floor(clipPos.x * _PrecisionX * scaleFactor) / _PrecisionX / scaleFactor;
            clipPos.y = floor(clipPos.y * _PrecisionY * scaleFactor) / _PrecisionY / scaleFactor;
            clipPos.xyz *= clipPos.w;
            v.vertex = clipPos;
            viewPos = mul(unity_CameraInvProjection, clipPos);
            worldPos = mul(unity_MatrixInvV, viewPos);
            v.vertex = mul(unity_WorldToObject, worldPos);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
