float4x4 matViewProjection;
float Time;
texture Texture;
float TexDimension;

struct VS_INPUT 
{
   float4 Position : POSITION0;
   float2 TexCoord : TEXCOORD0;
   
};

struct VS_OUTPUT 
{
   float4 Position : POSITION0;
   float2 TexCoord0 : TEXCOORD0;
   float2 TexCoord1 : TEXCOORD1;
   float2 TexCoord2 : TEXCOORD2;
};

VS_OUTPUT vs_main( VS_INPUT Input )
{
   VS_OUTPUT Output;

   Output.Position = mul( Input.Position, matViewProjection );
   Output.TexCoord0 = Input.TexCoord * TexDimension + float2(1.17*Time,2.31*Time);
   Output.TexCoord1 = Input.TexCoord * TexDimension + float2(0.51f*Time, -2.13f*Time);
   Output.TexCoord2 = Input.TexCoord * TexDimension + float2(-2.21f*Time, 0.29f*Time);
   
   return( Output );
   
}

sampler2D BaseTex = sampler_state
{
   Texture = (Texture);
   MINFILTER = ANISOTROPIC;
   MIPFILTER = ANISOTROPIC;
   MAGFILTER = ANISOTROPIC;
};

struct PS_INPUT
{
   float2 TexCoord0 : TEXCOORD0;
   float2 TexCoord1 : TEXCOORD1;
   float2 TexCoord2 : TEXCOORD2;
};

float4 ps_main(PS_INPUT Input) : COLOR0
{   
   float4 TotalColor;
   float4 Tex0 = tex2D(BaseTex, Input.TexCoord0);
   TotalColor = Tex0;
   Tex0 = normalize(Tex0);
   float4 Tex1 = tex2D(BaseTex, Input.TexCoord1);
   TotalColor += Tex1;
   Tex1 = normalize(Tex1);
   float4 Tex2 = tex2D(BaseTex, Input.TexCoord2);
   Tex2 = normalize(Tex2);
   TotalColor += Tex2;
   normalize(TotalColor);
   return( TotalColor/1.9f + float4(0.01f,0.01f,0.01f,0.01f) );
}

technique t0
{
   pass Pass_0
   {
      VertexShader = compile vs_2_0 vs_main();
      PixelShader = compile ps_2_0 ps_main();
   }
}

