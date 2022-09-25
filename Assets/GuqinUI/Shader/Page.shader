Shader "Missnish/Page"
{
    Properties
    {
        _MainTex ("Front Texture", 2D) = "white" {}
        _SecondTex ("Back Texture", 2D) = "white"{}
        _Angle ("Angle", Range(0, 180)) = 0
        _Warp("Warp", Range(0, 2)) = 0
        _WarpY("Warp sinY", Range(0, 1)) = 0
        _WarpX("Warp X", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Angle;
            float _Warp;
            float _WarpY;
            float _WarpX;

            v2f vert (appdata v)
            {
                v2f o;
                //绕书本左侧旋转，需将书本整体平移至轴心
                v.vertex += float4(5,0,0,0);
                //角度转弧度
                float rad = _Angle * UNITY_PI / 180;
                //绕z轴旋转矩阵
                float4x4 rotateZ_matrix = float4x4 ( cos(rad), -sin(rad), 0, 0,
                                                     sin(rad),  cos(rad), 0, 0,
                                                             0,       0,  1, 0,
                                                             0,       0,  0, 1);

                float rangeF = saturate(1 - abs(90 - _Angle) / 90);
                v.vertex.y += -_Warp * sin(v.vertex.x * 0.4 - _WarpY * v.vertex.x) * rangeF;
                v.vertex.x -= rangeF * v.vertex.x * _WarpX;

                o.pos = mul(rotateZ_matrix, v.vertex);
                //旋转完成，平移回原位
                o.pos += float4(-5,0,0,0);
                o.pos = UnityObjectToClipPos(o.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, -i.uv);
                return col;
            }
            ENDCG
        }

        Pass
        {
            Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _SecondTex;
            float4 _SecondTex_ST;
            float _Angle;
            float _Warp;
            float _WarpY;
            float _WarpX;

            v2f vert (appdata v)
            {
                v2f o;
                //绕书本左侧旋转，需将书本整体平移至轴心
                v.vertex += float4(5,0,0,0);
                //角度转弧度
                float rad = _Angle * UNITY_PI / 180;
                //绕z轴旋转矩阵
                float4x4 rotateZ_matrix = float4x4 ( cos(rad), -sin(rad), 0, 0,
                                                     sin(rad),  cos(rad), 0, 0,
                                                             0,       0,  1, 0,
                                                             0,       0,  0, 1);

                float rangeF = saturate(1 - abs(90 - _Angle) / 90);
                v.vertex.y += -_Warp * sin(v.vertex.x * 0.4 - _WarpY * v.vertex.x) * rangeF;
                v.vertex.x -= rangeF * v.vertex.x * _WarpX;

                o.pos = mul(rotateZ_matrix, v.vertex);
                //旋转完成，平移回原位
                o.pos += float4(-5,0,0,0);
                o.pos = UnityObjectToClipPos(o.pos);
                o.uv = TRANSFORM_TEX(v.uv, _SecondTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_SecondTex, float2(i.uv.x, -i.uv.y));
                return col;
            }
            ENDCG
        }
    }
}
