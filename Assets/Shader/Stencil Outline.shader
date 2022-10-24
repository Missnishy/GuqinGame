// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Missnish/Stencil Outline"
{
    Properties
    {
        [Header(Diffuse)]
        _MainTex ("Albedo Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1.0, 1.0, 1.0, 1.0)
        [Enum(Lambert, 0, Half_Lambert, 1)] _DiffuseModel ("Diffuse Model", int) = 0
        _DiffuseIntensity ("Diffuse Intensity", Range(0, 5)) = 1

        [Header(Specular)]
        _RoughnessMap ("Roughness Map", 2D) = "white" {}
        [Enum(Phong, 0, Blin_Phong, 1)] _SpecularModel ("Specular Model", int) = 0
        _Gloss ("Gloss", Range(0.01, 500)) = 300

        [Header(Outline)]
        _Outline ("Outline", Range(0, 1)) = 0.1
        [HDR]_OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        

        Pass
        {
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            //Vertex Input ：模型的顶点信息
            struct appdata
            {
                //顶点信息
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //法线信息
                float3 normal : NORMAL;
            };

            //Vertex Output ：由模型顶点信息换算而来的顶点屏幕位置
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos_CS : SV_POSITION;
                float3 normal_WS : TEXCOORD1;
                float4 pos_WS : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _RoughnessMap;
            float4 _BaseColor;
            float _DiffuseIntensity;
            float _Gloss;
            int _DiffuseModel;
            int _SpecularModel;


            //Vertex Input → Vertex Shader → Vertex Output
            v2f vert (appdata v)
            {
                v2f o;
                //顶点信息：模型空间 → 裁剪空间
                o.pos_CS = UnityObjectToClipPos(v.vertex);
                //顶点信息：模型空间 → 世界空间
                o.pos_WS = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //法线信息：模型空间 → 世界空间
                o.normal_WS = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            //Vertex Output → Fragment Shader → Pixel
            float4 frag (v2f i) : SV_Target
            {
                //向量计算
                float3 normal_Dir = normalize(i.normal_WS);
                float3 light_Dir = normalize(_WorldSpaceLightPos0.xyz);
                float3 view_Dir = normalize(_WorldSpaceCameraPos.xyz - i.pos_WS);
                float3 half_Dir = normalize(light_Dir + view_Dir);
                float3 NdotL = dot(normal_Dir, light_Dir);
                float3 NdotH = dot(normal_Dir, half_Dir);
                float3 VdotLr = dot(view_Dir, reflect(-light_Dir, normal_Dir));

                //贴图采样
                float4 Albedo = tex2D(_MainTex, i.uv);
                float4 Roughness = tex2D(_RoughnessMap, i.uv);

                //光照模型 = 直接光（①漫反射 + ②镜面反射） + 间接光（③漫反射 + ④镜面反射）
                //①直接光漫反射
                float Lambert = max(0, NdotL);
                float half_Lambert = max(0, Lambert * 0.5 + 0.5);
                float diffuse_Model = Lambert;
                switch(_DiffuseModel)
                {
                    case 0 : 
                        diffuse_Model = Lambert;
                        break;
                    case 1 :
                        diffuse_Model = half_Lambert;
                        break;
                    default:
                        diffuse_Model = half_Lambert;
                        break;
                }
                float3 Diffuse = Albedo * (diffuse_Model * _DiffuseIntensity);
                //①直接光镜面反射
                float3 Phong = max(0, VdotLr);
                float3 Blin_Phong = max(0, NdotH);
                float3 specular_Model = Blin_Phong;
                switch(_SpecularModel)
                {
                    case 0 : 
                        specular_Model = Phong;
                        break;
                    case 1 :
                        specular_Model = Blin_Phong;
                        break;
                    default:
                        specular_Model = Blin_Phong;
                        break;
                }
                float3 Specular = Roughness * pow(specular_Model, _Gloss);

                
                //结果混合
                float3 final_Color = _BaseColor * Diffuse + Specular;
                return float4(final_Color, 1.0);
                }
            
            ENDCG
        }

        
        Pass 
        {
            Stencil
            {
                Ref 1
                Comp NotEqual
            }

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            float _Outline;
            fixed4 _OutlineColor;

            struct a2v 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            }; 

            struct v2f 
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (a2v v) 
            {
                v2f o;

                v.vertex.xyz += v.normal * _Outline * 0.01;
				o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target 
            { 
                return float4(_OutlineColor.rgb, 1);               
            }

            ENDCG
        }

    }
}
