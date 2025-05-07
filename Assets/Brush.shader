Shader "Hidden/Brush"
{
    Properties {}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZWrite Off
            ZTest Always
            Cull Off
            Blend One OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Coord;   // Brush UV pozisyonu
            float _Size;     // Brush yarıçapı
            float4 _Color;   // Brush rengi (örneğin (0.5,0.5,0.5,1) köpük için)

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert(appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = distance(i.uv, _Coord.xy);
                if (dist > 0.1)
                    discard;

                return _Color;
            }
            ENDCG
        }
    }
}