Shader "Missnish/Leaf Animation"
{
    Properties
    {
        [Header(Material)]
        _MainTex ("Albedo Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1.0, 1.0, 1.0, 1.0)
        [Enum(Lambert, 0, Half_Lambert, 1)] _DiffuseModel ("Diffuse Model", int) = 0
        _DiffuseIntensity ("Diffuse Intensity", Range(0, 3)) = 1
        _Gloss("Gloss",Range(0.01,300)) = 50

        [Header(Animation)]
        _WindDirection ("Wind Direction", Vector) = (1.0, 0.0, 0.0, 0.0)
        _WindSpeed ("Wind Speed", Range(0, 2)) = 1.2
        _WindStrength ("Wind Strength", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
        LOD 100
        Cull off

        //阴影pass
        pass
        {
            //阴影模式
            Tags{ "LightMode" = "ShadowCaster" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            //创建阴影
            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            //-----------------噪声生成-----------------
            //随机数
            fixed2 randVec(fixed2 value)
			{
				fixed2 vec = fixed2(dot(value, fixed2(127.1, 337.1)), dot(value, fixed2(269.5, 183.3)));
				vec = -1 + 2 * frac(sin(vec) * 43758.5453123);
				return vec;
			}

            //柏林噪声
			float perlinNoise(float2 uv)
			{
				float a, b, c, d;
				float x0 = floor(uv.x); 
				float x1 = ceil(uv.x); 
				float y0 = floor(uv.y); 
				float y1 = ceil(uv.y); 
				fixed2 pos = frac(uv);
				a = dot(randVec(fixed2(x0, y0)), pos - fixed2(0, 0));
				b = dot(randVec(fixed2(x0, y1)), pos - fixed2(0, 1));
				c = dot(randVec(fixed2(x1, y1)), pos - fixed2(1, 1));
				d = dot(randVec(fixed2(x1, y0)), pos - fixed2(1, 0));
				float2 st = 6 * pow(pos, 5) - 15 * pow(pos, 4) + 10 * pow(pos, 3);
				a = lerp(a, d, st.x);
				b = lerp(b, c, st.x);
				a = lerp(a, b, st.y);
				return a;
			}

            //分形布朗运动
			float fbm(float2 uv)
			{
				float f = 0;
				float a = 1;
				for(int i = 0; i < 3; i++)
				{
					f += a * perlinNoise(uv);
					uv *= 2;
					a /= 2;
				}
				return f;
			}
            //-----------------噪声生成完毕-----------------

            float4 _WindDirection;
            float _WindSpeed;
            float _WindStrength;

            struct appdata
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
                half3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //申请阴影数据结构
                V2F_SHADOW_CASTER;
            };

            sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;
                float2 uv_offset_xz = normalize(_WindDirection.xy) * _WindSpeed * _Time.y;
                float noise = fbm(float2(v.vertex.x, v.vertex.z) + uv_offset_xz);
                v.vertex.xyz += _WindStrength * noise;
                v.vertex.w = 1;
                o.uv = v.uv;
                //放入阴影生成模块
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                clip(col.a - 0.5);
                //渲染阴影
                SHADOW_CASTER_FRAGMENT(i)
            }
                ENDCG
        }

        //渲染pass
        Pass
        {
            //受光标签：向前渲染基础灯光
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //告诉渲染管线这个pass以forwardbase渲染
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            //Unity光照函数库
            #include "AutoLight.cginc"

            //-----------------噪声生成-----------------
            //随机数
            fixed2 randVec(fixed2 value)
			{
				fixed2 vec = fixed2(dot(value, fixed2(127.1, 337.1)), dot(value, fixed2(269.5, 183.3)));
				vec = -1 + 2 * frac(sin(vec) * 43758.5453123);
				return vec;
			}

            //柏林噪声
			float perlinNoise(float2 uv)
			{
				float a, b, c, d;
				float x0 = floor(uv.x); 
				float x1 = ceil(uv.x); 
				float y0 = floor(uv.y); 
				float y1 = ceil(uv.y); 
				fixed2 pos = frac(uv);
				a = dot(randVec(fixed2(x0, y0)), pos - fixed2(0, 0));
				b = dot(randVec(fixed2(x0, y1)), pos - fixed2(0, 1));
				c = dot(randVec(fixed2(x1, y1)), pos - fixed2(1, 1));
				d = dot(randVec(fixed2(x1, y0)), pos - fixed2(1, 0));
				float2 st = 6 * pow(pos, 5) - 15 * pow(pos, 4) + 10 * pow(pos, 3);
				a = lerp(a, d, st.x);
				b = lerp(b, c, st.x);
				a = lerp(a, b, st.y);
				return a;
			}

            //分形布朗运动
			float fbm(float2 uv)
			{
				float f = 0;
				float a = 1;
				for(int i = 0; i < 3; i++)
				{
					f += a * perlinNoise(uv);
					uv *= 2;
					a /= 2;
				}
				return f;
			}
            //-----------------噪声生成完毕-----------------


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
                float4 pos : SV_POSITION;
                float3 normal_WS : TEXCOORD1;
                float4 pos_WS : TEXCOORD2;
                SHADOW_COORDS(3)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BaseColor;
            float _DiffuseIntensity;
            int _DiffuseModel;
            float4 _LightColor0;
            float _Gloss;

            float4 _WindDirection;
            float _WindSpeed;
            float _WindStrength;


            //Vertex Input → Vertex Shader → Vertex Output
            v2f vert (appdata v)
            {
                v2f o;
                //顶点信息：模型空间 → 世界空间
                o.pos_WS = mul(unity_ObjectToWorld, v.vertex);
                //使用posWS.xz进行偏移计算
                float2 uv_offset_xz = normalize(_WindDirection.xy) * _WindSpeed * _Time.y;
                float noise = fbm(float2(o.pos_WS.x, o.pos_WS.z) + uv_offset_xz);
                v.vertex.xyz += _WindStrength * noise;
                v.vertex.w = 1;
                //顶点信息：模型空间 → 裁剪空间
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                //法线信息：模型空间 → 世界空间
                o.normal_WS = UnityObjectToWorldNormal(v.normal);
                //处理阴影数据
                TRANSFER_SHADOW(o);
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
                //float3 VdotLr = dot(view_Dir, reflect(-light_Dir, normal_Dir));

                //贴图采样
                float4 Albedo = tex2D(_MainTex, i.uv);
                clip(Albedo.a - 0.5);

                //获取阴影
                float shadow = SHADOW_ATTENUATION(i);

                //光照模型
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
                float3 Diffuse = _LightColor0 * Albedo * (lerp(Albedo, _LightColor0, saturate(shadow * diffuse_Model) * _DiffuseIntensity));
                float3 Specular = pow(saturate(NdotH), _Gloss) * shadow * _LightColor0 * Albedo;
                
                //结果混合
                float4 final_RGB = float4(_BaseColor * Diffuse + Specular, Albedo.a);
                return final_RGB;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
