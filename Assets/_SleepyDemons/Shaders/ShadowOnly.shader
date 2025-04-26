Shader "Custom/ShadowOnly"
{
    SubShader
    {
        Tags { "Queue" = "Geometry-1" "RenderType" = "Opaque" }
        Lighting On
        Cull Off
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
        }
    }
}
