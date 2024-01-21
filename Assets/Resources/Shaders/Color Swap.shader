Shader "Custom/Color Swap" {
    Properties {
        _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        _MaskTex("Mask", 2D) = "white" {}
        _ShadeShift("Shade Shift", Vector) = (0, 0, 0, 0)
        _HueShift("Hue Shift", Vector) = (0, 0, 0, 0)
        _ColorChangeEffectToggle("Color Change Effect", Float) = 0
        _ColorChangeSpeed("Color Change Speed", Float) = 1.0
        _Flip("Flip", Vector) = (1,1,1,1)
    }
    
    SubShader {
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        
        LOD 100
        
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            
            #include "UnityCG.cginc"
            
            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _ShadeShift;
            fixed4 _HueShift;
            fixed4 _Color;
            sampler2D _MaskTex;
            float _ColorChangeEffectToggle;
            float _ColorChangeSpeed;

            fixed2 _Flip;
            
            inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
            {
                return float4(pos.xy * flip, pos.z, 1.0);
            }

            v2f vert(appdata_t v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityFlipSprite(v.vertex, _Flip);
                o.vertex = UnityObjectToClipPos(o.vertex);
                o.texcoord = v.texcoord;

                return o;
            }
            
            fixed3 HueShift(fixed3 Color, float Shift) {
                fixed k = 0.55735;
                fixed3 kV = fixed3(k, k, k);
                fixed3 P = kV * dot(kV, Color);
                fixed3 U = Color - P;
                fixed3 V = cross(kV, U);
                Color = U*cos(Shift*6.2832) + V*sin(Shift*6.2832) + P;
                return Color;
            }
            
            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;
                fixed3 mask = tex2D(_MaskTex, i.texcoord).rgb;
                
                fixed hueShift = dot(mask, _HueShift);
                if (_ColorChangeEffectToggle > 0.5) {
                    float time = _Time.y * _ColorChangeSpeed;
                    hueShift += sin(time) * 0.5;
                }
                col.rgb = HueShift(col.rgb, hueShift);
                
                fixed shadeShift = dot(mask, _ShadeShift);
                col.rgb *= (0.5 + shadeShift * 0.5 + 0.5);
            
                return col;
            }
            ENDCG
        }
    }
}
