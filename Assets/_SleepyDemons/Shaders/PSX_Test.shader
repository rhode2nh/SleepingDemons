Shader "Unlit/PSX_Test"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PrecisionX ("PrecisionX", float) = 0.0
        _PrecisionY ("PrecisionY", float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Name "PSX_Test"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _PrecisionX;
            float _PrecisionY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 vertex = o.vertex;
                vertex.xyz = o.vertex.xyz / o.vertex.w;
                vertex.x = floor(vertex.x * _PrecisionX) / _PrecisionX;
                vertex.y = floor(vertex.y * _PrecisionY) / _PrecisionY;
                vertex.xyz *= o.vertex.w;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.vertex = vertex;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
