using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont spriteFont;

        private Texture2D CurrTNT;
        private Texture2D TNT;
        private Texture2D TNTExploded;

        int t = 0;
        private Texture2D coin;

        Texture2D floorTexture;
        Rectangle rect;
        bool exploded = false;
        bool timer = false;

        private ExplosionParticleSystem _explosions;
        MouseState _priorMouse;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _explosions = new ExplosionParticleSystem(this, 20);
            Components.Add(_explosions);
            rect = new Rectangle(0, 300, 1000, 300);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            floorTexture = new Texture2D(GraphicsDevice, 1, 1);
            floorTexture.SetData(new Color[] { Color.Gray });

            coin = Content.Load<Texture2D>("PictoCoin");

            TNT = Content.Load<Texture2D>("TNT");
            TNTExploded = Content.Load<Texture2D>("TNTExploded");

            spriteFont = Content.Load<SpriteFont>("arial");

            CurrTNT = TNT;

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            MouseState currentMouse = Mouse.GetState();
            //Vector2 mousePosition = new Vector2(currentMouse.X, currentMouse.Y);

            if (currentMouse.LeftButton == ButtonState.Pressed && _priorMouse.LeftButton == ButtonState.Released && exploded == false)
            {
                _explosions.PlaceExplosion(new Vector2(GraphicsDevice.Viewport.Width / 2 - 16, 300-32));
                CurrTNT = TNTExploded;
                exploded = true;
                timer = true;
            }
            else if(currentMouse.LeftButton == ButtonState.Pressed && _priorMouse.LeftButton == ButtonState.Released && exploded == true && t == 0)
            {
                //replace keg
                CurrTNT = TNT;
                exploded = false;
                timer = false;
            }
            //small explosion animation to sprites
            if(t == -60)
            {
                timer = false;
            }
            else if(timer == true)
            {
                t -= 5;
            }
            if(t != 0 && timer == false)
            {
                t += 5;
            }
            _priorMouse = currentMouse;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);
            Matrix transform;

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(floorTexture, rect, Color.Gray);
            _spriteBatch.Draw(CurrTNT, new Vector2(GraphicsDevice.Viewport.Width / 2 - 16, 300 - 32), Color.White);
            if(exploded == true)
            {
                _spriteBatch.DrawString(spriteFont, "Click to reset!", new Vector2((GraphicsDevice.Viewport.Width / 2) - 250, (GraphicsDevice.Viewport.Height / 2) - 96), Color.White);
            }
            _spriteBatch.End();

            //Make coin go up from explosion
            transform = Matrix.CreateTranslation(0, t, 0);
            _spriteBatch.Begin(transformMatrix: transform);
            _spriteBatch.Draw(coin, new Vector2(100, 300 - 17), null, Color.White, 0, new Vector2(0, 0), .1f, SpriteEffects.None, 1);
            _spriteBatch.Draw(coin, new Vector2(300, 300 - 17), null, Color.White, 0, new Vector2(0, 0), .1f, SpriteEffects.None, 1);
            _spriteBatch.Draw(coin, new Vector2(550, 300 - 17), null, Color.White, 0, new Vector2(0, 0), .1f, SpriteEffects.None, 1);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
