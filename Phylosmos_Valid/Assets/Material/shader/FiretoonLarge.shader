// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FireToonLarge"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Texture0("Texture 0", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	Category 
	{
		SubShader
		{
			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_OUTPUT_STEREO
					
				};
				
				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform sampler2D_float _CameraDepthTexture;
				uniform float _InvFade;
				uniform sampler2D _TextureSample2;
				uniform float4 _TextureSample2_ST;
				uniform sampler2D _Texture0;

				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					float2 uv_TextureSample2 = v.texcoord.xy * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
					float smoothstepResult146 = smoothstep( -0.3053221 , 1.0 , tex2Dlod( _TextureSample2, float4( uv_TextureSample2, 0, 0.0) ).r);
					float3 temp_cast_0 = (smoothstepResult146).xxx;
					

					v.vertex.xyz += temp_cast_0;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float4 color142 = IsGammaSpace() ? float4(0.9921134,1,0.6273585,0) : float4(0.9821529,1,0.3514183,0);
					float4 appendResult8 = (float4(-0.5 , -0.5 , 0.0 , 0.0));
					float4 appendResult40 = (float4(1.22 , 0.8 , 0.0 , 0.0));
					float2 uv13 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner7 = ( 1.0 * _Time.y * appendResult8.xy + ( appendResult40 * float4( uv13, 0.0 , 0.0 ) ).xy);
					float4 appendResult16 = (float4(0.5 , -0.5 , 0.0 , 0.0));
					float4 appendResult42 = (float4(0.54 , 0.3 , 0.0 , 0.0));
					float2 panner15 = ( 1.0 * _Time.y * appendResult16.xy + ( float4( uv13, 0.0 , 0.0 ) * appendResult42 ).xy);
					float2 uv_TextureSample2 = i.texcoord.xy * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
					float smoothstepResult146 = smoothstep( -0.3053221 , 1.0 , tex2D( _TextureSample2, uv_TextureSample2 ).r);
					float smoothstepResult114 = smoothstep( 0.98 , 0.99 , ( ( ( tex2D( _Texture0, panner7 ).r * tex2D( _Texture0, panner15 ).r ) + smoothstepResult146 ) * smoothstepResult146 ));
					float4 color110 = IsGammaSpace() ? float4(0.9811321,0.8639403,0.3285867,0) : float4(0.957614,0.7179325,0.0881996,0);
					float smoothstepResult102 = smoothstep( 0.783829 , 0.789776 , ( ( ( tex2D( _Texture0, panner7 ).r * tex2D( _Texture0, panner15 ).r ) + smoothstepResult146 ) * smoothstepResult146 ));
					float smoothstepResult105 = smoothstep( 0.08772475 , 0.09 , ( ( ( tex2D( _Texture0, panner7 ).r * tex2D( _Texture0, panner15 ).r ) + smoothstepResult146 ) * smoothstepResult146 ));
					float temp_output_108_0 = ( smoothstepResult105 - smoothstepResult102 );
					float2 uv127 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float smoothstepResult126 = smoothstep( 0.0 , 0.9338064 , uv127.y);
					float4 color131 = IsGammaSpace() ? float4(0.9803922,0.6449748,0,0) : float4(0.9559735,0.3735871,0,0);
					float2 uv137 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float smoothstepResult135 = smoothstep( 0.0 , 0.2215404 , uv137.y);
					float4 color134 = IsGammaSpace() ? float4(1,0,0.1076922,0) : float4(1,0,0.01125833,0);
					

					fixed4 col = ( ( color142 * smoothstepResult114 ) + ( color110 * ( smoothstepResult102 - smoothstepResult114 ) ) + ( ( temp_output_108_0 * ( 1.0 - smoothstepResult126 ) * color131 ) + ( temp_output_108_0 * smoothstepResult135 * color134 ) ) );
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16100
105.6;55.2;1050;480;1980.196;601.1039;5.803155;True;False
Node;AmplifyShaderEditor.RangedFloatNode;12;-1637.266,-615.0311;Float;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;1.22;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1718.258,-508.7816;Float;False;Constant;_Float5;Float 5;1;0;Create;True;0;0;False;0;0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1711.898,-29.50213;Float;False;Constant;_Float6;Float 6;1;0;Create;True;0;0;False;0;0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1700.501,-113.2288;Float;False;Constant;_Float3;Float 3;1;0;Create;True;0;0;False;0;0.54;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;42;-1418.073,-70.57101;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;40;-1408.08,-573.7397;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;163;-1480.469,194.5834;Float;False;Constant;_Float16;Float 16;2;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1501.525,100.2151;Float;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;False;0;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-1466.067,-698.9734;Float;False;Constant;_Float15;Float 15;2;0;Create;True;0;0;False;0;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1440.599,-797.8422;Float;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1591.439,-369.5257;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1270.592,-110.6219;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-1204.413,-683.8287;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1244.171,-458.7976;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-1257.459,83.01836;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;15;-1091.049,-42.19161;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;7;-1038.575,-634.6744;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1155.619,-321.7343;Float;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;False;0;33bbbf8799352f841914bd9278a0c5b4;33bbbf8799352f841914bd9278a0c5b4;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;53;-1037.713,117.6361;Float;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;False;0;d6f2b79ae2b252640b643788c70d9bc4;d6f2b79ae2b252640b643788c70d9bc4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-818.3192,-552.7543;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-922.829,-217.8587;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;147;-859.4667,464.566;Float;False;Constant;_Float14;Float 14;2;0;Create;True;0;0;False;0;-0.3053221;0;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-335.8607,-309.1465;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;146;-415.1248,157.6806;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-50.46631,-282.2241;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;218.8745,-279.0464;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;104;717.2945,-649.063;Float;False;Constant;_Float8;Float 8;4;0;Create;True;0;0;False;0;0.783829;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;758.1793,96.67801;Float;False;Constant;_Float10;Float 10;4;0;Create;True;0;0;False;0;0.09;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;743.1469,-31.78463;Float;False;Constant;_Float9;Float 9;4;0;Create;True;0;0;False;0;0.08772475;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;57;579.0326,-261.3558;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;658.1791,-508.1936;Float;False;Constant;_Float7;Float 7;4;0;Create;True;0;0;False;0;0.789776;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;934.3865,444.1016;Float;False;Constant;_Float12;Float 12;3;0;Create;True;0;0;False;0;0.9338064;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;127;911.2637,210.9322;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;137;1083.555,576.9571;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;102;1059.07,-458.631;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;115;613.2948,-901.7961;Float;False;Constant;_Float4;Float 4;4;0;Create;True;0;0;False;0;0.99;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;140;586.6271,-631.1597;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;669.4102,-1042.666;Float;False;Constant;_Float11;Float 11;4;0;Create;True;0;0;False;0;0.98;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;105;1083.171,-138.4269;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;1090.314,740.576;Float;False;Constant;_Float13;Float 13;3;0;Create;True;0;0;False;0;0.2215404;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;126;1263.628,168.4826;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;134;1409.892,632.6885;Float;False;Constant;_Color2;Color 2;3;0;Create;True;0;0;False;0;1,0,0.1076922,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;108;1419.7,-127.2084;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;135;1419.555,464.957;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;114;1193.909,-1021.841;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;129;1486.677,126.2546;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;131;1447.331,263.3316;Float;False;Constant;_Color1;Color 1;3;0;Create;True;0;0;False;0;0.9803922,0.6449748,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;1803.183,-70.00858;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;142;1508.656,-1185.613;Float;False;Constant;_Color3;Color 3;3;0;Create;True;0;0;False;0;0.9921134,1,0.6273585,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;1831.636,307.9365;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;143;1493.279,-412.5532;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;110;1551.6,-681.9548;Float;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;False;0;0.9811321,0.8639403,0.3285867,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;1821.562,-478.9141;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;1818.538,-1005.371;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;132;2117.31,22.75127;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;165;495.5081,928.0044;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;113;2463.21,-150.0311;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;166;1839.706,905.5519;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;164;2862.296,-118.2158;Float;False;True;2;Float;ASEMaterialInspector;0;6;FireToonLarge;0b6a9f8b4f707c74ca64c0be8e590de0;0;0;SubShader 0 Pass 0;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;42;0;19;0
WireConnection;42;1;43;0
WireConnection;40;0;12;0
WireConnection;40;1;41;0
WireConnection;18;0;13;0
WireConnection;18;1;42;0
WireConnection;8;0;162;0
WireConnection;8;1;9;0
WireConnection;11;0;40;0
WireConnection;11;1;13;0
WireConnection;16;0;163;0
WireConnection;16;1;17;0
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
WireConnection;146;0;53;1
WireConnection;146;1;147;0
WireConnection;54;0;20;0
WireConnection;54;1;146;0
WireConnection;55;0;54;0
WireConnection;55;1;146;0
WireConnection;57;0;55;0
WireConnection;102;0;57;0
WireConnection;102;1;104;0
WireConnection;102;2;103;0
WireConnection;140;0;57;0
WireConnection;105;0;57;0
WireConnection;105;1;106;0
WireConnection;105;2;107;0
WireConnection;126;0;127;2
WireConnection;126;2;130;0
WireConnection;108;0;105;0
WireConnection;108;1;102;0
WireConnection;135;0;137;2
WireConnection;135;2;138;0
WireConnection;114;0;140;0
WireConnection;114;1;116;0
WireConnection;114;2;115;0
WireConnection;129;0;126;0
WireConnection;111;0;108;0
WireConnection;111;1;129;0
WireConnection;111;2;131;0
WireConnection;133;0;108;0
WireConnection;133;1;135;0
WireConnection;133;2;134;0
WireConnection;143;0;102;0
WireConnection;143;1;114;0
WireConnection;109;0;110;0
WireConnection;109;1;143;0
WireConnection;141;0;142;0
WireConnection;141;1;114;0
WireConnection;132;0;111;0
WireConnection;132;1;133;0
WireConnection;165;0;146;0
WireConnection;113;0;141;0
WireConnection;113;1;109;0
WireConnection;113;2;132;0
WireConnection;166;0;165;0
WireConnection;164;0;113;0
WireConnection;164;1;166;0
ASEEND*/
//CHKSM=994401785F1F4B3E0264428CCAC56AE8F1279BC7