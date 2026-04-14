Shader "Custom/Outline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 0.5, 0, 1)
        _OutlineThickness ("Outline Thickness", Float) = 0.02
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "Outline"
            Tags { "LightMode"="UniversalForward" }

            Cull Front
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            // IMPORTANT pour URP
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineThickness;
            float4 _OutlineColor;

            v2f vert(appdata v)
            {
                v2f o;

                // Normale en espace objet
                float3 normal = normalize(v.normal);

                // On pousse le vertex vers l'extérieur
                float3 pos = v.vertex.xyz + normal * _OutlineThickness;

                // Transformations URP
                float3 worldPos = TransformObjectToWorld(pos);
                o.pos = TransformWorldToHClip(worldPos);

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }

            ENDHLSL
        }
    }
}