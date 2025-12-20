Shader "Custom/SkyboxBlend"
{
    Properties
    {
        _SkyboxA ("Skybox A", Cube) = "" {}
        _SkyboxB ("Skybox B", Cube) = "" {}
        _Blend ("Blend", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _SkyboxA;
            samplerCUBE _SkyboxB;
            float _Blend;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 dir : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.dir = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 colA = texCUBE(_SkyboxA, normalize(i.dir));
                fixed4 colB = texCUBE(_SkyboxB, normalize(i.dir));
                return lerp(colA, colB, _Blend);
            }
            ENDCG
        }
    }
}
