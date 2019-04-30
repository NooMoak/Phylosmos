// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FireToon"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color110 = IsGammaSpace() ? float4(1,0.9235294,0,0) : float4(1,0.8347788,0,0);
			float4 appendResult8 = (float4(0.0 , -1.5 , 0.0 , 0.0));
			float4 appendResult40 = (float4(1.22 , 0.8 , 0.0 , 0.0));
			float2 panner7 = ( 1.0 * _Time.y * appendResult8.xy + ( appendResult40 * float4( i.uv_texcoord, 0.0 , 0.0 ) ).xy);
			float4 appendResult16 = (float4(0.0 , -1.0 , 0.0 , 0.0));
			float4 appendResult42 = (float4(0.54 , 0.3 , 0.0 , 0.0));
			float2 panner15 = ( 1.0 * _Time.y * appendResult16.xy + ( float4( i.uv_texcoord, 0.0 , 0.0 ) * appendResult42 ).xy);
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode53 = tex2D( _TextureSample2, uv_TextureSample2 );
			float temp_output_61_0 = saturate( ( ( ( tex2D( _Texture0, panner7 ).r * tex2D( _Texture0, panner15 ).r ) + tex2DNode53.r ) * tex2DNode53.r ) );
			float smoothstepResult114 = smoothstep( 0.89 , 0.9 , temp_output_61_0);
			float smoothstepResult102 = smoothstep( 0.59 , 0.6 , temp_output_61_0);
			float4 color112 = IsGammaSpace() ? float4(1,0.6452084,0.03301889,0) : float4(1,0.3738863,0.002555641,0);
			float smoothstepResult105 = smoothstep( 0.0840832 , 0.09 , temp_output_61_0);
			o.Emission = ( ( color110 * ( smoothstepResult114 + smoothstepResult102 ) ) + ( color112 * ( smoothstepResult105 - smoothstepResult102 ) ) ).rgb;
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			o.Alpha = tex2D( _TextureSample3, uv_TextureSample3 ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
-85.6;61.6;741;835;-280.0405;880.7148;1.535003;False;False
Node;AmplifyShaderEditor.RangedFloatNode;12;-1637.266,-615.0311;Float;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;1.22;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1670.27,-548.4241;Float;False;Constant;_Float5;Float 5;1;0;Create;True;0;0;False;0;0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1700.501,-113.2288;Float;False;Constant;_Float3;Float 3;1;0;Create;True;0;0;False;0;0.54;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1711.898,-29.50213;Float;False;Constant;_Float6;Float 6;1;0;Create;True;0;0;False;0;0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1591.439,-369.5257;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-1404.543,-709.7563;Float;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;-1.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;40;-1408.08,-573.7397;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1501.525,100.2151;Float;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;42;-1418.073,-70.57101;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-1257.459,83.01836;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-1204.413,-683.8287;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1270.592,-110.6219;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1244.171,-458.7976;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;15;-1091.049,-42.19161;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;7;-1038.575,-634.6744;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1155.619,-321.7343;Float;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;False;0;33bbbf8799352f841914bd9278a0c5b4;33bbbf8799352f841914bd9278a0c5b4;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;6;-765.2195,-435.1765;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-782.0289,-177.8587;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-335.8607,-309.1465;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;53;-155.3647,62.67506;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;021019cf1f8947b4db0fde38320f4793;021019cf1f8947b4db0fde38320f4793;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-50.46631,-282.2241;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;56;161.8046,-69.64392;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;218.8745,-279.0464;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;57;430.7237,-295.9715;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;61;718.1369,-252.1352;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;104;717.2945,-649.063;Float;False;Constant;_Float8;Float 8;4;0;Create;True;0;0;False;0;0.59;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;674.2292,88.07224;Float;False;Constant;_Float9;Float 9;4;0;Create;True;0;0;False;0;0.0840832;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;689.2616,216.5349;Float;False;Constant;_Float10;Float 10;4;0;Create;True;0;0;False;0;0.09;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;117;581.5063,-562.7598;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;669.4102,-1042.666;Float;False;Constant;_Float11;Float 11;4;0;Create;True;0;0;False;0;0.89;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;115;610.2948,-901.7961;Float;False;Constant;_Float4;Float 4;4;0;Create;True;0;0;False;0;0.9;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;658.1791,-508.1936;Float;False;Constant;_Float7;Float 7;4;0;Create;True;0;0;False;0;0.6;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;102;1059.07,-458.631;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;114;1085.232,-804.4876;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;105;1092.16,26.37628;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;119;1458.821,-471.1572;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;110;1551.6,-681.9548;Float;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;False;0;1,0.9235294,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;112;1594.671,181.9324;Float;False;Constant;_Color1;Color 1;4;0;Create;True;0;0;False;0;1,0.6452084,0.03301889,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;108;1581.507,-88.25491;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;1771.186,-443.6509;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;1876.797,-141.0676;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;82;1891.445,272.1917;Float;True;Property;_TextureSample3;Texture Sample 3;3;0;Create;True;0;0;False;0;021019cf1f8947b4db0fde38320f4793;021019cf1f8947b4db0fde38320f4793;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;113;2151.77,-288.098;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2420.643,26.32349;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;FireToon;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;40;0;12;0
WireConnection;40;1;41;0
WireConnection;42;0;19;0
WireConnection;42;1;43;0
WireConnection;16;1;17;0
WireConnection;8;1;9;0
WireConnection;18;0;13;0
WireConnection;18;1;42;0
WireConnection;11;0;40;0
WireConnection;11;1;13;0
WireConnection;15;0;18;0
WireConnection;15;2;16;0
WireConnection;7;0;11;0
WireConnection;7;2;8;0
WireConnection;6;0;1;0
WireConnection;6;1;7;0
WireConnection;14;0;1;0
WireConnection;14;1;15;0
WireConnection;20;0;6;1
WireConnection;20;1;14;1
WireConnection;54;0;20;0
WireConnection;54;1;53;1
WireConnection;56;0;53;1
WireConnection;55;0;54;0
WireConnection;55;1;56;0
WireConnection;57;0;55;0
WireConnection;61;0;57;0
WireConnection;117;0;61;0
WireConnection;102;0;61;0
WireConnection;102;1;104;0
WireConnection;102;2;103;0
WireConnection;114;0;117;0
WireConnection;114;1;116;0
WireConnection;114;2;115;0
WireConnection;105;0;61;0
WireConnection;105;1;106;0
WireConnection;105;2;107;0
WireConnection;119;0;114;0
WireConnection;119;1;102;0
WireConnection;108;0;105;0
WireConnection;108;1;102;0
WireConnection;109;0;110;0
WireConnection;109;1;119;0
WireConnection;111;0;112;0
WireConnection;111;1;108;0
WireConnection;113;0;109;0
WireConnection;113;1;111;0
WireConnection;0;2;113;0
WireConnection;0;9;82;1
ASEEND*/
//CHKSM=1572481B5848EE72308028EB3069B04DAC70833B