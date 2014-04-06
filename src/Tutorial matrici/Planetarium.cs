using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using XTeam.LogiX_Technologies;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Planetario
{
    class Planetario
    {

    }

    #region "Pianeta"

    class Pianeta : Planetario
    {
        public Model model;
        public Model model1;
        public Model anelli;
        public float distanzaDalSole;
        public float periodoRotazione;
        public float periodoRivoluzione;
        public float inclinazioneAsse;
        public float raggio;
        public string nomePianeta;
        public XTexture texture;
        public XTexture textureAnelli;
        public XTexture textureAnelliPattern;
        public VertexData position;
        public Matrix saveWorld;
        public Matrix matrix;
        public Matrix matrix1;
        Effect effect;
        EffectHandle[] effectHandles;

        public Pianeta(float DistanzaDalSoleInMilioniKm, float RaggioInKm, float InclinazioneAsse, float PeriodoDiRotazioneInGiorni, float PeriodoDiRivoluzioneInGiorni, XTexture Texture, string NomeDelPianeta)
        {
            nomePianeta = NomeDelPianeta;
            model = new Model("MEDIA//satellite.x",0);
            model1 = new Model("MEDIA//sfera4.x", 0);
            distanzaDalSole = DistanzaDalSoleInMilioniKm;
            periodoRotazione = PeriodoDiRotazioneInGiorni;
            periodoRivoluzione = PeriodoDiRivoluzioneInGiorni;
            inclinazioneAsse = InclinazioneAsse;
            raggio = RaggioInKm;
            texture = Texture;
            position = VertexData.Empty;
        }

        public void CreaAnelli(XTexture TextureBase, XTexture TexturePattern)
        {
            anelli = new Model("MEDIA\\ring.x", 0);
            textureAnelli = TextureBase;
            textureAnelliPattern = TexturePattern;
            EffectPool pool = new EffectPool();
            effect = Effect.FromFile(LogiX_Engine.Device, "MEDIA\\anelli.fx", null, ShaderFlags.None, pool);
            effectHandles = new EffectHandle[5];
            effectHandles[0] = effect.GetTechnique("t0");
            effectHandles[1] = effect.GetParameter(null, "matViewProjection");
            effectHandles[2] = effect.GetParameter(null, "matWorld");
            effectHandles[3] = effect.GetParameter(null, "colorTex");
            effectHandles[4] = effect.GetParameter(null, "alphaTex");
        }

        public void RenderizzaPianeta(float time, LogiX_Engine lxe, bool hd)
        {
            lxe.AmbientLight = Color.FromArgb(0, 0, 0);
            TransformPlanet(time);
            if (!hd)
            {
                model.meshTexture[0] = texture.DXTexture;
                model.RenderWithoutMatrices(0);
            }
            else
            {
                model1.meshTexture[0] = texture.DXTexture;
                model1.RenderWithoutMatrices(0);
            }
            XMaterial.ResetMaterials();
            XTexture.ResetTextures();
            lxe.AmbientLight = Color.FromArgb(0, 0, 0);
            RestoreWorld();
        }

        public void RenderizzaAtmosfera(float time, float RaggioAtmosferaRispettoPianeta, XMaterial mat, LogiX_Engine lxe, bool hd)
        {
            if (hd)
            {
                lxe.SetAlphaMode();
                lxe.SetCullMode(CullMode.Clockwise);
                lxe.AmbientLight = Color.FromArgb(255, 255, 255);
                lxe.SetLightMode(true, true);
                XTexture.ResetTextures();
                TransformAtm(time, RaggioAtmosferaRispettoPianeta);
                mat.SetMaterial();
                model1.DrawSubset(0);
                XMaterial.ResetMaterials();
                RestoreWorld();
                lxe.AmbientLight = Color.FromArgb(0, 0, 0);
                lxe.SetCullMode(CullMode.CounterClockwise);
                lxe.UnSetAlphaMode();
                lxe.SetLightMode(true, false);
            }
        }

        public void RenderizzaAnelli(float time, float Scaling)
        {
            effect.Begin(FX.None);
            matrix1 = LogiX_Engine.Device.Transform.View * LogiX_Engine.Device.Transform.Projection;
            //Matrix.TransposeMatrix(matrix1);
            effect.SetValue(effectHandles[1], matrix1);
            TransformRing(time, Scaling);
            //matrix = Matrix.TransposeMatrix(matrix);
            effect.SetValue(effectHandles[2], matrix);
            effect.SetValue(effectHandles[3], textureAnelli.DXTexture);
            effect.SetValue(effectHandles[4], textureAnelliPattern.DXTexture);
            effect.CommitChanges();
            effect.BeginPass(0);
            
            anelli.DrawSubset(0);

            effect.EndPass();
            effect.End();

            XTexture.ResetTextures();
            XMaterial.ResetMaterials();
            RestoreWorld();
        }

        private void TransformAtm(float time, float rapp)
        {
            saveWorld = LogiX_Engine.Device.Transform.World;
            matrix = Matrix.Scaling(rapp * raggio / 200000, rapp * raggio / 200000, rapp * raggio / 200000) * Matrix.RotationY(-time / periodoRotazione) * Matrix.RotationZ(inclinazioneAsse) * Matrix.RotationY(time / periodoRivoluzione) * Matrix.Translation(distanzaDalSole * 10 + 200, 0, 0) * Matrix.RotationY(-time / periodoRivoluzione);
            LogiX_Engine.Device.Transform.World = matrix;
            position.X = matrix.M41;
            position.Y = matrix.M42;
            position.Z = matrix.M43;
        }

        private void TransformRing(float time, float scaling)
        {
            saveWorld = LogiX_Engine.Device.Transform.World;
            matrix = Matrix.Scaling(scaling, scaling, scaling) * Matrix.RotationY(-time / periodoRotazione) * Matrix.RotationZ(inclinazioneAsse) * Matrix.RotationY(time / periodoRivoluzione) * Matrix.Translation(distanzaDalSole * 10 + 200, 0, 0) * Matrix.RotationY(-time / periodoRivoluzione);
            LogiX_Engine.Device.Transform.World = matrix;
            position.X = matrix.M41;
            position.Y = matrix.M42;
            position.Z = matrix.M43;
        }

        private void TransformPlanet(float time)
        {
            saveWorld = LogiX_Engine.Device.Transform.World;
            matrix = Matrix.Scaling(raggio / 200000, raggio / 200000, raggio / 200000) * Matrix.RotationY(-time / periodoRotazione) * Matrix.RotationZ(inclinazioneAsse) * Matrix.RotationY(time / periodoRivoluzione) * Matrix.Translation(distanzaDalSole * 10 + 200, 0, 0) * Matrix.RotationY(-time / periodoRivoluzione);
            LogiX_Engine.Device.Transform.World = matrix;
            position.X = matrix.M41;
            position.Y = matrix.M42;
            position.Z = matrix.M43;
        }

        private void RestoreWorld()
        {
            LogiX_Engine.Device.Transform.World = saveWorld;
        }

        public override string ToString()
        {
            return nomePianeta;
        }
    }

    #endregion

    #region "Satellite"

    class Satellite : Planetario
    {
        public Model model;
        public Model model1;
        public float distanzaDalPianeta;
        public float periodoRotazione;
        public float periodoRivoluzione;
        public float inclinazioneAsse;
        public float raggio;
        public string nomeSatellite;
        public XTexture texture;
        public VertexData position;
        public Matrix saveWorld;
        public Matrix matrix;
        public Pianeta pianeta;
        bool isdefault;

        public Satellite(Pianeta Pianeta, float DistanzaDalPianetaInMigliaiaKm, float RaggioInKm, float InclinazioneAsse, float PeriodoDiRotazioneInGiorni, float PeriodoDiRivoluzioneInGiorni, XTexture Texture, string NomeDelSatellite)
        {
            pianeta = Pianeta;
            nomeSatellite = NomeDelSatellite;
            model = new Model("MEDIA//satellite.x", 0);
            model1 = new Model("MEDIA//sfera4.x", 0);
            distanzaDalPianeta = DistanzaDalPianetaInMigliaiaKm;
            periodoRotazione = PeriodoDiRotazioneInGiorni;
            periodoRivoluzione = PeriodoDiRivoluzioneInGiorni;
            inclinazioneAsse = InclinazioneAsse;
            raggio = RaggioInKm;
            texture = Texture;
            isdefault = true;
            position = VertexData.Empty;
        }

        public Satellite(Pianeta Pianeta, float DistanzaDalPianetaInMigliaiaKm, float RaggioInKm, float InclinazioneAsse, float PeriodoDiRotazioneInGiorni, float PeriodoDiRivoluzioneInGiorni, XTexture Texture, string NomeDelSatellite, Model Model)
        {
            pianeta = Pianeta;
            nomeSatellite = NomeDelSatellite;
            model = Model;
            distanzaDalPianeta = DistanzaDalPianetaInMigliaiaKm;
            periodoRotazione = PeriodoDiRotazioneInGiorni;
            periodoRivoluzione = PeriodoDiRivoluzioneInGiorni;
            inclinazioneAsse = InclinazioneAsse;
            raggio = RaggioInKm;
            texture = Texture;
            isdefault = false;
            position = VertexData.Empty;
        }

        public void RenderizzaSatellite(float time, LogiX_Engine lxe, bool hd)
        {
            switch (isdefault)
            {
                case true:
                    lxe.AmbientLight = Color.FromArgb(0, 0, 0);
                    TransformPlanet(time, true);
                    if (!hd)
                    {
                        model.meshTexture[0] = texture.DXTexture;
                        model.RenderWithoutMatrices(0);
                    }
                    else
                    {
                        model1.meshTexture[0] = texture.DXTexture;
                        model1.RenderWithoutMatrices(0);
                    }
                    lxe.AmbientLight = Color.FromArgb(0, 0, 0);
                    RestoreWorld();
                    break;
                case false:
                    lxe.AmbientLight = Color.FromArgb(0, 0, 0);
                    //XMaterial.ResetMaterials();
                    TransformPlanet(time, false);
                    if (!hd)
                    {
                        model.meshTexture[0] = texture.DXTexture;
                        model.RenderWithoutMatrices(0);
                    }
                    else
                    {
                        model1.meshTexture[0] = texture.DXTexture;
                        model1.RenderWithoutMatrices(0);
                    }
                    lxe.AmbientLight = Color.FromArgb(0, 0, 0);
                    RestoreWorld();
                    break;
            }
        }

        private void TransformPlanet(float time, bool IsDefault)
        {
            switch (IsDefault)
            {
                case true:
                    saveWorld = LogiX_Engine.Device.Transform.World;
                    matrix = Matrix.Scaling(raggio / 200000, raggio / 200000, raggio / 200000) * Matrix.RotationY(-time / periodoRotazione) * Matrix.RotationZ(inclinazioneAsse) * Matrix.RotationY(time / periodoRivoluzione) * Matrix.Translation(distanzaDalPianeta / 10 * 2, 0, 0) * Matrix.RotationY(-time / periodoRivoluzione) * Matrix.Translation(pianeta.position.X, pianeta.position.Y, pianeta.position.Z);
                    LogiX_Engine.Device.Transform.World = matrix;
                    position.X = matrix.M41;
                    position.Y = matrix.M42;
                    position.Z = matrix.M43;
                    break;
                case false:
                    saveWorld = LogiX_Engine.Device.Transform.World;
                    matrix = Matrix.Scaling(raggio / 200000, raggio / 200000, raggio / 200000) * Matrix.RotationY(-time / periodoRotazione) * Matrix.RotationZ(inclinazioneAsse) * Matrix.RotationY(time / periodoRivoluzione) * Matrix.Translation(distanzaDalPianeta, 0, 0) * Matrix.RotationY(-time / periodoRivoluzione) * Matrix.Translation(pianeta.position.X, pianeta.position.Y, pianeta.position.Z);
                    LogiX_Engine.Device.Transform.World = matrix;
                    position.X = matrix.M41;
                    position.Y = matrix.M42;
                    position.Z = matrix.M43;
                    break;
            }
        }

        private void TransformAtm(float time, float rapp)
        {
            saveWorld = LogiX_Engine.Device.Transform.World;
            matrix = Matrix.Scaling(rapp * raggio / 200000, rapp * raggio / 200000, rapp * raggio / 200000) * Matrix.RotationY(-time / periodoRotazione) * Matrix.RotationZ(inclinazioneAsse) * Matrix.RotationY(time / periodoRivoluzione) * Matrix.Translation(distanzaDalPianeta/10 * 2 , 0, 0) * Matrix.RotationY(-time / periodoRivoluzione) * Matrix.Translation(pianeta.position.X, pianeta.position.Y, pianeta.position.Z);
            LogiX_Engine.Device.Transform.World = matrix;
            position.X = matrix.M41;
            position.Y = matrix.M42;
            position.Z = matrix.M43;
        }

        private void RestoreWorld()
        {
            LogiX_Engine.Device.Transform.World = saveWorld;
        }

        public void RenderizzaAtmosfera(float time, float RaggioAtmosferaRispettoPianeta, XMaterial mat, LogiX_Engine lxe)
        {
            lxe.SetAlphaMode();
            lxe.SetCullMode(CullMode.Clockwise);
            lxe.AmbientLight = Color.FromArgb(255, 255, 255);
            lxe.SetLightMode(true, true);
            XTexture.ResetTextures();
            TransformAtm(time, RaggioAtmosferaRispettoPianeta);
            mat.SetMaterial();
            model.DrawSubset(0);
            XMaterial.ResetMaterials();
            RestoreWorld();
            lxe.AmbientLight = Color.FromArgb(0, 0, 0);
            lxe.SetCullMode(CullMode.CounterClockwise);
            lxe.UnSetAlphaMode();
            lxe.SetLightMode(true, false);
        }
    }

    #endregion

    #region "Cielo Stellato"

    class CieloStellato : Planetario
    {
        public Model model;
        Effect effect;
        EffectHandle[] effectHandles;
        BaseTexture[] baseTexture;
        Matrix matrix;

        public CieloStellato()
        {
            model = new Model("MEDIA//satellite.x", 0);
            EffectPool pool = new EffectPool();
            effect = Effect.FromFile(LogiX_Engine.Device, "MEDIA//NightSky.fx", null, ShaderFlags.None, pool);
            effectHandles = new EffectHandle[6];
            effectHandles[0] = effect.GetTechnique("t0");
            effectHandles[1] = effect.GetParameter(null, "matViewProjection");
            effectHandles[2] = effect.GetParameter(null, "Time");
            effectHandles[3] = effect.GetParameter(null, "BaseTexture");
            effectHandles[4] = effect.GetParameter(null, "NoiseTexture");
            effectHandles[5] = effect.GetParameter(null, "TexDimension");
            baseTexture = new BaseTexture[2];
            baseTexture[0] = TextureLoader.FromFile(LogiX_Engine.Device, "MEDIA//StarMap1.jpg");
            baseTexture[1] = TextureLoader.FromFile(LogiX_Engine.Device, "MEDIA//Noise.jpg");
        }

        public void RenderizzaCieloStellato(float time, LogiX_Engine lxe, float TexDimension)
        {
            lxe.SetCullMode(CullMode.Clockwise);
            effect.Begin(FX.None);
            matrix = Matrix.Scaling(new Vector3(1000f,1000f,1000f)) * LogiX_Engine.Device.Transform.View * LogiX_Engine.Device.Transform.Projection;
            Matrix.TransposeMatrix(matrix);
            effect.SetValue(effectHandles[1], matrix);
            effect.SetValue(effectHandles[2], time/3);
            effect.SetValue(effectHandles[3], baseTexture[0]);
            effect.SetValue(effectHandles[4], baseTexture[1]);
            effect.SetValue(effectHandles[5], TexDimension);
            effect.CommitChanges();
            effect.BeginPass(0);

            model.DrawSubset(0);

            effect.EndPass();
            effect.End();
            lxe.SetCullMode(CullMode.CounterClockwise);
        }
    }

    #endregion

    #region "Sole"

    class Sole : Planetario
    {
        public Model model;
        Effect effect;
        EffectHandle[] effectHandles;
        BaseTexture baseTexture;
        Matrix matrix;
        XMaterial Alone;
        DirectionalLight light;
        public VertexData position = VertexData.Empty;

        public Sole()
        {
            model = new Model("MEDIA//Sfera5.x", 0);
            EffectPool pool = new EffectPool();
            effect = Effect.FromFile(LogiX_Engine.Device, "MEDIA//Sun.fx", null, ShaderFlags.None, pool);
            effectHandles = new EffectHandle[5];
            effectHandles[0] = effect.GetTechnique("t0");
            effectHandles[1] = effect.GetParameter(null, "matViewProjection");
            effectHandles[2] = effect.GetParameter(null, "Time");
            effectHandles[3] = effect.GetParameter(null, "Texture");
            effectHandles[4] = effect.GetParameter(null, "TexDimension");
            baseTexture = TextureLoader.FromFile(LogiX_Engine.Device, "MEDIA//N030.jpg");
            Alone = new XMaterial(Color.White, Color.White, Color.Black, 1);
            light = new DirectionalLight(VertexData.Empty, Color.FromArgb(60, 10, 0), Color.FromArgb(255, 170, 100), Color.FromArgb(250, 250, 250));
        }

        public void RenderizzaSole(float time, float TexDimension)
        {
            effect.Begin(FX.None);
            matrix = Matrix.Scaling(new Vector3(0.35f, 0.35f, 0.35f)) * LogiX_Engine.Device.Transform.View * LogiX_Engine.Device.Transform.Projection;
            Matrix.TransposeMatrix(matrix);
            effect.SetValue(effectHandles[1], matrix);
            effect.SetValue(effectHandles[2], time / 50);
            effect.SetValue(effectHandles[3], baseTexture);
            effect.SetValue(effectHandles[4], TexDimension);
            effect.CommitChanges();
            effect.BeginPass(0);

            model.DrawSubset(0);

            effect.EndPass();
            effect.End();
        }

        public void RenderizzaAlone(LogiX_Engine lxe, Camera cam)
        {
            lxe.SetCullMode(CullMode.Clockwise);
            lxe.SetLightMode(true, true);
            lxe.SetAlphaMode();
            light.Direction = cam.Position;
            light.Enabled = true;
            light.SetDirectionalLight();
            model.Scaling = new VertexData(0.48f, 0.48f, 0.48f);
            model.meshTexture = null;
            XTexture.ResetTextures();
            model.SetModelMatrices();
            Alone.SetMaterial();
            model.DrawSubset(0);
            XMaterial.ResetMaterials();
            model.UnSetModelMatrices();
            light.Enabled = false;
            light.SetDirectionalLight();
            lxe.UnSetAlphaMode();
            lxe.SetLightMode(true, false);
            lxe.SetCullMode(CullMode.CounterClockwise);
        }
    }

    #endregion

}
