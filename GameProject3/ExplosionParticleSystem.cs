using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject3
{
    public class ExplosionParticleSystem : ParticleSystem
    {
        Color color;
        Color[] colors = new Color[]
        {
            Color.Blue,
            Color.Red,
            Color.Yellow,
            Color.Green,
        };

        public ExplosionParticleSystem(Game game, int maxExplosions) : base(game, maxExplosions * 25)
        {

        }

        protected override void InitializeConstants()
        {
            textureFilename = "explosion";
            minNumParticles = 20;
            maxNumParticles = 25;
            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(40, 500);
            var lifetime = RandomHelper.NextFloat(0.5f, 1.0f);
            var acceleration = -velocity / lifetime;
            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);
            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);

            p.Initialize(where, velocity, acceleration, color, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;
            float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
            particle.Color = color * alpha;
            particle.Scale = .25f + .25f * normalizedLifetime;
        }

        public void PlaceExplosion(Vector2 where){
            color = colors[RandomHelper.Next(colors.Length)];
            AddParticles(where);
        }
    }
}
